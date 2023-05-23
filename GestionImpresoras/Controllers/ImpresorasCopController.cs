using GestionImpresoras.Models;
using Microsoft.AspNetCore.Mvc;
using GestionImpresoras.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using GestionImpresoras.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionImpresoras.Controllers
{
    // Controlador
    public class ImpresoraCopController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ImpresoraCopController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Acción para mostrar el formulario de creación
        public IActionResult Create()
        {
            ViewBag.Marcas = new SelectList(_context.Marcas, "Id", "Nombre");
            ViewBag.Modelos = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Estados = new SelectList(_context.Estados, "Id", "Nombre");
            return View();
        }

        // Acción para procesar el formulario de creación
        [HttpPost]
        public IActionResult Create(Impresora impresora)
        {
            if (ModelState.IsValid)
            {
                _context.Impresoras.Add(impresora);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Marcas = new SelectList(_context.Marcas, "Id", "Nombre", impresora.MarcaId);
            ViewBag.Modelos = new SelectList(_context.Modelos.Where(m => m.MarcaId == impresora.MarcaId), "Id", "Nombre", impresora.ModeloId);
            ViewBag.Estados = new SelectList(_context.Estados, "Id", "Nombre", impresora.EstadoId);
            return View(impresora);
        }

        // Acción para mostrar el formulario de edición
        public IActionResult Edit(int id)
        {
            var impresora = _context.Impresoras.Find(id);
            if (impresora == null)
            {
                return NotFound();
            }

            ViewBag.Marcas = new SelectList(_context.Marcas, "Id", "Nombre", impresora.MarcaId);
            ViewBag.Modelos = new SelectList(_context.Modelos.Where(m => m.MarcaId == impresora.MarcaId), "Id", "Nombre", impresora.ModeloId);
            ViewBag.Estados = new SelectList(_context.Estados, "Id", "Nombre", impresora.EstadoId);
            return View(impresora);
        }

        // Acción para procesar el formulario de edición
        [HttpPost]
        public IActionResult Edit(int id, Impresora impresora)
        {
            if (id != impresora.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Entry(impresora).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Marcas = new SelectList(_context.Marcas, "Id", "Nombre", impresora.MarcaId);
            ViewBag.Modelos = new SelectList(_context.Modelos.Where(m => m.MarcaId == impresora.MarcaId), "Id", "Nombre", impresora.ModeloId);
            ViewBag.Estados = new SelectList(_context.Estados, "Id", "Nombre", impresora.EstadoId);
            return View(impresora);
        }

        // Acción para obtener los modelos por marca
        public JsonResult GetModelos(int marcaId)
        {
            var modelos = _context.Modelos.Where(m => m.MarcaId == marcaId).ToList();
            return Json(new SelectList(modelos, "Id", "Nombre"));
        }
    }

}