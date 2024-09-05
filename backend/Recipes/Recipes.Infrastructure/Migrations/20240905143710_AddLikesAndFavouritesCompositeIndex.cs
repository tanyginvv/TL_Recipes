using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recipes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLikesAndFavouritesCompositeIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_User_AuthorId",
                table: "Recipe");

            migrationBuilder.DropIndex(
                name: "IX_Like_RecipeId",
                table: "Like");

            migrationBuilder.DropIndex(
                name: "IX_Favourite_RecipeId",
                table: "Favourite");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_RecipeId_UserId",
                table: "Like",
                columns: new[] { "RecipeId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favourites_RecipeId_UserId",
                table: "Favourite",
                columns: new[] { "RecipeId", "UserId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_User_AuthorId",
                table: "Recipe",
                column: "AuthorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_User_AuthorId",
                table: "Recipe");

            migrationBuilder.DropIndex(
                name: "IX_Likes_RecipeId_UserId",
                table: "Like");

            migrationBuilder.DropIndex(
                name: "IX_Favourites_RecipeId_UserId",
                table: "Favourite");

            migrationBuilder.CreateIndex(
                name: "IX_Like_RecipeId",
                table: "Like",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Favourite_RecipeId",
                table: "Favourite",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_User_AuthorId",
                table: "Recipe",
                column: "AuthorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
