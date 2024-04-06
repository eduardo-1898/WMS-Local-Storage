using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IAlistoServices
    {
        public bool getArticulo(string articulo, int id, int renglon);
    }
}
