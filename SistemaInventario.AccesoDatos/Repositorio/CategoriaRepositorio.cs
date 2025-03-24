using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using System;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {
        private readonly ApplicationDbContext _cnx;
        public CategoriaRepositorio(ApplicationDbContext cnx) : base(cnx)
        {
            _cnx = cnx;
        }

        public void Actualizar(Categoria categoria)
        {
            var Categoria = _cnx.Categorias.FirstOrDefault(f => f.Id == categoria.Id);
            if (Categoria != null)
            {
                Categoria.Nombre = categoria.Nombre;
                Categoria.Descripcion = categoria.Descripcion;
                Categoria.Estado = categoria.Estado;
                _cnx.SaveChanges();
            }
        }
    }
}
