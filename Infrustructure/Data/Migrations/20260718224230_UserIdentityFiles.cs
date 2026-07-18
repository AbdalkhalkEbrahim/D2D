using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserIdentityFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Confidence",
                table: "LicenseVerifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "LicenseVerifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserIdentityFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FrontImageID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackImageID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    IsSamePerson = table.Column<bool>(type: "bit", nullable: false),
                    FaceSimilarity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Confidence = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIdentityFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserIdentityFiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserIdentityFiles_UserId",
                table: "UserIdentityFiles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserIdentityFiles");

            migrationBuilder.DropColumn(
                name: "Confidence",
                table: "LicenseVerifications");

            migrationBuilder.DropColumn(
                name: "Observations",
                table: "LicenseVerifications");

            migrationBuilder.DropColumn(
                name: "NationalId",
                table: "AspNetUsers");
        }
    }
}
