using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class DespachoDetails
    {
        public int? id { get; set; }
        public string? Pedido { get; set; }
        public int? Caja { get; set; }
        public string? Tipo { get; set; }
        public string ? Registrado { get; set; }
        public int bulto { get; set; }
    }
}
