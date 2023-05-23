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
        //  Codigo para la vista index con la tabla  
        public async Task<IActionResult> Index()
        {
            //Codigo utilizando merge de codigo Hector de Leon, FG, R2W
            var listado = await _contexto.Marcas.OrderBy(m => m.Nombre).ToListAsync();
            return View(listado);
        }

        [HttpGet]
        // Codigo para Get y SelectListItem de FG
        public async Task<ActionResult<List<Marca>>> Get()
        {
            var listado = await _contexto.Marcas.ToListAsync();
            return listado;
        }


        [HttpGet("{marcaId}/modelo")]
        // Codigo FG  para listado de Modelos en vistas Blazor
        public async Task<List<Modelo>> GetEstados(int marcaId)
        {
            return  await _contexto.Modelos.Where(m => m.MarcaId == marcaId).OrderBy(m => m.Nombre).ToListAsync();
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
