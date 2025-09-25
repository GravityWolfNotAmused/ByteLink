using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateUrlTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TRIGGER UpdateUrlClickCount
                AFTER INSERT ON urlvisits
                FOR EACH ROW
                BEGIN
                    UPDATE urls
                    SET TotalVisits = TotalVisits + 1
                    WHERE Id = NEW.UrlId;
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS UpdateUrlClickCount;");
        }
    }
}
