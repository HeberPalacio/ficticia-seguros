using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FicticiaBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Roles",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AlterColumn<string>(
                name: "EnfermedadOtra",
                table: "Personas",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Roles",
                table: "Usuarios");

            migrationBuilder.AlterColumn<string>(
                name: "EnfermedadOtra",
                table: "Personas",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
