using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class producer_ProducerCustomerOffer_Realtionship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProducerDesignerOffers_Producers_ProducerID",
                table: "ProducerDesignerOffers");

            migrationBuilder.DropIndex(
                name: "IX_ProducerDesignerOffers_ProducerID",
                table: "ProducerDesignerOffers");

            migrationBuilder.DropColumn(
                name: "ProducerID",
                table: "ProducerDesignerOffers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProducerID",
                table: "ProducerDesignerOffers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ProducerDesignerOffers_ProducerID",
                table: "ProducerDesignerOffers",
                column: "ProducerID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProducerDesignerOffers_Producers_ProducerID",
                table: "ProducerDesignerOffers",
                column: "ProducerID",
                principalTable: "Producers",
                principalColumn: "Id");
        }
    }
}
