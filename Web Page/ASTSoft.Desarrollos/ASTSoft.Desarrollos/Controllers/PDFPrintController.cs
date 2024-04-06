using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Services.Messaging.ViewModels.Etiquetas;
using System.Collections.Generic;

namespace ASTSoft.Desarrollos.Controllers
{

    public class PDFPrintController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BultosEtiquetas()
        {
            return View();
        }

        public IActionResult BultosDocumento()
        {
            return View();
        }

        public IActionResult CajasEtiquetas() { 
            return View();
        }

        public IActionResult GenerarDatamatrix() {
            return View();
        }
    }
}
