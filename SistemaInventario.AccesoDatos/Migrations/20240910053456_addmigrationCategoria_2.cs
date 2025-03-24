using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaInventario.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class addmigrationCategoria_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Estado",
                table: "Categorias",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Estado",
                table: "Categorias",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);
        }
    }
}
