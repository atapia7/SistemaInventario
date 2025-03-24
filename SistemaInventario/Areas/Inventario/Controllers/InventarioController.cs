using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Modelos.ViewModels;
using SistemaInventario.Utilidades;
using System.Security.Claims;

namespace SistemaInventario.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    [Authorize( Roles = DS.Role_Admin + ","+ DS.Role_Inventario)]
    public class InventarioController : Controller
    {
        private readonly IUnidadTrabajo _unitofwork;

        [BindProperty]
        public InventarioVM inventarioVM {  get; set; }


        public InventarioController(IUnidadTrabajo unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> NuevoInventario() {
            inventarioVM = new InventarioVM()
            {
                Inventario = new Modelos.Inventario(),
                BodegaLista = _unitofwork.Inventario.ObtenerTodosDropdownLista("Bodega")
            };
            inventarioVM.Inventario.Estado = false;
            var ClaimIdentity = (ClaimsIdentity)User.Identity;
            var claim=ClaimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            inventarioVM.Inventario.UsuarioAplicacionId=claim.Value;
            inventarioVM.Inventario.UsuarioAplicacion=await _unitofwork.UsuarioAplicacion.ObtenerPrimero(u=>u.Id==claim.Value);
            inventarioVM.Inventario.FechaInicial=DateTime.Now;
            inventarioVM.Inventario.FechaFinal=DateTime.Now;
            
            return View(inventarioVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NuevoInventario(InventarioVM inventarioVM)
        {
            if (ModelState.IsValid)
            {
                inventarioVM.Inventario.FechaInicial = DateTime.Now;
                inventarioVM.Inventario.FechaFinal = DateTime.Now;
                
                await _unitofwork.Inventario.Agregar(inventarioVM.Inventario);
                await _unitofwork.Guardar();
                return RedirectToAction("DetalleInventario", new { id = inventarioVM.Inventario.Id });
            }
            inventarioVM.BodegaLista = _unitofwork.Inventario.ObtenerTodosDropdownLista("Bodega");
            return View(inventarioVM);
        }

        public async Task<IActionResult> DetalleInventario(int id)
        {
            inventarioVM = new InventarioVM();
            inventarioVM.Inventario=await _unitofwork.Inventario.ObtenerPrimero(i=>i.Id==id, incluirPropiedades:"Bodega");
            inventarioVM.InventarioDetalles = await _unitofwork.InventarioDetalle.ObtenerTodos(d => d.InventarioId == id, incluirPropiedades:"Producto,Producto.Marca");           
            
            return View (inventarioVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetalleInventario(int inventarioId, int productoId, int cantidadId)
        {
            inventarioVM = new InventarioVM();
            inventarioVM.Inventario = await _unitofwork.Inventario.ObtenerPrimero(i => i.Id == inventarioId);
            var bodegaProducto = await _unitofwork.BodegaProducto.ObtenerPrimero(b => b.ProductoId == productoId && b.BodegaId == inventarioVM.Inventario.BodegaId);

            var detalle = await _unitofwork.InventarioDetalle.ObtenerPrimero(id => id.InventarioId == inventarioId && id.ProductoId == productoId);

            if (detalle == null)
            {
                inventarioVM.InventarioDetalle = new InventarioDetalle();
                inventarioVM.InventarioDetalle.ProductoId = productoId;
                inventarioVM.InventarioDetalle.InventarioId = inventarioId;

                if (bodegaProducto != null)
                {
                    inventarioVM.InventarioDetalle.StockAnterior = bodegaProducto.Cantidad;
                }
                else
                {
                    inventarioVM.InventarioDetalle.StockAnterior = 0;
                }

                inventarioVM.InventarioDetalle.Cantidad = cantidadId;
                await _unitofwork.InventarioDetalle.Agregar(inventarioVM.InventarioDetalle);
                await _unitofwork.Guardar();

            }
            else
            {
                detalle.Cantidad += cantidadId;
                await _unitofwork.Guardar();
            }
            return RedirectToAction("DetalleInventario", new { id = inventarioId });

        }

        public async Task<IActionResult> Mas(int id) //recibe el id del detalle 
        {
            inventarioVM = new InventarioVM();
            var detalle = await _unitofwork.InventarioDetalle.Obtener(id);
            inventarioVM.Inventario = await _unitofwork.Inventario.Obtener(detalle.InventarioId);

            detalle.Cantidad += 1;
            await _unitofwork.Guardar();
            return RedirectToAction("DetalleInventario",new {id=inventarioVM.Inventario.Id});
        }

        public async Task<IActionResult> Menos(int id) //recibe el id del detalle 
        {
            inventarioVM = new InventarioVM();
            var detalle = await _unitofwork.InventarioDetalle.Obtener(id);
            inventarioVM.Inventario = await _unitofwork.Inventario.Obtener(detalle.InventarioId);
            if(detalle.Cantidad == 1)
            {
                _unitofwork.InventarioDetalle.Remover(detalle);
                await _unitofwork.Guardar();
            }
            else
            {
                detalle.Cantidad -= 1;
                await _unitofwork.Guardar();
            }

            return RedirectToAction("DetalleInventario", new { id = inventarioVM.Inventario.Id });
        }

        public async Task<IActionResult> GenerarStock(int id)
        {
            var inventario = await _unitofwork.Inventario.Obtener(id);
            var detalleLista= await _unitofwork.InventarioDetalle.ObtenerTodos(d=>d.InventarioId==id);
            foreach (var item in detalleLista)
            {
                var bodegaproducto = new BodegaProducto();
                bodegaproducto = await _unitofwork.BodegaProducto.ObtenerPrimero(bp => bp.ProductoId == item.ProductoId && bp.BodegaId == inventario.BodegaId,isTracking:false);
                
                if (inventario != null) //el registro de stock existe, hay que actualizar las cantidades 
                {
                    bodegaproducto.Cantidad += item.Cantidad;
                    await _unitofwork.Guardar();
                }
                else //registro de stock no existe, crearlo
                {
                    bodegaproducto = new BodegaProducto();
                    bodegaproducto.BodegaId= inventario.BodegaId; 
                    bodegaproducto.ProductoId=item.ProductoId;
                    bodegaproducto.Cantidad = item.Cantidad;
                    await _unitofwork.BodegaProducto.Agregar(bodegaproducto);
                    await _unitofwork.Guardar();

                }
            }
            //Actualizar La cabecera del inventario 
            inventario.Estado= true;    
            inventario.FechaFinal=DateTime.Now;
            await _unitofwork.Guardar();

            return RedirectToAction("Index");
        }



        #region APIS


        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var alls = await _unitofwork.BodegaProducto.ObtenerTodos(incluirPropiedades: "Bodega,Producto");

            return Ok(new {data=alls});
        }

        [HttpGet]
        public async Task<IActionResult> BuscarProducto(string term)
        {
            if (!string.IsNullOrEmpty(term))
            {
                var listProd = await _unitofwork.Producto.ObtenerTodos(p => p.Estado == true);
                var data=listProd.Where(x=>x.NumeroSerie.Contains(term,StringComparison.OrdinalIgnoreCase) || 
                                           x.Descripcion.Contains(term,StringComparison.OrdinalIgnoreCase)).ToList();
                return Ok(data);
            }
            return NotFound();
        }

        #endregion
    }
}
