using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebAPI.Migrations
{
    public partial class seedBusinessUnitTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BusinessUnits",
                columns: new[] { "BusinessUnitId", "BusinessUnitName" },
                values: new object[] { 1, "UKHO" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BusinessUnits",
                keyColumn: "BusinessUnitId",
                keyValue: 1);
        }
    }
}
