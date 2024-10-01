using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext cnx;  
        public  IBodegaRepositorio Bodega { get; private set; }
        public ICategoriaRepositorio Categoria { get; private set; }
        public IMarcaRepositorio Marca { get;private set; }

        public IProductoRepositorio Producto { get; private set; }


        public UnidadTrabajo(ApplicationDbContext _cnx)
        {
            cnx = _cnx;
            Bodega =new BodegaRepositorio(cnx);
            Categoria=new CategoriaRepositorio(cnx);
            Marca = new MarcaRepositorio(cnx);
            Producto = new ProductoRepositorio(cnx);
        }

        public void Dispose()
        {
            cnx.Dispose();
        }

        public async Task Guardar()
        {
            await cnx.SaveChangesAsync();
        }
    }
}
