using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthSubjectColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthSubject",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthSubject",
                table: "Users");
        }
    }
}
