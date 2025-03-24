using Microsoft.EntityFrameworkCore;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using System;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class KardexInventarioRepositorio : Repositorio<KardexInventario>, IKardexInventarioRepositorio
    {
        private readonly ApplicationDbContext _cnx;

        public KardexInventarioRepositorio(ApplicationDbContext cnx):base(cnx) 
        {
            _cnx= cnx;
        }

        public async Task RegistrarKarde(int bodegaProductoId, string tipo, string detalle, int stockAnterior, int cantidad, string UsuarioId)
        {
            var bodegaProducto = await _cnx.BodegasProductos.Include(b => b.Producto).FirstOrDefaultAsync(b=>b.Id==bodegaProductoId);

            if (tipo == "Entrada")
            {
                KardexInventario kardex= new KardexInventario();
                kardex.BodegaProductoId= bodegaProductoId;
                kardex.Tipo= tipo;
                kardex.Detalle= detalle;
                kardex.StockAnterior= stockAnterior;
                kardex.Cantidad= cantidad;
                kardex.Costo = bodegaProducto.Producto.Costo;
                kardex.Stock = stockAnterior + cantidad;
                kardex.Total=kardex.Stock*kardex.Costo;
                kardex.UsuarioAplicacionId = UsuarioId;
            }
        }
    }
}
