using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IsmagiLearningApp.Migrations
{
    /// <inheritdoc />
    public partial class AddLevelsToDifficulty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DifficultyId",
                table: "Levels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Levels_DifficultyId",
                table: "Levels",
                column: "DifficultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Levels_DifficultyLevels_DifficultyId",
                table: "Levels",
                column: "DifficultyId",
                principalTable: "DifficultyLevels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Levels_DifficultyLevels_DifficultyId",
                table: "Levels");

            migrationBuilder.DropIndex(
                name: "IX_Levels_DifficultyId",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "DifficultyId",
                table: "Levels");
        }
    }
}
