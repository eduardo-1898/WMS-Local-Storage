using IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace CondefaMovilAPI.Controllers
{
    [Route("Alisto")]
    [ApiController]
    [Authorize]
    public class AlistoController : ControllerBase
    {
        private readonly IAlistoServices _alistoService;
        private readonly IPedidoService _pedidoService;

        public AlistoController(IAlistoServices alistoService, IPedidoService pedidoService)
        {
            _alistoService = alistoService;
            _pedidoService = pedidoService;
        }

        [HttpGet]
        [Route("ValidateArticle")]
        public bool ValidateArticle(string articulo, int id, int renglon) {
            try
            {
                var response = _alistoService.getArticulo(articulo, id, renglon);
                return response;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
