using Dapper;
using IServices;
using Services.Messaging.ViewModels;
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
    public class ConsultaServices : IConsultaServices
    {
        public ConnectionString ConnectionString { get; }

        public ConsultaServices(ConnectionString connectionString)
        {
            ConnectionString = connectionString;
        }

        public DetalleArticulo ObtenerArticulo(string articulo) {
            try
            {
                string sql = "sp_AST_ObtenerDatosArticulos";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<DetalleArticulo>(sql, new { art = articulo }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new DetalleArticulo();
            }
        }

        public List<AlmacenArticulos> ObtenerDetalleArticulo(string articulo)
        {
            try
            {
                string sql = "SP_AST_ObtenerDetalleLote";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<AlmacenArticulos>(sql, new { art = articulo }, commandType: CommandType.StoredProcedure).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<AlmacenArticulos>();
            }
        }

        public List<DetalleDatamatrix> ObtenerHistorialDatamatrix(string articulo) {
            try
            {
                string sql = "sp_AST_ObtenerDatosConsultaDatamatrix";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<DetalleDatamatrix>(sql, new { art = articulo }, commandType: CommandType.StoredProcedure).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<DetalleDatamatrix>();
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

    }
}
