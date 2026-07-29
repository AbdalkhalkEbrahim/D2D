using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class chatmodelv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "View",
                table: "DesignStates",
                newName: "Views");

            migrationBuilder.RenameColumn(
                name: "PrimaryColor",
                table: "DesignStates",
                newName: "Secondary");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "DesignStates",
                newName: "ColorPrimary");

            migrationBuilder.AddColumn<string>(
                name: "FabricWeight",
                table: "DesignStates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InpaintMaskPrompt",
                table: "DesignStates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModificationArea",
                table: "DesignStates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Neckline",
                table: "DesignStates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalImageReference",
                table: "DesignStates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StyleCategory",
                table: "DesignStates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetGender",
                table: "DesignStates",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FabricWeight",
                table: "DesignStates");

            migrationBuilder.DropColumn(
                name: "InpaintMaskPrompt",
                table: "DesignStates");

            migrationBuilder.DropColumn(
                name: "ModificationArea",
                table: "DesignStates");

            migrationBuilder.DropColumn(
                name: "Neckline",
                table: "DesignStates");

            migrationBuilder.DropColumn(
                name: "OriginalImageReference",
                table: "DesignStates");

            migrationBuilder.DropColumn(
                name: "StyleCategory",
                table: "DesignStates");

            migrationBuilder.DropColumn(
                name: "TargetGender",
                table: "DesignStates");

            migrationBuilder.RenameColumn(
                name: "Views",
                table: "DesignStates",
                newName: "View");

            migrationBuilder.RenameColumn(
                name: "Secondary",
                table: "DesignStates",
                newName: "PrimaryColor");

            migrationBuilder.RenameColumn(
                name: "ColorPrimary",
                table: "DesignStates",
                newName: "Gender");
        }
    }
}
