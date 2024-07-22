using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recipes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialTables : Migration
    {
        /// <inheritdoc />
        protected override void Up( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.CreateTable(
                name: "Recipe",
                columns: table => new
                {
                    Id = table.Column<int>( type: "int", nullable: false )
                        .Annotation( "SqlServer:Identity", "1, 1" ),
                    Name = table.Column<string>( type: "nvarchar(100)", maxLength: 100, nullable: false ),
                    Description = table.Column<string>( type: "nvarchar(150)", maxLength: 150, nullable: false ),
                    CookTime = table.Column<int>( type: "int", nullable: false ),
                    PortionCount = table.Column<int>( type: "int", nullable: false ),
                    ImageUrl = table.Column<string>( type: "nvarchar(max)", nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_Recipe", x => x.Id );
                } );

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<int>( type: "int", nullable: false )
                        .Annotation( "SqlServer:Identity", "1, 1" ),
                    Name = table.Column<string>( type: "nvarchar(50)", maxLength: 50, nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_Tag", x => x.Id );
                } );

            migrationBuilder.CreateTable(
                name: "Ingredient",
                columns: table => new
                {
                    Id = table.Column<int>( type: "int", nullable: false )
                        .Annotation( "SqlServer:Identity", "1, 1" ),
                    RecipeId = table.Column<int>( type: "int", nullable: false ),
                    Title = table.Column<string>( type: "nvarchar(50)", maxLength: 50, nullable: false ),
                    Description = table.Column<string>( type: "nvarchar(250)", maxLength: 250, nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_Ingredient", x => x.Id );
                    table.ForeignKey(
                        name: "FK_Ingredient_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict );
                } );

            migrationBuilder.CreateTable(
                name: "Step",
                columns: table => new
                {
                    Id = table.Column<int>( type: "int", nullable: false )
                        .Annotation( "SqlServer:Identity", "1, 1" ),
                    RecipeId = table.Column<int>( type: "int", nullable: false ),
                    StepNumber = table.Column<int>( type: "int", nullable: false ),
                    StepDescription = table.Column<string>( type: "nvarchar(250)", maxLength: 250, nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_Step", x => x.Id );
                    table.ForeignKey(
                        name: "FK_Step_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade );
                } );

            migrationBuilder.CreateTable(
                name: "RecipeTag",
                columns: table => new
                {
                    RecipeId = table.Column<int>( type: "int", nullable: false ),
                    TagId = table.Column<int>( type: "int", nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_RecipeTag", x => new { x.RecipeId, x.TagId } );
                    table.ForeignKey(
                        name: "FK_RecipeTag_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade );
                    table.ForeignKey(
                        name: "FK_RecipeTag_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade );
                } );

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_RecipeId",
                table: "Ingredient",
                column: "RecipeId" );

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTag_TagId",
                table: "RecipeTag",
                column: "TagId" );

            migrationBuilder.CreateIndex(
                name: "IX_Step_RecipeId",
                table: "Step",
                column: "RecipeId" );
        }

        /// <inheritdoc />
        protected override void Down( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropTable(
                name: "Ingredient" );

            migrationBuilder.DropTable(
                name: "RecipeTag" );

            migrationBuilder.DropTable(
                name: "Step" );

            migrationBuilder.DropTable(
                name: "Tag" );

            migrationBuilder.DropTable(
                name: "Recipe" );
        }
    }
}
