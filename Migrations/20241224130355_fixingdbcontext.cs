using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IsmagiLearningApp.Migrations
{
    /// <inheritdoc />
    public partial class fixingdbcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Levels_ProgrammingLanguages_ProgrammingLanguageId",
                table: "Levels");

            migrationBuilder.DropForeignKey(
                name: "FK_Levels_DifficultyLevels_DifficultyId",
                table: "Levels");

            migrationBuilder.AddForeignKey(
                name: "FK_Levels_ProgrammingLanguages_ProgrammingLanguageId",
                table: "Levels",
                column: "ProgrammingLanguageId",
                principalTable: "ProgrammingLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Levels_DifficultyLevels_DifficultyId",
                table: "Levels",
                column: "DifficultyId",
                principalTable: "DifficultyLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Levels_ProgrammingLanguages_ProgrammingLanguageId",
                table: "Levels");

            migrationBuilder.DropForeignKey(
                name: "FK_Levels_DifficultyLevels_DifficultyId",
                table: "Levels");

            migrationBuilder.AddForeignKey(
                name: "FK_Levels_ProgrammingLanguages_ProgrammingLanguageId",
                table: "Levels",
                column: "ProgrammingLanguageId",
                principalTable: "ProgrammingLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Levels_DifficultyLevels_DifficultyId",
                table: "Levels",
                column: "DifficultyId",
                principalTable: "DifficultyLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
