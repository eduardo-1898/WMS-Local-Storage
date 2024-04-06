using Services.Messaging.ViewModels.Articulos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IArticulosServices
    {
        public List<ArticulosView> getArticulos(string pedido);

    }
}
