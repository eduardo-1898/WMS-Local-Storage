using Dapper;
using IServices;
using Services.Messaging.ViewModels.Alm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AlmService : IAlmService
    {
        public ConnectionString ConnectionString { get; }

        public AlmService(ConnectionString connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<AlmView> ObtenerAlmacenes()
        {
            try
            {
                string sql = "AST_ObtenerAlmacenes";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<AlmView>(sql, commandType: CommandType.StoredProcedure).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<AlmView>();
            }
        }
    }
}
