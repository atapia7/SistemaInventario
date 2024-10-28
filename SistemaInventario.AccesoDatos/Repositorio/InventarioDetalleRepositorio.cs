using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using System;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class InventarioDetalleRepositorio : Repositorio<InventarioDetalle>, IInventarioDetalleRepositorio
    {
        private readonly ApplicationDbContext _cnx;

        public InventarioDetalleRepositorio(ApplicationDbContext cnx) : base(cnx)
        {
            _cnx = cnx;
        }

        public void Actualizar(InventarioDetalle inventarioDet)
        {
            var find_inventarioDetalle = _cnx.InventarioDetalles.FirstOrDefault(f => f.Id == inventarioDet.Id);
            if (find_inventarioDetalle != null)
            {

                find_inventarioDetalle.StockAnterior = inventarioDet.StockAnterior;
                find_inventarioDetalle.Cantidad = inventarioDet.Cantidad;

                _cnx.SaveChanges();
            }
        }
    }

}
