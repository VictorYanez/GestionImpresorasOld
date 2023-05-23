using System.Web;

using GestionImpresoras.Models;
using Microsoft.AspNetCore.Mvc;
using GestionImpresoras.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using GestionImpresoras.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;   //Se usa entre otros para actualizar dinámicamente las listadesplegables 

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
            var lista = await _contexto.Impresoras
                .Include(x => x.Marca)
                .Include(x => x.Modelo)
                .Include(x => x.Estado).ToListAsync();
            return View(lista);
        }

        // GET: Impresoras/Creater
        public IActionResult Crear()
        {
            ViewBag.EstadoId = _contexto.Estados.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nombre }).ToList();
            ViewBag.MarcaId = _contexto.Marcas.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
            ViewBag.ModeloId = _contexto.Modelos.Where(m => m.MarcaId == 0).Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
            ///<!----------------------  Segundo Grupo de SelectListItems --------------------------->
            ViewBag.InstitucionId = _contexto.Instituciones.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nombre }).ToList();
            ViewBag.AreaId = _contexto.Areas.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Nombre }).ToList();
            ViewBag.UnidadId = _contexto.Unidades.Where(a =>a.AreaId == 0).Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Nombre }).ToList();
            return View();
        }

        // Devolver listados de Modelos correpondientes a dicha marca
        public JsonResult GetModelos(int marcaId)
        {
            var modelos = _contexto.Modelos.Where(m => m.MarcaId == marcaId).Select(m => new { id = m.Id, nombre = m.Nombre }).ToList();
            return Json(modelos);
        }

        // Devolver listados de unidades correpondientes a dicha área
        public JsonResult GetUnidades(int areaId)
        {
            var unidades = _contexto.Unidades.Where(a => a.AreaId == areaId).Select(a => new { id = a.Id, nombre = a.Nombre }).ToList();
            return Json(unidades);
        }

        [HttpGet]
        public IActionResult CrearViewModel()
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
        public async Task<IActionResult> Crear(Impresora impresora)
        {
            if (ModelState.IsValid)
            {
                _contexto.Impresoras.Add(impresora);
                await _contexto.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View();
        }


        // Codigo Video Hector de Leon 
        [HttpGet]
        public ActionResult<List<SelectListItem>> ListaModelos(int marcaId)
        {
            List<SelectListItem> listaMarcas = new List<SelectListItem>();

            if (marcaId != 0)
            {
                listaMarcas = (from m in _contexto.Modelos
                               where m.MarcaId == marcaId
                               select new SelectListItem
                               {
                                   Text = m.Nombre,
                                   Value = m.Id.ToString(),

                               }).ToList();
            }

            return View(listaMarcas);
        }

        [HttpGet("{marcaId}/modelo")]
        // Codigo FG  para listado de Modelos en vistas Blazor
        public async Task<List<Modelo>> WhenMarcaChanged(int marcaId)
        {
            //if (marcaId != 0)
            {
                return await _contexto.Modelos.Where(m => m.MarcaId == marcaId).OrderBy(m => m.Nombre).ToListAsync();
            }
 
        }

        // Codigo Video Codigo Oldest & FG
        [HttpGet("{marcaId}/modelo")]
        // Codigo FG  para listado de Modelos en vistas Blazor
        public async Task<List<Modelo>> WhenMarcaChanged2(int marcaId)
        {
            List<SelectListItem> listaMarcas = new List<SelectListItem>();

            {
                return await _contexto.Modelos.Where(m => m.MarcaId == marcaId).OrderBy(m => m.Nombre).ToListAsync();
            }
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

        // Acción para mostrar el formulario de creación codigo Copiloto 

        // Acción para mostrar el formulario de edición
        public IActionResult EditCop(int id)
        {
            var impresora = _contexto.Impresoras.Find(id);
            if (impresora == null)
            {
                return NotFound();
            }

            ViewBag.Marcas = new SelectList(_contexto.Marcas, "Id", "Nombre", impresora.MarcaId);
            ViewBag.Modelos = new SelectList(_contexto.Modelos.Where(m => m.MarcaId == impresora.MarcaId), "Id", "Nombre", impresora.ModeloId);
            ViewBag.Estados = new SelectList(_contexto.Estados, "Id", "Nombre", impresora.EstadoId);
            return View(impresora);
        }

        // Acción para procesar el formulario de edición
        [HttpPost]
        public IActionResult EditCop(int id, Impresora impresora)
        {
            if (id != impresora.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _contexto.Entry(impresora).State = EntityState.Modified;
                _contexto.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Marcas = new SelectList(_contexto.Marcas, "Id", "Nombre", impresora.MarcaId);
            ViewBag.Modelos = new SelectList(_contexto.Modelos.Where(m => m.MarcaId == impresora.MarcaId), "Id", "Nombre", impresora.ModeloId);
            ViewBag.Estados = new SelectList(_contexto.Estados, "Id", "Nombre", impresora.EstadoId);
            return View(impresora);
        }

        //-----------------------------------------
        //<!----------------    (IA)   --------------------------->

        // GET: Impresoras/Create
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_contexto.Estados, "Id", "Nombre");
            ViewData["MarcaId"] = new SelectList(_contexto.Marcas, "Id", "Nombre");
            ViewData["ModeloId"] = new SelectList(_contexto.Modelos.Where(m => m.MarcaId == 0), "Id", "Nombre");
            return View();
        }

        // POST: Impresoras/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CodigoActivoFijo,MarcaId,ModeloId,EstadoId,DireccionIP")] Impresora impresora)
        {
            if (ModelState.IsValid)
            {
                _contexto.Add(impresora);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_contexto.Estados, "Id", "Nombre", impresora.EstadoId);
            ViewData["MarcaId"] = new SelectList(_contexto.Marcas, "Id", "Nombre", impresora.MarcaId);
            ViewData["ModeloId"] = new SelectList(_contexto.Modelos.Where(m => m.MarcaId == impresora.MarcaId), "Id", "Nombre", impresora.ModeloId);
            return View(impresora);
        }

        //// Acción para obtener los modelos por marca
        //public JsonResult GetModelos(int marcaId)
        //{
        //    var modelos = _contexto.Modelos.Where(m => m.MarcaId == marcaId).ToList();
        //    return Json(new SelectList(modelos, "Id", "Nombre"));
        //}


        //<!-----------------------   Para vista Creater ---------------------->
        // GET: Impresoras/Creater
        public IActionResult Creater()
        {
            ViewBag.EstadoId = _contexto.Estados.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nombre }).ToList();
            ViewBag.MarcaId = _contexto.Marcas.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
            ViewBag.ModeloId = _contexto.Modelos.Where(m => m.MarcaId == 0).Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
            return View();
        }

        // POST: Impresoras/Creater
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Creater([Bind("Id,CodigoActivoFijo,MarcaId,ModeloId,EstadoId,DireccionIP")] Impresora impresora)
        {
            if (ModelState.IsValid)
            {
                _contexto.Add(impresora);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.EstadoId = _contexto.Estados.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nombre }).ToList();
            ViewBag.MarcaId = _contexto.Marcas.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
            ViewBag.ModeloId = _contexto.Modelos.Where(m => m.MarcaId == impresora.MarcaId).Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
            return View(impresora);
        }

        // Falla, podría ser el codigo JavaScript 
        [HttpGet]
        public JsonResult GetModelosByMarcaId(int marcaId)
        {
            var modelos = _contexto.Modelos.Where(m => m.MarcaId == marcaId).Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
            return Json(modelos);
        }

        //<!----------------    Copiloto --------------------------->
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
