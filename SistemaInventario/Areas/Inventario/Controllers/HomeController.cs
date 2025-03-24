using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Modelos.ErrorViewModels;
using SistemaInventario.Modelos.Especificaciones;
using System.Diagnostics;

namespace SistemaInventario.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnidadTrabajo _unidadTrabajo;

        public HomeController(ILogger<HomeController> logger, IUnidadTrabajo unidadTrabajo)
        {
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index(int pageNumber=1, string busqueda="", string busquedaActual="")
        {
            if (!string.IsNullOrEmpty(busqueda)) { pageNumber=1; } else { busqueda = busquedaActual; }

            ViewData["BusquedaActual"] = busqueda;

            if (pageNumber < 1) { pageNumber = 1; }
            Parametros parametros = new Parametros()
            {
                PageNumber = pageNumber,
                PageSize = 4
            };

            var GetProductos = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros);

            if (!string.IsNullOrEmpty(busqueda))
            {
                GetProductos = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros, p => p.Descripcion.Contains(busqueda));
            }

            ViewData["TotalPaginas"]=GetProductos.MetaData.TotalPages;
            ViewData["TotalRegistros"] = GetProductos.MetaData.TotalCount;
            ViewData["PageSize"] = GetProductos.MetaData.TotalCount;
            ViewData["PageNumber"] = pageNumber;
            ViewData["Previo"] = "disabled";
            ViewData["Siguiente"] = "";

            if(pageNumber> 1){ ViewData["Previo"] = ""; }
            if (GetProductos.MetaData.TotalPages <= pageNumber) { ViewData["Siguiente"] = "disabled"; }

            return View(GetProductos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
