using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarmaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAlumneModelv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alumnes_Grups_IdGrup",
                table: "Alumnes");

            migrationBuilder.AlterColumn<long>(
                name: "IdGrup",
                table: "Alumnes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Alumnes_Grups_IdGrup",
                table: "Alumnes",
                column: "IdGrup",
                principalTable: "Grups",
                principalColumn: "IdGrup");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alumnes_Grups_IdGrup",
                table: "Alumnes");

            migrationBuilder.AlterColumn<long>(
                name: "IdGrup",
                table: "Alumnes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Alumnes_Grups_IdGrup",
                table: "Alumnes",
                column: "IdGrup",
                principalTable: "Grups",
                principalColumn: "IdGrup",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
