using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels.Util
{
    public class EtiquetasModel
    {
        public string datamatrix { get; set; }
        public int consecutivo { get; set; }
        public string? qr { get; set; }
        public string? consecutivoReal { get; set; }
    }
}
