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
    public class CanastaService : ICanastaService
    {

        public ConnectionString ConnectionString { get; }

        public CanastaService(ConnectionString connectionString)
        {
            ConnectionString = connectionString;
        }

        public BasketInfo getBasketInfo()
        {
            try
            {
                string sql = "sp_AST_ObtieneCanastaInfo";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<BasketInfo>(sql, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new BasketInfo();
            }
        }

        public List<BasketInfo> getBasketList(string pedido)
        {
            try
            {
                string sql = "sp_AST_ObtieneCanastaInfo";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<BasketInfo>(sql, new { pedido = pedido} ,commandType: CommandType.StoredProcedure).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<BasketInfo>();
            }
        }

        public bool updateBasketInfo(BasketInfo request)
        {
            try
            {
                string sql = "sp_AST_ActualizaCanastas";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new
                {
                    id = request.IdPedido,
                    idCanasta = request.Canasta
                }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string insertBasket(BasketInfo request)
        {
            try
            {
                string sql = "sp_AST_InsertarCanastas";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<string>(sql, new
                {
                    idPedido = request.IdPedido,
                    canasta = request.Canasta
                }, commandType: CommandType.StoredProcedure);
                connection.Close();
                return response;

            }
            catch (Exception ex)
            {
                return "ERROR";
            }
        }
    }
}
