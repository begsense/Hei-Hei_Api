using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hei_Hei_Api.Migrations
{
    /// <inheritdoc />
    public partial class Images : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Heroes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Animators",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Heroes");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Animators");
        }
    }
}
