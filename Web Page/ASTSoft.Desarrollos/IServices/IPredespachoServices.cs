using Services.Messaging.ViewModels.Predespacho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IPredespachoServices
    {
        public bool ReAsignar(string pedido, string justificacion);
        public List<PredespachoView> getListData();

    }
}
