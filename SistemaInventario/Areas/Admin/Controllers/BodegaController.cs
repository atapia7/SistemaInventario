using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BodegaController : Controller
    {

        private readonly IUnidadTrabajo _unitofwork;


        public BodegaController(IUnidadTrabajo unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public IActionResult Index()
        {
            return View();
        }
        #region regionAPIS
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unitofwork.Bodega.ObtenerTodos();

            return Ok(new { data = todos });
        }

        public async Task<IActionResult> Upsert(int? id)
        {
                Bodega bodega = new Bodega();

            if (id == null)
            {
                bodega.Estado = true;
                return View(bodega);
            }
            bodega = await _unitofwork.Bodega.Obtener(id.GetValueOrDefault());

            if (bodega is null)
            {
                return NotFound();
            }

            return View(bodega);
        }
       [HttpPost]
       [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Bodega bodega)
        {
            if (ModelState.IsValid)
            {
                if (bodega.Id == 0)
                {
                    await _unitofwork.Bodega.Agregar(bodega);
                    TempData[DS.Exitosa]="Bodega Creada Exitosamente";
                }
                else
                {
                    _unitofwork.Bodega.Actualizar(bodega);
                    TempData[DS.Exitosa] = "Bodega Actualizada Exitosamente";
                }
                await _unitofwork.Guardar();
                return RedirectToAction(nameof(Index));
            }
                return View(bodega);
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
            {
                bool valor = false;
                var lista = await _unitofwork.Bodega.ObtenerTodos();
                if (id == 0)
                {
                    valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
                }
                else
                {
                    valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim() && b.Id != id);
                }
                if (valor)
                {
                    return Json(new { data = true });
                }
                return Json(new { data = false });

            }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var bodegafind = await _unitofwork.Bodega.Obtener(id);
            if (bodegafind == null)
            {
                return Json(new { success = false, message="Error al borrar bodega" });
            }
           _unitofwork.Bodega.Remover(bodegafind);
            await _unitofwork.Guardar();
            return Ok(new {success=true,message="Se elimino correctamente la bodega {}"});
        }


            #endregion




        }
    }
