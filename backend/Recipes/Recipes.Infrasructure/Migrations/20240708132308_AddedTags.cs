using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recipes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedTags : Migration
    {
        /// <inheritdoc />
        protected override void Up( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<int>( type: "int", nullable: false )
                        .Annotation( "SqlServer:Identity", "1, 1" ),
                    RecipeId = table.Column<int>( type: "int", nullable: false ),
                    Name = table.Column<string>( type: "nvarchar(50)", maxLength: 50, nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_Tag", x => x.Id );
                    table.ForeignKey(
                        name: "FK_Tag_Tag_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict );
                } );

            migrationBuilder.CreateIndex(
                name: "IX_Tag_RecipeId",
                table: "Tag",
                column: "RecipeId" );
        }

        /// <inheritdoc />
        protected override void Down( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropTable(
                name: "Tag" );
        }
    }
}
