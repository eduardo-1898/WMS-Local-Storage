using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels.Predespacho
{
    public class PredespachoView
    {
        public int consecutivo { get; set; }
        public string Ruta { get; set; }
        public string pedido { get; set; }
        public int bulto { get; set; }
        public string estado { get; set; }
        public bool finalizado { get; set; }

    }
}
