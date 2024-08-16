using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recipes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAndAuthorizationLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Recipe",
                type: "int",
                nullable: false,
                defaultValue: 0 );

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>( type: "int", nullable: false )
                        .Annotation( "SqlServer:Identity", "1, 1" ),
                    Name = table.Column<string>( type: "nvarchar(50)", maxLength: 50, nullable: false ),
                    Description = table.Column<string>( type: "nvarchar(200)", maxLength: 200, nullable: false ),
                    Login = table.Column<string>( type: "nvarchar(50)", maxLength: 50, nullable: false ),
                    PasswordHash = table.Column<string>( type: "nvarchar(250)", maxLength: 250, nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_User", x => x.Id );
                } );

            migrationBuilder.CreateTable(
                name: "UserAuthorizationToken",
                columns: table => new
                {
                    UserId = table.Column<int>( type: "int", nullable: false ),
                    RefreshToken = table.Column<string>( type: "nvarchar(40)", maxLength: 40, nullable: false ),
                    ExpiryDate = table.Column<DateTime>( type: "datetime2", nullable: false )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_UserAuthorizationToken", x => x.UserId );
                    table.ForeignKey(
                        name: "FK_UserAuthorizationToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade );
                } );

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_UserId",
                table: "Recipe",
                column: "UserId" );

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_User_UserId",
                table: "Recipe",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade );
        }

        /// <inheritdoc />
        protected override void Down( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_User_UserId",
                table: "Recipe" );

            migrationBuilder.DropTable(
                name: "UserAuthorizationToken" );

            migrationBuilder.DropTable(
                name: "User" );

            migrationBuilder.DropIndex(
                name: "IX_Recipe_UserId",
                table: "Recipe" );

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Recipe" );
        }
    }
}
