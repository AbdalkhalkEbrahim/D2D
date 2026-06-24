using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class publishedOfferDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "CustomerPublishedOffers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Colors",
                table: "CustomerPublishedOffers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CustomerPublishedOffers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Gender",
                table: "CustomerPublishedOffers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PrintingType",
                table: "CustomerPublishedOffers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Season",
                table: "CustomerPublishedOffers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sizes",
                table: "CustomerPublishedOffers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "SizesFile",
                table: "CustomerPublishedOffers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Style",
                table: "CustomerPublishedOffers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TargetAudience",
                table: "CustomerPublishedOffers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "CustomerCustomOffer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Colors",
                table: "CustomerCustomOffer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CustomerCustomOffer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Gender",
                table: "CustomerCustomOffer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PrintingType",
                table: "CustomerCustomOffer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Season",
                table: "CustomerCustomOffer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sizes",
                table: "CustomerCustomOffer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "SizesFile",
                table: "CustomerCustomOffer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Style",
                table: "CustomerCustomOffer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TargetAudience",
                table: "CustomerCustomOffer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "CustomerPublishedOffers");

            migrationBuilder.DropColumn(
                name: "Colors",
                table: "CustomerPublishedOffers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CustomerPublishedOffers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "CustomerPublishedOffers");

            migrationBuilder.DropColumn(
                name: "PrintingType",
                table: "CustomerPublishedOffers");

            migrationBuilder.DropColumn(
                name: "Season",
                table: "CustomerPublishedOffers");

            migrationBuilder.DropColumn(
                name: "Sizes",
                table: "CustomerPublishedOffers");

            migrationBuilder.DropColumn(
                name: "SizesFile",
                table: "CustomerPublishedOffers");

            migrationBuilder.DropColumn(
                name: "Style",
                table: "CustomerPublishedOffers");

            migrationBuilder.DropColumn(
                name: "TargetAudience",
                table: "CustomerPublishedOffers");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "CustomerCustomOffer");

            migrationBuilder.DropColumn(
                name: "Colors",
                table: "CustomerCustomOffer");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CustomerCustomOffer");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "CustomerCustomOffer");

            migrationBuilder.DropColumn(
                name: "PrintingType",
                table: "CustomerCustomOffer");

            migrationBuilder.DropColumn(
                name: "Season",
                table: "CustomerCustomOffer");

            migrationBuilder.DropColumn(
                name: "Sizes",
                table: "CustomerCustomOffer");

            migrationBuilder.DropColumn(
                name: "SizesFile",
                table: "CustomerCustomOffer");

            migrationBuilder.DropColumn(
                name: "Style",
                table: "CustomerCustomOffer");

            migrationBuilder.DropColumn(
                name: "TargetAudience",
                table: "CustomerCustomOffer");
        }
    }
}
