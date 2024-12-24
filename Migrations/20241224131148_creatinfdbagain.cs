using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IsmagiLearningApp.Migrations
{
    /// <inheritdoc />
    public partial class creatinfdbagain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Levels_DifficultyLevels_DifficultyId",
                table: "Levels");

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
                name: "FK_Levels_DifficultyLevels_DifficultyId",
                table: "Levels");

            migrationBuilder.AddForeignKey(
                name: "FK_Levels_DifficultyLevels_DifficultyId",
                table: "Levels",
                column: "DifficultyId",
                principalTable: "DifficultyLevels",
                principalColumn: "Id");
        }
    }
}
