using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CustomerAndProducerofferDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ActiveOfferLogs");

            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Diposit",
                table: "ProducerCustomerOffers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "ProducerCustomerOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Step",
                table: "ActiveOfferLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProducerSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StepName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinDuration = table.Column<int>(type: "int", nullable: false),
                    MaxDuration = table.Column<int>(type: "int", nullable: false),
                    ProducerCustomerOfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProducerSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProducerSteps_ProducerCustomerOffers_ProducerCustomerOfferId",
                        column: x => x.ProducerCustomerOfferId,
                        principalTable: "ProducerCustomerOffers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProducerSteps_ProducerCustomerOfferId",
                table: "ProducerSteps",
                column: "ProducerCustomerOfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveOfferLogs_CustomerCustomOffer_CustomOfferID",
                table: "ActiveOfferLogs",
                column: "CustomOfferID",
                principalTable: "CustomerCustomOffer",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveOfferLogs_CustomerPublishedOffers_PublishedOfferID",
                table: "ActiveOfferLogs",
                column: "PublishedOfferID",
                principalTable: "CustomerPublishedOffers",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveOfferLogs_CustomerCustomOffer_CustomOfferID",
                table: "ActiveOfferLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ActiveOfferLogs_CustomerPublishedOffers_PublishedOfferID",
                table: "ActiveOfferLogs");

            migrationBuilder.DropTable(
                name: "ProducerSteps");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Diposit",
                table: "ProducerCustomerOffers");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "ProducerCustomerOffers");

            migrationBuilder.DropColumn(
                name: "Step",
                table: "ActiveOfferLogs");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ActiveOfferLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
