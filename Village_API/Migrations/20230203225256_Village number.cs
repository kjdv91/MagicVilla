using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VillageAPI.Migrations
{
    /// <inheritdoc />
    public partial class Villagenumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VillageNumbers",
                columns: table => new
                {
                    VillageNro = table.Column<int>(type: "int", nullable: false),
                    VillaId = table.Column<int>(type: "int", nullable: false),
                    SpecialDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VillageNumbers", x => x.VillageNro);
                    table.ForeignKey(
                        name: "FK_VillageNumbers_Villas_VillaId",
                        column: x => x.VillaId,
                        principalTable: "Villas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EmitionCreated", "UpdateDate" },
                values: new object[] { new DateTime(2023, 2, 3, 14, 52, 56, 18, DateTimeKind.Local).AddTicks(7177), new DateTime(2023, 2, 3, 14, 52, 56, 18, DateTimeKind.Local).AddTicks(7274) });

            migrationBuilder.CreateIndex(
                name: "IX_VillageNumbers_VillaId",
                table: "VillageNumbers",
                column: "VillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VillageNumbers");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EmitionCreated", "UpdateDate" },
                values: new object[] { new DateTime(2023, 1, 30, 15, 1, 20, 695, DateTimeKind.Local).AddTicks(3012), new DateTime(2023, 1, 30, 15, 1, 20, 695, DateTimeKind.Local).AddTicks(3113) });
        }
    }
}
