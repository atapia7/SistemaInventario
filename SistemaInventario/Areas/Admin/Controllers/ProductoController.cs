using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Modelos.ViewModels;
using SistemaInventario.Utilidades;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Inventario)]
    public class ProductoController : Controller
    {
        private readonly IUnidadTrabajo unitofwork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductoController(IUnidadTrabajo Iunitofwork,IWebHostEnvironment env)
        {
            unitofwork = Iunitofwork;
            _webHostEnvironment = env;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                CategoriaLista = unitofwork.Producto.ObtenerTodosDropdownLista("Categoria"),
                MarcaLista = unitofwork.Producto.ObtenerTodosDropdownLista("Marca"),
                PadreLista=unitofwork.Producto.ObtenerTodosDropdownLista("Producto")
            };

            if (id == null)
            {
                productoVM.Producto.Estado = true;
                return View(productoVM);
            }
            else
            {
                productoVM.Producto = await unitofwork.Producto.Obtener(id.GetValueOrDefault());
                if(productoVM.Producto == null)
                {
                    return NotFound();
                }
                return View(productoVM);

            }
        }

        #region API

        [HttpPost]
        public async Task<IActionResult> Upsert(ProductoVM prod)
        {
            if(ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath=_webHostEnvironment.WebRootPath;

                if (prod.Producto.Id == 0) {
                    //Crear
                    string upload = webRootPath + DS.ImagenRuta;
                    string filename=Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    prod.Producto.ImagenUrl=filename+extension;
                    await unitofwork.Producto.Agregar(prod.Producto);
                }
                else
                {
                    var objProd=await unitofwork.Producto.ObtenerPrimero(f=>f.Id==prod.Producto.Id,isTracking:false);

                    if (files.Count > 0)
                    {
                        string upload=webRootPath+DS.ImagenRuta;
                        string filename=Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);
                        //borrar la img anterior
                        var anteriorFile=Path.Combine(upload,objProd.ImagenUrl);
                        if (System.IO.File.Exists(anteriorFile))
                        {
                            System.IO.File.Delete(anteriorFile);
                        }
                        using(var fileStream=new FileStream(Path.Combine(upload,filename+extension),FileMode.Create))
                        {
                            files[0].CopyTo((Stream)fileStream);
                        }
                        prod.Producto.ImagenUrl=filename+extension;
                    }
                    else
                    {
                        prod.Producto.ImagenUrl=objProd.ImagenUrl;
                    }
                    unitofwork.Producto.Actualizar(prod.Producto);
                }
                TempData[DS.Exitosa] = "transaccion exitosa ";
                await unitofwork.Guardar();
                return View("Index");
            }//if modelState is not valid

            prod.CategoriaLista = unitofwork.Producto.ObtenerTodosDropdownLista("Categoria");
            prod.MarcaLista= unitofwork.Producto.ObtenerTodosDropdownLista("Marca");
            prod.PadreLista = unitofwork.Producto.ObtenerTodosDropdownLista("Producto");

            return View(prod);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            IEnumerable<Producto> todos=await unitofwork.Producto.ObtenerTodos(incluirPropiedades:"Categoria,Marca");
            return Ok(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var findProductoDb = await unitofwork.Producto.Obtener(id);
            if(findProductoDb == null){
                return Ok(new {success=false,message="Error al borrar Producto" });
            }
            string upload = _webHostEnvironment.WebRootPath + DS.ImagenRuta;
            var anteriorFile = Path.Combine(upload, findProductoDb.ImagenUrl);
            if (System.IO.File.Exists(anteriorFile))
            {
                System.IO.File.Delete(anteriorFile);
            }
            unitofwork.Producto.Remover(findProductoDb);
            await unitofwork.Guardar();
            return Json(new { success = true, message = "Producto borrado exitosamente" });
        }

        [ActionName("ValidarSerie")]
        public async Task<IActionResult> ValidarSerie(string serie, int id = 0)
        {
            bool valor = false;
            IEnumerable<Producto> lista =await unitofwork.Producto.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(p => p.NumeroSerie.ToLower().Trim() == serie.ToLower().Trim());
            }
            else
            {
                valor=lista.Any(p=>p.NumeroSerie.ToLower().Trim()==serie.ToLower().Trim() && p.Id != id);
            }
            return Ok(new {data= valor});

        }
        #endregion

    }
}
