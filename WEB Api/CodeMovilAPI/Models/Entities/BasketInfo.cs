using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class BasketInfo
    {
        public string idCanasta {  get; set; }
        public int IdPedido { get; set; }
        public DateTime fechaAsociada { get; set; }
        public string? Canasta { get; set; }

    }
}
