using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Standards.Migrations
{
    public partial class AddNavigationPropertiesInModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResponcibleId",
                table: "Standards",
                newName: "ResponsibleId");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Services",
                newName: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationsJournal_PlaceId",
                table: "VerificationsJournal",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationsJournal_StandardId",
                table: "VerificationsJournal",
                column: "StandardId");

            migrationBuilder.CreateIndex(
                name: "IX_Standards_ResponsibleId",
                table: "Standards",
                column: "ResponsibleId");

            migrationBuilder.CreateIndex(
                name: "IX_Standards_WorkPlaceId",
                table: "Standards",
                column: "WorkPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicesJournal_PersonId",
                table: "ServicesJournal",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicesJournal_ServiceId",
                table: "ServicesJournal",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicesJournal_StandardId",
                table: "ServicesJournal",
                column: "StandardId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceTypeId",
                table: "Services",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_UnitId",
                table: "Materials",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Characteristics_GradeId",
                table: "Characteristics",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Characteristics_UnitId",
                table: "Characteristics",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationsJournal_PlaceId",
                table: "CalibrationsJournal",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationsJournal_StandardId",
                table: "CalibrationsJournal",
                column: "StandardId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalibrationsJournal_Places_PlaceId",
                table: "CalibrationsJournal",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CalibrationsJournal_Standards_StandardId",
                table: "CalibrationsJournal",
                column: "StandardId",
                principalTable: "Standards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Characteristics_Grades_GradeId",
                table: "Characteristics",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Characteristics_Units_UnitId",
                table: "Characteristics",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Units_UnitId",
                table: "Materials",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceTypes_ServiceTypeId",
                table: "Services",
                column: "ServiceTypeId",
                principalTable: "ServiceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServicesJournal_Persons_PersonId",
                table: "ServicesJournal",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServicesJournal_Services_ServiceId",
                table: "ServicesJournal",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServicesJournal_Standards_StandardId",
                table: "ServicesJournal",
                column: "StandardId",
                principalTable: "Standards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Standards_Persons_ResponsibleId",
                table: "Standards",
                column: "ResponsibleId",
                principalTable: "Persons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Standards_WorkPlaces_WorkPlaceId",
                table: "Standards",
                column: "WorkPlaceId",
                principalTable: "WorkPlaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationsJournal_Places_PlaceId",
                table: "VerificationsJournal",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationsJournal_Standards_StandardId",
                table: "VerificationsJournal",
                column: "StandardId",
                principalTable: "Standards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalibrationsJournal_Places_PlaceId",
                table: "CalibrationsJournal");

            migrationBuilder.DropForeignKey(
                name: "FK_CalibrationsJournal_Standards_StandardId",
                table: "CalibrationsJournal");

            migrationBuilder.DropForeignKey(
                name: "FK_Characteristics_Grades_GradeId",
                table: "Characteristics");

            migrationBuilder.DropForeignKey(
                name: "FK_Characteristics_Units_UnitId",
                table: "Characteristics");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Units_UnitId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceTypes_ServiceTypeId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_ServicesJournal_Persons_PersonId",
                table: "ServicesJournal");

            migrationBuilder.DropForeignKey(
                name: "FK_ServicesJournal_Services_ServiceId",
                table: "ServicesJournal");

            migrationBuilder.DropForeignKey(
                name: "FK_ServicesJournal_Standards_StandardId",
                table: "ServicesJournal");

            migrationBuilder.DropForeignKey(
                name: "FK_Standards_Persons_ResponsibleId",
                table: "Standards");

            migrationBuilder.DropForeignKey(
                name: "FK_Standards_WorkPlaces_WorkPlaceId",
                table: "Standards");

            migrationBuilder.DropForeignKey(
                name: "FK_VerificationsJournal_Places_PlaceId",
                table: "VerificationsJournal");

            migrationBuilder.DropForeignKey(
                name: "FK_VerificationsJournal_Standards_StandardId",
                table: "VerificationsJournal");

            migrationBuilder.DropIndex(
                name: "IX_VerificationsJournal_PlaceId",
                table: "VerificationsJournal");

            migrationBuilder.DropIndex(
                name: "IX_VerificationsJournal_StandardId",
                table: "VerificationsJournal");

            migrationBuilder.DropIndex(
                name: "IX_Standards_ResponsibleId",
                table: "Standards");

            migrationBuilder.DropIndex(
                name: "IX_Standards_WorkPlaceId",
                table: "Standards");

            migrationBuilder.DropIndex(
                name: "IX_ServicesJournal_PersonId",
                table: "ServicesJournal");

            migrationBuilder.DropIndex(
                name: "IX_ServicesJournal_ServiceId",
                table: "ServicesJournal");

            migrationBuilder.DropIndex(
                name: "IX_ServicesJournal_StandardId",
                table: "ServicesJournal");

            migrationBuilder.DropIndex(
                name: "IX_Services_ServiceTypeId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Materials_UnitId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Characteristics_GradeId",
                table: "Characteristics");

            migrationBuilder.DropIndex(
                name: "IX_Characteristics_UnitId",
                table: "Characteristics");

            migrationBuilder.DropIndex(
                name: "IX_CalibrationsJournal_PlaceId",
                table: "CalibrationsJournal");

            migrationBuilder.DropIndex(
                name: "IX_CalibrationsJournal_StandardId",
                table: "CalibrationsJournal");

            migrationBuilder.RenameColumn(
                name: "ResponsibleId",
                table: "Standards",
                newName: "ResponcibleId");

            migrationBuilder.RenameColumn(
                name: "ServiceTypeId",
                table: "Services",
                newName: "TypeId");
        }
    }
}
