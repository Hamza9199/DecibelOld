using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Decibel.Migrations
{
    /// <inheritdoc />
    public partial class FinalnaMigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Pretplata",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cijena = table.Column<decimal>(type: "money", nullable: false),
                    dostupno = table.Column<bool>(type: "bit", nullable: false),
                    kreiranDatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pretplata", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Zanr",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    kreiranDatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zanr", x => x.ID);
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
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
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
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
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
                name: "Korisnik",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    statusKorisnika = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    putanjaProfilneSlike = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    datumRegistracije = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    zadnjaPrijava = table.Column<DateTime>(type: "datetime2", nullable: true),
                    brojPratilaca = table.Column<decimal>(type: "decimal(20,0)", nullable: false, defaultValue: 0m),
                    obrisan = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnik", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Korisnik_AspNetUsers_ID",
                        column: x => x.ID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Album",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    korisnikID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    opis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    kreiranDatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    odobreno = table.Column<bool>(type: "bit", nullable: false),
                    javno = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    brojLajkova = table.Column<decimal>(type: "decimal(20,0)", nullable: false, defaultValue: 0m),
                    putanjaSlika = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    putanjaGif = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Album", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Album_Korisnik_korisnikID",
                        column: x => x.korisnikID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KorisnikPretplata",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    korisnikID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    pretplataID = table.Column<long>(type: "bigint", nullable: false),
                    PretplataStatus = table.Column<int>(type: "int", nullable: false),
                    kreiranDatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    datumVrijemeObnove = table.Column<DateTime>(type: "datetime2", nullable: false),
                    datumIsteka = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KorisnikPretplata", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KorisnikPretplata_Korisnik_korisnikID",
                        column: x => x.korisnikID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KorisnikPretplata_Pretplata_pretplataID",
                        column: x => x.pretplataID,
                        principalTable: "Pretplata",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayLista",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    korisnikID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    opis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    kreiranDatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    javno = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    brojLajkova = table.Column<decimal>(type: "decimal(20,0)", nullable: false, defaultValue: 0m),
                    putanjaSlika = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    putanjaGif = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayLista", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlayLista_Korisnik_korisnikID",
                        column: x => x.korisnikID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PratilacKorisnik",
                columns: table => new
                {
                    korisnikID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    pratilacID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    kreiranDatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PratilacKorisnik", x => new { x.korisnikID, x.pratilacID });
                    table.ForeignKey(
                        name: "FK_PratilacKorisnik_Korisnik_korisnikID",
                        column: x => x.korisnikID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PratilacKorisnik_Korisnik_pratilacID",
                        column: x => x.pratilacID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KorisnikAlbum",
                columns: table => new
                {
                    korisnikID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    albumID = table.Column<long>(type: "bigint", nullable: false),
                    kreiranDatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KorisnikAlbum", x => new { x.korisnikID, x.albumID });
                    table.ForeignKey(
                        name: "FK_KorisnikAlbum_Album_albumID",
                        column: x => x.albumID,
                        principalTable: "Album",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KorisnikAlbum_Korisnik_korisnikID",
                        column: x => x.korisnikID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pjesma",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    albumID = table.Column<long>(type: "bigint", nullable: true),
                    redniBrojUAlbumu = table.Column<long>(type: "bigint", nullable: true),
                    naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    opis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    datumObjave = table.Column<DateOnly>(type: "date", nullable: false),
                    trajanjeSekunde = table.Column<long>(type: "bigint", nullable: false),
                    javno = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    odobreno = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    putanjaAudio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    putanjaSlika = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    putanjaGif = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    brojReprodukcija = table.Column<decimal>(type: "decimal(20,0)", nullable: false, defaultValue: 0m),
                    brojLajkova = table.Column<decimal>(type: "decimal(20,0)", nullable: false, defaultValue: 0m),
                    jezikPjesme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    licenca = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    eksplicitniSadrzaj = table.Column<bool>(type: "bit", nullable: false),
                    tekst = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    kreiranDatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pjesma", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Pjesma_Album_albumID",
                        column: x => x.albumID,
                        principalTable: "Album",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ObnovaPretplate",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    korisnikPretplataID = table.Column<long>(type: "bigint", nullable: false),
                    datumObnove = table.Column<DateTime>(type: "datetime2", nullable: false),
                    iznosObnove = table.Column<decimal>(type: "money", nullable: false),
                    kreiranDatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObnovaPretplate", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ObnovaPretplate_KorisnikPretplata_korisnikPretplataID",
                        column: x => x.korisnikPretplataID,
                        principalTable: "KorisnikPretplata",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KorisnikPlayLista",
                columns: table => new
                {
                    korisnikID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    playlistaID = table.Column<long>(type: "bigint", nullable: false),
                    kreiranDatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KorisnikPlayLista", x => new { x.korisnikID, x.playlistaID });
                    table.ForeignKey(
                        name: "FK_KorisnikPlayLista_Korisnik_korisnikID",
                        column: x => x.korisnikID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KorisnikPlayLista_PlayLista_playlistaID",
                        column: x => x.playlistaID,
                        principalTable: "PlayLista",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistorijaSlusanja",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    korisnikID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    pjesmaID = table.Column<long>(type: "bigint", nullable: false),
                    playlistaID = table.Column<long>(type: "bigint", nullable: true),
                    kreiranDatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    trajanje = table.Column<int>(type: "int", nullable: false),
                    kontekstPustanja = table.Column<int>(type: "int", nullable: false),
                    offline = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorijaSlusanja", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HistorijaSlusanja_Korisnik_korisnikID",
                        column: x => x.korisnikID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistorijaSlusanja_Pjesma_pjesmaID",
                        column: x => x.pjesmaID,
                        principalTable: "Pjesma",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistorijaSlusanja_PlayLista_playlistaID",
                        column: x => x.playlistaID,
                        principalTable: "PlayLista",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "IzvodjacPjesma",
                columns: table => new
                {
                    izvodjacID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    pjesmaID = table.Column<long>(type: "bigint", nullable: false),
                    kreiranDatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IzvodjacPjesma", x => new { x.izvodjacID, x.pjesmaID });
                    table.ForeignKey(
                        name: "FK_IzvodjacPjesma_Korisnik_izvodjacID",
                        column: x => x.izvodjacID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IzvodjacPjesma_Pjesma_pjesmaID",
                        column: x => x.pjesmaID,
                        principalTable: "Pjesma",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Komentar",
                columns: table => new
                {
                    korisnikID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    pjesmaID = table.Column<long>(type: "bigint", nullable: false),
                    tekst = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vrijemePjesmeSekunde = table.Column<long>(type: "bigint", nullable: false),
                    kreiranDatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Komentar", x => new { x.korisnikID, x.pjesmaID });
                    table.ForeignKey(
                        name: "FK_Komentar_Korisnik_korisnikID",
                        column: x => x.korisnikID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Komentar_Pjesma_pjesmaID",
                        column: x => x.pjesmaID,
                        principalTable: "Pjesma",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KorisnikPjesma",
                columns: table => new
                {
                    korisnikID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    pjesmaID = table.Column<long>(type: "bigint", nullable: false),
                    kreiranDatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KorisnikPjesma", x => new { x.korisnikID, x.pjesmaID });
                    table.ForeignKey(
                        name: "FK_KorisnikPjesma_Korisnik_korisnikID",
                        column: x => x.korisnikID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KorisnikPjesma_Pjesma_pjesmaID",
                        column: x => x.pjesmaID,
                        principalTable: "Pjesma",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PjesmaPlayLista",
                columns: table => new
                {
                    pjesmaID = table.Column<long>(type: "bigint", nullable: false),
                    playlistaID = table.Column<long>(type: "bigint", nullable: false),
                    pokazivacNaSljedecuPjesmuID = table.Column<long>(type: "bigint", nullable: true),
                    pokazivacNaPrethodnuPjesmuID = table.Column<long>(type: "bigint", nullable: true),
                    kreiranDatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PjesmaPlayLista", x => new { x.pjesmaID, x.playlistaID });
                    table.ForeignKey(
                        name: "FK_PjesmaPlayLista_Pjesma_pjesmaID",
                        column: x => x.pjesmaID,
                        principalTable: "Pjesma",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PjesmaPlayLista_Pjesma_pokazivacNaPrethodnuPjesmuID",
                        column: x => x.pokazivacNaPrethodnuPjesmuID,
                        principalTable: "Pjesma",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PjesmaPlayLista_Pjesma_pokazivacNaSljedecuPjesmuID",
                        column: x => x.pokazivacNaSljedecuPjesmuID,
                        principalTable: "Pjesma",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PjesmaPlayLista_PlayLista_playlistaID",
                        column: x => x.playlistaID,
                        principalTable: "PlayLista",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PjesmaZanr",
                columns: table => new
                {
                    pjesmaID = table.Column<long>(type: "bigint", nullable: false),
                    zanrID = table.Column<long>(type: "bigint", nullable: false),
                    kreiranDatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PjesmaZanr", x => new { x.pjesmaID, x.zanrID });
                    table.ForeignKey(
                        name: "FK_PjesmaZanr_Pjesma_pjesmaID",
                        column: x => x.pjesmaID,
                        principalTable: "Pjesma",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PjesmaZanr_Zanr_zanrID",
                        column: x => x.zanrID,
                        principalTable: "Zanr",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatistikaReprodukcije",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    korisnikID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    pjesmaID = table.Column<long>(type: "bigint", nullable: false),
                    brojReprodukcija = table.Column<int>(type: "int", nullable: false),
                    ukupnoTrajanje = table.Column<int>(type: "int", nullable: false),
                    zadnjePustanje = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatistikaReprodukcije", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StatistikaReprodukcije_Korisnik_korisnikID",
                        column: x => x.korisnikID,
                        principalTable: "Korisnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StatistikaReprodukcije_Pjesma_pjesmaID",
                        column: x => x.pjesmaID,
                        principalTable: "Pjesma",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Album_korisnikID",
                table: "Album",
                column: "korisnikID");

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
                name: "IX_HistorijaSlusanja_korisnikID",
                table: "HistorijaSlusanja",
                column: "korisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_HistorijaSlusanja_pjesmaID",
                table: "HistorijaSlusanja",
                column: "pjesmaID");

            migrationBuilder.CreateIndex(
                name: "IX_HistorijaSlusanja_playlistaID",
                table: "HistorijaSlusanja",
                column: "playlistaID");

            migrationBuilder.CreateIndex(
                name: "IX_IzvodjacPjesma_pjesmaID",
                table: "IzvodjacPjesma",
                column: "pjesmaID");

            migrationBuilder.CreateIndex(
                name: "IX_Komentar_pjesmaID",
                table: "Komentar",
                column: "pjesmaID");

            migrationBuilder.CreateIndex(
                name: "IX_KorisnikAlbum_albumID",
                table: "KorisnikAlbum",
                column: "albumID");

            migrationBuilder.CreateIndex(
                name: "IX_KorisnikPjesma_pjesmaID",
                table: "KorisnikPjesma",
                column: "pjesmaID");

            migrationBuilder.CreateIndex(
                name: "IX_KorisnikPlayLista_playlistaID",
                table: "KorisnikPlayLista",
                column: "playlistaID");

            migrationBuilder.CreateIndex(
                name: "IX_KorisnikPretplata_korisnikID",
                table: "KorisnikPretplata",
                column: "korisnikID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KorisnikPretplata_pretplataID",
                table: "KorisnikPretplata",
                column: "pretplataID");

            migrationBuilder.CreateIndex(
                name: "IX_ObnovaPretplate_korisnikPretplataID",
                table: "ObnovaPretplate",
                column: "korisnikPretplataID");

            migrationBuilder.CreateIndex(
                name: "IX_Pjesma_albumID",
                table: "Pjesma",
                column: "albumID");

            migrationBuilder.CreateIndex(
                name: "IX_PjesmaPlayLista_playlistaID",
                table: "PjesmaPlayLista",
                column: "playlistaID");

            migrationBuilder.CreateIndex(
                name: "IX_PjesmaPlayLista_pokazivacNaPrethodnuPjesmuID",
                table: "PjesmaPlayLista",
                column: "pokazivacNaPrethodnuPjesmuID");

            migrationBuilder.CreateIndex(
                name: "IX_PjesmaPlayLista_pokazivacNaSljedecuPjesmuID",
                table: "PjesmaPlayLista",
                column: "pokazivacNaSljedecuPjesmuID");

            migrationBuilder.CreateIndex(
                name: "IX_PjesmaZanr_zanrID",
                table: "PjesmaZanr",
                column: "zanrID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayLista_korisnikID",
                table: "PlayLista",
                column: "korisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_PratilacKorisnik_pratilacID",
                table: "PratilacKorisnik",
                column: "pratilacID");

            migrationBuilder.CreateIndex(
                name: "IX_StatistikaReprodukcije_korisnikID",
                table: "StatistikaReprodukcije",
                column: "korisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_StatistikaReprodukcije_pjesmaID",
                table: "StatistikaReprodukcije",
                column: "pjesmaID");
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
                name: "HistorijaSlusanja");

            migrationBuilder.DropTable(
                name: "IzvodjacPjesma");

            migrationBuilder.DropTable(
                name: "Komentar");

            migrationBuilder.DropTable(
                name: "KorisnikAlbum");

            migrationBuilder.DropTable(
                name: "KorisnikPjesma");

            migrationBuilder.DropTable(
                name: "KorisnikPlayLista");

            migrationBuilder.DropTable(
                name: "ObnovaPretplate");

            migrationBuilder.DropTable(
                name: "PjesmaPlayLista");

            migrationBuilder.DropTable(
                name: "PjesmaZanr");

            migrationBuilder.DropTable(
                name: "PratilacKorisnik");

            migrationBuilder.DropTable(
                name: "StatistikaReprodukcije");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "KorisnikPretplata");

            migrationBuilder.DropTable(
                name: "PlayLista");

            migrationBuilder.DropTable(
                name: "Zanr");

            migrationBuilder.DropTable(
                name: "Pjesma");

            migrationBuilder.DropTable(
                name: "Pretplata");

            migrationBuilder.DropTable(
                name: "Album");

            migrationBuilder.DropTable(
                name: "Korisnik");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
