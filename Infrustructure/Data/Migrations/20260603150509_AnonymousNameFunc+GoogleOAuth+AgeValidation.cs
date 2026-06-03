using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AnonymousNameFuncGoogleOAuthAgeValidation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnonName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "AnonymousName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnnonName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
            migrationBuilder.DropColumn(
                name: "AnonymousName",
                table: "AspNetUsers");
        }
    }
}
