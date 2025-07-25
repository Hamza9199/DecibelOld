using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Decibel.Migrations
{
    /// <inheritdoc />
    public partial class _4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "eksplicitniSadrzaj",
                table: "Pjesma",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "korisnikID",
                table: "Pjesma",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pjesma_korisnikID",
                table: "Pjesma",
                column: "korisnikID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pjesma_Korisnik_korisnikID",
                table: "Pjesma",
                column: "korisnikID",
                principalTable: "Korisnik",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pjesma_Korisnik_korisnikID",
                table: "Pjesma");

            migrationBuilder.DropIndex(
                name: "IX_Pjesma_korisnikID",
                table: "Pjesma");

            migrationBuilder.DropColumn(
                name: "korisnikID",
                table: "Pjesma");

            migrationBuilder.AlterColumn<bool>(
                name: "eksplicitniSadrzaj",
                table: "Pjesma",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
