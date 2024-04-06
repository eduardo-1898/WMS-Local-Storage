using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels.Datamatrix
{
    public class AST_DatamatrixView
    {
        public int id { get; set; }
        public string Articulo { get; set; }
        public string Consecutivo { get; set; }
        public string Lote { get; set; }
        public string FechaVencimiento { get; set; }
        public int idCaja { get; set; }
        public string Datamatrix { get; set; }
        public int MovID { get; set; }
        public int RenglonID { get; set; }

    }
}
