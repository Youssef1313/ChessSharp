using Microsoft.EntityFrameworkCore.Migrations;

namespace ChessSharpWeb.Data.Migrations;

public partial class InitialMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Games",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                WhitePlayerId = table.Column<string>(nullable: true),
                BlackPlayerId = table.Column<string>(nullable: true),
                GameBoardJson = table.Column<string>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Games", x => x.Id);
                table.ForeignKey(
                    name: "FK_Games_AspNetUsers_BlackPlayerId",
                    column: x => x.BlackPlayerId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Games_AspNetUsers_WhitePlayerId",
                    column: x => x.WhitePlayerId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Games_BlackPlayerId",
            table: "Games",
            column: "BlackPlayerId");

        migrationBuilder.CreateIndex(
            name: "IX_Games_WhitePlayerId",
            table: "Games",
            column: "WhitePlayerId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Games");
    }
}
