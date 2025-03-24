using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using System;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {
        private readonly ApplicationDbContext _cnx;

        public ProductoRepositorio(ApplicationDbContext cnx) : base(cnx)
        {
            _cnx = cnx;
        }

        public void Actualizar(Producto prod)
        {
            var ProdFirst = _cnx.Productos.FirstOrDefault(f => f.Id == prod.Id);
            if (ProdFirst != null)
            {
                if (prod.ImagenUrl != null)
                {
                    ProdFirst.ImagenUrl = prod.ImagenUrl;
                }
                ProdFirst.NumeroSerie = prod.NumeroSerie;
                ProdFirst.Descripcion = prod.Descripcion;
                ProdFirst.Precio = prod.Precio;
                ProdFirst.Costo = prod.Costo;
                ProdFirst.CategoriaId = prod.CategoriaId;
                ProdFirst.MarcaId = prod.MarcaId;
                ProdFirst.PadreId = prod.PadreId;
                ProdFirst.Estado = prod.Estado;
                _cnx.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj)
        {
            if (obj == "Categoria")
            {
                return _cnx.Categorias.Where(f => f.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }
            if (obj == "Marca")
            {
                return _cnx.Marcas.Where(f => f.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }
            if (obj == "Producto")
            {
                return _cnx.Productos.Where(f => f.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Descripcion,
                    Value = c.Id.ToString()
                });
            }
            return null;

        }
        public async Task<List<Producto>> getprod()
        {
            var prodSum = await _cnx.Database.SqlQuery<Producto>(@$"""
        SELECT p.ProductId, p.ProductName, SUM(oi.Quantity * oi.UnitPrice) AS TotalSales
        FROM Products p
        JOIN OrderItems oi ON p.ProductId = oi.ProductId
        WHERE p.CategoryId = 1
        GROUP BY p.ProductId, p.ProductName
        """).ToListAsync();
            return prodSum;
        }
    }
    
}
