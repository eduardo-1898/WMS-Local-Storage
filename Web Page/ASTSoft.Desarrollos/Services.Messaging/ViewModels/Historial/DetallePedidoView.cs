using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels.Historial
{
    public class DetallePedidoView
    {
        public string articulo { get; set; }
        public int linea { get; set; }
        public int cantidad { get; set; }
        public string descripcion { get; set; }
    }
}
