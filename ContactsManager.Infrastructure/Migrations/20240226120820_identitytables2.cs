using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactsManager.Infrastructure.Migrations
{
    public partial class identitytables2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CHK_TIN",
                table: "Persons");

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: new Guid("12e15727-d369-49a9-8b13-bc22e9362179"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: new Guid("14629847-905a-4a0e-9abe-80b61655c5cb"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: new Guid("501c6d33-1bbe-45f1-8fbd-2275913c6218"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: new Guid("56bf46a4-02b8-4693-a0f5-0a95e2218bdc"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: new Guid("8f30bedc-47dd-4286-8950-73d8a68e5d41"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Personid",
                keyValue: new Guid("012107df-862f-4f16-ba94-e5c16886f005"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Personid",
                keyValue: new Guid("28d11936-9466-4a4b-b9c5-2f0a8e0cbde9"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Personid",
                keyValue: new Guid("29339209-63f5-492f-8459-754943c74abf"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Personid",
                keyValue: new Guid("2a6d3738-9def-43ac-9279-0310edc7ceca"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Personid",
                keyValue: new Guid("89e5f445-d89f-4e12-94e0-5ad5b235d704"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Personid",
                keyValue: new Guid("a3b9833b-8a4d-43e9-8690-61e08df81a9a"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Personid",
                keyValue: new Guid("ac660a73-b0b7-4340-abc1-a914257a6189"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Personid",
                keyValue: new Guid("c03bbe45-9aeb-4d24-99e0-4743016ffce9"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Personid",
                keyValue: new Guid("c3abddbd-cf50-41d2-b6c4-cc7d5a750928"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Personid",
                keyValue: new Guid("c6d50a47-f7e6-4482-8be0-4ddfc057fa6e"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Personid",
                keyValue: new Guid("cb035f22-e7cf-4907-bd07-91cfee5240f3"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Personid",
                keyValue: new Guid("d15c6d9f-70b4-48c5-afd3-e71261f1a9be"));

            migrationBuilder.RenameColumn(
                name: "TaxIdentificationNumbers",
                table: "Persons",
                newName: "TIN");

            migrationBuilder.AlterColumn<string>(
                name: "TIN",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(8)",
                oldNullable: true,
                oldDefaultValue: "ABC12345");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TIN",
                table: "Persons",
                newName: "TaxIdentificationNumbers");

            migrationBuilder.AlterColumn<string>(
                name: "TaxIdentificationNumbers",
                table: "Persons",
                type: "varchar(8)",
                nullable: true,
                defaultValue: "ABC12345",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryID", "CountryName" },
                values: new object[,]
                {
                    { new Guid("12e15727-d369-49a9-8b13-bc22e9362179"), "China" },
                    { new Guid("14629847-905a-4a0e-9abe-80b61655c5cb"), "Philippines" },
                    { new Guid("501c6d33-1bbe-45f1-8fbd-2275913c6218"), "China" },
                    { new Guid("56bf46a4-02b8-4693-a0f5-0a95e2218bdc"), "Thailand" },
                    { new Guid("8f30bedc-47dd-4286-8950-73d8a68e5d41"), "Palestinian Territory" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Personid", "Address", "CountryId", "DateOfbirth", "Email", "Gender", "PersonName", "ReceiveNewsLetters" },
                values: new object[,]
                {
                    { new Guid("012107df-862f-4f16-ba94-e5c16886f005"), "413 Sachtjen Way", null, null, "hmosco8@tripod.com", "Male", "Hansiain", true },
                    { new Guid("28d11936-9466-4a4b-b9c5-2f0a8e0cbde9"), "2 Warrior Avenue", null, null, "mconachya@va.gov", "Female", "Minta", true },
                    { new Guid("29339209-63f5-492f-8459-754943c74abf"), "57449 Brown Way", null, null, "mjarrell6@wisc.edu", "Male", "Maddy", true },
                    { new Guid("2a6d3738-9def-43ac-9279-0310edc7ceca"), "97570 Raven Circle", null, null, "mlingfoot5@netvibes.com", "Male", "Mitchael", false },
                    { new Guid("89e5f445-d89f-4e12-94e0-5ad5b235d704"), "50467 Holy Cross Crossing", null, null, "ttregona4@stumbleupon.com", "Gender", "Tani", false },
                    { new Guid("a3b9833b-8a4d-43e9-8690-61e08df81a9a"), "9334 Fremont Street", null, null, "vklussb@nationalgeographic.com", "Female", "Verene", true },
                    { new Guid("ac660a73-b0b7-4340-abc1-a914257a6189"), "4 Stuart Drive", null, null, "pretchford7@virginia.edu", "Female", "Pegeen", true },
                    { new Guid("c03bbe45-9aeb-4d24-99e0-4743016ffce9"), "4 Parkside Point", null, null, "mwebsdale0@people.com.cn", "Female", "Marguerite", false },
                    { new Guid("c3abddbd-cf50-41d2-b6c4-cc7d5a750928"), "6 Morningstar Circle", null, null, "ushears1@globo.com", "Female", "Ursa", false },
                    { new Guid("c6d50a47-f7e6-4482-8be0-4ddfc057fa6e"), "73 Heath Avenue", null, null, "fbowsher2@howstuffworks.com", "Male", "Franchot", true },
                    { new Guid("cb035f22-e7cf-4907-bd07-91cfee5240f3"), "484 Clarendon Court", null, null, "lwoodwing9@wix.com", "Male", "Lombard", false },
                    { new Guid("d15c6d9f-70b4-48c5-afd3-e71261f1a9be"), "83187 Merry Drive", null, null, "asarvar3@dropbox.com", "Male", "Angie", true }
                });

            migrationBuilder.AddCheckConstraint(
                name: "CHK_TIN",
                table: "Persons",
                sql: "len([TaxIdentificationNumbers]) = 8");
        }
    }
}
