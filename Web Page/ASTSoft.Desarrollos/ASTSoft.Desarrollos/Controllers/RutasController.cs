using IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Services.Messaging.ViewModels.Procesos;
using System;
using ASTSoft.Desarrollos.Utils;
using System.Linq;
using ASTSoft.Desarrollos.Extensions;

namespace ASTSoft.Desarrollos.Controllers
{
    [FilterConfig]
    public class RutasController : Controller
    {

        private readonly IHistorialServices _historialServices;
        private readonly IAST_RutasServices _ast_rutasServices;

        public RutasController(IHistorialServices historialServices, IAST_RutasServices ast_rutasServices)
        {
            _historialServices = historialServices;
            _ast_rutasServices = ast_rutasServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Aprobaciones()
        {
            return View();
        }

        public ActionResult FinalizarDespacho(int id)
        {
            var model = new ProcesosView { id = id, finalizarDespacho = true};
            try
            {
                var response = _ast_rutasServices.aprovOut(model);
                if (response) {
                    return Json(response);
                }
                return BadRequest("No se ha logrado ejecutar la acción correctamente");
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al intentar finalizar la orden");
            }
        }

        public ActionResult ReDespachar(int id) { 
            var model = new ProcesosView { id = id, estado = "DESPACHO" };
            try
            {
                var response = _ast_rutasServices.volverDespachar(model);
                if (response)
                {
                    return Json(response);
                }
                return BadRequest("Nose ha logrado ejecutar la acción correctamente");
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al intentar finalizar la orden");
            }
        }

        public ActionResult EnviarNotificacion()
        {
            try
            {
                var model = new CorreoService();
                var list = _historialServices.getList().Where(x => x.estado == "Despacho");
                model.Cuerpo = "Buenas, se les informa que el listado de los siguientes movimientos se encuentran pendientes de aprobación </br>";
                model.Destinatarios = new System.Collections.Generic.List<string>();
                model.Destinatarios.Add("eduardo.jaen@astsoftcr.com");
                foreach (var item in list) 
                {
                    model.Cuerpo += "Número de pedido: "+item.pedido+", cliente: "+ item.cliente + ", Fecha de estado: "+ item.fecha + " </br>";
                }

                model.Asunto = "Registros pendientes de aprobación";
                InterfaceEmail.SendEmail(model);
                return RedirectToAction("Aprobaciones");
            }
            catch (Exception)
            {
                return RedirectToAction("Aprobaciones");
            }
        }
    }
}
