using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recipes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedTags4 : Migration
    {
        /// <inheritdoc />
        protected override void Up( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Recipe_RecipeId1",
                table: "Tag" );

            migrationBuilder.DropIndex(
                name: "IX_Tag_RecipeId1",
                table: "Tag" );

            migrationBuilder.DropColumn(
                name: "RecipeId1",
                table: "Tag" );
        }

        /// <inheritdoc />
        protected override void Down( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.AddColumn<int>(
                name: "RecipeId1",
                table: "Tag",
                type: "int",
                nullable: true );

            migrationBuilder.CreateIndex(
                name: "IX_Tag_RecipeId1",
                table: "Tag",
                column: "RecipeId1" );

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Recipe_RecipeId1",
                table: "Tag",
                column: "RecipeId1",
                principalTable: "Recipe",
                principalColumn: "Id" );
        }
    }
}
