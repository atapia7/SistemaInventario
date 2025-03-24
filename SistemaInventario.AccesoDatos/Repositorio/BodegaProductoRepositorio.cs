using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using System;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class BodegaProductoRepositorio : Repositorio<BodegaProducto>, IBodegaProductoRepositorio
    {
        private readonly ApplicationDbContext _cnx;

        public BodegaProductoRepositorio(ApplicationDbContext cnx):base(cnx) 
        {
            _cnx= cnx;
        }

        public void Actualizar(BodegaProducto bodegaproducto)
        {
            var bodegaproductoDB= _cnx.BodegasProductos.FirstOrDefault(b=>b.Id==bodegaproducto.Id);

            if(bodegaproductoDB != null)
            {
                bodegaproductoDB.Cantidad = bodegaproducto.Cantidad;
                _cnx.SaveChanges();
            }
        }
    }
}
