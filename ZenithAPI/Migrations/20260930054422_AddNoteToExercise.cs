using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZenithAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNoteToExercise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Exercises",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Exercises");
        }
    }
}
