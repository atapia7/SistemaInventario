﻿using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using System;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class UsuarioAplicacionRepositorio : Repositorio<UsuarioAplicacion>, IUsuarioAplicacionRepositorio
    {
        private readonly ApplicationDbContext _cnx;

        public UsuarioAplicacionRepositorio(ApplicationDbContext cnx):base(cnx)
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
