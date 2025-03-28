﻿using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using System;

namespace SistemaInventario.AccesoDatos.Repositorio;

public class BodegaRepositorio : Repositorio<Bodega>, IBodegaRepositorio
{
    private readonly ApplicationDbContext _cnx;

    public BodegaRepositorio(ApplicationDbContext cnx):base(cnx) 
    {
        _cnx= cnx;
    }

    public void Actualizar(Bodega bodega)
    {
        var BodegaDb= _cnx.Bodegas.FirstOrDefault(f=>f.Id==bodega.Id);
        if (BodegaDb!=null)
        {
            BodegaDb.Nombre=bodega.Nombre;
            BodegaDb.Descripcion=bodega.Descripcion;
            BodegaDb.Estado=bodega.Estado;
            _cnx.SaveChanges();
        }
    }
}
