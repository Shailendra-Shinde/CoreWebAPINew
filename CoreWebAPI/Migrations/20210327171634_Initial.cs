using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebAPI.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Acl",
                columns: table => new
                {
                    AclId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReadUsers = table.Column<string>(nullable: true),
                    ReadGroups = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acl", x => x.AclId);
                });

            migrationBuilder.CreateTable(
                name: "BatchDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BusinessUnit = table.Column<string>(nullable: true),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    AclId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BatchDetails_Acl_AclId",
                        column: x => x.AclId,
                        principalTable: "Acl",
                        principalColumn: "AclId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attribute",
                columns: table => new
                {
                    AttributeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    BatchId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attribute", x => x.AttributeId);
                    table.ForeignKey(
                        name: "FK_Attribute_BatchDetails_BatchId",
                        column: x => x.BatchId,
                        principalTable: "BatchDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attribute_BatchId",
                table: "Attribute",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchDetails_AclId",
                table: "BatchDetails",
                column: "AclId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attribute");

            migrationBuilder.DropTable(
                name: "BatchDetails");

            migrationBuilder.DropTable(
                name: "Acl");
        }
    }
}
