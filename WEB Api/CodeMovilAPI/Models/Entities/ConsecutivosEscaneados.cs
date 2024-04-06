using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class ConsecutivosEscaneados
    {
        public int id { get; set; }
        public string articulo { get; set; }
        public string consecutivo { get; set; }
        public string lote { get; set; }
        public DateTime vencimiento { get; set; }
    }
}
