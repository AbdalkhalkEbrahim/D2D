using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class designNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelGeneratedDesigns_Customers_CustomerId",
                table: "ModelGeneratedDesigns");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelGeneratedDesigns_DesignStates_DesignStateId",
                table: "ModelGeneratedDesigns");

            migrationBuilder.DropIndex(
                name: "IX_ModelGeneratedDesigns_DesignStateId",
                table: "ModelGeneratedDesigns");

            migrationBuilder.DropColumn(
                name: "DesignStateId",
                table: "ModelGeneratedDesigns");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "ProducerDesigns",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "ModelGeneratedDesigns",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ModelGeneratedDesigns",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ModelGeneratedDesigns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "ModelGeneratedDesigns",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PromptUsed",
                table: "ModelGeneratedDesigns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ModelGeneratedDesigns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "DesignerDesigns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Design",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "CustomerDesigns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelGeneratedDesigns_Customers_CustomerId",
                table: "ModelGeneratedDesigns",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelGeneratedDesigns_Customers_CustomerId",
                table: "ModelGeneratedDesigns");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "ProducerDesigns");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ModelGeneratedDesigns");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ModelGeneratedDesigns");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "ModelGeneratedDesigns");

            migrationBuilder.DropColumn(
                name: "PromptUsed",
                table: "ModelGeneratedDesigns");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ModelGeneratedDesigns");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "DesignerDesigns");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Design");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "CustomerDesigns");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "ModelGeneratedDesigns",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DesignStateId",
                table: "ModelGeneratedDesigns",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ModelGeneratedDesigns_DesignStateId",
                table: "ModelGeneratedDesigns",
                column: "DesignStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelGeneratedDesigns_Customers_CustomerId",
                table: "ModelGeneratedDesigns",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelGeneratedDesigns_DesignStates_DesignStateId",
                table: "ModelGeneratedDesigns",
                column: "DesignStateId",
                principalTable: "DesignStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
