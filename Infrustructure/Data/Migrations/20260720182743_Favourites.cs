using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Favourites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Favourites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProducerDesignId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DesignerDesignId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favourites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favourites_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favourites_DesignerDesigns_DesignerDesignId",
                        column: x => x.DesignerDesignId,
                        principalTable: "DesignerDesigns",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Favourites_ProducerDesigns_ProducerDesignId",
                        column: x => x.ProducerDesignId,
                        principalTable: "ProducerDesigns",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favourites_DesignerDesignId",
                table: "Favourites",
                column: "DesignerDesignId");

            migrationBuilder.CreateIndex(
                name: "IX_Favourites_ProducerDesignId",
                table: "Favourites",
                column: "ProducerDesignId");

            migrationBuilder.CreateIndex(
                name: "IX_Favourites_UserId",
                table: "Favourites",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favourites");
        }
    }
}
