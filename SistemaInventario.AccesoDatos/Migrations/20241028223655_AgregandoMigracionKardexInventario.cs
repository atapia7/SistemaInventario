using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaInventario.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class AgregandoMigracionKardexInventario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KardexInventario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BodegaProductoId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Detalle = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StockAnterior = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Costo = table.Column<double>(type: "double", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<double>(type: "double", nullable: false),
                    UsuarioAplicacionId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KardexInventario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KardexInventario_AspNetUsers_UsuarioAplicacionId",
                        column: x => x.UsuarioAplicacionId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KardexInventario_BodegasProductos_BodegaProductoId",
                        column: x => x.BodegaProductoId,
                        principalTable: "BodegasProductos",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_KardexInventario_BodegaProductoId",
                table: "KardexInventario",
                column: "BodegaProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_KardexInventario_UsuarioAplicacionId",
                table: "KardexInventario",
                column: "UsuarioAplicacionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KardexInventario");
        }
    }
}
