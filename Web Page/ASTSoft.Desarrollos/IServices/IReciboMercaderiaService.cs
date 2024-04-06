using Services.Messaging.ViewModels.Articulos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IReciboMercaderiaService
    {
        public List<ArticulosView> ObtenerArtOC(string MovID);
        public bool GuardarCantidad(List<ArticulosView> articulos);
        public List<ArticulosView> ObtenerDiferenciasOC(string OrdenCompra);
        public bool EnviarCorreo(string OrdenCompra, string email, string ruta);
        public int FinalizarOC(string OrdenCompra, int Tipo);
        public bool GuardarCodigoInex(string OrdenCompra, string CodigoBarras);
        public List<ArticulosView> ObtenerArtOCCodigoBarras(string OrdenCompra, string CodigoBarras);
        public List<ArticulosView> ObtenerArt(string Filtro);
        public int ObtenerCantidadEscaneados(string OrdenCompra);
    }
}
