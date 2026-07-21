using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class optionalEscrow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Escrows_EscrowID",
                table: "Transactions");

            migrationBuilder.AlterColumn<Guid>(
                name: "EscrowID",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Escrows_EscrowID",
                table: "Transactions",
                column: "EscrowID",
                principalTable: "Escrows",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Escrows_EscrowID",
                table: "Transactions");

            migrationBuilder.AlterColumn<Guid>(
                name: "EscrowID",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Escrows_EscrowID",
                table: "Transactions",
                column: "EscrowID",
                principalTable: "Escrows",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
