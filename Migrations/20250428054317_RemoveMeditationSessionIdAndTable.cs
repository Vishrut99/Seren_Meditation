using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEDIT.Migrations
{
    public partial class RemoveMeditationSessionIdAndTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // leave empty because MeditationSessionId column and MeditationSessions table are already manually dropped
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MeditationSessionId",
                table: "ProgressTrackings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MeditationSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeditationSessions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProgressTrackings_MeditationSessionId",
                table: "ProgressTrackings",
                column: "MeditationSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProgressTrackings_MeditationSessions_MeditationSessionId",
                table: "ProgressTrackings",
                column: "MeditationSessionId",
                principalTable: "MeditationSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
