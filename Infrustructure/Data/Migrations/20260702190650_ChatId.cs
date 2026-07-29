using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChatId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "PublishedOfferID",
                table: "ActiveOfferLogs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublishedOfferActive",
                table: "ActiveOfferLogs",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCustomOfferActive",
                table: "ActiveOfferLogs",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomOfferID",
                table: "ActiveOfferLogs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
            migrationBuilder.AddColumn<int>(
    name: "ChatID",
    table: "ActiveOfferLogs",
    type: "int",
    nullable: false,
    defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ActiveOfferLogs_ChatID",
                table: "ActiveOfferLogs",
                column: "ChatID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveOfferLogs_Chats_ChatID",
                table: "ActiveOfferLogs",
                column: "ChatID",
                principalTable: "Chats",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "PublishedOfferID",
                table: "ActiveOfferLogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublishedOfferActive",
                table: "ActiveOfferLogs",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsCustomOfferActive",
                table: "ActiveOfferLogs",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomOfferID",
                table: "ActiveOfferLogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
            migrationBuilder.DropForeignKey(
    name: "FK_ActiveOfferLogs_Chats_ChatID",
    table: "ActiveOfferLogs");

            migrationBuilder.DropIndex(
                name: "IX_ActiveOfferLogs_ChatID",
                table: "ActiveOfferLogs");

            migrationBuilder.DropColumn(
                name: "ChatID",
                table: "ActiveOfferLogs");
        }
    }
}
