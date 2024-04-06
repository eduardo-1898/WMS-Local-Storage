using Dapper;
using IServices;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Services
{
    public class PedidoService : IPedidoService
    {
        public ConnectionString ConnectionString { get; }

        public PedidoService(ConnectionString connectionString)
        {
            ConnectionString = connectionString;
        }

        public OrderDetails getOrderDetails(string pedido, string usuario)
        {
            try
            {
                string sql = "sp_AST_ObtieneOrdenDetalle";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<OrderDetails>(sql, new { 
                    pedido= pedido,
                    usuario = usuario
                }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new OrderDetails();
            }
        }

        public List<OrderDetails> getOrderDetailsComplete(string pedido, string usuario)
        {
            try
            {
                string sql = "sp_AST_ObtieneOrdenDetalleCompleto";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<OrderDetails>(sql, new
                {
                    pedido = pedido,
                    usuario = usuario
                }, commandType: CommandType.StoredProcedure).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<OrderDetails>();
            }
        }

        public List<BodegasArticulos> ObtenerBodegasArticulo(string articulo)
        {
            try
            {
                string sql = "sp_AST_ObtenerBodegasArticulos";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<BodegasArticulos>(sql, new { art = articulo }, commandType: CommandType.StoredProcedure).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<BodegasArticulos>();
            }
        }
        public OrderInfo getOrderInfo(string usuario)
        {
            try
            {
                string sql = "sp_AST_ObtieneOrdenInfo";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<OrderInfo>(sql, new { usuario = usuario }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new OrderInfo();
            }
        }

        public bool updateInfo(OrderDetails request)
        {
            try
            {
                string sql = "sp_AST_ActualizarOrden";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new { 
                    cantidad = request.Cantidad, 
                    renglon = request.Renglon,
                    articulo = request.Articulo,
                    idPedido = request.Id
                } ,commandType: CommandType.StoredProcedure);
                connection.Close();

                return response>0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool updateSituation(string pedido, string situacion, string usuario)
        {
            try
            {
                string sql = "sp_AST_ActualizarSituacion";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new
                {
                    pedido = pedido,
                    situacion = situacion,
                    usuario = usuario
                }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool cancelLineOrder(string pedido, int renglon)
        {
            try
            {
                string sql = "sp_AST_CancelarOrden";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new
                {
                    pedido = pedido,
                    renglon = renglon,
                    estado = "CANCELADA"
                }, commandType: CommandType.StoredProcedure); ;
                connection.Close();

                return response > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public List<OrderDetails> getOrdersDetails(string pedido, string usuario)
        { 
            try
            {
                string sql = "sp_AST_ObtieneOrdenes";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<OrderDetails>(sql, new { pedido=pedido, usuario=usuario}, commandType: CommandType.StoredProcedure).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<OrderDetails>();
            }
        }

        public bool deleteOrder(int id, int renglon)
        {
            try
            {
                string sql = "sp_AST_EliminarLinea";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new
                {
                    id = id,
                    renglon = renglon
                }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool actualizarUnidades(int id, int renglon, string art, string tipo, string usuario)
        {
            try
            {
                string sql = "sp_AST_ActualizarUnidades";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new
                {
                    id = id,
                    renglon = renglon,
                    art = art,
                    tipo = tipo,
                    usuario = usuario
                }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool validateBeforeFinish(int id)
        {
            try
            {
                string sql = "SELECT COUNT(*) AS cantidad FROM VentaD WHERE id = (SELECT ID from Venta WHERE MovID = @id and Mov = 'Pedido') AND ISNULL(AST_EstatusAlisto,0) = 0";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<OrderInfo>(sql, new { id=id } ,commandType: CommandType.Text);
                connection.Close();

                return response.Cantidad>0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool validateBeforeFinishBod1(int id)
        {
            try
            {
                string sql = "SELECT COUNT(*) AS cantidad FROM VentaD WHERE id = (SELECT ID from Venta WHERE MovID = @id and Mov = 'Pedido') AND ISNULL(AST_EstatusAlistoBod1,0) = 0 AND Almacen = '112' ";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<OrderInfo>(sql, new { id = id }, commandType: CommandType.Text);
                connection.Close();

                return response.Cantidad > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool basketAvaible(int id) {
            try
            {
                string sql = "SELECT count(*) as cantidad FROM AST_CanastasPedidos WHERE idPedido = @id";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<OrderInfo>(sql, new { id = id }, commandType: CommandType.Text);
                connection.Close();

                return response.Cantidad > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ActiveOrderAlistando(int id,string usuario) {
            try
            {
                /*RECIBIR ACA EL USUARIO Y VER SI LA BODEGA DEL USUARIO ES 112 VALIDAR CONTRA SituacionBod1 SINO CONTRA SITUACION*/
                /*JEREMY 03/05/2024 SE CAMBIA PORQUE NO SIEMPRE SE ESTAN ALISTANDO DE PRIMERO EN LA BODEGA 114 Y EN CASOS PUEDE SER EN LA 112*/
                string sql = "IF @usuario = '' " +
                    "         BEGIN " +
                    "           SELECT COUNT(*) AS cantidad FROM Venta WHERE MovId = @id AND (ISNULL(Situacion, 'ACTIVO') IN ('Alistando') OR ISNULL(SITUACIONBOD1, 'ACTIVO') IN ('Alistando')) AND Estatus = 'PENDIENTE' " +
                    "         END " +
                    "         ELSE " +
                    "         BEGIN " +
                    "           IF(SELECT ISNULL(AlmacenAsignado,'114') FROM AST_USUARIOS WHERE USUARIO = @usuario) = '114' " +
                    "           BEGIN " +
                    "               SELECT COUNT(*) AS cantidad FROM Venta WHERE MovId = @id AND ISNULL(Situacion, 'ACTIVO') IN ('Alistando') AND Estatus = 'PENDIENTE' " +
                    "           END " +
                    "           ELSE " +
                    "           BEGIN " +
                    "               SELECT COUNT(*) AS cantidad FROM Venta WHERE MovId = @id AND ISNULL(SituacionBod1, 'ACTIVO') IN ('Alistando') AND Estatus = 'PENDIENTE' " +
                    "           END  " +
                    "          END ";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<OrderInfo>(sql, new { id= id, usuario = usuario }, commandType: CommandType.Text);
                connection.Close();

                return response.Cantidad > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool saveJustityMethod(int id, int renglon, string tipo, string? usuario, string justificacion)
        {
            try
            {
                string sql = "sp_AST_JustificacionBocom";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new
                {
                    id = id,
                    renglon = renglon,
                    tipo = tipo,
                    usuario = usuario,
                    justificacion = justificacion
                }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public consultasUtils checkInfoData(int id, int renglon, string art, string serie)
        {
            try
            {
                string sql = "sp_AST_RevisarArticuloScan";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<consultasUtils>(sql, new { 
                    art = art,        
                    id= id,
                    renglon = renglon
                } ,commandType: CommandType.StoredProcedure);
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new consultasUtils();
            }
        }

        public bool checkRoleUser(string user, string pass)
        {
            try
            {
                string sql = "SELECT usuario FROM AST_Usuarios WHERE AST_Supervisor = 1 AND Usuario = @usuario AND Contrasenna = @pass";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<string>(sql, new
                {
                   usuario = user,
                   pass= pass
                }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return (response == null || response == string.Empty);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<DatamatrixView> getDatamatrixInfo(string pedido) {
            try
            {
                string sql = "sp_AST_ObtenerConsencutivosEscaneado";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<DatamatrixView>(sql, new
                {
                    pedido = pedido
                }, commandType: CommandType.StoredProcedure).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<DatamatrixView>();
            }
        }
        public consultasUtils validateDatamatrixForInsert(string datamatrix)
        {
            try
            {
                string sql = "SELECT COUNT(*) as cantidad, (SUBSTRING(sl.SerieLote, CHARINDEX(sl.Propiedades, sl.serielote)-7,5)) as consecutivo FROM AST_SerieLoteMov ast LEFT JOIN SerieLote sl ON ast.DataMatrix = sl.SerieLote WHERE DataMatrix = @datamatrix AND Modulo = 'VTAS' GROUP BY sl.SerieLote, sl.Propiedades";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<consultasUtils>(sql, new
                {
                    datamatrix = datamatrix
                }, commandType: CommandType.Text);
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new consultasUtils();
            }
        }

        public consultasMovId DatamatrixPedidos(string datamatrix)
        {
            try
            {
                string sql = "SELECT top 1 ast.ID as MovId FROM AST_SerieLoteMov ast LEFT JOIN SerieLote sl ON ast.DataMatrix = sl.SerieLote " +
                    "         WHERE DataMatrix = @datamatrix AND Modulo = 'VTAS' " +
                    "         ORDER BY AST.ID DESC";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<consultasMovId>(sql, new
                {
                    datamatrix = datamatrix
                }, commandType: CommandType.Text);
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new consultasMovId();
            }
        }

        public bool deleteDatamatrix(int id)
        {
            try
            {
                string sql = "sp_AST_EliminarDatamatrix";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new
                {
                    id = id
                }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<ConsecutivosEscaneados> searchArticleInfo(string articulo, string pedido)
        {
            try
            {
                string sql = "sp_AST_ObtenerConsecutivosEscaneados";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<ConsecutivosEscaneados>(sql, new
                {
                    articulo = articulo,
                    pedido = pedido
                }, commandType: CommandType.StoredProcedure).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<ConsecutivosEscaneados>();
            }
        }

        public string storageUser(string usuario)
        {
            try
            {
                string sql = " SELECT AlmacenAsignado FROM AST_Usuarios WHERE usuario = @usuario ";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<string>(sql, new
                {
                    usuario = usuario
                }, commandType: CommandType.Text);
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public VencimientosModel expiredDateMonths(string art, string id)
        {
            try
            {
                string sql = "sp_AST_ValidacionVencimientoProducto";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<VencimientosModel>(sql, new
                {
                    art = art,
                    id = id
                }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string validateStorageInfo(string usuario)
        {
            try
            {
                string sql = "SELECT AlmacenAsignado FROM AST_Usuarios WHERE Usuario = @usuario ";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<string>(sql, new
                {
                    usuario = usuario
                }, commandType: CommandType.Text);
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool eliminarArticulos(string pedido, string articulo)
        {
            try
            {
                string sql = "sp_AST_EliminarDatamatrixTotal";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new{ articulo, pedido }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response>0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool cantidadBoconPorPedido(string pedido)
        {
            try
            {
                string sql = "SELECT COUNT(*) AS CANTIDAD FROM VentaD WHERE id = (SELECT ID FROM Venta WHERE MovID = @pedido AND Mov = 'Pedido' AND Estatus = 'PENDIENTE') AND AST_BoconTipo is not null";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QueryFirstOrDefault<int>(sql, new { pedido = pedido }, commandType: CommandType.Text);
                connection.Close();

                return response > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool updateSituationAlistando(string pedido, string situacion, string usuario, string superUser)
        {
            try
            {
                string sql = "sp_AST_ActualizarSituacionAlistando";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new
                {
                    pedido = pedido,
                    situacion = situacion,
                    usuario = usuario,
                    superUsuario = superUser
                }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool searchDatamatrixCaja(string caja)
        {
            try
            {
                string sql = "sp_AST_AgregarDatamatrixCajaScan";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new
                {
                    @caja = caja,
                }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
