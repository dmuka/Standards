using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Standards.Migrations
{
    public partial class AddHousingsInDepartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Housing_HousingId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_HousingId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "HousingId",
                table: "Departments");

            migrationBuilder.CreateTable(
                name: "DepartmentHousing",
                columns: table => new
                {
                    DepartmentsId = table.Column<int>(type: "int", nullable: false),
                    HousingsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentHousing", x => new { x.DepartmentsId, x.HousingsId });
                    table.ForeignKey(
                        name: "FK_DepartmentHousing_Departments_DepartmentsId",
                        column: x => x.DepartmentsId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentHousing_Housing_HousingsId",
                        column: x => x.HousingsId,
                        principalTable: "Housing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentHousing_HousingsId",
                table: "DepartmentHousing",
                column: "HousingsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentHousing");

            migrationBuilder.AddColumn<int>(
                name: "HousingId",
                table: "Departments",
                type: "int",
                nullable: true);

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
        }
    }
}
