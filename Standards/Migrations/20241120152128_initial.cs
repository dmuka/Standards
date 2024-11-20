using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Standards.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Housings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FloorsCount = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Housings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IsTwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    LockOutEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsLockOutEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sectors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sectors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sectors_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        name: "FK_DepartmentHousing_Housings_HousingsId",
                        column: x => x.HousingsId,
                        principalTable: "Housings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HousingId = table.Column<int>(type: "int", nullable: false),
                    Floor = table.Column<int>(type: "int", nullable: false),
                    Length = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Width = table.Column<double>(type: "float", nullable: false),
                    SectorId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Housings_HousingId",
                        column: x => x.HousingId,
                        principalTable: "Housings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rooms_Sectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sectors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    BirthdayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SectorId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Persons_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Persons_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Persons_Sectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sectors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Persons_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Standards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResponsibleId = table.Column<int>(type: "int", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VerificationInterval = table.Column<int>(type: "int", nullable: false),
                    CalibrationInterval = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Standards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Standards_Persons_ResponsibleId",
                        column: x => x.ResponsibleId,
                        principalTable: "Persons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CalibrationsJournal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StandardId = table.Column<int>(type: "int", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SertificateId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SertificateImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalibrationsJournal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalibrationsJournal_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalibrationsJournal_Standards_StandardId",
                        column: x => x.StandardId,
                        principalTable: "Standards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceTypeId = table.Column<int>(type: "int", nullable: false),
                    StandardId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_ServiceTypes_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Services_Standards_StandardId",
                        column: x => x.StandardId,
                        principalTable: "Standards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VerificationsJournal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StandardId = table.Column<int>(type: "int", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SertificateId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SertificateImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationsJournal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VerificationsJournal_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VerificationsJournal_Standards_StandardId",
                        column: x => x.StandardId,
                        principalTable: "Standards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkPlaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    ResponsibleId = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SectorId = table.Column<int>(type: "int", nullable: true),
                    StandardId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkPlaces_Persons_ResponsibleId",
                        column: x => x.ResponsibleId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkPlaces_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkPlaces_Sectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sectors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkPlaces_Standards_StandardId",
                        column: x => x.StandardId,
                        principalTable: "Standards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Quantities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quantities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quantities_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServicesJournal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StandardId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicesJournal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicesJournal_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServicesJournal_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServicesJournal_Standards_StandardId",
                        column: x => x.StandardId,
                        principalTable: "Standards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuantityId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RuName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RuSymbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Units_Quantities_QuantityId",
                        column: x => x.QuantityId,
                        principalTable: "Quantities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Units_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Characteristics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RangeStart = table.Column<double>(type: "float", nullable: false),
                    RangeEnd = table.Column<double>(type: "float", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    GradeId = table.Column<int>(type: "int", nullable: false),
                    GradeValue = table.Column<double>(type: "float", nullable: false),
                    GradeValueStart = table.Column<double>(type: "float", nullable: false),
                    GradeValueEnd = table.Column<double>(type: "float", nullable: false),
                    StandardId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characteristics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characteristics_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characteristics_Standards_StandardId",
                        column: x => x.StandardId,
                        principalTable: "Standards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characteristics_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materials_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Materials_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Comments", "Name", "ShortName" },
                values: new object[,]
                {
                    { 1, "Comments1", "Без категории", "" },
                    { 2, "Comments1", "Вторая квалификационная категория", "2 категория" },
                    { 3, "Comments1", "Первая квалификационная категория", "1 категория" },
                    { 4, "Comments1", "Ведущий", "Ведущий" }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Comments", "Name", "ShortName" },
                values: new object[,]
                {
                    { 1, "Comments1", "Department1", "d1" },
                    { 2, "Comments2", "Department2", "d2" },
                    { 3, "Comments3", "Department3", "d3" }
                });

            migrationBuilder.InsertData(
                table: "Housings",
                columns: new[] { "Id", "Address", "Comments", "FloorsCount", "Name", "ShortName" },
                values: new object[,]
                {
                    { 1, "Address1", "Comments1", 2, "Housing1", "h1" },
                    { 2, "Address2", "Comments2", 1, "Housing2", "h2" },
                    { 3, "Address3", "Comments3", 2, "Housing3", "h3" }
                });

            migrationBuilder.InsertData(
                table: "Positions",
                columns: new[] { "Id", "Comments", "Name", "ShortName" },
                values: new object[,]
                {
                    { 1, "Comments1", "Техник", "Техник" },
                    { 2, "Comments1", "Инженер", "Инженер" },
                    { 3, "Comments1", "Начальник сектора", "Нач. сектора" },
                    { 4, "Comments1", "Начальник отдела", "Нач. отдела" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "AccessToken", "Email", "IsEmailConfirmed", "IsLockOutEnabled", "IsTwoFactorEnabled", "LockOutEnd", "PasswordHash", "PasswordSalt", "RefreshToken", "UserName" },
                values: new object[,]
                {
                    { 1, 0, null, "user1@email.com", false, false, false, null, null, null, null, "user1" },
                    { 2, 0, null, "user2@email.com", false, false, false, null, null, null, null, "user2" },
                    { 3, 0, null, "user3@email.com", false, false, false, null, null, null, null, "user3" },
                    { 4, 0, null, "user4@email.com", false, false, false, null, null, null, null, "user4" },
                    { 5, 0, null, "user5@email.com", false, false, false, null, null, null, null, "user5" }
                });

            migrationBuilder.InsertData(
                table: "DepartmentHousing",
                columns: new[] { "DepartmentsId", "HousingsId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 3 },
                    { 2, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "Sectors",
                columns: new[] { "Id", "Comments", "DepartmentId", "Name", "ShortName" },
                values: new object[,]
                {
                    { 1, "Comments1", 1, "Sector1", "s1" },
                    { 2, "Comments2", 1, "Sector2", "s2" },
                    { 3, "Comments3", 1, "Sector3", "s3" },
                    { 4, "Comments4", 2, "Sector4", "s4" },
                    { 5, "Comments5", 2, "Sector5", "s5" },
                    { 6, "Comments6", 3, "Sector6", "s6" },
                    { 7, "Comments7", 3, "Sector7", "s7" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Comments", "Floor", "Height", "HousingId", "Length", "Name", "SectorId", "ShortName", "Width" },
                values: new object[,]
                {
                    { 1, "Comments1", 2, 3.0, 1, 4.0, "Room1", 1, "r1", 5.0 },
                    { 2, "Comments2", 1, 3.0, 2, 5.0, "Room2", 1, "r2", 5.0 },
                    { 3, "Comments3", 2, 3.0, 3, 6.0, "Room3", 3, "r3", 5.0 },
                    { 4, "Comments4", 1, 3.0, 1, 5.0, "Room4", 2, "r4", 5.0 },
                    { 5, "Comments5", 2, 3.0, 3, 6.0, "Room5", 3, "r5", 5.0 },
                    { 6, "Comments6", 1, 3.0, 3, 3.0, "Room6", 2, "r6", 4.0 },
                    { 7, "Comments7", 2, 3.0, 1, 6.0, "Room7", 4, "r7", 5.0 },
                    { 8, "Comments8", 1, 3.0, 2, 3.0, "Room8", 5, "r8", 4.0 },
                    { 9, "Comments9", 1, 3.0, 2, 6.0, "Room9", 4, "r9", 5.0 },
                    { 10, "Comments10", 1, 3.0, 2, 3.0, "Room10", 5, "r10", 4.0 },
                    { 11, "Comments11", 2, 3.0, 3, 6.0, "Room11", 6, "r11", 7.0 },
                    { 12, "Comments12", 2, 3.0, 3, 6.0, "Room12", 7, "r12", 5.0 }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "BirthdayDate", "CategoryId", "Comments", "FirstName", "LastName", "MiddleName", "PositionId", "Role", "RoomId", "SectorId" },
                values: new object[,]
                {
                    { 1, new DateTime(2000, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Comments1", "Антон", "Чехов", "Сергеевич", 2, "Engineer", 1, 1 },
                    { 2, new DateTime(2001, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Comments1", "Дмитрий", "Тургенев", "Анатольевич", 2, "Engineer", 2, 1 },
                    { 3, new DateTime(1999, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Comments1", "Сергей", "Толстой", "Романович", 3, "SectorHead", 3, 3 },
                    { 4, new DateTime(1998, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Comments1", "Петр", "Достоевский", "Артемович", 4, "DepartmentHead", 4, 2 },
                    { 5, new DateTime(2002, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Comments1", "Иван", "Пушкин", "Никодимович", 2, "Engineer", 5, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationsJournal_PlaceId",
                table: "CalibrationsJournal",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationsJournal_StandardId",
                table: "CalibrationsJournal",
                column: "StandardId");

            migrationBuilder.CreateIndex(
                name: "IX_Characteristics_GradeId",
                table: "Characteristics",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Characteristics_StandardId",
                table: "Characteristics",
                column: "StandardId");

            migrationBuilder.CreateIndex(
                name: "IX_Characteristics_UnitId",
                table: "Characteristics",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentHousing_HousingsId",
                table: "DepartmentHousing",
                column: "HousingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ServiceId",
                table: "Materials",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_UnitId",
                table: "Materials",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_CategoryId",
                table: "Persons",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_PositionId",
                table: "Persons",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_RoomId",
                table: "Persons",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_SectorId",
                table: "Persons",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Quantities_ServiceId",
                table: "Quantities",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HousingId",
                table: "Rooms",
                column: "HousingId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_SectorId",
                table: "Rooms",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sectors_DepartmentId",
                table: "Sectors",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceTypeId",
                table: "Services",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_StandardId",
                table: "Services",
                column: "StandardId");

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
                name: "IX_Standards_ResponsibleId",
                table: "Standards",
                column: "ResponsibleId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_QuantityId",
                table: "Units",
                column: "QuantityId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_ServiceId",
                table: "Units",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationsJournal_PlaceId",
                table: "VerificationsJournal",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationsJournal_StandardId",
                table: "VerificationsJournal",
                column: "StandardId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlaces_ResponsibleId",
                table: "WorkPlaces",
                column: "ResponsibleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlaces_RoomId",
                table: "WorkPlaces",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlaces_SectorId",
                table: "WorkPlaces",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlaces_StandardId",
                table: "WorkPlaces",
                column: "StandardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalibrationsJournal");

            migrationBuilder.DropTable(
                name: "Characteristics");

            migrationBuilder.DropTable(
                name: "DepartmentHousing");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "ServicesJournal");

            migrationBuilder.DropTable(
                name: "VerificationsJournal");

            migrationBuilder.DropTable(
                name: "WorkPlaces");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropTable(
                name: "Quantities");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropTable(
                name: "Standards");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Housings");

            migrationBuilder.DropTable(
                name: "Sectors");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
