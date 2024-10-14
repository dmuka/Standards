using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Standards.Migrations
{
    public partial class AddPersonsUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                table: "Users",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "Users",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LockOutEnd",
                table: "Users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LockOutEnd",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
