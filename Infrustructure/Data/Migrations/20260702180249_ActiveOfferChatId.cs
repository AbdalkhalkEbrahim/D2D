using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ActiveOfferChatId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
