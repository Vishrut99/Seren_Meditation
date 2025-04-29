using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEDIT.Migrations
{
    public partial class SimplifyProgressTrackings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the 'Notes' column from the 'ProgressTrackings' table.
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "ProgressTrackings");

            // Add the 'CompletedSessionsCount' column to the 'ProgressTrackings' table.
            migrationBuilder.AddColumn<int>(
                name: "CompletedSessionsCount",
                table: "ProgressTrackings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);  // Initialize with default value 0

            // Alter 'CompletedOn' to be nullable, if it was non-nullable.
            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedOn",
                table: "ProgressTrackings",
                type: "TEXT",
                nullable: true,  // Allow nulls for 'CompletedOn'
                oldClrType: typeof(DateTime),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Add back the 'Notes' column in case of rollback.
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "ProgressTrackings",
                type: "TEXT",
                nullable: true);

            // Drop the 'CompletedSessionsCount' column, as it was added in the Up method.
            migrationBuilder.DropColumn(
                name: "CompletedSessionsCount",
                table: "ProgressTrackings");

            // Alter 'CompletedOn' to be non-nullable again.
            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedOn",
                table: "ProgressTrackings",
                type: "TEXT",
                nullable: false,  // Make 'CompletedOn' non-nullable
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
