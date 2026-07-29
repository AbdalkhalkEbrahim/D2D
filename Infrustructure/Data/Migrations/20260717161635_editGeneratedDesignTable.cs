using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editGeneratedDesignTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelChats_Customers_CustomerID",
                table: "ModelChats");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelGeneratedDesigns_Customers_CustomerId",
                table: "ModelGeneratedDesigns");

            migrationBuilder.DropTable(
                name: "DesignStates");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "ModelChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_ModelGeneratedDesigns_CustomerId",
                table: "ModelGeneratedDesigns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DesignVerifications",
                table: "DesignVerifications");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "ModelGeneratedDesigns");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ModelGeneratedDesigns");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ModelChats");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedAt",
                table: "ModelGeneratedDesigns",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                table: "ModelChats",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelChats_CustomerID",
                table: "ModelChats",
                newName: "IX_ModelChats_CustomerId");

            migrationBuilder.AddColumn<int>(
                name: "ModelChatId",
                table: "ModelGeneratedDesigns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxChatTokens",
                table: "ModelChats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "StepUrl",
                table: "DesignVerifications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DesignVerifications",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "DesignId",
                table: "DesignVerifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FinalDesign",
                table: "DesignVerifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DesignId",
                table: "DesignerDesigns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DesignVerifications",
                table: "DesignVerifications",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ModelGeneratedDesigns_ModelChatId",
                table: "ModelGeneratedDesigns",
                column: "ModelChatId");

            migrationBuilder.CreateIndex(
                name: "IX_DesignerDesigns_DesignId",
                table: "DesignerDesigns",
                column: "DesignId");

            migrationBuilder.AddForeignKey(
                name: "FK_DesignerDesigns_DesignVerifications_DesignId",
                table: "DesignerDesigns",
                column: "DesignId",
                principalTable: "DesignVerifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelChats_Customers_CustomerId",
                table: "ModelChats",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelGeneratedDesigns_ModelChats_ModelChatId",
                table: "ModelGeneratedDesigns",
                column: "ModelChatId",
                principalTable: "ModelChats",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DesignerDesigns_DesignVerifications_DesignId",
                table: "DesignerDesigns");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelChats_Customers_CustomerId",
                table: "ModelChats");

            migrationBuilder.DropForeignKey(
                name: "FK_ModelGeneratedDesigns_ModelChats_ModelChatId",
                table: "ModelGeneratedDesigns");

            migrationBuilder.DropIndex(
                name: "IX_ModelGeneratedDesigns_ModelChatId",
                table: "ModelGeneratedDesigns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DesignVerifications",
                table: "DesignVerifications");

            migrationBuilder.DropIndex(
                name: "IX_DesignerDesigns_DesignId",
                table: "DesignerDesigns");

            migrationBuilder.DropColumn(
                name: "ModelChatId",
                table: "ModelGeneratedDesigns");

            migrationBuilder.DropColumn(
                name: "MaxChatTokens",
                table: "ModelChats");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DesignVerifications");

            migrationBuilder.DropColumn(
                name: "DesignId",
                table: "DesignVerifications");

            migrationBuilder.DropColumn(
                name: "FinalDesign",
                table: "DesignVerifications");

            migrationBuilder.DropColumn(
                name: "DesignId",
                table: "DesignerDesigns");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "ModelGeneratedDesigns",
                newName: "LastUpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "ModelChats",
                newName: "CustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_ModelChats_CustomerId",
                table: "ModelChats",
                newName: "IX_ModelChats_CustomerID");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "ModelGeneratedDesigns",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ModelGeneratedDesigns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ModelChats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "StepUrl",
                table: "DesignVerifications",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DesignVerifications",
                table: "DesignVerifications",
                column: "StepUrl");

            migrationBuilder.CreateTable(
                name: "DesignStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Background = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColorPrimary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FabricWeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InpaintMaskPrompt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Length = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Material = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModificationArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Neckline = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalImageReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Secondary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sleeve = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StyleCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetGender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Texture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Theme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Views = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DesignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Base64String = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_ModelGeneratedDesigns_DesignId",
                        column: x => x.DesignId,
                        principalTable: "ModelGeneratedDesigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModelChatMessages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelChatID = table.Column<int>(type: "int", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Limit = table.Column<int>(type: "int", nullable: false),
                    LimitCounter = table.Column<int>(type: "int", nullable: false),
                    Sender = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelChatMessages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ModelChatMessages_ModelChats_ModelChatID",
                        column: x => x.ModelChatID,
                        principalTable: "ModelChats",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModelGeneratedDesigns_CustomerId",
                table: "ModelGeneratedDesigns",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_DesignId",
                table: "Images",
                column: "DesignId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelChatMessages_ModelChatID",
                table: "ModelChatMessages",
                column: "ModelChatID");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelChats_Customers_CustomerID",
                table: "ModelChats",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelGeneratedDesigns_Customers_CustomerId",
                table: "ModelGeneratedDesigns",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
