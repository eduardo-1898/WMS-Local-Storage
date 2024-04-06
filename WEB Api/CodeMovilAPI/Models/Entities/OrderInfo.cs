using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class OrderInfo
    {
        public string? Pedido { get; set; }
        public int NumPedido { get; set; }
        public string? Referencia { get; set; }
        public string? Cliente { get; set; }
        public string? Observacion { get; set; }
        public string? Ruta { get; set; }
        public int? Cantidad { get; set; }
        public string? Estado { get; set; }
        public DateTime fecha { get; set; }
    }
}
