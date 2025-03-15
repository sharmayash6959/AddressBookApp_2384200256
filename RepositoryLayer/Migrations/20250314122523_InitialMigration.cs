using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AddressBooks",
                table: "AddressBooks");

            migrationBuilder.RenameTable(
                name: "AddressBooks",
                newName: "Contacts");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Contacts");

            migrationBuilder.RenameTable(
                name: "Contacts",
                newName: "AddressBooks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AddressBooks",
                table: "AddressBooks",
                column: "Id");
        }
    }
}
