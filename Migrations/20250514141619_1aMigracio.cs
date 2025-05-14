using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarmaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class _1aMigracio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alumne",
                columns: table => new
                {
                    NIA = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Cognoms = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Actiu = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alumne", x => x.NIA);
                });

            migrationBuilder.CreateTable(
                name: "AnyEscolar",
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
                    table.PrimaryKey("PK_AnyEscolar", x => x.IdAnyEscolar);
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
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
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
                name: "Categoria",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.IdCategoria);
                });

            migrationBuilder.CreateTable(
                name: "Materia",
                columns: table => new
                {
                    IdMateria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materia", x => x.IdMateria);
                });

            migrationBuilder.CreateTable(
                name: "Professor",
                columns: table => new
                {
                    IdProfessor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Cognoms = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Actiu = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professor", x => x.IdProfessor);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracioKarma",
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
                    table.PrimaryKey("PK_ConfiguracioKarma", x => x.IdConfiguracioKarma);
                    table.ForeignKey(
                        name: "FK_ConfiguracioKarma_AnyEscolar_IdAnyEscolar",
                        column: x => x.IdAnyEscolar,
                        principalTable: "AnyEscolar",
                        principalColumn: "IdAnyEscolar",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Periode",
                columns: table => new
                {
                    IdPeriode = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataInici = table.Column<DateOnly>(type: "date", nullable: false),
                    DataFi = table.Column<DateOnly>(type: "date", nullable: false),
                    IdAnyEscolar = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periode", x => x.IdPeriode);
                    table.ForeignKey(
                        name: "FK_Periode_AnyEscolar_IdAnyEscolar",
                        column: x => x.IdAnyEscolar,
                        principalTable: "AnyEscolar",
                        principalColumn: "IdAnyEscolar",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Privilegi",
                columns: table => new
                {
                    IdPrivilegi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nivell = table.Column<int>(type: "int", nullable: false),
                    Descripcio = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EsIndividualGrup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdAnyEscolar = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privilegi", x => x.IdPrivilegi);
                    table.ForeignKey(
                        name: "FK_Privilegi_AnyEscolar_IdAnyEscolar",
                        column: x => x.IdAnyEscolar,
                        principalTable: "AnyEscolar",
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
                name: "Grup",
                columns: table => new
                {
                    IdAnyEscolar = table.Column<int>(type: "int", nullable: false),
                    IdGrup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcio = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    KarmaBase = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdProfessorTutor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grup", x => new { x.IdAnyEscolar, x.IdGrup });
                    table.ForeignKey(
                        name: "FK_Grup_AnyEscolar_IdAnyEscolar",
                        column: x => x.IdAnyEscolar,
                        principalTable: "AnyEscolar",
                        principalColumn: "IdAnyEscolar",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grup_Professor_IdProfessorTutor",
                        column: x => x.IdProfessorTutor,
                        principalTable: "Professor",
                        principalColumn: "IdProfessor");
                });

            migrationBuilder.CreateTable(
                name: "AlumneEnGrup",
                columns: table => new
                {
                    IdAlumneEnGrup = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NIA = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IdAnyEscolar = table.Column<int>(type: "int", nullable: false),
                    IdGrup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PuntuacioTotal = table.Column<int>(type: "int", nullable: false),
                    Karma = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlumneEnGrup", x => x.IdAlumneEnGrup);
                    table.ForeignKey(
                        name: "FK_AlumneEnGrup_Alumne_NIA",
                        column: x => x.NIA,
                        principalTable: "Alumne",
                        principalColumn: "NIA",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlumneEnGrup_AnyEscolar_IdAnyEscolar",
                        column: x => x.IdAnyEscolar,
                        principalTable: "AnyEscolar",
                        principalColumn: "IdAnyEscolar");
                    table.ForeignKey(
                        name: "FK_AlumneEnGrup_Grup_IdAnyEscolar_IdGrup",
                        columns: x => new { x.IdAnyEscolar, x.IdGrup },
                        principalTable: "Grup",
                        principalColumns: new[] { "IdAnyEscolar", "IdGrup" });
                });

            migrationBuilder.CreateTable(
                name: "ProfessorDeGrup",
                columns: table => new
                {
                    IdProfessorDeGrup = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProfessor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdMateria = table.Column<int>(type: "int", nullable: false),
                    IdAnyEscolar = table.Column<int>(type: "int", nullable: false),
                    IdGrup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessorDeGrup", x => x.IdProfessorDeGrup);
                    table.ForeignKey(
                        name: "FK_ProfessorDeGrup_AnyEscolar_IdAnyEscolar",
                        column: x => x.IdAnyEscolar,
                        principalTable: "AnyEscolar",
                        principalColumn: "IdAnyEscolar",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfessorDeGrup_Grup_IdAnyEscolar_IdGrup",
                        columns: x => new { x.IdAnyEscolar, x.IdGrup },
                        principalTable: "Grup",
                        principalColumns: new[] { "IdAnyEscolar", "IdGrup" });
                    table.ForeignKey(
                        name: "FK_ProfessorDeGrup_Materia_IdMateria",
                        column: x => x.IdMateria,
                        principalTable: "Materia",
                        principalColumn: "IdMateria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfessorDeGrup_Professor_IdProfessor",
                        column: x => x.IdProfessor,
                        principalTable: "Professor",
                        principalColumn: "IdProfessor");
                });

            migrationBuilder.CreateTable(
                name: "PrivilegiAssignat",
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
                    DataExecucio = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivilegiAssignat", x => x.IdPrivilegiAssignat);
                    table.ForeignKey(
                        name: "FK_PrivilegiAssignat_AlumneEnGrup_IdAlumneEnGrup",
                        column: x => x.IdAlumneEnGrup,
                        principalTable: "AlumneEnGrup",
                        principalColumn: "IdAlumneEnGrup",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrivilegiAssignat_Privilegi_IdPrivilegi",
                        column: x => x.IdPrivilegi,
                        principalTable: "Privilegi",
                        principalColumn: "IdPrivilegi",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Puntuacio",
                columns: table => new
                {
                    IdPuntuacio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataEntrada = table.Column<DateOnly>(type: "date", nullable: false),
                    Punts = table.Column<int>(type: "int", nullable: false),
                    Motiu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UsuariCreacio = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdPeriode = table.Column<int>(type: "int", nullable: false),
                    IdCategoria = table.Column<int>(type: "int", nullable: true),
                    IdAlumneEnGrup = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puntuacio", x => x.IdPuntuacio);
                    table.ForeignKey(
                        name: "FK_Puntuacio_AlumneEnGrup_IdAlumneEnGrup",
                        column: x => x.IdAlumneEnGrup,
                        principalTable: "AlumneEnGrup",
                        principalColumn: "IdAlumneEnGrup",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Puntuacio_Categoria_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "Categoria",
                        principalColumn: "IdCategoria");
                    table.ForeignKey(
                        name: "FK_Puntuacio_Periode_IdPeriode",
                        column: x => x.IdPeriode,
                        principalTable: "Periode",
                        principalColumn: "IdPeriode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlumneEnGrup_IdAnyEscolar_IdGrup",
                table: "AlumneEnGrup",
                columns: new[] { "IdAnyEscolar", "IdGrup" });

            migrationBuilder.CreateIndex(
                name: "IX_AlumneEnGrup_NIA",
                table: "AlumneEnGrup",
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
                name: "IX_ConfiguracioKarma_IdAnyEscolar",
                table: "ConfiguracioKarma",
                column: "IdAnyEscolar");

            migrationBuilder.CreateIndex(
                name: "IX_Grup_IdProfessorTutor",
                table: "Grup",
                column: "IdProfessorTutor");

            migrationBuilder.CreateIndex(
                name: "IX_Periode_IdAnyEscolar",
                table: "Periode",
                column: "IdAnyEscolar");

            migrationBuilder.CreateIndex(
                name: "IX_Privilegi_IdAnyEscolar",
                table: "Privilegi",
                column: "IdAnyEscolar");

            migrationBuilder.CreateIndex(
                name: "IX_PrivilegiAssignat_IdAlumneEnGrup",
                table: "PrivilegiAssignat",
                column: "IdAlumneEnGrup");

            migrationBuilder.CreateIndex(
                name: "IX_PrivilegiAssignat_IdPrivilegi",
                table: "PrivilegiAssignat",
                column: "IdPrivilegi");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorDeGrup_IdAnyEscolar_IdGrup",
                table: "ProfessorDeGrup",
                columns: new[] { "IdAnyEscolar", "IdGrup" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorDeGrup_IdMateria",
                table: "ProfessorDeGrup",
                column: "IdMateria");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorDeGrup_IdProfessor",
                table: "ProfessorDeGrup",
                column: "IdProfessor");

            migrationBuilder.CreateIndex(
                name: "IX_Puntuacio_IdAlumneEnGrup",
                table: "Puntuacio",
                column: "IdAlumneEnGrup");

            migrationBuilder.CreateIndex(
                name: "IX_Puntuacio_IdCategoria",
                table: "Puntuacio",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Puntuacio_IdPeriode",
                table: "Puntuacio",
                column: "IdPeriode");
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
                name: "ConfiguracioKarma");

            migrationBuilder.DropTable(
                name: "PrivilegiAssignat");

            migrationBuilder.DropTable(
                name: "ProfessorDeGrup");

            migrationBuilder.DropTable(
                name: "Puntuacio");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Privilegi");

            migrationBuilder.DropTable(
                name: "Materia");

            migrationBuilder.DropTable(
                name: "AlumneEnGrup");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "Periode");

            migrationBuilder.DropTable(
                name: "Alumne");

            migrationBuilder.DropTable(
                name: "Grup");

            migrationBuilder.DropTable(
                name: "AnyEscolar");

            migrationBuilder.DropTable(
                name: "Professor");
        }
    }
}
