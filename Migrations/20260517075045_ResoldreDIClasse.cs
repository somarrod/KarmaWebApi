using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarmaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class ResoldreDIClasse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdAnyEscolar",
                table: "Grups");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdAnyEscolar",
                table: "Grups",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
