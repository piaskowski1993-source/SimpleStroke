using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddShapeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Shape",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Shape",
                table: "Users");
        }
    }
}
