using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class customercutomofferwithproducerdesigns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveOfferLogs_CustomerCustomOffer_CustomOfferID",
                table: "ActiveOfferLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ActiveOfferLogs_CustomerCustomOffer_CustomOfferID_IsCustomOfferActive",
                table: "ActiveOfferLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerCustomOffer_Customers_CustomerID",
                table: "CustomerCustomOffer");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerCustomOffer_ProducerCustomerOffers_ProducerCustomerOfferID",
                table: "CustomerCustomOffer");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerCustomOffer_ProducerDesigns_ProducerDesignID",
                table: "CustomerCustomOffer");

            migrationBuilder.DropForeignKey(
                name: "FK_Escrows_CustomerCustomOffer_CustomerOfferID_IsOfferActive",
                table: "Escrows");

            migrationBuilder.DropForeignKey(
                name: "FK_ProducerCustomerOffers_CustomerCustomOffer_CustomerCustomOfferID",
                table: "ProducerCustomerOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProducerDesigns_CustomerCustomOffer_CustomerCustomOfferID",
                table: "ProducerDesigns");

            migrationBuilder.DropIndex(
                name: "IX_ProducerDesigns_CustomerCustomOfferID",
                table: "ProducerDesigns");

            migrationBuilder.DropIndex(
                name: "IX_ProducerCustomerOffers_CustomerCustomOfferID",
                table: "ProducerCustomerOffers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CustomerCustomOffer_ID_IsActive",
                table: "CustomerCustomOffer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerCustomOffer",
                table: "CustomerCustomOffer");

            migrationBuilder.DropColumn(
                name: "CustomerCustomOfferID",
                table: "ProducerDesigns");

            migrationBuilder.DropColumn(
                name: "CustomerCustomOfferID",
                table: "ProducerCustomerOffers");

            migrationBuilder.RenameTable(
                name: "CustomerCustomOffer",
                newName: "CustomerCustomOffers");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerCustomOffer_ProducerDesignID",
                table: "CustomerCustomOffers",
                newName: "IX_CustomerCustomOffers_ProducerDesignID");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerCustomOffer_ProducerCustomerOfferID",
                table: "CustomerCustomOffers",
                newName: "IX_CustomerCustomOffers_ProducerCustomerOfferID");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerCustomOffer_ID_IsActive",
                table: "CustomerCustomOffers",
                newName: "IX_CustomerCustomOffers_ID_IsActive");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerCustomOffer_CustomerID",
                table: "CustomerCustomOffers",
                newName: "IX_CustomerCustomOffers_CustomerID");

            migrationBuilder.AddColumn<Guid>(
                name: "ProducerDesignID1",
                table: "CustomerCustomOffers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CustomerCustomOffers_ID_IsActive",
                table: "CustomerCustomOffers",
                columns: new[] { "ID", "IsActive" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerCustomOffers",
                table: "CustomerCustomOffers",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCustomOffers_ProducerDesignID1",
                table: "CustomerCustomOffers",
                column: "ProducerDesignID1");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveOfferLogs_CustomerCustomOffers_CustomOfferID",
                table: "ActiveOfferLogs",
                column: "CustomOfferID",
                principalTable: "CustomerCustomOffers",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveOfferLogs_CustomerCustomOffers_CustomOfferID_IsCustomOfferActive",
                table: "ActiveOfferLogs",
                columns: new[] { "CustomOfferID", "IsCustomOfferActive" },
                principalTable: "CustomerCustomOffers",
                principalColumns: new[] { "ID", "IsActive" });

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerCustomOffers_Customers_CustomerID",
                table: "CustomerCustomOffers",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerCustomOffers_ProducerCustomerOffers_ProducerCustomerOfferID",
                table: "CustomerCustomOffers",
                column: "ProducerCustomerOfferID",
                principalTable: "ProducerCustomerOffers",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerCustomOffers_ProducerDesigns_ProducerDesignID",
                table: "CustomerCustomOffers",
                column: "ProducerDesignID",
                principalTable: "ProducerDesigns",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerCustomOffers_ProducerDesigns_ProducerDesignID1",
                table: "CustomerCustomOffers",
                column: "ProducerDesignID1",
                principalTable: "ProducerDesigns",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Escrows_CustomerCustomOffers_CustomerOfferID_IsOfferActive",
                table: "Escrows",
                columns: new[] { "CustomerOfferID", "IsOfferActive" },
                principalTable: "CustomerCustomOffers",
                principalColumns: new[] { "ID", "IsActive" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveOfferLogs_CustomerCustomOffers_CustomOfferID",
                table: "ActiveOfferLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ActiveOfferLogs_CustomerCustomOffers_CustomOfferID_IsCustomOfferActive",
                table: "ActiveOfferLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerCustomOffers_Customers_CustomerID",
                table: "CustomerCustomOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerCustomOffers_ProducerCustomerOffers_ProducerCustomerOfferID",
                table: "CustomerCustomOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerCustomOffers_ProducerDesigns_ProducerDesignID",
                table: "CustomerCustomOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerCustomOffers_ProducerDesigns_ProducerDesignID1",
                table: "CustomerCustomOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_Escrows_CustomerCustomOffers_CustomerOfferID_IsOfferActive",
                table: "Escrows");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CustomerCustomOffers_ID_IsActive",
                table: "CustomerCustomOffers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerCustomOffers",
                table: "CustomerCustomOffers");

            migrationBuilder.DropIndex(
                name: "IX_CustomerCustomOffers_ProducerDesignID1",
                table: "CustomerCustomOffers");

            migrationBuilder.DropColumn(
                name: "ProducerDesignID1",
                table: "CustomerCustomOffers");

            migrationBuilder.RenameTable(
                name: "CustomerCustomOffers",
                newName: "CustomerCustomOffer");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerCustomOffers_ProducerDesignID",
                table: "CustomerCustomOffer",
                newName: "IX_CustomerCustomOffer_ProducerDesignID");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerCustomOffers_ProducerCustomerOfferID",
                table: "CustomerCustomOffer",
                newName: "IX_CustomerCustomOffer_ProducerCustomerOfferID");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerCustomOffers_ID_IsActive",
                table: "CustomerCustomOffer",
                newName: "IX_CustomerCustomOffer_ID_IsActive");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerCustomOffers_CustomerID",
                table: "CustomerCustomOffer",
                newName: "IX_CustomerCustomOffer_CustomerID");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerCustomOfferID",
                table: "ProducerDesigns",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerCustomOfferID",
                table: "ProducerCustomerOffers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CustomerCustomOffer_ID_IsActive",
                table: "CustomerCustomOffer",
                columns: new[] { "ID", "IsActive" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerCustomOffer",
                table: "CustomerCustomOffer",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_ProducerDesigns_CustomerCustomOfferID",
                table: "ProducerDesigns",
                column: "CustomerCustomOfferID");

            migrationBuilder.CreateIndex(
                name: "IX_ProducerCustomerOffers_CustomerCustomOfferID",
                table: "ProducerCustomerOffers",
                column: "CustomerCustomOfferID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveOfferLogs_CustomerCustomOffer_CustomOfferID",
                table: "ActiveOfferLogs",
                column: "CustomOfferID",
                principalTable: "CustomerCustomOffer",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveOfferLogs_CustomerCustomOffer_CustomOfferID_IsCustomOfferActive",
                table: "ActiveOfferLogs",
                columns: new[] { "CustomOfferID", "IsCustomOfferActive" },
                principalTable: "CustomerCustomOffer",
                principalColumns: new[] { "ID", "IsActive" });

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerCustomOffer_Customers_CustomerID",
                table: "CustomerCustomOffer",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerCustomOffer_ProducerCustomerOffers_ProducerCustomerOfferID",
                table: "CustomerCustomOffer",
                column: "ProducerCustomerOfferID",
                principalTable: "ProducerCustomerOffers",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerCustomOffer_ProducerDesigns_ProducerDesignID",
                table: "CustomerCustomOffer",
                column: "ProducerDesignID",
                principalTable: "ProducerDesigns",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Escrows_CustomerCustomOffer_CustomerOfferID_IsOfferActive",
                table: "Escrows",
                columns: new[] { "CustomerOfferID", "IsOfferActive" },
                principalTable: "CustomerCustomOffer",
                principalColumns: new[] { "ID", "IsActive" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProducerCustomerOffers_CustomerCustomOffer_CustomerCustomOfferID",
                table: "ProducerCustomerOffers",
                column: "CustomerCustomOfferID",
                principalTable: "CustomerCustomOffer",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProducerDesigns_CustomerCustomOffer_CustomerCustomOfferID",
                table: "ProducerDesigns",
                column: "CustomerCustomOfferID",
                principalTable: "CustomerCustomOffer",
                principalColumn: "ID");
        }
    }
}
