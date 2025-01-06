using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageStoreBase.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateDB2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Commands",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.CreateIndex(
                name: "IX_Functions_ParentId",
                table: "Functions",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Functions_Functions_ParentId",
                table: "Functions",
                column: "ParentId",
                principalTable: "Functions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Functions_Functions_ParentId",
                table: "Functions");

            migrationBuilder.DropIndex(
                name: "IX_Functions_ParentId",
                table: "Functions");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Commands",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
