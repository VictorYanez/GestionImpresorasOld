using GestionImpresoras.Data;
using GestionImpresoras.Models;
using GestionImpresoras.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionImpresoras.Controllers
{
    public class ModelosController : Controller
    {
        private readonly ApplicationDBContext _contexto;

        public ModelosController(ApplicationDBContext contexto)
        {
            this._contexto = contexto;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var lista = await _contexto.Modelos.Include(x => x.Marca).ToListAsync();
            return View(lista);
        }


        [HttpGet]
        public IActionResult Crear()
        {
            {
                ViewBag.MarcaId = _contexto.Marcas.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
                ViewBag.ModeloId = _contexto.Modelos.Where(m => m.MarcaId == 0).Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
                return View();
            }
        }


            //[HttpGet]
            //public IActionResult Creater()
            //{
            //    ModeloViewModel modeloViewModel = new()
            //    {
            //        vModelo = new Modelo(),
            //        vListaMarcas = _contexto.Marcas.Select(marca => new SelectListItem()
            //        {
            //            Text = marca.Descripcion,
            //            Value = marca.Id.ToString()
            //        }).ToList()
            //    };

            //    return View(modeloViewModel);
            //}

            [HttpPost]
            [ValidateAntiForgeryToken]  //Para validar ataques 
            public async Task<IActionResult> Crear(Modelo modelo)
            {
                if ((ModelState.IsValid)  || (modelo.MarcaId != 0))
                {
                    _contexto.Modelos.Add(modelo);
                    await _contexto.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
            {
                ModelState.AddModelError("MarcaId", "Debe seleccionar un área.");
                return RedirectToAction("~/Shared/Error");
            }
           
            }

            // Este codigo es el que funciona 
            public JsonResult GetModelos(int MarcaId)
            {
                var modelos = _contexto.Modelos.Where(m => m.MarcaId == MarcaId).Select(m => new { id = m.Id, nombre = m.Nombre }).ToList();
                return Json(modelos);
            }
      
    }
}
