using GestionImpresoras.Data;
using GestionImpresoras.Models;
using GestionImpresoras.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionImpresoras.Controllers
{
    public class UnidadesController : Controller
    {
        private readonly ApplicationDBContext _contexto;

        public UnidadesController(ApplicationDBContext contexto)
        {
            this._contexto = contexto;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //Uso de carga diligente con Include
            var lista = await _contexto.Unidades.Include(x => x.Area).ToListAsync();
            return View(lista);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            {
                ViewBag.AreaId = _contexto.Areas.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
                ViewBag.UnidadId = _contexto.Unidades.Where(m => m.AreaId == 0).Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
                return View();
            }

            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  //Para validar ataques 
        public async Task<IActionResult> Crear(Unidad unidad)
        {
            if ((ModelState.IsValid)  || (unidad.AreaId != 0))
            {
                _contexto.Unidades.Add(unidad);
                await _contexto.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            else
            {
                ModelState.AddModelError("AreaId", "Debe seleccionar un área.");
                return RedirectToAction("~/Shared/Error");
            }
           
        }


    }
}
