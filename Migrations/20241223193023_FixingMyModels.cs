using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IsmagiLearningApp.Migrations
{
    /// <inheritdoc />
    public partial class FixingMyModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Levels_DifficultyLevels_DifficultyId",
                table: "Levels");

            migrationBuilder.DropForeignKey(
                name: "FK_Levels_ProgrammingLanguages_ProgrammingLanguageId",
                table: "Levels");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgress_Levels_LevelId",
                table: "UserProgress");

            migrationBuilder.DropIndex(
                name: "IX_Levels_ProgrammingLanguageId",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Levels");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserProgress",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "DifficultyId",
                table: "Levels",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProgress_UserId_LevelId",
                table: "UserProgress",
                columns: new[] { "UserId", "LevelId" });

            migrationBuilder.CreateIndex(
                name: "IX_Levels_ProgrammingLanguageId_DifficultyId_OrderIndex",
                table: "Levels",
                columns: new[] { "ProgrammingLanguageId", "DifficultyId", "OrderIndex" });

            migrationBuilder.AddForeignKey(
                name: "FK_Levels_DifficultyLevels_DifficultyId",
                table: "Levels",
                column: "DifficultyId",
                principalTable: "DifficultyLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Levels_ProgrammingLanguages_ProgrammingLanguageId",
                table: "Levels",
                column: "ProgrammingLanguageId",
                principalTable: "ProgrammingLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgress_Levels_LevelId",
                table: "UserProgress",
                column: "LevelId",
                principalTable: "Levels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Levels_DifficultyLevels_DifficultyId",
                table: "Levels");

            migrationBuilder.DropForeignKey(
                name: "FK_Levels_ProgrammingLanguages_ProgrammingLanguageId",
                table: "Levels");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgress_Levels_LevelId",
                table: "UserProgress");

            migrationBuilder.DropIndex(
                name: "IX_UserProgress_UserId_LevelId",
                table: "UserProgress");

            migrationBuilder.DropIndex(
                name: "IX_Levels_ProgrammingLanguageId_DifficultyId_OrderIndex",
                table: "Levels");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserProgress",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "DifficultyId",
                table: "Levels",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Difficulty",
                table: "Levels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Levels_ProgrammingLanguageId",
                table: "Levels",
                column: "ProgrammingLanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Levels_DifficultyLevels_DifficultyId",
                table: "Levels",
                column: "DifficultyId",
                principalTable: "DifficultyLevels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Levels_ProgrammingLanguages_ProgrammingLanguageId",
                table: "Levels",
                column: "ProgrammingLanguageId",
                principalTable: "ProgrammingLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgress_Levels_LevelId",
                table: "UserProgress",
                column: "LevelId",
                principalTable: "Levels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
