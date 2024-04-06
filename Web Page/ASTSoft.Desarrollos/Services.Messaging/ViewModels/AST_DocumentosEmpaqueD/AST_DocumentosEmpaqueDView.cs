using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels.AST_DocumentosEmpaqueD
{
    public class AST_DocumentosEmpaqueDView
    {
        public long id { get; set; }
        public long idDocumento { get; set; }
        public string pedido { get; set; }
        public DateTime fechaAsociacion { get; set; }
        public int cantidadBultos { get; set; }


    }
}
