using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Decibel.Migrations
{
    /// <inheritdoc />
    public partial class _7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<long>(
                name: "ID",
                table: "Komentar",
                type: "bigint",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

                migrationBuilder.DropColumn(
                        name: "ID",
                        table: "Komentar");

        }
    }
}
