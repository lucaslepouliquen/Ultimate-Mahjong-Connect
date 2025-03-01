using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UltimateMahjongConnect.Database.Net.Migrations
{
    /// <inheritdoc />
    public partial class RenameGamerIdToId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GamerId",
                table: "Gamers",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Gamers",
                newName: "GamerId");
        }
    }
}
