using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Modelos.ErrorViewModels;
using SistemaInventario.Utilidades;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]
    public class MarcaController : Controller
    {

        private readonly IUnidadTrabajo unitofwork;

        public MarcaController(IUnidadTrabajo _unitofwork)
        {
            unitofwork = _unitofwork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? Id)
        {
            Marca marca = new Marca();
            
            if(Id == null)
            {
                marca.Estado = true;
                return View(marca);
            }
            marca =await unitofwork.Marca.Obtener(Id.GetValueOrDefault());

            return marca == null? NotFound(): View(marca);
            
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos() {
            IEnumerable<Marca> getall = await unitofwork.Marca.ObtenerTodos();
            if(getall == null)
            {
                return NotFound();
            }
            return Ok(new { data = getall });
        }


        [HttpPost][ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Marca marca)
        {
            if(ModelState.IsValid)
            {
                if(marca.Id == 0)
                {
                    await unitofwork.Marca.Agregar(marca);
                    TempData[DS.Exitosa] = $"Marca {marca.Nombre} registrada exitosamente";
                }
                else
                {
                    unitofwork.Marca.Actualizar(marca);
                    TempData[DS.Exitosa]= $"Marca {marca.Nombre} Actualizada exitosamente";
                }
                await unitofwork.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al actualizar/registrar marca";
            return View(marca);
        }

        [HttpPost] public async Task<IActionResult> Delete(int id)
        {
            var find= await unitofwork.Marca.Obtener(id);
            if(find==null)
            {
                return Ok(new { success = false, message = "Error al eliminar Categoria" });
            }
            unitofwork.Marca.Remover(find);
            await unitofwork.Guardar();
            return Ok(new { success = true, message = $"Marca {find.Nombre} fue eliminado correctamente" });
            
        }

        [HttpGet]public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            IEnumerable<Marca> all = await unitofwork.Marca.ObtenerTodos();
            if(id==0)
            {
                valor=all.Any(a=>a.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                valor = all.Any(a => a.Nombre.ToLower().Trim() == nombre.ToLower().Trim() && a.Id != id);
            }

            return Ok(new { data = valor });
        }






    }
}
