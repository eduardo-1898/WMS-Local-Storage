using Microsoft.AspNetCore.Mvc;
using Services.Messaging.ViewModels.DataTable;
using Services.Messaging.ViewModels.Procesos;
using System.Collections.Generic;
using System;
using IServices;
using Services;
using System.Linq;
using ASTSoft.Desarrollos.Extensions;

namespace ASTSoft.Desarrollos.Controllers
{
    [FilterConfig]
    public class HistorialController : Controller
    {
        private readonly IHistorialServices _historialServices;

        public HistorialController(IHistorialServices historialServices) {
            _historialServices = historialServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            try
            {
                var list = _historialServices.getList();

                var response = new DataTableResponseViewModel<ProcesosView>
                {
                    Data = list.ToList(),
                    RecordsFiltered = list.Count(),
                    RecordsTotal = list.Count()
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new DataTableResponseViewModel<ProcesosView>
                {
                    Data = new List<ProcesosView>(),
                    RecordsFiltered = 0,
                    RecordsTotal = 0,
                    Error = $"Error obtiendo los registros: {ex.Message}"
                };

                return Json(response);
            }
        }

        public ActionResult changeStatus(int id, bool status) 
        {
            var model = new ProcesosView { id = id, cancelar = status };
            try
            {
                var response = _historialServices.changeStatusRegister(model);
                TempData["SuccessMesagge"] = "Se ha realizado la modificacion de la ruta correctamente";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["MessageError"] = "Ha ocurrido un error al intentar accesar a la encuesta solicitada";
                return RedirectToAction("Index");
            }
        }
    }
}
