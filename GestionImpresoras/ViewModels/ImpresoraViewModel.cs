using GestionImpresoras.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionImpresoras.ViewModels
{
    public class ImpresoraViewModel
    {
        public Impresora vImpresora { get; set; }
        public List<SelectListItem> vListaEstado { get; set; }  
    }
}
