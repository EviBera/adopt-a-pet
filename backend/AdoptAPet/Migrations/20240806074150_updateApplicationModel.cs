using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AdoptAPet.Migrations
{
    /// <inheritdoc />
    public partial class updateApplicationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "UserId",
                table: "Applications",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "291d8bfe-1e9d-430f-8998-49bdd695d160", null, "Rescue Team", "RESCUE TEAM" },
                    { "356b2695-4c30-4d2e-b147-123e88d8a872", null, "Admin", "ADMIN" },
                    { "5078686b-a154-4bb1-b4cf-3577900c1e52", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "291d8bfe-1e9d-430f-8998-49bdd695d160");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "356b2695-4c30-4d2e-b147-123e88d8a872");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5078686b-a154-4bb1-b4cf-3577900c1e52");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Applications",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

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
    }
}
