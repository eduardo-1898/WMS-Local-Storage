using Services.Messaging.ViewModels.Canasta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface ICanastaService
    {
        public List<CanastaView> getCanastas(string pedido);
    }
}
