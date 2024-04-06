using IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace CondefaMovilAPI.Controllers
{
    [Route("Orders")]
    [ApiController]
    [Authorize]

    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet]
        [Route("getOrderInfo")]
        public ActionResult getOrderInfo(string usuario)
        {
            try
            {
                var response = _pedidoService.getOrderInfo(usuario);
                if (response != null)
                    return Ok(response);

                return BadRequest("No se ha obtenido pedidos pendientes");
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al obtener los datos");
            }
        }

        [HttpGet]
        [Route("getOrdersDetailsList")]
        public ActionResult getOrdersDetailsList(string pedido, string usuario) {
            try
            {
                var response = _pedidoService.getOrdersDetails(pedido, usuario);
                if (response != null)
                    return Ok(response);
                return BadRequest("No se ha obtenido pedidos pendientes");
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al obtener los datos");
            }
        }

        [HttpGet]
        [Route("getDatamatrixList")]
        public ActionResult getDatamatrixList(string pedido) {
            try
            {
                var response = _pedidoService.getDatamatrixInfo(pedido);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al obtener los datos de los articulos escaneados");
            }
        }

        [HttpDelete]
        [Route("deleteDamatrix")]
        public ActionResult deleteDatamatrix(int id)
        {
            try
            {
                var response = _pedidoService.deleteDatamatrix(id);
                if (response)
                    return Ok("Se ha eliminado el registro exitosamente");
                return BadRequest("No se ha logrado eliminar el registro: ID:" + id);
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al intentar eliminar el registro");
            }
        }

        [HttpDelete]
        [Route("deleteOrder")]
        public ActionResult deleteOrder(int id, int renglon) {
            try
            {
                var response = _pedidoService.deleteOrder(id, renglon);
                if (response)
                    return Ok("Se ha eliminado el registro exitosamente");
                return BadRequest("No se ha logrado eliminar el registro: ID:" + id + " renglon: " + renglon);
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al intentar eliminar el registro");
            }
        }

        [HttpGet]
        [Route("getOrderDetails")]
        public ActionResult getOrderDetails(string pedido, string usuario)
        {
            try
            {
                var response = _pedidoService.getOrderDetails(pedido, usuario);
                if (response != null)
                    return Ok(response);
                return BadRequest("No se ha obtenido pedidos pendientes");
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al obtener los datos");
            }
        }

        [HttpGet]
        [Route("getOrderDetailsComplete")]
        public ActionResult getOrderDetails(string pedido, string usuario, string? renglonID, bool next)
        {
            try
            {
                var response = _pedidoService.getOrderDetailsComplete(pedido, usuario);
                if (next)
                {
                    //Si es la primera vez que se carga
                    if (renglonID == null || renglonID == "undefined" || renglonID == "0")
                    {
                        int id = response.Select(x => x.renglon2).Min();
                        return Ok(response.Where(x => x.renglon2 == id).FirstOrDefault());
                    }

                    //Si ya llego al ultimo registro de la lista, volver a iniciar
                    if (Convert.ToInt32(renglonID) == response.Select(x => x.renglon2).Max())
                    {
                        var renglon = response.Select(x => x.renglon2).Min();
                        return Ok(response.Where(x => x.renglon2 == renglon).FirstOrDefault());
                    }

                    //Buscar el siguiente registro valido
                    foreach (var item in response.OrderBy(x => x.renglon2))
                    {
                        if (item.renglon2 > Convert.ToInt32(renglonID))
                        {
                            return Ok(item);
                        }
                    }
                    if (response != null)
                        return Ok(response);
                    return BadRequest("No se ha obtenido pedidos pendientes");
                }
                else {
                    //Si es la primera vez que se carga
                    if (renglonID == null || renglonID == "undefined" || renglonID == "0")
                    {
                        int id = response.Select(x => x.renglon2).Min();
                        return Ok(response.Where(x => x.renglon2 == id).FirstOrDefault());
                    }
                    else if (Convert.ToInt32(renglonID) > response.Select(x => x.renglon2).Max()) {

                        int id = response.Select(x => x.renglon2).Min();
                        return Ok(response.Where(x => x.renglon2 == id).FirstOrDefault());
                    }
                    else {
                        return Ok(response.Where(x => x.renglon2 == Convert.ToInt32(renglonID)).FirstOrDefault());
                    }
                }

            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al obtener los datos");
            }
        }

        [HttpPut]
        [Route("updateOrderById")]
        public ActionResult updateOrderById(OrderDetails request) {
            try
            {
                var response = _pedidoService.updateInfo(request);
                if (response)
                    return Ok(true);
                return BadRequest("No se ha logrado modificar el registro seleccionado");
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al obtener los datos");
            }
        }

        [HttpPatch]
        [Route("updateScanOrder")]
        public ActionResult updateScanOrder(int id, int renglon, string art, string tipo, string usuario) {
            try
            {
                var subsArt1 = art.Substring(0, art.Length / 2);
                var subsArt12 = art.Substring((art.Length / 2), art.Length / 2);

                if (subsArt1 == subsArt12) {
                    art = subsArt1;
                }

                if (!_pedidoService.ActiveOrderAlistando(id,usuario))
                    return BadRequest("La orden que desea empezar a escanear no se ha iniciado, favor iniciar primero");
                
                if (tipo.ToLower() == "serie" && art.Length < 20 && _pedidoService.validateStorageInfo(usuario) != "112")
                    return BadRequest("El articulo que estás escaneando es trazable, favor escanear el datamatrix");

                var checkInfoResponse = _pedidoService.checkInfoData(id, renglon, art, tipo);
                if (checkInfoResponse.cantidad<=0)
                    return BadRequest("El articulo "+ checkInfoResponse.articulo + " no coincide con el de la linea o bien, el consecutivo no se encuentra registrado en inventario");

                if (_pedidoService.searchDatamatrixCaja(art)) {
                    return Ok(true);
                }

                if (tipo.ToLower() == "serie") {
                    var checkDatamatrix = _pedidoService.validateDatamatrixForInsert(art);
                    if (checkDatamatrix != null && checkDatamatrix.cantidad > 0)
                    {
                        var PedidosUtilizados = _pedidoService.DatamatrixPedidos(art);
                        return BadRequest($"El articulo con el datamatrix {checkDatamatrix.consecutivo} ya se encuentra utilizado en el pedido: " + PedidosUtilizados.MovId );
                    }
                }

                var response = _pedidoService.actualizarUnidades(id, renglon, art, tipo, usuario);
                if (response)
                    return Ok(response);
                return BadRequest("No hay unidades disponibles o bien que cumplan los criterios de vencimiento del cliente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("expiredDateMonths")]
        public ActionResult getExpiredMonths(string articulo, string id) {
            try
            {
                var response = _pedidoService.expiredDateMonths(articulo, id);
                if (response.mesesVencimiento < response.meses)
                    return Ok(response);
                return BadRequest(response);
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al procesar los datos");
            }
        }

        [HttpGet]
        [Route("getStorageData")]
        public ActionResult getStorageData(string articulo) {
            try
            {
                var response = _pedidoService.ObtenerBodegasArticulo(articulo);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al intentar obtener las bodegas asociadas");
            }
        }

        [HttpPatch]
        [Route("updateSituation")]
        public ActionResult updateSituation(string pedido, string situation, string usuario, string ?superUser) {
            try
            {

                if (_pedidoService.storageUser(usuario) == "112")
                {
                    if (_pedidoService.validateBeforeFinishBod1(Convert.ToInt16(pedido)) && situation == "En Empaque")
                        return BadRequest("Existen pedidos pendientes de finalizar, no se puede dar por terminada esta orden");

                    if (_pedidoService.updateSituation(pedido, situation, usuario))
                        return Ok(true);
                    return BadRequest("No se ha logrado modificar el registro seleccionado");
                }
                else {
                    if (superUser != null)
                    {
                        var separate = superUser.Split('/');
                        if (separate.Length < 2)
                            return BadRequest("El usuario indicado no cumple las caracteristicas para aplicar a un bocon y finalizar el pedido");

                        if (_pedidoService.checkRoleUser(separate[0], separate[1]))
                            return BadRequest("El usuario que intentas ingresar no posee los permisos para aplicar el bocon y finalizar el pedido");
                    }
                    // Valida si el usuario esta registrado como bodega 112 para no afectar el movimiento a los otros 
                    // Alistadores que tienen el pedido activo
                    if (_pedidoService.validateBeforeFinish(Convert.ToInt16(pedido)) && situation == "En Empaque")
                        return BadRequest("Existen pedidos pendientes de finalizar, no se puede dar por terminada esta orden");

                    if (!_pedidoService.basketAvaible(Convert.ToInt32(pedido)) && situation == "En Empaque")
                        return BadRequest("No se ha seleccionado ninguna canasta para este pedido, no se puede finalizar");

                    if (situation == "En Empaque")
                    {
                        if (_pedidoService.updateSituationAlistando(pedido, situation, usuario, superUser))
                            return Ok(true);
                        return BadRequest("No se ha logrado modificar el registro seleccionado");
                    }
                    else { 

                        if (_pedidoService.updateSituation(pedido, situation, usuario))
                            return Ok(true);
                        return BadRequest("No se ha logrado modificar el registro seleccionado");
                    }
                }
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al intentar iniciar el pedido");
            }
        }


        [HttpPost]
        [Route("saveJustifyBocom")]
        public ActionResult saveJustifyBocom(int id, int renglon, string tipo, string? usuario, string justificacion) {
            try
            {
                var separate = usuario.Split('/');
                if (separate.Length < 2 && tipo == "OCC")
                    return BadRequest("El usuario indicado no cumple las caracteristicas para aplicar a un bocon");

                if (_pedidoService.checkRoleUser(separate[0], separate[1]) && tipo == "OCC")
                    return BadRequest("El usuario que intentas ingresar no posee los permisos para aplicar el bocon");

                var response = _pedidoService.saveJustityMethod(id, renglon, tipo, separate[0]??"", justificacion);

                if (response)
                    return Ok("Se ha registrado la informacion correctamente");
                return BadRequest("No se ha podido registar la justificación del pedido");
            }
            catch (Exception ex)
            {
                return BadRequest("Ha ocurrido un error al intenter insertar los datos de proceso : "+ ex.Message);
            }
        }

        [HttpGet]
        [Route("searchDatamatrix")]
        public ActionResult searchDatamatrix(string articulo, string pedido) {
            try
            {
                var subsArt1 = articulo.Substring(0, articulo.Length / 2);
                var subsArt12 = articulo.Substring((articulo.Length / 2), articulo.Length / 2);

                if (subsArt1 == subsArt12)
                {
                    articulo = subsArt1;
                }

                var response = _pedidoService.searchArticleInfo(articulo, pedido);
                if (response.Count > 0)
                    return Ok(response);
                return BadRequest("El articulo o el datamatrix indicado aún no ha sido escaneado");
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al intentar realizar la consulta");
            }
        }

        [HttpDelete]
        [Route("deleteDatamatrix")]
        public ActionResult deleteDatamatrix(string pedido, string articulo) {
            try
            {
                var response = _pedidoService.eliminarArticulos(pedido, articulo);
                if (response)
                    return Ok();
                return BadRequest("No se ha logrado eliminar los articulos escaneados para estos articulos");
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al intentar eliminar los escaneos");
            }
        }

        [HttpGet]
        [Route("getValidateBocon")]
        public ActionResult getValidateBocon(string pedido) {
            try
            {
                return Ok(new { estado = _pedidoService.cantidadBoconPorPedido(pedido) });
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al intentar obtener datos");
            }
        }
    }
}
