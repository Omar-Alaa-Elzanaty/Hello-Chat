using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hello.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "12253d50-3d46-4471-8d01-1cdd33d22c21");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e4a6d231-074a-4151-b188-475e9fb43c52", null, "GroupAdmin", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "e4a6d231-074a-4151-b188-475e9fb43c52");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "12253d50-3d46-4471-8d01-1cdd33d22c21", null, "GroupAdmin", null });
        }
    }
}
