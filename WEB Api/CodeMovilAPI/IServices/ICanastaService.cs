using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface ICanastaService
    {
        public BasketInfo getBasketInfo();
        public List<BasketInfo> getBasketList(string pedido);
        public bool updateBasketInfo(BasketInfo request);
        public string insertBasket(BasketInfo request);

    }
}
