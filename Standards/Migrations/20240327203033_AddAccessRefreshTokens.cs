using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Standards.Migrations
{
    public partial class AddAccessRefreshTokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Token",
                table: "Users",
                newName: "RefreshToken");

            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                table: "Users",
                newName: "Token");
        }
    }
}
