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
    public class UsuariosService : IUsuariosService
    {
        public ConnectionString ConnectionString { get; }

        public UsuariosService(ConnectionString connectionString)
        {
            ConnectionString = connectionString;
        }

        public AuthRequest SingIn(AuthRequest request)
        {
            try
            {
                string sql = "sp_AST_IniciarSesion";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<AuthRequest>(sql, new{ username = request.Username, password = request.Password }, commandType: CommandType.StoredProcedure);
                if (response != null)
                {
                    response.IsLogued = true;
                }
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new AuthRequest();
            }
        }
    }
}
