using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AdoptAPet.Migrations
{
    /// <inheritdoc />
    public partial class addRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
