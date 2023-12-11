using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoroscopePredictorAPI.Migrations
{
    /// <inheritdoc />
    public partial class ReAddingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PredictionData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PersonalLife = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Profession = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Health = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Travel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Luck = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Emotions = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredictionData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HoroscopeData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SunSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PredictionDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PredictionId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoroscopeData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HoroscopeData_PredictionData_PredictionId",
                        column: x => x.PredictionId,
                        principalTable: "PredictionData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HoroscopeData_PredictionId",
                table: "HoroscopeData",
                column: "PredictionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HoroscopeData");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "PredictionData");
        }
    }
}
