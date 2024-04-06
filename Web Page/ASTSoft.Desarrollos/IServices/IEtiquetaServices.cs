using Services.Messaging.ViewModels.Etiquetas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IEtiquetaServices
    {
        public EtiquetasView getDataInfo(string pedido);
        public bool aceptarAlisto(string pedido, int bultos, string usuario, string ObservacionesExtra);
        public bool CambiarSituacionPredespacho(string pedido);
    }
}
