using IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using Services.Messaging.ViewModels.Articulos;
using Services.Messaging.ViewModels.AST_Usuarios;
using Services.Messaging.ViewModels.Etiquetas;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ASTSoft.Desarrollos.Controllers
{
    public class ReciboMercaderiaController : Controller
    {
        private readonly IReciboMercaderiaService _IReciboMercaderiaService;  

        public ReciboMercaderiaController(IReciboMercaderiaService iReciboMercaderiaService)
        {
            _IReciboMercaderiaService = iReciboMercaderiaService;
        }

        public IActionResult Index(EtiquetasView model)
        {
            model.articulos = new List<ArticulosView>();

            if (model.OrdenCompra != null)
            {
                model.articulos = _IReciboMercaderiaService.ObtenerArtOC(model.OrdenCompra);
                model.Cantidad = _IReciboMercaderiaService.ObtenerCantidadEscaneados(model.OrdenCompra);
                model.Finalizado = model.articulos.Count > 0 ? model.articulos.First().Finalizado == 1 : false;
            }

            return View(model);
        }

        public IActionResult BuscarArt(string CodigoBarras, string OrdenCompra)
        {
            var articulos = _IReciboMercaderiaService.ObtenerArtOCCodigoBarras(OrdenCompra, CodigoBarras);
            if (articulos.Count == 0)
            {
                _IReciboMercaderiaService.GuardarCodigoInex(OrdenCompra, CodigoBarras);
            }
            return Json(articulos);
        }

        public IActionResult Buscador(string Filtro)
        {
            var articulos = _IReciboMercaderiaService.ObtenerArt(Filtro);
            
            return Json(articulos);
        }

        public IActionResult GuardarCantidad(string Detalle)
        {
            var articulos = JsonConvert.DeserializeObject<List<ArticulosView>>(Detalle);
            var articulosNuevaLista = new List<ArticulosView>();
            foreach (var articulo in articulos)
            {
                var articuloNuevo = articulosNuevaLista.Find(x => x.idArticulo == articulo.idArticulo);
                if (articuloNuevo != null)
                {
                    articuloNuevo.Cantidad += articulo.Cantidad;
                    articuloNuevo.CantidadTotal = articulo.CantidadTotal;
                } else
                {
                    articulosNuevaLista.Add(articulo);
                }
            }

            var response = _IReciboMercaderiaService.GuardarCantidad(articulosNuevaLista);

            return Json(response);
        }

        public IActionResult GuardarOC(string OrdenCompra)
        {
            //HttpContext.Session.SetString("OrdenCompra", OrdenCompra);
            //return Json(true);
            var articulos = _IReciboMercaderiaService.ObtenerDiferenciasOC(OrdenCompra);
            return Json(articulos);
        }

        public IActionResult FinalizarOC(string OrdenCompra, int Tipo)
        {
            var response = _IReciboMercaderiaService.FinalizarOC(OrdenCompra, Tipo);
            return Json(response);
        }

        public IActionResult Diferencias(string OrdenCompra)
        {
            var articulos = _IReciboMercaderiaService.ObtenerDiferenciasOC(OrdenCompra);
            var plantilla = new List<ArticulosViewDocument>();
            foreach (var articulo in articulos)
            {
                var nuevoArt = new ArticulosViewDocument();
                nuevoArt.Articulo = articulo.idArticulo;
                nuevoArt.Descripcion = articulo.Descripcion;
                nuevoArt.Cantidad = articulo.Cantidad;
                nuevoArt.CantidadRecibida = articulo.CantidadRecibida;
                nuevoArt.Diferencia = articulo.Diferencia;
                nuevoArt.CodigoBarras = articulo.AST_CodigoBarras;
                nuevoArt.CodigoBarrasAnt = articulo.AST_CodigoBarrasAnt;
                plantilla.Add(nuevoArt);
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string excelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //string excelContentType = "application/octet-stream";

            using (var libro = new ExcelPackage())
            {
                var worksheet = libro.Workbook.Worksheets.Add("Hoja1");

                worksheet.Cells["A2"].LoadFromCollection(plantilla, PrintHeaders: true);

                worksheet.Cells["A1:G1"].Merge = true;
                worksheet.Cells["A1"].Value = "Diferencias Orden Compra " + OrdenCompra;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Font.Size = 16;
                worksheet.Cells["A1"].Style.Font.Italic = true;
                for (var col = 1; col < 4; col++)
                {
                    worksheet.Column(col).AutoFit();
                }

                string rptFile = @"\\INTELISISDB\Archivos\Archivo\Listado de articulos.xlsx";
                if (Directory.Exists(rptFile))
                {
                    System.IO.File.Delete(rptFile);
                }
                libro.SaveAs(new FileInfo((rptFile)));

                return File(libro.GetAsByteArray(), excelContentType, "Listado de articulos.xlsx");
            }

            //return Json(true);
        }

        public IActionResult EnviarCorreo(string OrdenCompra, string email)
        {
            var articulos = _IReciboMercaderiaService.ObtenerDiferenciasOC(OrdenCompra);
            var plantilla = new List<ArticulosViewDocument>();
            foreach (var articulo in articulos)
            {
                var nuevoArt = new ArticulosViewDocument();
                nuevoArt.Articulo = articulo.idArticulo;
                nuevoArt.Descripcion = articulo.Descripcion;
                nuevoArt.Cantidad = articulo.Cantidad;
                nuevoArt.CantidadRecibida = articulo.CantidadRecibida;
                nuevoArt.Diferencia = articulo.Diferencia;
                nuevoArt.CodigoBarras = articulo.AST_CodigoBarras;
                nuevoArt.CodigoBarrasAnt = articulo.AST_CodigoBarrasAnt;
                plantilla.Add(nuevoArt);
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string excelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //string excelContentType = "application/octet-stream";

            using (var libro = new ExcelPackage())
            {
                var worksheet = libro.Workbook.Worksheets.Add("Hoja1");

                worksheet.Cells["A2"].LoadFromCollection(plantilla, PrintHeaders: true);

                worksheet.Cells["A1:G1"].Merge = true;
                worksheet.Cells["A1"].Value = "Diferencias Orden Compra " + OrdenCompra;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Font.Size = 16;
                worksheet.Cells["A1"].Style.Font.Italic = true;
                for (var col = 1; col < 4; col++)
                {
                    worksheet.Column(col).AutoFit();
                }

                string rptFile = @"\\INTELISISDB\Archivos\Archivo\Listado de articulos.xlsx";
                if (Directory.Exists(rptFile))
                {
                    System.IO.File.Delete(rptFile);
                }
                libro.SaveAs(new FileInfo((rptFile)));
                
                //enviar correo
                var response = _IReciboMercaderiaService.EnviarCorreo(OrdenCompra, email, rptFile);

                return Json(response);

            }

        }
    }
}
