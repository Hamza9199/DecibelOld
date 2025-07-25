﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Decibel.Migrations
{
    /// <inheritdoc />
    public partial class _5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "slazemSe",
                table: "Pjesma",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "slazemSe",
                table: "Pjesma");
        }
    }
}
