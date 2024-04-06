using Dapper;
using IServices;
using Services.Messaging.ViewModels.Etiquetas;
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
    public class ProcesosServices : IProcesosServices
    {
        public ConnectionString ConnectionString { get; }

        public ProcesosServices(ConnectionString connectionString) {
            ConnectionString = connectionString;
        }

        public bool CancelOrder(ProcesosView request)
        {
            try
            {
                using IDbConnection db = new SqlConnection(ConnectionString.Value);
                string insertQuery = "sp_AST_CancelarOrden";
                var result = db.Execute(insertQuery, request, commandType: CommandType.StoredProcedure);
                db.Close();
                return (result > 0);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<ProcesosView> getList()
        {
            try
            {
                string sql = "sp_AST_ObtenerListado";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<ProcesosView>(sql, commandType: CommandType.StoredProcedure).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<ProcesosView>();
            }
        }

        public bool UpdateEnlisted(ProcesosView request)
        {
            try
            {
                using IDbConnection db = new SqlConnection(ConnectionString.Value);
                string insertQuery = "sp_AST_ActualizarAlistado";
                var result = db.Execute(insertQuery, request, commandType: CommandType.StoredProcedure);
                db.Close();
                return (result > 0);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateRoute(ProcesosView request)
        {
            try
            {
                using IDbConnection db = new SqlConnection(ConnectionString.Value);
                string insertQuery = "sp_AST_ActualizarRuta";
                var result = db.Execute(insertQuery, new { 
                    id = request.id, 
                    idRuta = request.idRuta, 
                    usuario = request.usuario, 
                    usuarioBOD = request.usuarioBOD1, 
                    prioridad = request.prioridad }, commandType: CommandType.StoredProcedure);
                db.Close();
                return (result > 0);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<ProcesosView> getListHistorial(DateTime fechaInicio, DateTime fechaFinal, string estado)
        {
            try
            {
                string sql = "sp_AST_ObtenerListadoHitorial";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<ProcesosView>(sql, new { 
                    fechaInicio = fechaInicio, 
                    fechafinal = fechaFinal, 
                    estado = estado } 
                ,commandType: CommandType.StoredProcedure).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<ProcesosView>();
            }
        }

        public BultosEtiquetas getPedidoInfo(string pedido)
        {
            try
            {
                using IDbConnection db = new SqlConnection(ConnectionString.Value);
                string insertQuery = "sp_AST_ObtenerDatosEtiquetasBultos";
                var result = db.QuerySingleOrDefault<BultosEtiquetas>(insertQuery, new { pedido = pedido}, commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception ex)
            {
                return new BultosEtiquetas();
            }
        }

        public BultosEtiquetas getDocInfo(long id)
        {
            try
            {
                using IDbConnection db = new SqlConnection(ConnectionString.Value);
                string insertQuery = "sp_AST_ObtenerDatosDocBultos";
                var result = db.QuerySingleOrDefault<BultosEtiquetas>(insertQuery, new { id }, commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception ex)
            {
                return new BultosEtiquetas();
            }
        }
    }
}
