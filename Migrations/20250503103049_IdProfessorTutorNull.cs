using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarmaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class IdProfessorTutorNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grup_Professor_IdProfessorTutor",
                table: "Grup");

            migrationBuilder.AlterColumn<string>(
                name: "IdProfessorTutor",
                table: "Grup",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Grup_Professor_IdProfessorTutor",
                table: "Grup",
                column: "IdProfessorTutor",
                principalTable: "Professor",
                principalColumn: "IdProfessor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grup_Professor_IdProfessorTutor",
                table: "Grup");

            migrationBuilder.AlterColumn<string>(
                name: "IdProfessorTutor",
                table: "Grup",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Grup_Professor_IdProfessorTutor",
                table: "Grup",
                column: "IdProfessorTutor",
                principalTable: "Professor",
                principalColumn: "IdProfessor",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
