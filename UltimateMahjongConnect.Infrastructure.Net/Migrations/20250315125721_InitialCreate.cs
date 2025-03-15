using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UltimateMahjongConnect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gamers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Pseudonyme = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    BankDetails = table.Column<string>(type: "TEXT", nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gamers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Gamers",
                columns: new[] { "Id", "Age", "BankDetails", "Email", "Password", "Pseudonyme", "Score" },
                values: new object[,]
                {
                    { 1, 25, "FR76 1234 5678 9012 3456 7890 123", "testgamer@example.com", "SecurePassword123!", "TestGamer123", 1000 },
                    { 2, 18, "FR76 7890 5678 9012 3456 1234 123", "darkzelios@example.com", "DarkZelios123!", "DarkZelios", 10000 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gamers");
        }
    }
}
