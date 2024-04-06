using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels.AST_SerieLoteMov
{
    public class AST_SerieLoteMovView
    {
        public string Empresa { get; set; }
        public string Modulo { get; set; }
        public int ID { get; set; }
        public int RenglonID { get; set; }
        public string Articulo { get; set; }
        public string SerieLote { get; set; }
        public int Cantidad { get; set; }
        public string Datamatrix { get; set; }
        public string Procesado { get; set; }
        public int IdFactura { get; set; }
        public DateTime Vencimiento { get; set; }
    }
}
