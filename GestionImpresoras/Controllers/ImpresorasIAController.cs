using GestionImpresoras.Data;
using GestionImpresoras.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionImpresoras.Controllers
{
    public class ImpresorasIAController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ImpresorasIAController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Impresoras/Create
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Nombre");
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Nombre");
            ViewData["ModeloId"] = new SelectList(_context.Modelos.Where(m => m.MarcaId == 0), "Id", "Nombre");
            return View();
        }

        // POST: Impresoras/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CodigoActivoFijo,MarcaId,ModeloId,EstadoId,DireccionIP")] Impresora impresora)
        {
            if (ModelState.IsValid)
            {
                _context.Add(impresora);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Nombre", impresora.EstadoId);
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Nombre", impresora.MarcaId);
            ViewData["ModeloId"] = new SelectList(_context.Modelos.Where(m => m.MarcaId == impresora.MarcaId), "Id", "Nombre", impresora.ModeloId);
            return View(impresora);
        }

        // GET: Impresoras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var impresora = await _context.Impresoras.FindAsync(id);
            if (impresora == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Nombre", impresora.EstadoId);
            ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Nombre", impresora.MarcaId);
            ViewData["ModeloId"] = new SelectList(_context.Modelos.Where(m => m.MarcaId == impresora.MarcaId), "Id", "Nombre", impresora.ModeloId);
            return View(impresora);
        }

        
        //// POST: Impresoras/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,CodigoActivoFijo,MarcaId,ModeloId,EstadoId,DireccionIP")] Impresora impresora)
        //{
        //    if (id != impresora.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(impresora);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ImpresoraExists(impresora.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }

        //    ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Nombre", impresora.EstadoId);
        //    ViewData["MarcaId"] = new SelectList(_context.Marcas, "Id", "Nombre", impresora.MarcaId);
        //    ViewData["ModeloId"] = new SelectList(_context.Modelos.Where(m => m.MarcaId == impresora.MarcaId));
        //}
    }

}
