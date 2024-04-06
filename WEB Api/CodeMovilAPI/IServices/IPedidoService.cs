using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IPedidoService
    {
        public OrderInfo getOrderInfo(string usuario);
        public OrderDetails getOrderDetails(string pedido, string usuario);
        public List<OrderDetails> getOrderDetailsComplete(string pedido, string usuario);
        public List<OrderDetails> getOrdersDetails(string pedido, string usuario);
        public bool updateInfo(OrderDetails request);
        public bool updateSituation(string pedido, string situacion, string usuario);
        public bool updateSituationAlistando(string pedido, string situacion, string usuario, string superUser);
        public bool cancelLineOrder(string pedido, int renglon);
        public bool deleteOrder(int id, int renglon);
        public bool actualizarUnidades(int id, int renglon, string art, string tipo, string usuario);
        public bool validateBeforeFinish(int id);
        public bool ActiveOrderAlistando(int id,string usuario);
        public bool saveJustityMethod(int id, int renglon, string tipo, string? usuario, string justificacion);
        public consultasUtils checkInfoData(int id, int renglon, string art, string serie);
        public bool checkRoleUser(string user, string pass);
        public bool basketAvaible(int id);
        public bool validateBeforeFinishBod1(int id);
        public consultasUtils validateDatamatrixForInsert(string datamatrix);
        public consultasMovId DatamatrixPedidos(string datamatrix);
        public bool deleteDatamatrix(int id);
        public List<DatamatrixView> getDatamatrixInfo(string pedido);
        public List<BodegasArticulos> ObtenerBodegasArticulo(string articulo);
        public List<ConsecutivosEscaneados> searchArticleInfo(string articulo, string pedido);
        public string storageUser(string usuario);
        public VencimientosModel expiredDateMonths(string art, string id);
        public string validateStorageInfo(string usuario);
        public bool eliminarArticulos(string pedido, string articulo);
        public bool cantidadBoconPorPedido(string pedido);
        public bool searchDatamatrixCaja(string caja);
    }
}
