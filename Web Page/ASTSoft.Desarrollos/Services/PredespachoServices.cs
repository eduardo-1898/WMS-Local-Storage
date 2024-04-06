using Dapper;
using IServices;
using Services.Messaging.ViewModels;
using Services.Messaging.ViewModels.Predespacho;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PredespachoServices : IPredespachoServices
    {

        public ConnectionString ConnectionString { get; }

        public PredespachoServices(ConnectionString connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<PredespachoView> getListData()
        {
            try
            {
                string sql = "sp_AST_ObtenerListadoRutas";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<PredespachoView>(sql, commandType: CommandType.Text).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<PredespachoView>();
            }
        }

        public bool ReAsignar(string pedido, string justificacion)
        {
            try
            {
                using IDbConnection db = new SqlConnection(ConnectionString.Value);
                string insertQuery = "sp_AST_ReDespachar";

                var result = db.Execute(insertQuery, new { 
                    id = pedido, 
                    situacion = "Despacho",
                    justificacion = justificacion }, 
                    commandType: CommandType.StoredProcedure);

                db.Close();
                return (result > 0);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
