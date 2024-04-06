using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Messaging.ViewModels.DataTable;
using System.Collections.Generic;
using System;
using IServices;
using Services.Messaging.ViewModels.Procesos;
using System.Linq;
using System.Web;
using ASTSoft.Desarrollos.Extensions;
using Services.Messaging.ViewModels.Historial;

namespace ASTSoft.Desarrollos.Controllers
{
    [FilterConfig]
    public class AlistoController : Controller
    {

        private readonly IProcesosServices _procesosServices;
        private readonly IAST_RutasServices _rutasServices;
        private readonly IAST_UsuariosService _UsuariosService;
        private readonly IHistorialServices _historialServices;

        public AlistoController(IProcesosServices procesosServices, IAST_RutasServices rutasServices, IAST_UsuariosService UsuariosService, IHistorialServices historialServices = null)
        {
            _procesosServices = procesosServices;
            _rutasServices = rutasServices;
            _UsuariosService = UsuariosService;
            _historialServices = historialServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Historial()
        {
            return View();
        }

        public IActionResult List(string situacion) {
            try
            {
                var list = _procesosServices.getList().Where(x => x.estado.ToLower() == situacion.ToLower()).ToList();
                var rutas = _rutasServices.getListRutas();
                var usuarios = _UsuariosService.UserList();
                var prioridades = new List<Prioridades>
                {
                    new Prioridades { idPrioridad = 1, prioridad = "Urgente" },
                    new Prioridades { idPrioridad = 2, prioridad = "Normal" }
                };

                foreach (var item in list)
                {
                    item.Rutas = rutas;
                    item.Usuarios = usuarios;
                    item.Prioridades = prioridades;
                }

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

        public IActionResult getDetailsInfo(string pedido)
        {
            try
            {
                var list = _historialServices.getListDetails(pedido);

                var response = new DataTableResponseViewModel<DetallePedidoView>
                {
                    Data = list.ToList(),
                    RecordsFiltered = list.Count(),
                    RecordsTotal = list.Count()
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new DataTableResponseViewModel<DetallePedidoView>
                {
                    Data = new List<DetallePedidoView>(),
                    RecordsFiltered = 0,
                    RecordsTotal = 0,
                    Error = $"Error obtiendo los registros: {ex.Message}"
                };

                return Json(response);
            }
        }

        public IActionResult ListHistorial()
        {
            try
            {
                var list = _procesosServices.getList().ToList();

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

        public IActionResult ListHistorialFilter(DateTime fechaInicio, DateTime fechaFinal, string estado)
        {
            try
            {
                var list = _procesosServices.getListHistorial(fechaInicio, fechaFinal, estado).ToList();

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

        public ActionResult updateData(int pedido, string ruta, string user, string userbod, int? prioridad) 
        {
            var model = new ProcesosView { id = pedido, idRuta = ruta, usuario = user, usuarioBOD1 = userbod, prioridad = prioridad??1 };
            try
            {
                var response = _procesosServices.UpdateRoute(model);
                return Json(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Ha ocurrido un error al intentar modificar el registro seleccionado");
            }
        }

    }
}
