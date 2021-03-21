using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class assignedadmin_to_admin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Administrators_AssignedAdminId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "AssignedAdminId",
                table: "Tickets",
                newName: "AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_AssignedAdminId",
                table: "Tickets",
                newName: "IX_Tickets_AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Administrators_AdminId",
                table: "Tickets",
                column: "AdminId",
                principalTable: "Administrators",
                principalColumn: "AdminId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Administrators_AdminId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "AdminId",
                table: "Tickets",
                newName: "AssignedAdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_AdminId",
                table: "Tickets",
                newName: "IX_Tickets_AssignedAdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Administrators_AssignedAdminId",
                table: "Tickets",
                column: "AssignedAdminId",
                principalTable: "Administrators",
                principalColumn: "AdminId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
