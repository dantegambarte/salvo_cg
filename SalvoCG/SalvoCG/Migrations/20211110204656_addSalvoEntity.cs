using Microsoft.EntityFrameworkCore.Migrations;

namespace SalvoCG.Migrations
{
    public partial class addSalvoEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Salvo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Turn = table.Column<int>(type: "int", nullable: false),
                    GamePlayerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salvo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Salvo_GamePlayers_GamePlayerId",
                        column: x => x.GamePlayerId,
                        principalTable: "GamePlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalvoLocations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalvoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalvoLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalvoLocations_Salvo_SalvoId",
                        column: x => x.SalvoId,
                        principalTable: "Salvo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Salvo_GamePlayerId",
                table: "Salvo",
                column: "GamePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_SalvoLocations_SalvoId",
                table: "SalvoLocations",
                column: "SalvoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalvoLocations");

            migrationBuilder.DropTable(
                name: "Salvo");
        }
    }
}
