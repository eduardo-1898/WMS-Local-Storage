using ASTSoft.Desarrollos.Extensions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Messaging.ViewModels.Etiquetas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static iTextSharp.text.pdf.AcroFields;

namespace ASTSoft.Desarrollos.Controllers
{
    [FilterConfig]
    public class TiqueteController : Controller
    {

        public ActionResult sendBarCode()
        {
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            Font times = new Font(bfTimes, 8, Font.NORMAL, BaseColor.BLACK);

            var model = new EtiquetasView();

            try
            {
                var pgSize = new iTextSharp.text.Rectangle(240, 170);
                Document doc = new Document(pgSize, 5, 5, 2, 2);
                MemoryStream mi = new MemoryStream();
                var writter = PdfWriter.GetInstance(doc, mi);
                Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

                PdfPTable table = new PdfPTable(2); //tabla con dos columnas
                table.WidthPercentage = 100;

                string path = System.IO.Path.GetFullPath("wwwroot/img/logo.png");

                Image logo = Image.GetInstance(path);

                doc.AddAuthor("Condefa");
                doc.AddTitle("Barcodes generated");
                doc.Open();

                var qr = generateQR("TESTEANDOANDO");

                PdfPCell cell1 = new PdfPCell(qr.GetImage()); //celda con la primera imagen
                PdfPCell cell2 = new PdfPCell(logo);

                doc.Add(qr.GetImage());
                //doc.Add(new Phrase(model.Cliente));
                doc.Add(new Phrase("EDUARDO JOSE JAEN ARIAS TEST", times));
                doc.Add(new Phrase("\n"));
                doc.Add(new Phrase("Ruta: "+ "PRUEBA", times));
                doc.Add(new Phrase("\n"));
                doc.Add(new Phrase("Pedido: TESTEANDOANDO", times));
                doc.Add(p);
                doc.Add(new Phrase("Empacado por: EDUARDO JAEN", times));

                doc.Close();
                writter.Close();

                var myFile = mi.ToArray();
                return File(myFile, "application/pdf", "barcode.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private BarcodeQRCode generateQR(string vin)
        {
            try
            {
                BarcodeQRCode qr = new BarcodeQRCode(vin, 50, 50, null);
                return qr;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error al intentar generar el código de barras");
            }

        }

    }
}
