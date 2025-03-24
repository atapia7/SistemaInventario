using Microsoft.AspNetCore.Http;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using System;
using System.Security.Claims;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class UsuarioAplicacionRepositorio : Repositorio<UsuarioAplicacion>, IUsuarioAplicacionRepositorio
    {
        private readonly ApplicationDbContext _cnx;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UsuarioAplicacionRepositorio(ApplicationDbContext cnx,IHttpContextAccessor httpContextAccessor):base(cnx)
        {
            _cnx = cnx;
            _httpContextAccessor = httpContextAccessor;
        }

        public UsuarioAplicacion GetUserSession()
        {
            var SessionUsuarioActual=new UsuarioAplicacion();

            var claimIdentity = _httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            SessionUsuarioActual=_cnx.UsuarioAplicacion.Where(u=>u.Id==userId).FirstOrDefault();
            var claim = claimIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            if (claim?.Value == null)
            {
                return null;
            }
            return SessionUsuarioActual;
        }
    }
}
