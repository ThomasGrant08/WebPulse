using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPulse2023.Data.Migrations
{
    /// <inheritdoc />
    public partial class Sharing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Role",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Resource",
                table: "Permissions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "CanShare",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Role_GroupId",
                table: "Role",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Group_GroupId",
                table: "Role",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Role_Group_GroupId",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Role_GroupId",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "CanShare",
                table: "Permissions");

            migrationBuilder.AlterColumn<string>(
                name: "Resource",
                table: "Permissions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
