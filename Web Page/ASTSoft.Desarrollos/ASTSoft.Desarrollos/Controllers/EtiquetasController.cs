using ASTSoft.Desarrollos.Extensions;
using EllipticCurve.Utils;
using IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.qrcode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml.ConditionalFormatting;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using PdfiumViewer;
using QRCoder;
using Rotativa.AspNetCore;
using SendGrid;
using Services;
using Services.Messaging.ViewModels.Articulos;
using Services.Messaging.ViewModels.AST_DocumentosEmpaque;
using Services.Messaging.ViewModels.AST_DocumentosEmpaqueD;
using Services.Messaging.ViewModels.AST_Usuarios;
using Services.Messaging.ViewModels.Canasta;
using Services.Messaging.ViewModels.DataTable;
using Services.Messaging.ViewModels.Etiquetas;
using Services.Messaging.ViewModels.Procesos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace ASTSoft.Desarrollos.Controllers
{
    [FilterConfig]
    public class EtiquetasController : Controller
    {

        private readonly IEtiquetaServices _etiquetaServices;
        private readonly IArticulosServices _articulosServices;
        private readonly ICanastaService _canastaService;
        private readonly IProcesosServices _procesos;
        private readonly IAST_UsuariosService _usuariosService;
        private readonly IAST_DocumentosEmpaqueServices _ast_documentosEmpaqueServices;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public EtiquetasController(IEtiquetaServices etiquetaServices, IArticulosServices articulosServices, ICanastaService canastaService, IProcesosServices procesos, IWebHostEnvironment env, IConfiguration config, IAST_DocumentosEmpaqueServices ast_documentosEmpaqueServices, IAST_UsuariosService usuariosService)
        {
            _etiquetaServices = etiquetaServices;
            _articulosServices = articulosServices;
            _canastaService = canastaService;
            _procesos = procesos;
            _env = env;
            _config = config;
            _ast_documentosEmpaqueServices = ast_documentosEmpaqueServices;
            _usuariosService = usuariosService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Crear() {
            return View();
        }

        public IActionResult List()
        {
            try
            {
                var list = _ast_documentosEmpaqueServices.getDocuments();
                var response = new DataTableResponseViewModel<AST_DocumentosEmpaqueView>
                {
                    Data = list.ToList(),
                    RecordsFiltered = list.Count(),
                    RecordsTotal = list.Count()
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new DataTableResponseViewModel<AST_DocumentosEmpaqueView>
                {
                    Data = new List<AST_DocumentosEmpaqueView>(),
                    RecordsFiltered = 0,
                    RecordsTotal = 0,
                    Error = $"Error obtiendo los registros: {ex.Message}"
                };

                return Json(response);
            }
        }

        public IActionResult ListPedidos(long idDocumento)
        {
            try
            {
                var list = _ast_documentosEmpaqueServices.getPedidos(idDocumento);
                var response = new DataTableResponseViewModel<AST_DocumentosEmpaqueDView>
                {
                    Data = list.ToList(),
                    RecordsFiltered = list.Count(),
                    RecordsTotal = list.Count()
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new DataTableResponseViewModel<AST_DocumentosEmpaqueDView>
                {
                    Data = new List<AST_DocumentosEmpaqueDView>(),
                    RecordsFiltered = 0,
                    RecordsTotal = 0,
                    Error = $"Error obtiendo los registros: {ex.Message}"
                };

                return Json(response);
            }
        }

        public ActionResult BusquedaPedido(string PedidoConsulta) {
            try
            {
                var model = new EtiquetasView();
                var data = _etiquetaServices.getDataInfo(PedidoConsulta);
                if (data==null) {
                    TempData["ErrorMesagge"] = "Este pedido no se encuentra disponible para poder confirmar el empaque";
                    return RedirectToAction("Index");
                }

                model.Cliente = data.Cliente;
                model.Ruta = data.Ruta;
                model.Pedido = data.Pedido;
                model.Alistador = data.Alistador;
                model.Observacion = data.Observacion;
                model.Bultos = data.Bultos;
                model.usuario = data.usuario;
                model.AST_ObservacionesExtraEtiquetado = data.AST_ObservacionesExtraEtiquetado;
                model.canastas = _canastaService.getCanastas(data.Pedido);
                model.articulos = _articulosServices.getArticulos(data.Pedido);
                model.usuarios = _usuariosService.GetUsuarios();
                model.cantidadSinLeer = data.cantidadSinLeer;

                return View("Index", model);

            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        [AllowAnonymous]
        public ActionResult ImprimirEtiqueta(string pedido, int cantidad, bool fragil)
        {
            try
            {
                var list = new List<BultosEtiquetas>();
                var userInfo = JsonConvert.DeserializeObject<AST_UsuariosView>(HttpContext.Session.GetString("userInfo"));
                var dataInfo = _procesos.getPedidoInfo(pedido);
                for (int i = 1; i <= cantidad; i++)
                {
                    var qr = generateQRBase(pedido, i);
                    var model = new BultosEtiquetas
                    {
                        bulto = i,
                        cantidadBultos = cantidad,
                        cliente = dataInfo.cliente,
                        fragil = fragil,
                        ruta = dataInfo.ruta,
                        lugar = dataInfo.lugar,
                        pedido = pedido,
                        canastas = dataInfo.canastas,
                        telefono = dataInfo.telefono,
                        usuarioAlistador = dataInfo.usuarioEmpacador,
                        usuarioEmpacador = dataInfo.usuario,
                        codClie = dataInfo.codClie,
                        Qr = qr
                    };

                    list.Add(model);
                }

                var changeStatus = _etiquetaServices.CambiarSituacionPredespacho(pedido);

                return new ViewAsPdf("~/Views/PDFPrint/BultosEtiquetas.cshtml", list)
                {
                    PageHeight = Convert.ToInt32(_config["Etiquetas:grandes:h"].ToString()),
                    PageWidth = Convert.ToInt32(_config["Etiquetas:grandes:w"].ToString()),
                    PageMargins = new Rotativa.AspNetCore.Options.Margins(0, 0, 0, 0)
                };
            }
            catch (System.Exception)
            {
                TempData["ErrorMesagge"] = "Ha ocurrido un error al generar el proceso";
                return RedirectToAction("Index");
            }
        }

        [AllowAnonymous]
        public ActionResult ImprimirEtiquetaDoc(long id)
        {
            try
            {
                var list = new List<BultosEtiquetas>();
                var userInfo = JsonConvert.DeserializeObject<AST_UsuariosView>(HttpContext.Session.GetString("userInfo"));
                var dataInfo = _procesos.getDocInfo(id);
                for (int i = 1; i <= 1; i++)
                {
                    var qr = generateQR(id.ToString());
                    var model = new BultosEtiquetas
                    {
                        bulto = i,
                        cantidadBultos = 1,
                        cliente = dataInfo.cliente,
                        fragil = true,
                        ruta = dataInfo.ruta,
                        lugar = dataInfo.lugar,
                        pedido = dataInfo.pedido,
                        documento = (int)id,
                        telefono = dataInfo.telefono,
                        usuarioAlistador = dataInfo.usuarioEmpacador,
                        usuarioEmpacador = dataInfo.usuario,
                        codClie = dataInfo.codClie,
                        Qr = qr
                    };

                    list.Add(model);
                }

                return new ViewAsPdf("~/Views/PDFPrint/BultosDocumento.cshtml", list)
                {
                    PageHeight = Convert.ToInt32(_config["Etiquetas:grandes:h"].ToString()),
                    PageWidth = Convert.ToInt32(_config["Etiquetas:grandes:w"].ToString()),
                    PageMargins = new Rotativa.AspNetCore.Options.Margins(0, 0, 0, 0)
                };
            }
            catch (System.Exception)
            {
                return BadRequest("Ha ocurrido un error al obtener el archivo por imprimir");
            }
        }

        public ActionResult AceptarAlistado(string pedido, int bultos, string usuario, string ObservacionesExtra) {
            try
            {
                var response = _etiquetaServices.aceptarAlisto(pedido, bultos, usuario, ObservacionesExtra);
                if (response)
                    return Ok();
                return BadRequest("No se ha podido realizar la solicitud");
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al procesar la solicitud");
            }
        }

        public string generateQR(string pedido)
        {
            try
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode($"{pedido}", QRCodeGenerator.ECCLevel.Q);
                Base64QRCode qrCode = new Base64QRCode(qrCodeData);
                string qrCodeImageAsBase64 = qrCode.GetGraphic(20);
                return qrCodeImageAsBase64;
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }

        public string generateQRBase(string pedido, int bulto) {
            try
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode($"{pedido}/{bulto}", QRCodeGenerator.ECCLevel.Q);
                Base64QRCode qrCode = new Base64QRCode(qrCodeData);
                string qrCodeImageAsBase64 = qrCode.GetGraphic(20);
                return qrCodeImageAsBase64;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public ActionResult CrearConsecutivo() {
            try
            {
                var user = JsonConvert.DeserializeObject<AST_UsuariosView>(HttpContext.Session.GetString("userInfo"));
                var response = _ast_documentosEmpaqueServices.createDocument(user.usuario);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al intentar procesar los datos");
            }
        }

        public ActionResult ObtenerDocumento(long id) {
            try
            {
                var response = _ast_documentosEmpaqueServices.getDocument(id);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al intentar obtener los datos del documento");
            }
        }

        public ActionResult AgregarDocumento(string pedido, long idDocumento) {
            try
            {
                var model = new AST_DocumentosEmpaqueDView
                {
                    idDocumento = idDocumento,
                    pedido = pedido
                };

                var response = _ast_documentosEmpaqueServices.addNewOrder(model);
                if (response)
                    return Ok();
                return BadRequest("El pedido no se encuentra disponible para asociar al documento");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public ActionResult EliminarRegistro(long id) {
            try
            {
                var response = _ast_documentosEmpaqueServices.deleteOrder(id);
                if (response)
                    return Ok();
                return BadRequest("No se ha logrado eliminar el registro indicado");
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al intentar eliminar el registro seleccionado");
            }
        }

    }
}
