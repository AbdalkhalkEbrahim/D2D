using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class audits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "ActiveOfferLogs",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "ActiveOfferLogs",
                newName: "UpdatedAt");
         }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
