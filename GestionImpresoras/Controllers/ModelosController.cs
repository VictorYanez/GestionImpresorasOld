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
            ModeloViewModel modeloViewModel = new()
            {
                vModelo = new Modelo(),
                vListaMarcas = _contexto.Marcas.Select(marca => new SelectListItem()
                {
                    Text = marca.Descripcion,
                    Value = marca.Id.ToString()
                }).ToList()
            };

            return View(modeloViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  //Para validar ataques 
        public async Task<IActionResult> Crear(ModeloViewModel modeloViewModel)
        {
            if (ModelState.IsValid)
            {
                _contexto.Modelos.Add(modeloViewModel.vModelo);
                await _contexto.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View();
        }

    }
}
