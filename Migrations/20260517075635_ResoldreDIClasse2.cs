using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarmaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class ResoldreDIClasse2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alumnes_Classes_IdAnyEscolar_IdClasse",
                table: "Alumnes");

            migrationBuilder.DropForeignKey(
                name: "FK_Grups_Classes_ClasseIdAnyEscolar_ClasseIdClasse",
                table: "Grups");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfessorsDeClasse_Classes_ClasseIdAnyEscolar_ClasseIdClasse",
                table: "ProfessorsDeClasse");

            migrationBuilder.DropIndex(
                name: "IX_ProfessorsDeClasse_ClasseIdAnyEscolar_ClasseIdClasse",
                table: "ProfessorsDeClasse");

            migrationBuilder.DropIndex(
                name: "IX_Grups_ClasseIdAnyEscolar_ClasseIdClasse",
                table: "Grups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Classes",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Alumnes_IdAnyEscolar_IdClasse",
                table: "Alumnes");

            migrationBuilder.DropColumn(
                name: "ClasseIdAnyEscolar",
                table: "ProfessorsDeClasse");

            migrationBuilder.DropColumn(
                name: "ClasseIdAnyEscolar",
                table: "Grups");

            migrationBuilder.AlterColumn<long>(
                name: "ClasseIdClasse",
                table: "ProfessorsDeClasse",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ClasseIdClasse",
                table: "Grups",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "ClasseIdClasse",
                table: "Alumnes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Classes",
                table: "Classes",
                column: "IdClasse");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorsDeClasse_ClasseIdClasse",
                table: "ProfessorsDeClasse",
                column: "ClasseIdClasse");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorsDeClasse_IdClasse",
                table: "ProfessorsDeClasse",
                column: "IdClasse");

            migrationBuilder.CreateIndex(
                name: "IX_Grups_ClasseIdClasse",
                table: "Grups",
                column: "ClasseIdClasse");

            migrationBuilder.CreateIndex(
                name: "IX_Grups_IdClasse",
                table: "Grups",
                column: "IdClasse");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_IdAnyEscolar",
                table: "Classes",
                column: "IdAnyEscolar");

            migrationBuilder.CreateIndex(
                name: "IX_Alumnes_ClasseIdClasse",
                table: "Alumnes",
                column: "ClasseIdClasse");

            migrationBuilder.CreateIndex(
                name: "IX_Alumnes_IdClasse",
                table: "Alumnes",
                column: "IdClasse");

            migrationBuilder.AddForeignKey(
                name: "FK_Alumnes_Classes_ClasseIdClasse",
                table: "Alumnes",
                column: "ClasseIdClasse",
                principalTable: "Classes",
                principalColumn: "IdClasse");

            migrationBuilder.AddForeignKey(
                name: "FK_Alumnes_Classes_IdClasse",
                table: "Alumnes",
                column: "IdClasse",
                principalTable: "Classes",
                principalColumn: "IdClasse");

            migrationBuilder.AddForeignKey(
                name: "FK_Grups_Classes_ClasseIdClasse",
                table: "Grups",
                column: "ClasseIdClasse",
                principalTable: "Classes",
                principalColumn: "IdClasse");

            migrationBuilder.AddForeignKey(
                name: "FK_Grups_Classes_IdClasse",
                table: "Grups",
                column: "IdClasse",
                principalTable: "Classes",
                principalColumn: "IdClasse",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessorsDeClasse_Classes_ClasseIdClasse",
                table: "ProfessorsDeClasse",
                column: "ClasseIdClasse",
                principalTable: "Classes",
                principalColumn: "IdClasse");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessorsDeClasse_Classes_IdClasse",
                table: "ProfessorsDeClasse",
                column: "IdClasse",
                principalTable: "Classes",
                principalColumn: "IdClasse",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alumnes_Classes_ClasseIdClasse",
                table: "Alumnes");

            migrationBuilder.DropForeignKey(
                name: "FK_Alumnes_Classes_IdClasse",
                table: "Alumnes");

            migrationBuilder.DropForeignKey(
                name: "FK_Grups_Classes_ClasseIdClasse",
                table: "Grups");

            migrationBuilder.DropForeignKey(
                name: "FK_Grups_Classes_IdClasse",
                table: "Grups");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfessorsDeClasse_Classes_ClasseIdClasse",
                table: "ProfessorsDeClasse");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfessorsDeClasse_Classes_IdClasse",
                table: "ProfessorsDeClasse");

            migrationBuilder.DropIndex(
                name: "IX_ProfessorsDeClasse_ClasseIdClasse",
                table: "ProfessorsDeClasse");

            migrationBuilder.DropIndex(
                name: "IX_ProfessorsDeClasse_IdClasse",
                table: "ProfessorsDeClasse");

            migrationBuilder.DropIndex(
                name: "IX_Grups_ClasseIdClasse",
                table: "Grups");

            migrationBuilder.DropIndex(
                name: "IX_Grups_IdClasse",
                table: "Grups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Classes",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_IdAnyEscolar",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Alumnes_ClasseIdClasse",
                table: "Alumnes");

            migrationBuilder.DropIndex(
                name: "IX_Alumnes_IdClasse",
                table: "Alumnes");

            migrationBuilder.DropColumn(
                name: "ClasseIdClasse",
                table: "Alumnes");

            migrationBuilder.AlterColumn<long>(
                name: "ClasseIdClasse",
                table: "ProfessorsDeClasse",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClasseIdAnyEscolar",
                table: "ProfessorsDeClasse",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<long>(
                name: "ClasseIdClasse",
                table: "Grups",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClasseIdAnyEscolar",
                table: "Grups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Classes",
                table: "Classes",
                columns: new[] { "IdAnyEscolar", "IdClasse" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorsDeClasse_ClasseIdAnyEscolar_ClasseIdClasse",
                table: "ProfessorsDeClasse",
                columns: new[] { "ClasseIdAnyEscolar", "ClasseIdClasse" });

            migrationBuilder.CreateIndex(
                name: "IX_Grups_ClasseIdAnyEscolar_ClasseIdClasse",
                table: "Grups",
                columns: new[] { "ClasseIdAnyEscolar", "ClasseIdClasse" });

            migrationBuilder.CreateIndex(
                name: "IX_Alumnes_IdAnyEscolar_IdClasse",
                table: "Alumnes",
                columns: new[] { "IdAnyEscolar", "IdClasse" });

            migrationBuilder.AddForeignKey(
                name: "FK_Alumnes_Classes_IdAnyEscolar_IdClasse",
                table: "Alumnes",
                columns: new[] { "IdAnyEscolar", "IdClasse" },
                principalTable: "Classes",
                principalColumns: new[] { "IdAnyEscolar", "IdClasse" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grups_Classes_ClasseIdAnyEscolar_ClasseIdClasse",
                table: "Grups",
                columns: new[] { "ClasseIdAnyEscolar", "ClasseIdClasse" },
                principalTable: "Classes",
                principalColumns: new[] { "IdAnyEscolar", "IdClasse" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessorsDeClasse_Classes_ClasseIdAnyEscolar_ClasseIdClasse",
                table: "ProfessorsDeClasse",
                columns: new[] { "ClasseIdAnyEscolar", "ClasseIdClasse" },
                principalTable: "Classes",
                principalColumns: new[] { "IdAnyEscolar", "IdClasse" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
