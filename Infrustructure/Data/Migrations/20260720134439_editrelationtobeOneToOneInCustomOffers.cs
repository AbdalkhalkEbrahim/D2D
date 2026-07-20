using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editrelationtobeOneToOneInCustomOffers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CustomOfferId",
                table: "ProducerCustomerOffers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProducerCustomerOffers_CustomOfferId",
                table: "ProducerCustomerOffers",
                column: "CustomOfferId",
                unique: true,
                filter: "[CustomOfferId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ProducerCustomerOffers_CustomerCustomOffers_CustomOfferId",
                table: "ProducerCustomerOffers",
                column: "CustomOfferId",
                principalTable: "CustomerCustomOffers",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProducerCustomerOffers_CustomerCustomOffers_CustomOfferId",
                table: "ProducerCustomerOffers");

            migrationBuilder.DropIndex(
                name: "IX_ProducerCustomerOffers_CustomOfferId",
                table: "ProducerCustomerOffers");

            migrationBuilder.DropColumn(
                name: "CustomOfferId",
                table: "ProducerCustomerOffers");
        }
    }
}
