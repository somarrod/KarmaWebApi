using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarmaWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnyEscolars",
                columns: table => new
                {
                    IdAnyEscolar = table.Column<int>(type: "int", nullable: false),
                    DataIniciCurs = table.Column<DateOnly>(type: "date", nullable: false),
                    DataFiCurs = table.Column<DateOnly>(type: "date", nullable: false),
                    SaldoKarmaInicial = table.Column<double>(type: "float", nullable: false),
                    ReiniciaCadaAvaluacio = table.Column<bool>(type: "bit", nullable: false),
                    Actiu = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyEscolars", x => x.IdAnyEscolar);
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
                name: "Materies",
                columns: table => new
                {
                    IdMateria = table.Column<long>(type: "bigint", nullable: false)
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
                    IdProfessor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Cognoms = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Actiu = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PertanyAEquipDirectiu = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professors", x => x.IdProfessor);
                });

            migrationBuilder.CreateTable(
                name: "TipusCategories",
                columns: table => new
                {
                    IdTipusCategoria = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Actiu = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipusCategories", x => x.IdTipusCategoria);
                });

            migrationBuilder.CreateTable(
                name: "Avaluacions",
                columns: table => new
                {
                    IdAvaluacio = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataInicial = table.Column<DateOnly>(type: "date", nullable: false),
                    DataFinal = table.Column<DateOnly>(type: "date", nullable: false),
                    NotaMinimaKarma = table.Column<double>(type: "float", nullable: false),
                    NotaMaximaKarma = table.Column<double>(type: "float", nullable: false),
                    IdAnyEscolar = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avaluacions", x => x.IdAvaluacio);
                    table.ForeignKey(
                        name: "FK_Avaluacions_AnyEscolars_IdAnyEscolar",
                        column: x => x.IdAnyEscolar,
                        principalTable: "AnyEscolars",
                        principalColumn: "IdAnyEscolar",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    IdClasse = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAnyEscolar = table.Column<int>(type: "int", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => new { x.IdAnyEscolar, x.IdClasse });
                    table.ForeignKey(
                        name: "FK_Classes_AnyEscolars_IdAnyEscolar",
                        column: x => x.IdAnyEscolar,
                        principalTable: "AnyEscolars",
                        principalColumn: "IdAnyEscolar",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracionsKarma",
                columns: table => new
                {
                    IdConfiguracioKarma = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumPuntsMinim = table.Column<double>(type: "float", nullable: false),
                    NumPuntsMaxim = table.Column<double>(type: "float", nullable: false),
                    ColorKarma = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NivellPrivilegis = table.Column<int>(type: "int", nullable: false),
                    IdAnyEscolar = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionsKarma", x => x.IdConfiguracioKarma);
                    table.ForeignKey(
                        name: "FK_ConfiguracionsKarma_AnyEscolars_IdAnyEscolar",
                        column: x => x.IdAnyEscolar,
                        principalTable: "AnyEscolars",
                        principalColumn: "IdAnyEscolar",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Privilegis",
                columns: table => new
                {
                    IdPrivilegi = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdAnyEscolar = table.Column<long>(type: "bigint", nullable: false),
                    AnyEscolarIdAnyEscolar = table.Column<int>(type: "int", nullable: false),
                    NivellPrivilegi = table.Column<int>(type: "int", nullable: false),
                    Actiu = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privilegis", x => x.IdPrivilegi);
                    table.ForeignKey(
                        name: "FK_Privilegis_AnyEscolars_AnyEscolarIdAnyEscolar",
                        column: x => x.AnyEscolarIdAnyEscolar,
                        principalTable: "AnyEscolars",
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
                name: "Categories",
                columns: table => new
                {
                    IdCategoria = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumPunts = table.Column<double>(type: "float", nullable: false),
                    Editable = table.Column<bool>(type: "bit", nullable: false),
                    Comentaris = table.Column<string>(type: "nvarchar(999)", maxLength: 999, nullable: true),
                    Activa = table.Column<bool>(type: "bit", nullable: false),
                    IdTipusCategoria = table.Column<long>(type: "bigint", nullable: false),
                    TipusCategoriaIdTipusCategoria = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.IdCategoria);
                    table.ForeignKey(
                        name: "FK_Categories_TipusCategories_TipusCategoriaIdTipusCategoria",
                        column: x => x.TipusCategoriaIdTipusCategoria,
                        principalTable: "TipusCategories",
                        principalColumn: "IdTipusCategoria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grups",
                columns: table => new
                {
                    IdGrup = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IdAnyEscolar = table.Column<int>(type: "int", nullable: false),
                    IdClasse = table.Column<long>(type: "bigint", nullable: false),
                    KarmaBase = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DataUltimaActualitzacioKarma = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grups", x => x.IdGrup);
                    table.ForeignKey(
                        name: "FK_Grups_Classes_IdAnyEscolar_IdClasse",
                        columns: x => new { x.IdAnyEscolar, x.IdClasse },
                        principalTable: "Classes",
                        principalColumns: new[] { "IdAnyEscolar", "IdClasse" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfessorsDeClasse",
                columns: table => new
                {
                    IdProfessorDeClasse = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProfessor = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    IdAnyEscolar = table.Column<int>(type: "int", nullable: false),
                    IdClasse = table.Column<long>(type: "bigint", nullable: false),
                    IdMateria = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessorsDeClasse", x => x.IdProfessorDeClasse);
                    table.ForeignKey(
                        name: "FK_ProfessorsDeClasse_Classes_IdAnyEscolar_IdClasse",
                        columns: x => new { x.IdAnyEscolar, x.IdClasse },
                        principalTable: "Classes",
                        principalColumns: new[] { "IdAnyEscolar", "IdClasse" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfessorsDeClasse_Materies_IdMateria",
                        column: x => x.IdMateria,
                        principalTable: "Materies",
                        principalColumn: "IdMateria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfessorsDeClasse_Professors_IdProfessor",
                        column: x => x.IdProfessor,
                        principalTable: "Professors",
                        principalColumn: "IdProfessor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alumnes",
                columns: table => new
                {
                    NIA = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Cognoms = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Actiu = table.Column<bool>(type: "bit", nullable: false),
                    IdAnyEscolar = table.Column<int>(type: "int", nullable: true),
                    IdClasse = table.Column<long>(type: "bigint", nullable: true),
                    IdGrup = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alumnes", x => x.NIA);
                    table.ForeignKey(
                        name: "FK_Alumnes_Classes_IdAnyEscolar_IdClasse",
                        columns: x => new { x.IdAnyEscolar, x.IdClasse },
                        principalTable: "Classes",
                        principalColumns: new[] { "IdAnyEscolar", "IdClasse" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alumnes_Grups_IdGrup",
                        column: x => x.IdGrup,
                        principalTable: "Grups",
                        principalColumn: "IdGrup");
                });

            migrationBuilder.CreateTable(
                name: "KarmaAlumnes",
                columns: table => new
                {
                    IdKarmaAlumne = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NIA = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    IdAvaluacio = table.Column<long>(type: "bigint", nullable: false),
                    NumPuntsInicials = table.Column<double>(type: "float", nullable: false),
                    NumPuntsActuals = table.Column<double>(type: "float", nullable: false),
                    KarmaInicial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KarmaActual = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotaKarma = table.Column<double>(type: "float", nullable: true),
                    Comentaris = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumPuntsAvaluacioAnteriorDrv = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KarmaAlumnes", x => x.IdKarmaAlumne);
                    table.ForeignKey(
                        name: "FK_KarmaAlumnes_Alumnes_NIA",
                        column: x => x.NIA,
                        principalTable: "Alumnes",
                        principalColumn: "NIA",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KarmaAlumnes_Avaluacions_IdAvaluacio",
                        column: x => x.IdAvaluacio,
                        principalTable: "Avaluacions",
                        principalColumn: "IdAvaluacio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrivilegisAssignats",
                columns: table => new
                {
                    IdPrivilegiAssignat = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NivellPrivilegi = table.Column<int>(type: "int", nullable: false),
                    Descripcio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCreacio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tipus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodiIntern = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataExecucio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NIA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlumneNIA = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    IdPrivilegi = table.Column<long>(type: "bigint", nullable: false),
                    PrivilegiIdPrivilegi = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivilegisAssignats", x => x.IdPrivilegiAssignat);
                    table.ForeignKey(
                        name: "FK_PrivilegisAssignats_Alumnes_AlumneNIA",
                        column: x => x.AlumneNIA,
                        principalTable: "Alumnes",
                        principalColumn: "NIA");
                    table.ForeignKey(
                        name: "FK_PrivilegisAssignats_Privilegis_PrivilegiIdPrivilegi",
                        column: x => x.PrivilegiIdPrivilegi,
                        principalTable: "Privilegis",
                        principalColumn: "IdPrivilegi",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Puntuacions",
                columns: table => new
                {
                    IdPuntuacio = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NIA = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AlumneNIA = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    IdProfessor = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    IdCategoria = table.Column<long>(type: "bigint", nullable: false),
                    CategoriaIdCategoria = table.Column<long>(type: "bigint", nullable: false),
                    IdClasse = table.Column<long>(type: "bigint", nullable: false),
                    NomClasse = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdGrup = table.Column<long>(type: "bigint", nullable: true),
                    NomGrup = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NumPunts = table.Column<double>(type: "float", nullable: false),
                    Tipus = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Motiu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescripcioAdicional = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataEvent = table.Column<DateOnly>(type: "date", nullable: false),
                    DataCreacio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdAvaluacio = table.Column<long>(type: "bigint", nullable: false),
                    AvaluacioIdAvaluacio = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puntuacions", x => x.IdPuntuacio);
                    table.ForeignKey(
                        name: "FK_Puntuacions_Alumnes_AlumneNIA",
                        column: x => x.AlumneNIA,
                        principalTable: "Alumnes",
                        principalColumn: "NIA");
                    table.ForeignKey(
                        name: "FK_Puntuacions_Avaluacions_AvaluacioIdAvaluacio",
                        column: x => x.AvaluacioIdAvaluacio,
                        principalTable: "Avaluacions",
                        principalColumn: "IdAvaluacio",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Puntuacions_Categories_CategoriaIdCategoria",
                        column: x => x.CategoriaIdCategoria,
                        principalTable: "Categories",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Puntuacions_Professors_IdProfessor",
                        column: x => x.IdProfessor,
                        principalTable: "Professors",
                        principalColumn: "IdProfessor",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alumnes_IdAnyEscolar_IdClasse",
                table: "Alumnes",
                columns: new[] { "IdAnyEscolar", "IdClasse" });

            migrationBuilder.CreateIndex(
                name: "IX_Alumnes_IdGrup",
                table: "Alumnes",
                column: "IdGrup");

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
                name: "IX_Avaluacions_IdAnyEscolar",
                table: "Avaluacions",
                column: "IdAnyEscolar");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_TipusCategoriaIdTipusCategoria",
                table: "Categories",
                column: "TipusCategoriaIdTipusCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionsKarma_IdAnyEscolar",
                table: "ConfiguracionsKarma",
                column: "IdAnyEscolar");

            migrationBuilder.CreateIndex(
                name: "IX_Grups_IdAnyEscolar_IdClasse",
                table: "Grups",
                columns: new[] { "IdAnyEscolar", "IdClasse" });

            migrationBuilder.CreateIndex(
                name: "IX_KarmaAlumnes_IdAvaluacio",
                table: "KarmaAlumnes",
                column: "IdAvaluacio");

            migrationBuilder.CreateIndex(
                name: "IX_KarmaAlumnes_NIA_IdAvaluacio",
                table: "KarmaAlumnes",
                columns: new[] { "NIA", "IdAvaluacio" });

            migrationBuilder.CreateIndex(
                name: "IX_Privilegis_AnyEscolarIdAnyEscolar",
                table: "Privilegis",
                column: "AnyEscolarIdAnyEscolar");

            migrationBuilder.CreateIndex(
                name: "IX_PrivilegisAssignats_AlumneNIA",
                table: "PrivilegisAssignats",
                column: "AlumneNIA");

            migrationBuilder.CreateIndex(
                name: "IX_PrivilegisAssignats_PrivilegiIdPrivilegi",
                table: "PrivilegisAssignats",
                column: "PrivilegiIdPrivilegi");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorsDeClasse_IdAnyEscolar_IdClasse",
                table: "ProfessorsDeClasse",
                columns: new[] { "IdAnyEscolar", "IdClasse" });

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorsDeClasse_IdMateria",
                table: "ProfessorsDeClasse",
                column: "IdMateria");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorsDeClasse_IdProfessor_IdClasse_IdMateria",
                table: "ProfessorsDeClasse",
                columns: new[] { "IdProfessor", "IdClasse", "IdMateria" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Puntuacions_AlumneNIA",
                table: "Puntuacions",
                column: "AlumneNIA");

            migrationBuilder.CreateIndex(
                name: "IX_Puntuacions_AvaluacioIdAvaluacio",
                table: "Puntuacions",
                column: "AvaluacioIdAvaluacio");

            migrationBuilder.CreateIndex(
                name: "IX_Puntuacions_CategoriaIdCategoria",
                table: "Puntuacions",
                column: "CategoriaIdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Puntuacions_IdProfessor",
                table: "Puntuacions",
                column: "IdProfessor");

            migrationBuilder.CreateIndex(
                name: "IX_Puntuacions_NIA_IdAvaluacio",
                table: "Puntuacions",
                columns: new[] { "NIA", "IdAvaluacio" });
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
                name: "KarmaAlumnes");

            migrationBuilder.DropTable(
                name: "PrivilegisAssignats");

            migrationBuilder.DropTable(
                name: "ProfessorsDeClasse");

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
                name: "Alumnes");

            migrationBuilder.DropTable(
                name: "Avaluacions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Professors");

            migrationBuilder.DropTable(
                name: "Grups");

            migrationBuilder.DropTable(
                name: "TipusCategories");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "AnyEscolars");
        }
    }
}
