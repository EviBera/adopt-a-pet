using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AdoptAPet.Migrations
{
    /// <inheritdoc />
    public partial class extendPictureLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4553e464-f69e-45df-86b9-d6583eebcc9d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ab19b7fe-b11b-4225-90da-834a89a8081e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e7f7d65d-bc88-41fd-9000-1563cfea1e93");

            migrationBuilder.AlterColumn<string>(
                name: "PictureLink",
                table: "Pets",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2201d47f-6d7b-43d0-b055-f6fbe6e07f65", null, "User", "USER" },
                    { "a4065081-7b46-4658-b2d0-af83042dd59b", null, "Rescue Team", "RESCUE TEAM" },
                    { "c971a65c-3bff-47cf-8c91-f3d9cf4953c0", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2201d47f-6d7b-43d0-b055-f6fbe6e07f65");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a4065081-7b46-4658-b2d0-af83042dd59b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c971a65c-3bff-47cf-8c91-f3d9cf4953c0");

            migrationBuilder.AlterColumn<string>(
                name: "PictureLink",
                table: "Pets",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4553e464-f69e-45df-86b9-d6583eebcc9d", null, "Admin", "ADMIN" },
                    { "ab19b7fe-b11b-4225-90da-834a89a8081e", null, "Rescue Team", "RESCUE TEAM" },
                    { "e7f7d65d-bc88-41fd-9000-1563cfea1e93", null, "User", "USER" }
                });
        }
    }
}
