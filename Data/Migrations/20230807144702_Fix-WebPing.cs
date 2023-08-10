using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPulse2023.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixWebPing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WebPing_WebsiteId",
                table: "WebPing",
                column: "WebsiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebPing_Website_WebsiteId",
                table: "WebPing",
                column: "WebsiteId",
                principalTable: "Website",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebPing_Website_WebsiteId",
                table: "WebPing");

            migrationBuilder.DropIndex(
                name: "IX_WebPing_WebsiteId",
                table: "WebPing");
        }
    }
}
