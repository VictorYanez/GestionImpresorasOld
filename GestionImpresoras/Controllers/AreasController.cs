using GestionImpresoras.Data;
using GestionImpresoras.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionImpresoras.Controllers
{
    public class AreasController : Controller
    {
        private readonly ApplicationDBContext _contexto;

        public AreasController(ApplicationDBContext contexto)
        {
            this._contexto = contexto;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var listado = await _contexto.Areas.ToListAsync();
            return View(listado);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Area area)
        {
            if (ModelState.IsValid)
            {
                _contexto.Areas.Add(area);
                await _contexto.SaveChangesAsync();
                return RedirectToAction("Index");    // RedirectToAction(nameof(Index))
            }
            return View();
        }
    }
}
