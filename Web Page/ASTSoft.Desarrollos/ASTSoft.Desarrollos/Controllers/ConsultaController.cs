using IServices;
using Microsoft.AspNetCore.Mvc;
using Services.Messaging.ViewModels;

namespace ASTSoft.Desarrollos.Controllers
{
    public class ConsultaController : Controller
    {

        private readonly IConsultaServices _consultaServices;


        public ConsultaController(IConsultaServices consultaServices)
        {
            _consultaServices = consultaServices;
        }

        public IActionResult Index()
        {
            var model = new ConsultaViewModel();
            return View();
        }

        public ActionResult ObtenerModelo(ConsultaViewModel model)
        {
            try
            {
                var modelNew = new ConsultaViewModel();                
                var detalle = _consultaServices.ObtenerArticulo(model.detalleArticulo.ArticuloBusqueda);

                if (detalle == null) {
                    TempData["ErrorMesagge"] = "El articulo no se encuentra registrado";
                    return View("Index");
                }

                var lote = _consultaServices.ObtenerDetalleArticulo(model.detalleArticulo.ArticuloBusqueda);
                var datamatrix = _consultaServices.ObtenerHistorialDatamatrix(model.detalleArticulo.ArticuloBusqueda);
                var bodegas = _consultaServices.ObtenerBodegasArticulo(model.detalleArticulo.ArticuloBusqueda);
                modelNew.detalleArticulo = detalle;
                modelNew.almacenArticulos = lote;
                modelNew.detalleDatamatrix = datamatrix;
                modelNew.bodegasArticulos = bodegas;

                return View("Index", modelNew);

            }
            catch (System.Exception)
            {
                return View("Index", new ConsultaViewModel());
            }
        }
    }
}
