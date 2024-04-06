using Microsoft.AspNetCore.Mvc;
using Services.Messaging.ViewModels.DataTable;
using Services.Messaging.ViewModels.Procesos;
using System.Collections.Generic;
using System;
using IServices;
using System.Linq;
using Services.Messaging.ViewModels.Predespacho;
using ASTSoft.Desarrollos.Extensions;

namespace ASTSoft.Desarrollos.Controllers
{
    [FilterConfig]
    public class RedespachoController : Controller
    {
        private readonly IPredespachoServices _predespachoServices;

        public RedespachoController(IPredespachoServices predespachoServices)
        {
            _predespachoServices = predespachoServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            try
            {
                var list = _predespachoServices.getListData();
                var response = new DataTableResponseViewModel<PredespachoView>
                {
                    Data = list.ToList(),
                    RecordsFiltered = list.Count(),
                    RecordsTotal = list.Count()
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new DataTableResponseViewModel<PredespachoView>
                {
                    Data = new List<PredespachoView>(),
                    RecordsFiltered = 0,
                    RecordsTotal = 0,
                    Error = $"Error obtiendo los registros: {ex.Message}"
                };

                return Json(response);
            }
        }

        public ActionResult ReDespachar(string pedido, string justificacion) {
            try
            {
                var response = _predespachoServices.ReAsignar(pedido, justificacion);
                if (response)
                    return Ok();
                return BadRequest("No se ha logrado redespachar el pedido indicado");
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al procesar la información solicitada");
            }
        }

    }
}
