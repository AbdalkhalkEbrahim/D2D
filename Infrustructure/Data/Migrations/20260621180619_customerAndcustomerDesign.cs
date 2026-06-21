using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class customerAndcustomerDesign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "CustomerDesigns",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDesigns_CustomerId",
                table: "CustomerDesigns",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerDesigns_Customers_CustomerId",
                table: "CustomerDesigns",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerDesigns_Customers_CustomerId",
                table: "CustomerDesigns");

            migrationBuilder.DropIndex(
                name: "IX_CustomerDesigns_CustomerId",
                table: "CustomerDesigns");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "CustomerDesigns");
        }
    }
}
