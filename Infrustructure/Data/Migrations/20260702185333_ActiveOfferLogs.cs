using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ActiveOfferLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             migrationBuilder.CreateTable(
                 name: "ActiveOfferLogs",
                 columns: table => new
                 {
                     ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                     Status = table.Column<int>(type: "int", nullable: false),
                     StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                     EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                     Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     PublishedOfferID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                     CustomOfferID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                     IsPublishedOfferActive = table.Column<bool>(type: "bit", nullable: false),
                     IsCustomOfferActive = table.Column<bool>(type: "bit", nullable: false),
                 },
                 constraints: table =>
                 {
                     table.PrimaryKey("PK_ActiveOfferLogs", x => x.ID);
                 });
            migrationBuilder.CreateIndex(
                name: "IX_ActiveOfferLogs_CustomOfferID_IsCustomOfferActive",
                table: "ActiveOfferLogs",
                columns: new[] { "CustomOfferID", "IsCustomOfferActive" });

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveOfferLogs_CustomerCustomOffer_CustomOfferID_IsCustomOfferActive",
                table: "ActiveOfferLogs",
                columns: new[] { "CustomOfferID", "IsCustomOfferActive" },
                principalTable: "CustomerCustomOffer",
                principalColumns: new[] { "ID", "IsActive" });
            
            migrationBuilder.AddForeignKey(
                name: "FK_ActiveOfferLogs_CustomerPublishedOffers_PublishedOfferID_IsPublishedOfferActive",
                table: "ActiveOfferLogs",
                columns: new[] { "PublishedOfferID", "IsPublishedOfferActive" },
                principalTable: "CustomerPublishedOffers",
                principalColumns: new[] { "ID", "IsActive" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveOfferLogs_CustomerCustomOffer_CustomOfferID_IsCustomOfferActive",
                table: "ActiveOfferLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ActiveOfferLogs_CustomerPublishedOffers_PublishedOfferID_IsPublishedOfferActive",
                table: "ActiveOfferLogs");

            migrationBuilder.DropIndex(
                name: "IX_ActiveOfferLogs_CustomOfferID_IsCustomOfferActive",
                table: "ActiveOfferLogs");

            migrationBuilder.DropColumn(
                name: "CustomOfferID",
                table: "ActiveOfferLogs");

            migrationBuilder.DropColumn(
                name: "IsCustomOfferActive",
                table: "ActiveOfferLogs");

            migrationBuilder.RenameColumn(
                name: "PublishedOfferID",
                table: "ActiveOfferLogs",
                newName: "OfferID");

            migrationBuilder.RenameColumn(
                name: "IsPublishedOfferActive",
                table: "ActiveOfferLogs",
                newName: "IsOfferActive");

            migrationBuilder.RenameIndex(
                name: "IX_ActiveOfferLogs_PublishedOfferID_IsPublishedOfferActive",
                table: "ActiveOfferLogs",
                newName: "IX_ActiveOfferLogs_OfferID_IsOfferActive");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveOfferLogs_CustomerCustomOffer_OfferID_IsOfferActive",
                table: "ActiveOfferLogs",
                columns: new[] { "OfferID", "IsOfferActive" },
                principalTable: "CustomerCustomOffer",
                principalColumns: new[] { "ID", "IsActive" });

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveOfferLogs_CustomerPublishedOffers_OfferID_IsOfferActive",
                table: "ActiveOfferLogs",
                columns: new[] { "OfferID", "IsOfferActive" },
                principalTable: "CustomerPublishedOffers",
                principalColumns: new[] { "ID", "IsActive" });
        }
    }
}
