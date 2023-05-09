using GestionImpresoras.Data;
using GestionImpresoras.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionImpresoras.Controllers
{
    public class MarcasController : Controller
    {
        private readonly ApplicationDBContext _contexto;

        public MarcasController(ApplicationDBContext contexto)
        {
            this._contexto = contexto;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var listado = await _contexto.Marcas.ToListAsync();
            return View(listado);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Marca marca)
        {
            if (ModelState.IsValid)
            {
                _contexto.Marcas.Add(marca);
                await _contexto.SaveChangesAsync();
                return RedirectToAction("Index");    // RedirectToAction(nameof(Index))
            }
            return View();
        }
    }
}
