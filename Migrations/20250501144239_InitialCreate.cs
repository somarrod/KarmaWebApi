using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarmaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alumnes",
                columns: table => new
                {
                    NIA = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cognoms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Actiu = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alumnes", x => x.NIA);
                });

            migrationBuilder.CreateTable(
                name: "AnysEscolar",
                columns: table => new
                {
                    IdAnyEscolar = table.Column<int>(type: "int", nullable: false),
                    DataIniciCurs = table.Column<DateOnly>(type: "date", nullable: false),
                    DataFiCurs = table.Column<DateOnly>(type: "date", nullable: false),
                    Actiu = table.Column<bool>(type: "bit", nullable: false),
                    DiesPeriode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnysEscolar", x => x.IdAnyEscolar);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false),
                    Descripcio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.IdCategoria);
                });

            migrationBuilder.CreateTable(
                name: "Materies",
                columns: table => new
                {
                    IdMateria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materies", x => x.IdMateria);
                });

            migrationBuilder.CreateTable(
                name: "Professors",
                columns: table => new
                {
                    IdProfessor = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cognoms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Actiu = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professors", x => x.IdProfessor);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracionsKarma",
                columns: table => new
                {
                    IdConfiguracioKarma = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAnyEscolar = table.Column<int>(type: "int", nullable: false),
                    KarmaMinim = table.Column<int>(type: "int", nullable: false),
                    KarmaMaxim = table.Column<int>(type: "int", nullable: false),
                    ColorNivell = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NivellPrivilegis = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionsKarma", x => x.IdConfiguracioKarma);
                    table.ForeignKey(
                        name: "FK_ConfiguracionsKarma_AnysEscolar_IdAnyEscolar",
                        column: x => x.IdAnyEscolar,
                        principalTable: "AnysEscolar",
                        principalColumn: "IdAnyEscolar",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Periodes",
                columns: table => new
                {
                    IdPeriodo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataInici = table.Column<DateOnly>(type: "date", nullable: false),
                    DataFi = table.Column<DateOnly>(type: "date", nullable: false),
                    IdAnyEscolar = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periodes", x => x.IdPeriodo);
                    table.ForeignKey(
                        name: "FK_Periodes_AnysEscolar_IdAnyEscolar",
                        column: x => x.IdAnyEscolar,
                        principalTable: "AnysEscolar",
                        principalColumn: "IdAnyEscolar",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Privilegis",
                columns: table => new
                {
                    IdPrivilegi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nivell = table.Column<int>(type: "int", nullable: false),
                    Descripcio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EsIndividualGrup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdAnyEscolar = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privilegis", x => x.IdPrivilegi);
                    table.ForeignKey(
                        name: "FK_Privilegis_AnysEscolar_IdAnyEscolar",
                        column: x => x.IdAnyEscolar,
                        principalTable: "AnysEscolar",
                        principalColumn: "IdAnyEscolar",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grups",
                columns: table => new
                {
                    IdAnyEscolar = table.Column<int>(type: "int", nullable: false),
                    IdGrup = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Descripcio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KarmaBase = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdProfessorTutor = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grups", x => new { x.IdAnyEscolar, x.IdGrup });
                    table.ForeignKey(
                        name: "FK_Grups_AnysEscolar_IdAnyEscolar",
                        column: x => x.IdAnyEscolar,
                        principalTable: "AnysEscolar",
                        principalColumn: "IdAnyEscolar",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grups_Professors_IdProfessorTutor",
                        column: x => x.IdProfessorTutor,
                        principalTable: "Professors",
                        principalColumn: "IdProfessor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlumnesEnGrup",
                columns: table => new
                {
                    IdAlumneEnGrup = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NIA = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdAnyEscolar = table.Column<int>(type: "int", nullable: false),
                    IdGrup = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlumnesEnGrup", x => x.IdAlumneEnGrup);
                    table.ForeignKey(
                        name: "FK_AlumnesEnGrup_Alumnes_NIA",
                        column: x => x.NIA,
                        principalTable: "Alumnes",
                        principalColumn: "NIA",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlumnesEnGrup_AnysEscolar_IdAnyEscolar",
                        column: x => x.IdAnyEscolar,
                        principalTable: "AnysEscolar",
                        principalColumn: "IdAnyEscolar");
                    table.ForeignKey(
                        name: "FK_AlumnesEnGrup_Grups_IdAnyEscolar_IdGrup",
                        columns: x => new { x.IdAnyEscolar, x.IdGrup },
                        principalTable: "Grups",
                        principalColumns: new[] { "IdAnyEscolar", "IdGrup" });
                });

            migrationBuilder.CreateTable(
                name: "ProfessorsDeGrup",
                columns: table => new
                {
                    IdProfessorDeGrup = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdProfessor = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdMateria = table.Column<int>(type: "int", nullable: false),
                    IdAnyEscolar = table.Column<int>(type: "int", nullable: false),
                    IdGrup = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessorsDeGrup", x => x.IdProfessorDeGrup);
                    table.ForeignKey(
                        name: "FK_ProfessorsDeGrup_AnysEscolar_IdAnyEscolar",
                        column: x => x.IdAnyEscolar,
                        principalTable: "AnysEscolar",
                        principalColumn: "IdAnyEscolar",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfessorsDeGrup_Grups_IdAnyEscolar_IdGrup",
                        columns: x => new { x.IdAnyEscolar, x.IdGrup },
                        principalTable: "Grups",
                        principalColumns: new[] { "IdAnyEscolar", "IdGrup" });
                    table.ForeignKey(
                        name: "FK_ProfessorsDeGrup_Materies_IdMateria",
                        column: x => x.IdMateria,
                        principalTable: "Materies",
                        principalColumn: "IdMateria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfessorsDeGrup_Professors_IdProfessor",
                        column: x => x.IdProfessor,
                        principalTable: "Professors",
                        principalColumn: "IdProfessor");
                });

            migrationBuilder.CreateTable(
                name: "PrivilegisAssignats",
                columns: table => new
                {
                    IdPrivilegiAssignat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPrivilegi = table.Column<int>(type: "int", nullable: false),
                    IdAlumneEnGrup = table.Column<int>(type: "int", nullable: false),
                    Nivell = table.Column<int>(type: "int", nullable: false),
                    Descripcio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EsIndividualGrup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataAssignacio = table.Column<DateOnly>(type: "date", nullable: false),
                    DataExecucio = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivilegisAssignats", x => x.IdPrivilegiAssignat);
                    table.ForeignKey(
                        name: "FK_PrivilegisAssignats_AlumnesEnGrup_IdAlumneEnGrup",
                        column: x => x.IdAlumneEnGrup,
                        principalTable: "AlumnesEnGrup",
                        principalColumn: "IdAlumneEnGrup",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrivilegisAssignats_Privilegis_IdPrivilegi",
                        column: x => x.IdPrivilegi,
                        principalTable: "Privilegis",
                        principalColumn: "IdPrivilegi",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Puntuacions",
                columns: table => new
                {
                    IdPuntuacio = table.Column<int>(type: "int", nullable: false),
                    DataEntrada = table.Column<DateOnly>(type: "date", nullable: false),
                    Punts = table.Column<int>(type: "int", nullable: false),
                    Motiu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdProfessorCreacio = table.Column<int>(type: "int", nullable: false),
                    IdPeriode = table.Column<int>(type: "int", nullable: false),
                    IdCategoria = table.Column<int>(type: "int", nullable: false),
                    IdAlumneEnGrup = table.Column<int>(type: "int", nullable: false),
                    ProfessorCreacioIdProfessor = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puntuacions", x => x.IdPuntuacio);
                    table.ForeignKey(
                        name: "FK_Puntuacions_AlumnesEnGrup_IdAlumneEnGrup",
                        column: x => x.IdAlumneEnGrup,
                        principalTable: "AlumnesEnGrup",
                        principalColumn: "IdAlumneEnGrup",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Puntuacions_Categories_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "Categories",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Puntuacions_Periodes_IdPeriode",
                        column: x => x.IdPeriode,
                        principalTable: "Periodes",
                        principalColumn: "IdPeriodo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Puntuacions_Professors_ProfessorCreacioIdProfessor",
                        column: x => x.ProfessorCreacioIdProfessor,
                        principalTable: "Professors",
                        principalColumn: "IdProfessor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlumnesEnGrup_IdAnyEscolar_IdGrup",
                table: "AlumnesEnGrup",
                columns: new[] { "IdAnyEscolar", "IdGrup" });

            migrationBuilder.CreateIndex(
                name: "IX_AlumnesEnGrup_NIA",
                table: "AlumnesEnGrup",
                column: "NIA");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionsKarma_IdAnyEscolar",
                table: "ConfiguracionsKarma",
                column: "IdAnyEscolar");

            migrationBuilder.CreateIndex(
                name: "IX_Grups_IdProfessorTutor",
                table: "Grups",
                column: "IdProfessorTutor");

            migrationBuilder.CreateIndex(
                name: "IX_Periodes_IdAnyEscolar",
                table: "Periodes",
                column: "IdAnyEscolar");

            migrationBuilder.CreateIndex(
                name: "IX_Privilegis_IdAnyEscolar",
                table: "Privilegis",
                column: "IdAnyEscolar");

            migrationBuilder.CreateIndex(
                name: "IX_PrivilegisAssignats_IdAlumneEnGrup",
                table: "PrivilegisAssignats",
                column: "IdAlumneEnGrup");

            migrationBuilder.CreateIndex(
                name: "IX_PrivilegisAssignats_IdPrivilegi",
                table: "PrivilegisAssignats",
                column: "IdPrivilegi");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorsDeGrup_IdAnyEscolar_IdGrup",
                table: "ProfessorsDeGrup",
                columns: new[] { "IdAnyEscolar", "IdGrup" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorsDeGrup_IdMateria",
                table: "ProfessorsDeGrup",
                column: "IdMateria");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorsDeGrup_IdProfessor",
                table: "ProfessorsDeGrup",
                column: "IdProfessor");

            migrationBuilder.CreateIndex(
                name: "IX_Puntuacions_IdAlumneEnGrup",
                table: "Puntuacions",
                column: "IdAlumneEnGrup");

            migrationBuilder.CreateIndex(
                name: "IX_Puntuacions_IdCategoria",
                table: "Puntuacions",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Puntuacions_IdPeriode",
                table: "Puntuacions",
                column: "IdPeriode");

            migrationBuilder.CreateIndex(
                name: "IX_Puntuacions_ProfessorCreacioIdProfessor",
                table: "Puntuacions",
                column: "ProfessorCreacioIdProfessor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ConfiguracionsKarma");

            migrationBuilder.DropTable(
                name: "PrivilegisAssignats");

            migrationBuilder.DropTable(
                name: "ProfessorsDeGrup");

            migrationBuilder.DropTable(
                name: "Puntuacions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Privilegis");

            migrationBuilder.DropTable(
                name: "Materies");

            migrationBuilder.DropTable(
                name: "AlumnesEnGrup");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Periodes");

            migrationBuilder.DropTable(
                name: "Alumnes");

            migrationBuilder.DropTable(
                name: "Grups");

            migrationBuilder.DropTable(
                name: "AnysEscolar");

            migrationBuilder.DropTable(
                name: "Professors");
        }
    }
}
