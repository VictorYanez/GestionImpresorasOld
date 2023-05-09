using GestionImpresoras.Models;
using Microsoft.AspNetCore.Mvc;
using GestionImpresoras.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using GestionImpresoras.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionImpresoras.Controllers
{
    public class ImpresorasController : Controller
    {
        private readonly ApplicationDBContext _contexto;

        public ImpresorasController(ApplicationDBContext contexto)
        {
            _contexto = contexto;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var lista = await _contexto.Impresoras.Include(x =>x.Estado).ToListAsync();
            return View(lista);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            ImpresoraViewModel impresoraViewModel = new()
            {
                vImpresora = new Impresora(),
                vListaEstado = _contexto.Estados.Select(estado => new SelectListItem()
                {
                    Text = estado.Nombre,
                    Value = estado.Id.ToString()
                }).ToList(),
                vListaMarca = _contexto.Marcas.Select(marca => new SelectListItem()
                {
                    Text = marca.Nombre,
                    Value = marca.Id.ToString()
                }).ToList(),
                vListaModelo = _contexto.Modelos.Select(modelo => new SelectListItem()
                {
                    Text = modelo.Nombre,
                    Value = modelo.Id.ToString()
                }).ToList(),
                vListaArea = _contexto.Areas.Select(area => new SelectListItem()
                {
                    Text = area.Nombre,
                    Value = area.Id.ToString()
                }).ToList(),
                vListaUnidad = _contexto.Unidades.Select(unidad => new SelectListItem()
                {
                    Text = unidad.Nombre,
                    Value = unidad.Id.ToString()
                }).ToList()
            };

            return View(impresoraViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  //Para validar ataques 
        public async Task<IActionResult> Crear(ImpresoraViewModel impresoraViewModel)
        {
            if (ModelState.IsValid)
            {
                _contexto.Impresoras.Add(impresoraViewModel.vImpresora);
                await _contexto.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Editar(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }

            var contacto = _contexto.Impresoras.Find(id);

            if (contacto == null)
            {
                return NotFound();
            }

            return View(contacto);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Impresora impresora)
        {
            if (ModelState.IsValid)
            {
                _contexto.Update(impresora);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));  //  return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Borrar(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }

            var contacto = _contexto.Impresoras.Find(id);

            if (contacto == null)
            {
                return NotFound();
            }

            return View(contacto);

        }

        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]  //Para validar ataques 
        public async Task<IActionResult> BorrarImpresora(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }

            var contacto = await _contexto.Impresoras.FindAsync(id);

            if (contacto == null)
            {
                return NotFound();
            }

            // Borrado de registro
            _contexto.Impresoras.Remove(contacto);
            await _contexto.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult NoEncontrado()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
