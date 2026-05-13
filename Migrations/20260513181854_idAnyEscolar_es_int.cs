using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarmaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class idAnyEscolar_es_int : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_TipusCategories_TipusCategoriaIdTipusCategoria",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_TipusCategoriaIdTipusCategoria",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "TipusCategoriaIdTipusCategoria",
                table: "Categories");

            migrationBuilder.AlterColumn<int>(
                name: "IdAnyEscolar",
                table: "Privilegis",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_IdTipusCategoria",
                table: "Categories",
                column: "IdTipusCategoria");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_TipusCategories_IdTipusCategoria",
                table: "Categories",
                column: "IdTipusCategoria",
                principalTable: "TipusCategories",
                principalColumn: "IdTipusCategoria",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_TipusCategories_IdTipusCategoria",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_IdTipusCategoria",
                table: "Categories");

            migrationBuilder.AlterColumn<long>(
                name: "IdAnyEscolar",
                table: "Privilegis",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "TipusCategoriaIdTipusCategoria",
                table: "Categories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_TipusCategoriaIdTipusCategoria",
                table: "Categories",
                column: "TipusCategoriaIdTipusCategoria");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_TipusCategories_TipusCategoriaIdTipusCategoria",
                table: "Categories",
                column: "TipusCategoriaIdTipusCategoria",
                principalTable: "TipusCategories",
                principalColumn: "IdTipusCategoria",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
