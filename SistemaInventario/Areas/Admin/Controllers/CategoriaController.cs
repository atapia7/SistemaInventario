using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriaController : Controller
    {
        private readonly IUnidadTrabajo _unitofwork;

        public CategoriaController(IUnidadTrabajo unitofwork)
        {
            _unitofwork = unitofwork;   
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            Categoria categoria= new Categoria();
            if (id == null)
            {
                categoria.Estado = true;
                return View(categoria);
            }
            categoria = await _unitofwork.Categoria.Obtener(id.GetValueOrDefault());
            if(categoria==null)
            {
                return NotFound();
            }
            return View(categoria);
        }


        #region APIS
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Categoria categ)
        {
            if(ModelState.IsValid)
            {
                if(categ.Id == 0)
                {
                    await _unitofwork.Categoria.Agregar(categ);
                    TempData[DS.Exitosa] = $"Categoria {categ.Nombre} registrada exitosamente";
                }
                else
                {
                    _unitofwork.Categoria.Actualizar(categ);
                    TempData[DS.Exitosa] = $"Categoria {categ.Nombre} Actualizada correctamente";
                }
                await _unitofwork.Guardar();
                return RedirectToAction(nameof(Index));
            }

            TempData[DS.Exitosa] = "Error al actualizar categoria";
            return View(categ);
        }
       [HttpGet] public async Task<IActionResult> ObtenerTodos()
        {
            var all = await _unitofwork.Categoria.ObtenerTodos();
            if (all == null)
            {
                return BadRequest(new {success=false });
            }
            return Ok(new { data = all });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id) {

            var categoriafind = await _unitofwork.Categoria.Obtener(id);
            
            if (categoriafind == null)
            {
                return Ok(new { success = false, message = "Error al eliminar Categoria" });
            }
            
            _unitofwork.Categoria.Remover(categoriafind);
            await _unitofwork.Guardar();
            
            return Ok(new {success=true,message=$"categoria {categoriafind.Nombre} eliminado correctamente"});
        }

        [HttpGet]
        public async Task<IActionResult> ValidarNombre(string nombre,int id=0)
        {
            bool valor = false;
            IEnumerable<Categoria> allCateg= await _unitofwork.Categoria.ObtenerTodos();
            if(id==0)
            {
                valor = allCateg.Any(f => f.Nombre.ToLower().Trim() == nombre.ToLower().Trim()); 
            }
            else
            {
                valor= allCateg.Any(f=>f.Nombre.ToLower().Trim()==nombre.ToLower().Trim() && f.Id != id);
            }
            
            return Ok(new { data = valor });
        }
        #endregion
    }
}
