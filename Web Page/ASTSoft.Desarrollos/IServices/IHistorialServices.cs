using Services.Messaging.ViewModels.Historial;
using Services.Messaging.ViewModels.Procesos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IHistorialServices
    {
        public List<ProcesosView> getList();
        public bool changeStatusRegister(ProcesosView request);
        public List<DetallePedidoView> getListDetails(string pedido);

    }
}
