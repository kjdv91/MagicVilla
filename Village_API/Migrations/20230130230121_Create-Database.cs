using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VillageAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Villas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmitionCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fee = table.Column<double>(type: "float", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    SquareMetters = table.Column<double>(type: "float", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Villas", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Capacity", "Details", "EmitionCreated", "Fee", "ImageUrl", "Name", "SquareMetters", "UpdateDate" },
                values: new object[] { 1, 4, "Casa en ciudadela privada", new DateTime(2023, 1, 30, 15, 1, 20, 695, DateTimeKind.Local).AddTicks(3012), 120.0, "", "Villa Real", 125.0, new DateTime(2023, 1, 30, 15, 1, 20, 695, DateTimeKind.Local).AddTicks(3113) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Villas");
        }
    }
}
