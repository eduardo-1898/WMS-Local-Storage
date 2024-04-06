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
    public class AlistoServices : IAlistoServices
    {

        public ConnectionString ConnectionString { get; }

        public AlistoServices(ConnectionString connectionString)
        {
            ConnectionString = connectionString;
        }

        public bool getArticulo(string articulo, int id, int renglon)
        {
			try
			{
                string sql = "sp_AST_ActualizaCanastas";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new
                {
                    articulo = articulo,
                    id = id,
                    renglon = renglon
                }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response > 0;
            }
			catch (Exception)
			{
                return false;
			}
        }
    }
}
