using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class RutasInfo
    {
        public int Consecutivo { get; set; }
        public string? ruta { get; set; }
        public bool Finalizado { get; set; }
    }
}
