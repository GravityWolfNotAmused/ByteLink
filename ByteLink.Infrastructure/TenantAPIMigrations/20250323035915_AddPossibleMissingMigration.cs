using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteLink.Infrastructure.TenantAPIMigrations
{
    /// <inheritdoc />
    public partial class AddPossibleMissingMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "ApplicationUser");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "ApplicationUser",
                newName: "IX_ApplicationUser_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Users_DatabaseUser",
                table: "ApplicationUser",
                newName: "IX_ApplicationUser_DatabaseUser");

            migrationBuilder.RenameIndex(
                name: "IX_Users_DatabasePWD",
                table: "ApplicationUser",
                newName: "IX_ApplicationUser_DatabasePWD");

            migrationBuilder.RenameIndex(
                name: "IX_Users_DatabaseName",
                table: "ApplicationUser",
                newName: "IX_ApplicationUser_DatabaseName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUser",
                table: "ApplicationUser",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUser",
                table: "ApplicationUser");

            migrationBuilder.RenameTable(
                name: "ApplicationUser",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUser_Email",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUser_DatabaseUser",
                table: "Users",
                newName: "IX_Users_DatabaseUser");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUser_DatabasePWD",
                table: "Users",
                newName: "IX_Users_DatabasePWD");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUser_DatabaseName",
                table: "Users",
                newName: "IX_Users_DatabaseName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");
        }
    }
}
