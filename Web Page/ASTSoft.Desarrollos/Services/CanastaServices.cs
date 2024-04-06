using Dapper;
using IServices;
using Services.Messaging.ViewModels.Articulos;
using Services.Messaging.ViewModels.Canasta;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CanastaServices : ICanastaService
    {
        public ConnectionString ConnectionString { get; }

        public CanastaServices(ConnectionString connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<CanastaView> getCanastas(string pedido)
        {
            try
            {
                string sql = "SELECT id, canasta, leida, idPedido, IdCanasta FROM AST_CanastasPedidos where idPedido = @idPedido";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<CanastaView>(sql, new {idPedido = pedido} ,commandType: CommandType.Text).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<CanastaView>();
            }
        }
    }
}
