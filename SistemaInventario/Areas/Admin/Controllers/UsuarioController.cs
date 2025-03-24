﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Utilidades;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =DS.Role_Admin)]
    public class UsuarioController : Controller
    {
        private readonly IUnidadTrabajo unitofwork;
        private readonly ApplicationDbContext cnx;

        public UsuarioController(IUnidadTrabajo _unitofwork,ApplicationDbContext _cnx)
        {
            unitofwork = _unitofwork;
            cnx = _cnx;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Region API´s

        public async Task<IActionResult> ObtenerTodos()
        {
            var usuarioLista = await unitofwork.UsuarioAplicacion.ObtenerTodos();
            var userRole = cnx.UserRoles.ToListAsync().Result;
            var roles = await cnx.Roles.ToListAsync();
            foreach (var usuario in usuarioLista)
            {
                var roleId=userRole.FirstOrDefault(u=>u.UserId==usuario.Id).RoleId;
                usuario.Role=roles.FirstOrDefault(u=>u.Id==roleId).Name;
            }
            return Ok(new { data = usuarioLista });
        }
        public async Task<IActionResult> BloquearDesbloquear([FromBody] string id)
        {
            var usuariofind=await unitofwork.UsuarioAplicacion.ObtenerPrimero(U=>U.Id==id);
            if (usuariofind == null)
            {
                return Ok(new { success = false, message = "Error de Usuario" });
            }

            if(usuariofind.LockoutEnd!=null && usuariofind.LockoutEnd>DateTime.Now) //usuario bloqueado
            {
                usuariofind.LockoutEnd = DateTime.Now;
            }
            else
            {
                usuariofind.LockoutEnd = DateTime.Now.AddMonths(3);
            }
            await unitofwork.Guardar();
            return Ok(new { success=true,message="Operacion Exitosa :)" });
        }
        
        #endregion
    }
}
