using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Decibel.Migrations
{
    /// <inheritdoc />
    public partial class Treca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayLista_Korisnik_korisnikID",
                table: "PlayLista");

            migrationBuilder.AlterColumn<string>(
                name: "korisnikID",
                table: "PlayLista",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "jezikPjesme",
                table: "Pjesma",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "eksplicitniSadrzaj",
                table: "Pjesma",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "potvrdjeno",
                table: "IzvodjacPjesma",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "udio",
                table: "IzvodjacPjesma",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayLista_Korisnik_korisnikID",
                table: "PlayLista",
                column: "korisnikID",
                principalTable: "Korisnik",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayLista_Korisnik_korisnikID",
                table: "PlayLista");

            migrationBuilder.DropColumn(
                name: "potvrdjeno",
                table: "IzvodjacPjesma");

            migrationBuilder.DropColumn(
                name: "udio",
                table: "IzvodjacPjesma");

            migrationBuilder.AlterColumn<string>(
                name: "korisnikID",
                table: "PlayLista",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "jezikPjesme",
                table: "Pjesma",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "eksplicitniSadrzaj",
                table: "Pjesma",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayLista_Korisnik_korisnikID",
                table: "PlayLista",
                column: "korisnikID",
                principalTable: "Korisnik",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
