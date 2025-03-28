﻿using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using System;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class InventarioRepositorio : Repositorio<Inventario>, IInventarioRepositorio
    {
        private readonly ApplicationDbContext _cnx;

        public InventarioRepositorio(ApplicationDbContext cnx):base(cnx) 
        {
            _cnx= cnx;
        }

        public void Actualizar(Inventario inventario)
        {
            var inventarioDB = _cnx.Inventarios.FirstOrDefault(b => b.Id == inventario.Id);

            if (inventarioDB != null)
            {
                inventarioDB.BodegaId= inventario.BodegaId;
                inventarioDB.FechaFinal = inventario.FechaFinal;
                inventarioDB.Estado=inventario.Estado;
                
                _cnx.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj)
        {
            if(obj == "Bodega")
            {
                return _cnx.Bodegas.Where(b => b.Estado == true).Select(b => new SelectListItem
                {
                    Text=b.Nombre,
                    Value=b.Id.ToString()
                });
            }
            return null;
        }
    }
}
