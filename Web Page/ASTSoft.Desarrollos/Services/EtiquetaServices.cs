using Dapper;
using IServices;
using Services.Messaging.ViewModels.Canasta;
using Services.Messaging.ViewModels.Etiquetas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class EtiquetaServices : IEtiquetaServices
    {

        public ConnectionString ConnectionString { get; }

        public EtiquetaServices(ConnectionString connectionString)
        {
            ConnectionString = connectionString;
        }

        public EtiquetasView getDataInfo(string pedido)
        {
            try
            {
                string sql = "sp_AST_BusquedaPedidoWeb";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<EtiquetasView>(sql, new { pedido = pedido }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new EtiquetasView();
            }
        }

        public bool aceptarAlisto(string pedido, int bultos, string usuario, string ObservacionesExtra)
        {
            try
            {
                string sql = "sp_AST_AprobarPredespacho";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new { pedido = pedido, bultos = bultos, usuario = usuario, ObservacionesExtra= ObservacionesExtra }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response>0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CambiarSituacionPredespacho(string pedido) {
            try
            {
                string sql = "sp_AST_CambiarEstadoPredespacho";
                using var connecion = new SqlConnection(ConnectionString.Value);
                connecion.Open();
                var response = connecion.Execute(sql, new { pedido = pedido }, commandType: CommandType.StoredProcedure);
                connecion.Close();
                return response > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
