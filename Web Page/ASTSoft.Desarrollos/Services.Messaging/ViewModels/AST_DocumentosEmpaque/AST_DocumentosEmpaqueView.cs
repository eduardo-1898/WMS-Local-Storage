using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels.AST_DocumentosEmpaque
{
    public class AST_DocumentosEmpaqueView
    {
        public long id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string usuarioCreador { get; set; }
        public int cantidadBultos { get; set; }
    }
}
