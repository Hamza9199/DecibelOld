using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Decibel.Migrations
{
    /// <inheritdoc />
    public partial class Druga : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "putanjaAudio",
                table: "Pjesma",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "korisnickoIme",
                table: "Korisnik",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "RedoslijedPjesama",
                columns: table => new
                {
                    redoslijedPjesama = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pjesmaID = table.Column<long>(type: "bigint", nullable: false),
                    playlistaID = table.Column<long>(type: "bigint", nullable: false),
                    pokazivacNaSljedecuPjesmuID = table.Column<long>(type: "bigint", nullable: false),
                    pokazivacNaPrethodnuPjesmuID = table.Column<long>(type: "bigint", nullable: true),
                    kreiranDatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedoslijedPjesama", x => x.redoslijedPjesama);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RedoslijedPjesama");

            migrationBuilder.DropColumn(
                name: "korisnickoIme",
                table: "Korisnik");

            migrationBuilder.AlterColumn<string>(
                name: "putanjaAudio",
                table: "Pjesma",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
