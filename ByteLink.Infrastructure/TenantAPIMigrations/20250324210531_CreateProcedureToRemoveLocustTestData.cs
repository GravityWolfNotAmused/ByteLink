using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteLink.Infrastructure.TenantAPIMigrations
{
    /// <inheritdoc />
    public partial class CreateProcedureToRemoveLocustTestData : Migration
    {
        private readonly string _procedureName = "remove_locust_create_databases";
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"
DROP PROCEDURE IF EXISTS {_procedureName};

CREATE PROCEDURE {_procedureName}(IN in_development_user_id INT)
BEGIN
    DECLARE done INT DEFAULT 0;
    DECLARE var_id INT;
    DECLARE var_dbname VARCHAR(255);
    DECLARE var_dbuser VARCHAR(255);
    DECLARE var_dbpwd VARCHAR(255);
        
    DECLARE cur1 CURSOR FOR 
        SELECT Id, DatabaseName, DatabaseUser, DatabasePWD 
        FROM bytelinktenant.applicationuser 
        WHERE Id != IFNULL(in_development_user_id, 4);
    
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1;
    
    CREATE TEMPORARY TABLE IF NOT EXISTS temp_ids_to_delete (id INT PRIMARY KEY);
    OPEN cur1;
    
    read_loop: LOOP
        FETCH cur1 INTO var_id, var_dbname, var_dbuser, var_dbpwd;
        IF done THEN
            LEAVE read_loop;
        END IF;
        
        -- Drop the database dynamically
        SET @drop_sql = CONCAT('DROP DATABASE IF EXISTS `', var_dbname, '`');
        PREPARE stmt FROM @drop_sql;
        EXECUTE stmt;
        DEALLOCATE PREPARE stmt;
        
        -- Drop the user dynamically (using host '%' here, adjust as needed)
        SET @drop_user_sql = CONCAT('DROP USER IF EXISTS \'', var_dbuser, '\'@\'%\'');
        PREPARE stmt FROM @drop_user_sql;
        EXECUTE stmt;
        DEALLOCATE PREPARE stmt;
        
        -- Instead of deleting now, store the id for later deletion
        INSERT INTO temp_ids_to_delete VALUES (var_id);
    END LOOP;
    
    CLOSE cur1;
    
    -- Now, delete rows from the table using the collected IDs
    DELETE FROM bytelinktenant.applicationuser
    WHERE Id IN (SELECT id FROM temp_ids_to_delete);
    
    -- Clean up the temporary table
    DROP TEMPORARY TABLE IF EXISTS temp_ids_to_delete;
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"
DROP PROCEDURE IF EXISTS {_procedureName};
");
        }
    }
}
