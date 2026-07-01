using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class systemCounter_CustomerPublishedOfferName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserID",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "ActiveOfferLogs");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "ActiveOfferLogs",
                newName: "UpdatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CustomerPublishedOffers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CustomerCustomOffer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ActiveOfferLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "SystemCounters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCounter = table.Column<int>(type: "int", nullable: false),
                    ProducerCounter = table.Column<int>(type: "int", nullable: false),
                    DesignerCounter = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemCounters", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserID",
                table: "Notifications",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserID",
                table: "Notifications");

            migrationBuilder.DropTable(
                name: "SystemCounters");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CustomerPublishedOffers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CustomerCustomOffer");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ActiveOfferLogs");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "ActiveOfferLogs",
                newName: "StartDate");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "ActiveOfferLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserID",
                table: "Notifications",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
