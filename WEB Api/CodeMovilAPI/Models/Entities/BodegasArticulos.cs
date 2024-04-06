using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class BodegasArticulos
    {
        public string Bodega { get; set; }
        public string Localizacion { get; set; }
        public string Disponible { get; set; }
        public string Reservado { get; set; }
    }
}
