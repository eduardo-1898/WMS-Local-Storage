using IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CondefaMovilAPI.Controllers
{
    [Route("Despacho")]
    [ApiController]
    [Authorize]
    public class DespachoController : ControllerBase
    {
        private readonly IDespachosService _despachosService;

        public DespachoController(IDespachosService despachosService)
        {
            _despachosService = despachosService;
        }

        [HttpPost]
        [Route("createNewDespacho")]
        public ActionResult createNewDespacho(string ruta) {
            try
            {
                var response = _despachosService.createNewDespacho(ruta);
                if (response > 0)
                    return Ok(response);
                return BadRequest("No se puedo crear el nuevo despacho para la ruta seleccionada ruta:" + ruta);
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al intentar generar el nuevo despacho para esta ruta");
            }
        }

        [HttpPost]
        [Route("insertNewScan")]
        public ActionResult insertNewScan(string pedido, int consecutivo) {
            try
            {
                var arr = pedido.Split('/');
                if (arr.Length > 1)
                {
                    var response = _despachosService.InsertNewScan(arr[0], consecutivo, Convert.ToInt32(arr[1]));
                    if (response)
                        return Ok(response);
                    return BadRequest("No se puedo insertar el pedido en el despacho:  " + consecutivo);
                }
                else {
                    var response = _despachosService.InsertNewScanDoc(Convert.ToInt64(pedido), consecutivo);
                    if (response)
                        return Ok(response);
                    return BadRequest("No se puedo insertar el documento en el despacho: " + consecutivo);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getListScans")]
        public ActionResult getListScans(int consecutivo) {
            try
            {
                var response = _despachosService.getListDespacho(consecutivo);
                if (response != null)
                    return Ok(response);
                return BadRequest("No se ha logrado obtener el listo de pedidos escaneados");
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al intentar obtener el listado de escaneos");
            }
        }

        [HttpPatch]
        [Route("finishDespacho")]
        public ActionResult finishDespacho(int consecutivo, string conductor) {
            try
            {
                if (_despachosService.ValidateBeforeFinish(consecutivo))
                    return BadRequest("Hay pedidos a los cuales, no se han escaneado todos los bultos");
                var response = _despachosService.finishScanDespacho(consecutivo, conductor);
                if (response)
                    return Ok(response);
                return BadRequest("No se ha logrado finalizar el despacho de pedidos para esta ruta");
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al intentar finalizar el despacho");
            }
        }

        [HttpDelete]
        [Route("deleteScan")]
        public ActionResult deleteScan(int id) {
            try
            {
                var response = _despachosService.deleteScanDespacho(id);
                if (response)
                    return Ok(response);
                return BadRequest("No se ha logrado eliminar el registro solicitado");
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al intentar eliminar el registro");
            }
        }

        [HttpGet]
        [Route("getRouteList")]
        public ActionResult getRouteList() {
            try
            {
                var response = _despachosService.getRoutes();
                if (response.Count > 0)
                    return Ok(response);
                return BadRequest("No hay registros para mostrar");
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al intentar obtener el listado de rutas");
            }
        }

        [HttpPatch]
        [Route("updateBultos")]
        public ActionResult updateBultos(string canasta, int bultos) {
            try
            {
                if (!_despachosService.ValidateBultoForUpdate(canasta))
                    return BadRequest("El pedido que se encuentra asociada a esta canasta no se encuentra aprobado o bien disponible para empaque");
                var response = _despachosService.updateBultos(canasta, bultos);
                if (response)
                    return Ok(response);
                return BadRequest("No se ha logrado actualizar el registro solicitado");
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al intentar actualizar el registro");
            }
        }

    }
}
