using IServices;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Services
{
    public class DespachosService : IDespachosService
    {

        public ConnectionString ConnectionString { get; }

        public DespachosService(ConnectionString connectionString)
        {
            ConnectionString = connectionString;
        }

        public int createNewDespacho(string ruta)
        {
            try
            {
                string sql = "sp_AST_CrearNuevoDespacho";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.ExecuteScalar(sql, new { ruta=ruta } ,commandType: CommandType.StoredProcedure);
                connection.Close();

                return (int)response;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool InsertNewScan(string pedido, int consecutivo, int bulto) {
            try
            {
                string sql = "sp_AST_InsertarEscaneo";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new { pedido = pedido, consecutivo = consecutivo, bulto = bulto }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response>0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DespachoDetails> ObtenerPedidosBultos(long doc) {
            try
            {
                string sql = "SELECT MovId as Pedido, AST_Bultos as bulto FROM Venta WHERE MovID IN (SELECT Pedido FROM AST_DocumentosEmpaqueD WHERE idDocumento = @doc) AND Mov = 'Pedido'";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<DespachoDetails>(sql, new { doc }, commandType: CommandType.Text).ToList();
                connection.Close();

                return response;

            }
            catch (Exception ex) 
            {
                return new List<DespachoDetails>();
            }
        }

        public bool InsertNewScanDoc(long doc, int consecutivo)
        {
            try
            {
                var list = ObtenerPedidosBultos(doc);
                foreach (var item in list)
                {
                    for (int i = 1; i <= item.bulto; i++)
                    {
                        string sql = "sp_AST_InsertarEscaneoDoc";
                        using var connection = new SqlConnection(ConnectionString.Value);
                        connection.Open();
                        var response = connection.Execute(sql, new { pedido = item.Pedido, consecutivo, bulto = i }, commandType: CommandType.StoredProcedure);
                        connection.Close();
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DespachoInfo getListDespacho(int id)
        {
            try
            {
                var despacho = new DespachoInfo();
                string sql = "sp_AST_ObtieneListadoDespacho";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<DespachoDetails>(sql, new { consecutivo = id} ,commandType: CommandType.StoredProcedure).ToList();
                connection.Close();

                despacho.Despachos = response;
                despacho.Consecutivo = id;
                return despacho;
            }
            catch (Exception ex)
            {
                return new DespachoInfo();
            }
        }

        public bool finishScanDespacho(int consecutivo, string conductor) {
            try
            {
                string sql = "sp_AST_ActualizarEstado";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new { consecutivo = consecutivo, conductor = conductor}, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool validaForInsert(string ruta)
        {
            try
            {
                string sql = "SELECT COUNT(*) CANTIDAD FROM AST_BultosDespacho WHERE ruta = @ruta AND CAST(fechaCreacion as date) = CAST(GETDATE() as date) AND Finalizado = 0";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.ExecuteScalar(sql, new { ruta = ruta }, commandType: CommandType.Text);
                connection.Close();

                return (int)response > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool deleteScanDespacho(int id)
        {
            try
            {
                string sql = "DELETE FROM AST_BultosDespachoD WHERE ID = @id";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new { id = id }, commandType: CommandType.Text);
                connection.Close();

                return response > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<RutasInfo> getRoutes()
        {
            try
            {

                string sql = "SELECT ruta, consecutivo, ISNULL(Finalizado,0) as Finalizado FROM AST_BultosDespacho WHERE CAST(fechaCreacion AS DATE) = CAST(GETDATE() AS DATE)";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<RutasInfo>(sql, commandType: CommandType.Text).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<RutasInfo>();
            }
        }

        public bool updateBultos(string canasta, int bultos)
        {
            try
            {
                string sql = "sp_AST_ActualizarBultos";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new { canasta = canasta, bultos = bultos }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ValidateBultoForUpdate(string canasta)
        {
            try
            {
                string sql = "SELECT COUNT(v.ID) FROM Venta v LEFT JOIN AST_CanastasPedidos c ON v.MovID = c.idPedido WHERE ISNULL(v.AST_AprobacionDespacho,0) = 1 AND Situacion = 'En Empaque' AND c.canasta = @canasta";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.ExecuteScalar(sql, new { canasta = canasta }, commandType: CommandType.Text);
                connection.Close();

                return (int)response > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ValidateBeforeFinish(int consecutivo)
        {
            try
            {
                string sql = "sp_AST_RevisarFinalizacionDespacho";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<int>(sql, new { consecutivo = consecutivo }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response == 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
