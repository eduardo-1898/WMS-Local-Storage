using Dapper;
using IServices;
using Services.Messaging.ViewModels;
using Services.Messaging.ViewModels.Procesos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AST_RutasServices : IAST_RutasServices
    {

        public ConnectionString ConnectionString { get; }

        public AST_RutasServices(ConnectionString connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<AST_RutasView> getListRutas()
        {
            try
            {
                string sql = "SELECT Ruta as id, Ruta as descripcion FROM Ruta";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<AST_RutasView>(sql, commandType: CommandType.Text).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<AST_RutasView>();
            }
        }

        public bool aprovOut(ProcesosView request)
        {
            try
            {
                using IDbConnection db = new SqlConnection(ConnectionString.Value);
                string insertQuery = "sp_AST_FinalizarDespacho";
                var result = db.Execute(insertQuery, new { ID = request.id, ESTADO = request.finalizarDespacho  }, commandType: CommandType.StoredProcedure);
                db.Close();
                return (result > 0);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool volverDespachar(ProcesosView request)
        {
            try
            {
                using IDbConnection db = new SqlConnection(ConnectionString.Value);
                string insertQuery = "sp_AST_ReDespachar";
                var result = db.Execute(insertQuery, new { ID = request.id, situacion = request.estado }, commandType: CommandType.StoredProcedure);
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
