using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Migrations
{
    /// <inheritdoc />
    public partial class inti3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitImage_Units_UnitId",
                table: "UnitImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UnitImage",
                table: "UnitImage");

            migrationBuilder.RenameTable(
                name: "UnitImage",
                newName: "UnitImages");

            migrationBuilder.RenameIndex(
                name: "IX_UnitImage_UnitId",
                table: "UnitImages",
                newName: "IX_UnitImages_UnitId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UnitImages",
                table: "UnitImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitImages_Units_UnitId",
                table: "UnitImages",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitImages_Units_UnitId",
                table: "UnitImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UnitImages",
                table: "UnitImages");

            migrationBuilder.RenameTable(
                name: "UnitImages",
                newName: "UnitImage");

            migrationBuilder.RenameIndex(
                name: "IX_UnitImages_UnitId",
                table: "UnitImage",
                newName: "IX_UnitImage_UnitId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UnitImage",
                table: "UnitImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitImage_Units_UnitId",
                table: "UnitImage",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
