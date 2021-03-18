using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class accesstoken2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "Administrators");

            migrationBuilder.AddColumn<byte[]>(
                name: "Token",
                table: "Administrators",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "Administrators");

            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "Administrators",
                type: "varchar(max)",
                nullable: true);
        }
    }
}
