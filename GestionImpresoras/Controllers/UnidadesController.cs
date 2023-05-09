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
            var lista = await _contexto.Unidades.Include(x => x.Area).ToListAsync();
            return View(lista);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            UnidadViewModel unidadViewModel = new()
            {
                vUnidad = new Unidad(),
                vListaAreas = _contexto.Areas.Select(area => new SelectListItem()
                {
                    Text = area.Nombre,
                    Value = area.Id.ToString()
                }).ToList()
            };

            return View(unidadViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  //Para validar ataques 
        public async Task<IActionResult> Crear(UnidadViewModel unidadViewModel)
        {
            if (ModelState.IsValid)
            {
                _contexto.Unidades.Add(unidadViewModel.vUnidad);
                await _contexto.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
