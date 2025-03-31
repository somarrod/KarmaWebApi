using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarmaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AnyEscolar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnyEscolar",
                columns: table => new
                {
                    id_anyEscolar = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dataIniciCurs = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dataFiCurs = table.Column<DateTime>(type: "datetime2", nullable: false),
                    actiu = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyEscolar", x => x.id_anyEscolar);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnyEscolar");
        }
    }
}
