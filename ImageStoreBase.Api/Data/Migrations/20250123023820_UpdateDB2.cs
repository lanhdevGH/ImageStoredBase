using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageStoreBase.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDB2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Functions_Functions_ParentId",
                table: "Functions");

            migrationBuilder.AddForeignKey(
                name: "FK_Functions_Functions_ParentId",
                table: "Functions",
                column: "ParentId",
                principalTable: "Functions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Functions_Functions_ParentId",
                table: "Functions");

            migrationBuilder.AddForeignKey(
                name: "FK_Functions_Functions_ParentId",
                table: "Functions",
                column: "ParentId",
                principalTable: "Functions",
                principalColumn: "Id");
        }
    }
}
