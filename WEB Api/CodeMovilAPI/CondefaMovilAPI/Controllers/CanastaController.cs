using IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Services;

namespace CondefaMovilAPI.Controllers
{
    [Route("Basket")]
    [ApiController]
    [Authorize]
    public class CanastaController : ControllerBase
    {
        private readonly ICanastaService _canastaService;
        private readonly IPedidoService _pedidoService;

        public CanastaController(ICanastaService canastaService, IPedidoService pedidoService)
        {
            _canastaService = canastaService;
            _pedidoService = pedidoService;
        }

        [HttpGet]
        [Route("GetBasketInfo")]
        public ActionResult GetBasketInfo()
        {
            try
            {
                var response = _canastaService.getBasketInfo();
                if (response != null)
                    return Ok(response);
                return BadRequest("No se ha obtenido información de una canasta asociada");
            }
            catch (Exception)
            {
                return BadRequest("No se ha logrado realizar la solicitud exitosamente");
            }
        }

        [HttpGet]
        [Route("GetBaskets")]
        public ActionResult GetBaskets(string pedido)
        {
            try
            {
                var response = _canastaService.getBasketList(pedido);
                if (response != null)
                    return Ok(response);
                return BadRequest("No se ha obtenido información de una canasta asociada");
            }
            catch (Exception)
            {
                return BadRequest("No se ha logrado realizar la solicitud exitosamente");
            }
        }

        [HttpPut]
        [Route("updateBasketInfo")]
        public ActionResult updateBasketInfo(BasketInfo request) 
        {
            try
            {
                var response = _canastaService.updateBasketInfo(request);
                if (response)
                    return Ok(true);
                return BadRequest("No se ha logrado modificar el registro seleccionado");
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al obtener los datos");
            }
        }

        [HttpPost]
        [Route("insertBasket")]
        public ActionResult insertBasket(BasketInfo request) {
            try
            {
                if (!_pedidoService.ActiveOrderAlistando(request.IdPedido,""))
                    return BadRequest("La orden que desea asociar una canasta no se ha iniciado, favor iniciar primero");
                
                if (request.Canasta.Length != 4 && request.Canasta.Length != 5)
                    return BadRequest("La canasta ingresada no concuerda con el estandar registrado");
                
                var response = _canastaService.insertBasket(request);

                if (response == "OK")
                    return Ok();
                else if (response == "ERROR")
                    return BadRequest("No se ha logrado insertar el nuevo registro");
                else
                    return BadRequest(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
