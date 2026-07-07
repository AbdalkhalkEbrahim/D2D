using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReportCoulmnToCustomerTaple : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProducer",
                table: "Reports");

            migrationBuilder.AlterColumn<string>(
                name: "DesignerID",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CustomerID",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Reporter",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CustomerID",
                table: "Reports",
                column: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Customers_CustomerID",
                table: "Reports",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Customers_CustomerID",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_CustomerID",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Reporter",
                table: "Reports");

            migrationBuilder.AlterColumn<string>(
                name: "DesignerID",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsProducer",
                table: "Reports",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
