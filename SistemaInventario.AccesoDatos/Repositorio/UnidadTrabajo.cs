﻿using SistemaInventario.AccesoDatos.Data;
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

        public IUsuarioAplicacionRepositorio UsuarioAplicacion { get; private set; }

        public IBodegaProductoRepositorio BodegaProducto { get; private set; }

        public IInventarioRepositorio Inventario { get; private set; }

        public IInventarioDetalleRepositorio InventarioDetalle { get; set; }

        public IKardexInventarioRepositorio KardexInventario { get; set; }


        public UnidadTrabajo(ApplicationDbContext _cnx)
        {
            cnx = _cnx;
            Marca = new MarcaRepositorio(cnx);
            Bodega =new BodegaRepositorio(cnx);
            Producto = new ProductoRepositorio(cnx);
            Categoria=new CategoriaRepositorio(cnx);
            Inventario = new InventarioRepositorio(cnx);
            BodegaProducto = new BodegaProductoRepositorio(cnx);
            UsuarioAplicacion=new UsuarioAplicacionRepositorio(cnx);
            InventarioDetalle=new InventarioDetalleRepositorio(cnx);
            KardexInventario = new KardexInventarioRepositorio(cnx);
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
