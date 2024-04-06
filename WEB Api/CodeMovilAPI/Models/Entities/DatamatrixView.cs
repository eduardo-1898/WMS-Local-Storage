using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class DatamatrixView
    {
        public int id { get; set; }
        public string articulo { get; set; }
        public string consecutivo { get; set; }
        public string lote { get; set; }
    }

    public class consultasUtils { 
        public int cantidad { get; set; }
        public string? consecutivo { get; set; }
        public string? articulo { get; set; }
    }

    public class consultasMovId
    {
        public string? MovId { get; set; }
    }
}
