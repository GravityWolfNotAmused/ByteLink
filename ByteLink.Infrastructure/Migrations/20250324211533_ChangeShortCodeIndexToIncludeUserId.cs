using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeShortCodeIndexToIncludeUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Urls_ShortCode",
                table: "Urls");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Urls",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Urls_ShortCode_UserId",
                table: "Urls",
                columns: new[] { "ShortCode", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Urls_ShortCode_UserId",
                table: "Urls");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Urls",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Urls_ShortCode",
                table: "Urls",
                column: "ShortCode",
                unique: true);
        }
    }
}
