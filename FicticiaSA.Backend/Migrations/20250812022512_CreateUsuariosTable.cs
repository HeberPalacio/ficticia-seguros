using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FicticiaBackend.Migrations
{
    public partial class CreateUsuariosTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NombreUsuario",
                table: "Usuarios",
                newName: "Username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Usuarios",
                newName: "NombreUsuario");
        }
    }
}
