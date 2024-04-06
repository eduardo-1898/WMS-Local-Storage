using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class DespachoInfo
    {
        public int Consecutivo { get; set; }
        public List<DespachoDetails>? Despachos { get; set; }
    }
}
