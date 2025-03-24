using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using System;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class MarcaRepositorio : Repositorio<Marca>, IMarcaRepositorio
    {
        private readonly ApplicationDbContext _cnx;

        public MarcaRepositorio(ApplicationDbContext cnx) : base(cnx)
        {
            _cnx = cnx;
        }

        public void Actualizar(Marca marca)
        {
            var MarcaId = _cnx.Marcas.FirstOrDefault(f => f.Id == marca.Id);
            if (MarcaId != null)
            {
                MarcaId.Nombre = marca.Nombre;
                MarcaId.Descripcion = marca.Descripcion;
                MarcaId.Estado = marca.Estado;
                _cnx.SaveChanges();
            }
        }

        
    }
}
