using IServices;
using Microsoft.AspNetCore.Mvc;
using Services.Messaging.ViewModels.DataTable;
using Services.Messaging.ViewModels.Procesos;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Services.Messaging.ViewModels.Datamatrix;
using AutoMapper.Configuration;
using Microsoft.Extensions.Configuration;
using Services.Messaging.ViewModels.AST_SerieLoteMov;
using System.Drawing.Printing;
using QRCoder;
using System.Drawing;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.qrcode;
using System.IO;
using ASTSoft.Desarrollos.Extensions;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using QRCode = QRCoder.QRCode;
using Services.Messaging.ViewModels.Util;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Services.Messaging.ViewModels.AST_Usuarios;

namespace ASTSoft.Desarrollos.Controllers
{
    [FilterConfig]
    public class DataMatrixController : Controller
    {

        private readonly IDatamatrixServices _datamatrix;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public DataMatrixController(IDatamatrixServices datamatrix, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _datamatrix = datamatrix;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var model = new DatamatrixViewData();
            model.Almacenes = _datamatrix.ObtenerAlmacenes();
            model.Usuarios = _datamatrix.getUsersDatamatrix();
            return View(model);
        }

        public IActionResult Reimprimir()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de datos parar mostar en la tabla al escanaear el codigo de barras o digitar el articulo
        /// </summary>
        /// <param name="articulo"></param>
        /// <returns></returns>
        public ActionResult ObtenerDatosArticulo(string articulo)
        {
            try
            {
                var list = _datamatrix.getDatamatrixInfo(articulo);

                var response = new DataTableResponseViewModel<DatamatrixView>
                {
                    Data = list.ToList(),
                    RecordsFiltered = list.Count(),
                    RecordsTotal = list.Count()
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new DataTableResponseViewModel<DatamatrixView>
                {
                    Data = new List<DatamatrixView>(),
                    RecordsFiltered = 0,
                    RecordsTotal = 0,
                    Error = $"Error obtiendo los registros: {ex.Message}"
                };

                return Json(response);
            }
        }

        /// <summary>
        /// Obtiene la informacion necesaria para el generado de etiquetas basado en la seleccion del usuario
        /// </summary>
        /// <param name="articulo"></param>
        /// <param name="id"></param>
        /// <param name="modulo"></param>
        /// <returns></returns>
        public ActionResult ObtenerDatosAsociados(string articulo, int id, string modulo)
        {
            try
            {
                var response = _datamatrix.getDatamatrixAsoc(articulo, id, modulo);
                if (response == null)
                    return BadRequest("No se ha podido obtener la informacion asociada a este articulo");
                return Ok(response);
            }
            catch (System.Exception)
            {
                return BadRequest("Ha ocurrido un error al generar la informacion solicitada");
            }
        }


        /// <summary>
        /// Se realiza toda lo logica para la creacion de datamatrix
        /// </summary>
        /// <param name="lote"></param>
        /// <param name="fechaVencimiento"></param>
        /// <param name="etiqueta"></param>
        /// <param name="consecutivo"></param>
        /// <param name="revisador"></param>
        /// <param name="imprimir"></param>
        /// <param name="articulo"></param>
        /// <param name="qr"></param>
        /// <param name="id"></param>
        /// <param name="renglon"></param>
        /// <param name="cantidadCajas"></param>
        /// <param name="cantidadArt"></param>
        /// <param name="almacen"></param>
        /// <param name="modulo"></param>
        /// <returns></returns>
        public ActionResult EnviarDatosDatamatrix(string lote, string fechaVencimiento, bool etiqueta, bool consecutivo, string revisador, int imprimir, string articulo, string qr, int id, int renglon, int cantidadCajas, int cantidadArt, string almacen, string modulo, string tipo)
        {
            try
            {
                //Obtener los renglones, y cantidad A, con el artículo y id
                modulo = modulo.Trim().Replace(",", "");
                articulo = articulo.Trim().Replace(",", "");

                var usuarioRevisador = string.Empty;
                if (revisador != null)
                {
                    usuarioRevisador = _datamatrix.getUsersDatamatrix().Find(x => x.Id.ToString() == revisador.Trim()).Usuario;
                }

                //Obtener los artículos que se pueden procesar dependiendo de las cantidades ingresadas.
                var listArt = _datamatrix.getArtDatamatrixAsoc(articulo, id, modulo);
                var listArtNueva = new List<DatamatrixAdd>();
                var cantidadImprimir = imprimir;
                foreach (var art in listArt)
                {
                    if (cantidadImprimir > 0)
                    {
                        var cantidadA = art.CantidadRecibidas - art.CantidadA;
                        if (cantidadA <= cantidadImprimir)
                        {
                            art.CantidadImprimir = cantidadA;
                            if (art.CantidadImprimir > 0)
                            {
                                listArtNueva.Add(art);
                            }
                            cantidadImprimir -= cantidadA;
                        }
                        else
                        {
                            art.CantidadImprimir = cantidadImprimir;
                            if (art.CantidadImprimir > 0)
                            {
                                listArtNueva.Add(art);
                            }
                            cantidadImprimir -= cantidadImprimir;
                        }
                    }
                }
                //

                int year = Convert.ToInt32("20" + fechaVencimiento.Substring(0, 2));
                int month = Convert.ToInt32(fechaVencimiento.Substring(2, 2));
                int day = Convert.ToInt32(fechaVencimiento.Substring(4, 2));
                DateTime fecha = new DateTime(year, month, day);

                foreach (var art in listArtNueva)
                {
                    renglon = art.RenglonID;
                    if ((tipo == "Normal" || tipo == "NORMAL") && (modulo.Trim() == "COMS"))
                    {
                        if (!_datamatrix.ActualizarAlmacenCompra(id.ToString(), renglon.ToString(), almacen, modulo))
                        {
                            return BadRequest("No se ha logrado modificar la orden de compra con el nuevo almacen");
                        }
                        var responseCan = ModificarCantidades(id.ToString(), renglon, art.CantidadImprimir);
                        if (responseCan)
                            return Ok();
                        return BadRequest("");
                    }

                    if (!_datamatrix.ActualizarAlmacenCompra(id.ToString(), renglon.ToString(), almacen, modulo))
                    {
                        return BadRequest("No se ha logrado modificar la orden de compra con el nuevo almacen");
                    }

                    var changeData = ActualizarCantidad(art.CantidadImprimir, id, renglon);

                }

                var listOrder = new List<DatamatrixDistribucion>();
                if (listArtNueva.Count > 1)
                {
                    string renglonCompuesto = string.Empty;
                    foreach (var item in listArtNueva)
                    {
                        renglonCompuesto += item.RenglonID + ",";
                    }
                    renglonCompuesto = renglonCompuesto.Substring(0, renglonCompuesto.Length - 1);

                    listOrder = GetDatamatrixOrderRC(cantidadCajas, cantidadArt, imprimir, id, renglon, renglonCompuesto);
                }
                else
                {
                    listOrder = GetDatamatrixOrder(cantidadCajas, cantidadArt, imprimir, id, renglon);
                }

                var registrados = _datamatrix.ObtenerConsecutivo(articulo, lote, fechaVencimiento);
                foreach (var artNuevo in listArtNueva)
                {
                    if (consecutivo)
                    {
                        for (int i = 1; i <= artNuevo.CantidadImprimir; i++)
                        {
                            try
                            {
                                var filter = listOrder.Find(x => x.ConsecutivoInicio <= i && x.ConsecutivoFinal >= i);
                                int idCaja = (filter == null) ? 0 : filter.IdCaja;
                                var datamatrix = GenerarDatamatrix((registrados > 0) ? registrados + 1 : i, qr, fechaVencimiento, lote);
                                var saveDatamatrix = AlmacenarDatamatrix(lote, fechaVencimiento, articulo, ObtenerConsecutivo((registrados > 0) ? registrados + 1 : i), datamatrix, idCaja, id, artNuevo.RenglonID);
                                var saveSerieLote = AlmacenarSerieLote(articulo, 1, datamatrix, lote, id, artNuevo.RenglonID, fecha, modulo);
                                registrados++;
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= artNuevo.CantidadImprimir; i++)
                        {
                            var datamatrix = GenerarDatamatrix((registrados > 0) ? registrados + 1 : i, qr, fechaVencimiento, lote);
                            var saveDatamatrix = AlmacenarDatamatrix(lote, fechaVencimiento, articulo, ObtenerConsecutivo((registrados > 0) ? registrados + 1 : i), datamatrix, 0, id, artNuevo.RenglonID);
                            var saveSerieLote = AlmacenarSerieLote(articulo, 1, datamatrix, lote, id, artNuevo.RenglonID, fecha, modulo);
                            registrados++;
                        }
                    }

                }

                var listCajas = new List<ArticuloInfo>();
                var userinfo = JsonConvert.DeserializeObject<AST_UsuariosView>(HttpContext.Session.GetString("userInfo"));
                

                foreach (var item in listOrder)
                {
                    var modelCaja = _datamatrix.ObtenerInformacionEtiqueta(item.IdCaja);
                    //modelCaja.Qr = generateQRCaja(item.IdCaja);
                    modelCaja.Qr = generateQRCaja(ObtenerConsecutivo(item.ConsecutivoInicio) + " al " + ObtenerConsecutivo(item.ConsecutivoFinal));
                    modelCaja.usuario = usuarioRevisador;
                    modelCaja.fechaVencimiento = fecha;
                    modelCaja.consecutivos = ObtenerConsecutivo(item.ConsecutivoInicio) + " - " + ObtenerConsecutivo(item.ConsecutivoFinal);
                    modelCaja.cantidad = cantidadArt;
                    listCajas.Add(modelCaja);
                }

                var list = new List<EtiquetasModel>();
                var response = _datamatrix.ObtenerDatosImprimir(lote, articulo, 1, imprimir);

                foreach (var item in response)
                {
                    list.Add(new EtiquetasModel
                    {
                        qr = generateQR(item.datamatrix, ObtenerConsecutivo(item.consecutivo)),
                        consecutivoReal = ObtenerConsecutivo(item.consecutivo)
                    });
                }

                var pdf = new ViewAsPdf("~/Views/PDFPrint/GenerarDatamatrix.cshtml", list)
                {
                    PageHeight = Convert.ToInt32(_configuration["Etiquetas:pequennas:h"].ToString()),
                    PageWidth = Convert.ToInt32(_configuration["Etiquetas:pequennas:w"].ToString()),
                    //Asigno margen dinamicamente
                    //PageMargins = new Rotativa.AspNetCore.Options.Margins(0, 0, 0, Convert.ToInt32(_configuration["Etiquetas:pequennas:ml"].ToString()))
                    PageMargins = new Rotativa.AspNetCore.Options.Margins(Convert.ToInt32(_configuration["Etiquetas:pequennas:mt"].ToString()), 0, 0, Convert.ToInt32(_configuration["Etiquetas:pequennas:ml"].ToString())),
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape

                };

                var pdfReal = pdf.BuildFile(ControllerContext).Result;
                string basePDF = Convert.ToBase64String(pdfReal);

                var pdfCaja = new ViewAsPdf("~/Views/PDFPrint/CajasEtiquetas.cshtml", listCajas)
                {
                    PageHeight = Convert.ToInt32(_configuration["Etiquetas:grandes:h"].ToString()),
                    PageWidth = Convert.ToInt32(_configuration["Etiquetas:grandes:w"].ToString()),
                    PageMargins = new Rotativa.AspNetCore.Options.Margins(0, 0, 0, 0)
                };


                var pdfRealCaja = pdfCaja.BuildFile(ControllerContext).Result;
                string basePDFCaja = Convert.ToBase64String(pdfRealCaja);

                return Ok(new { datamatrix = basePDF, caja = basePDFCaja });

            }
            catch (System.Exception ex)
            {
                return BadRequest("Ha ocurrido un error al solicitar la información solicitada");
            }
        }


        public bool ModificarCantidades(string id, int renglon, int cantidad)
        {
            try
            {
                return _datamatrix.ObtenerActualizarCantidades(id, renglon, cantidad);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ActionResult EnviarDatosLote(string lote, string fechaVencimiento, bool etiqueta, bool consecutivo, string revisador, int imprimir, string articulo, string qr, int id, int renglon, string modulo)
        {
            try
            {
                int year = Convert.ToInt32("20" + fechaVencimiento.Substring(0, 2));
                int month = Convert.ToInt32(fechaVencimiento.Substring(2, 2));
                int day = Convert.ToInt32(fechaVencimiento.Substring(4, 2));
                DateTime fecha = new DateTime(year, month, day);
                var registrados = _datamatrix.ObtenerConsecutivo(articulo, lote, fechaVencimiento);
                var saveSerieLote = AlmacenarSerieLote(articulo, imprimir, null, lote, id, renglon, fecha, modulo);
                return Ok();
            }
            catch (System.Exception)
            {
                return BadRequest("Ha ocurrido un error al solicitar la información solicitada");
            }
        }

        private string GenerarDatamatrix(int id, string codigoBarras, string vencimiento, string lote)
        {
            try
            {
                var indicadorBarras = _configuration["DatamatrixInfo:indicadorBarras"].ToString();
                var indicadorFecha = _configuration["DatamatrixInfo:indicadorFecha"].ToString();
                var indicadorConsecutivo = _configuration["DatamatrixInfo:indicadorConsecutivo"].ToString();
                var indicadorLote = _configuration["DatamatrixInfo:indicadorLote"].ToString();
                return $"{indicadorBarras}{codigoBarras.Trim()}{indicadorFecha}{vencimiento.Trim()}{indicadorConsecutivo}{ObtenerConsecutivo(id).Trim()}{indicadorLote}{lote.Trim()}";
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public bool ActualizarCantidad(int cantidad, int id, int RenglonID)
        {
            try
            {
                return _datamatrix.ActualizarCantidad(cantidad, id, RenglonID);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string ObtenerConsecutivo(int id)
        {
            var consecutivo = string.Empty;
            switch (id.ToString().Length)
            {
                case 1:
                    consecutivo = $"0000{id}";
                    break;
                case 2:
                    consecutivo = $"000{id}";
                    break;
                case 3:
                    consecutivo = $"00{id}";
                    break;
                case 4:
                    consecutivo = $"0{id}";
                    break;
                case 5:
                    consecutivo = $"{id}";
                    break;
            }
            return consecutivo;
        }

        private bool AlmacenarDatamatrix(string lote, string fechaVencimiento, string articulo, string consecutivo, string Datamatrix, int idCaja, int id, int renglon)
        {
            try
            {
                var model = new AST_DatamatrixView
                {
                    Articulo = articulo,
                    Consecutivo = consecutivo,
                    Datamatrix = Datamatrix,
                    FechaVencimiento = fechaVencimiento,
                    Lote = lote,
                    idCaja = idCaja,
                    MovID = id,
                    RenglonID = renglon
                };
                var response = _datamatrix.InsertarDatamatrix(model);
                return response;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool AlmacenarSerieLote(string articulo, int cantidad, string datamatrix, string serie, int id, int renglon, DateTime Vencimiento, string modulo)
        {
            try
            {
                var model = new AST_SerieLoteMovView
                {
                    Articulo = articulo,
                    Cantidad = cantidad,
                    Datamatrix = datamatrix,
                    Empresa = "CNDF1",
                    Modulo = modulo.Split(',')[0],
                    SerieLote = serie,
                    ID = id,
                    RenglonID = renglon,
                    Vencimiento = Vencimiento
                };
                var response = _datamatrix.InsertarSerieLote(model);
                return response;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpGet]
        public IActionResult ReimprimirDatamatrix(string serieLote, string articulo, int inicio, int final)
        {
            try
            {
                var response = _datamatrix.ObtenerDatosImprimir(serieLote, articulo, inicio, final);
                var list = new List<EtiquetasModel>();
                foreach (var item in response)
                {
                    list.Add(new EtiquetasModel
                    {
                        qr = generateQR(item.datamatrix, ObtenerConsecutivo(item.consecutivo)),
                        consecutivoReal = ObtenerConsecutivo(item.consecutivo)
                    });
                }

                var pdf = new ViewAsPdf("~/Views/PDFPrint/GenerarDatamatrix.cshtml", list)
                {
                    PageHeight = Convert.ToInt32(_configuration["Etiquetas:pequennas:h"].ToString()),
                    PageWidth = Convert.ToInt32(_configuration["Etiquetas:pequennas:w"].ToString()),
                    //Asigno margen dinamicamente
                    //PageMargins = new Rotativa.AspNetCore.Options.Margins(0, 0, 0, Convert.ToInt32(_configuration["Etiquetas:pequennas:ml"].ToString())),
                    
                    //PageMargins = new Rotativa.AspNetCore.Options.Margins(Convert.ToInt32(_configuration["Etiquetas:pequennas:mt"].ToString()), 0, 0, Convert.ToInt32(_configuration["Etiquetas:pequennas:ml"].ToString())),
                    PageMargins = new Rotativa.AspNetCore.Options.Margins(Convert.ToInt32(_configuration["Etiquetas:pequennas:mt"].ToString()), 0, 0, Convert.ToInt32(_configuration["Etiquetas:pequennas:ml"].ToString())),
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
                    //PageMargins = new Rotativa.AspNetCore.Options.Margins(Convert.ToInt32(_configuration["Etiquetas:pequennas:mt"].ToString()), 0, 0, Convert.ToInt32(_configuration["Etiquetas:pequennas:ml"].ToString()))
                };

                var pdfReal = pdf.BuildFile(ControllerContext).Result;
                string basePDF = Convert.ToBase64String(pdfReal);

                return Ok(basePDF);
            }
            catch (Exception ex)
            {
                return BadRequest("Ha ocurrido un error al intentar procesar la impresion de las etiquetas");
            }
        }

        private string generateQR(string datamatrix, string consecutivo)
        {
            try
            {
                QRCodeGenerator generator = new QRCodeGenerator();
                QRCodeData data = generator.CreateQrCode(datamatrix, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(data);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);

                using (Graphics graphics = Graphics.FromImage(qrCodeImage))
                {
                    using (Font font = new Font("Arial", 12))
                    {
                        graphics.DrawString(consecutivo, font, Brushes.Black, new PointF(0, qrCodeImage.Height + 10));
                    }
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageBytes = ms.ToArray();
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error al intentar generar el código de barras");
            }
        }

        private string generateQRCaja(string idCaja)
        {
            try
            {
                QRCodeGenerator generator = new QRCodeGenerator();
                QRCodeData data = generator.CreateQrCode(idCaja.ToString(), QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(data);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);

                using (Graphics graphics = Graphics.FromImage(qrCodeImage))
                {
                    using (Font font = new Font("Arial", 12))
                    {
                        graphics.DrawString(idCaja.ToString(), font, Brushes.Black, new PointF(0, qrCodeImage.Height + 10));
                    }
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageBytes = ms.ToArray();
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error al intentar generar el código de barras");
            }
        }

        private List<DatamatrixDistribucion> GetDatamatrixOrder(int cajas, int articuloXCaja, int cantidadTotal, int renglon, int id)
        {
            try
            {
                int redestribucion = cantidadTotal;
                int stop = 1;
                var list = new List<DatamatrixDistribucion>();
                for (int i = 1; i <= cajas; i++)
                {
                    if (redestribucion < articuloXCaja)
                    {
                        var idCaja = _datamatrix.InsertarConsecutivoDatamatrix(id, renglon).id;
                        list.Add(new DatamatrixDistribucion
                        {
                            IdCaja = idCaja,
                            ConsecutivoInicio = stop,
                            ConsecutivoFinal = (stop + redestribucion) - 1
                        });
                    }
                    else
                    {
                        var idCaja = _datamatrix.InsertarConsecutivoDatamatrix(id, renglon).id;
                        list.Add(new DatamatrixDistribucion
                        {
                            IdCaja = idCaja,
                            ConsecutivoInicio = stop,
                            ConsecutivoFinal = (stop == 1) ? articuloXCaja : (stop + articuloXCaja) - 1
                        });
                        redestribucion = redestribucion - articuloXCaja;
                        stop = stop + articuloXCaja;
                    }

                }

                return list;
            }
            catch (Exception)
            {
                return new List<DatamatrixDistribucion>();
            }
        }

        private List<DatamatrixDistribucion> GetDatamatrixOrderRC(int cajas, int articuloXCaja, int cantidadTotal, int id, int renglonID, string renglonCompuesto)
        {
            try
            {
                int redestribucion = cantidadTotal;
                int stop = 1;
                var list = new List<DatamatrixDistribucion>();
                for (int i = 1; i <= cajas; i++)
                {
                    if (redestribucion < articuloXCaja)
                    {
                        var idCaja = _datamatrix.InsertarConsecutivoDatamatrixRC(id, renglonCompuesto, renglonID).id;
                        list.Add(new DatamatrixDistribucion
                        {
                            IdCaja = idCaja,
                            ConsecutivoInicio = stop,
                            ConsecutivoFinal = (stop + redestribucion) - 1
                        });
                    }
                    else
                    {
                        var idCaja = _datamatrix.InsertarConsecutivoDatamatrixRC(id, renglonCompuesto, renglonID).id;
                        list.Add(new DatamatrixDistribucion
                        {
                            IdCaja = idCaja,
                            ConsecutivoInicio = stop,
                            ConsecutivoFinal = (stop == 1) ? articuloXCaja : (stop + articuloXCaja) - 1
                        });
                        redestribucion = redestribucion - articuloXCaja;
                        stop = stop + articuloXCaja;
                    }

                }

                return list;
            }
            catch (Exception)
            {
                return new List<DatamatrixDistribucion>();
            }
        }

    }
}
