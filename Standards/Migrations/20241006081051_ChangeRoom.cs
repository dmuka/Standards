using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Standards.Migrations
{
    public partial class ChangeRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HousingId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Housing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FloorsCount = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Housing", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HousingId",
                table: "Rooms",
                column: "HousingId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_HousingId",
                table: "Departments",
                column: "HousingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Housing_HousingId",
                table: "Departments",
                column: "HousingId",
                principalTable: "Housing",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Housing_HousingId",
                table: "Rooms",
                column: "HousingId",
                principalTable: "Housing",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Housing_HousingId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Housing_HousingId",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "Housing");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_HousingId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Departments_HousingId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "HousingId",
                table: "Departments");
        }
    }
}
