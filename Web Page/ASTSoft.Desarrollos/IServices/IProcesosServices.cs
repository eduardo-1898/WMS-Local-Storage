using Services.Messaging.ViewModels.Etiquetas;
using Services.Messaging.ViewModels.Procesos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IProcesosServices
    {
        public List<ProcesosView> getList();
        public List<ProcesosView> getListHistorial(DateTime fechaInicio, DateTime fechaFinal, string estado);
        public bool UpdateRoute(ProcesosView request);
        public bool UpdateEnlisted(ProcesosView request);
        public bool CancelOrder(ProcesosView request);
        public BultosEtiquetas getPedidoInfo(string pedido);
        
        /// <summary>
        /// Obtiene los datos del documento en caso de que unifiquen varios pedidos en una unica caja
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BultosEtiquetas getDocInfo(long id);

    }
}
