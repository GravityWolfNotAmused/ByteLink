using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteLink.Infrastructure.Migrations
{
    // <inheritdoc />
    public partial class CreateURLTotalVisitsColumn : Migration
    {
        // <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TotalVisits",
                table: "Urls",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

        }

        // <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalVisits",
                table: "Urls");
        }
    }
}
