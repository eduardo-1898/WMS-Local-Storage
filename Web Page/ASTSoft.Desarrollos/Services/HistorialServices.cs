using Dapper;
using IServices;
using Services.Messaging.ViewModels.Historial;
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
    public class HistorialServices : IHistorialServices
    {

        public ConnectionString ConnectionString { get; }

        public HistorialServices(ConnectionString connectionString)
        {
            ConnectionString = connectionString;
        }

        public bool changeStatusRegister(ProcesosView request)
        {
            try
            {
                using IDbConnection db = new SqlConnection(ConnectionString.Value);
                string insertQuery = "sp_AST_CambiarEstado";
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

        public List<DetallePedidoView> getListDetails(string pedido)
        {
            try
            {
                string sql = "SELECT vd.Articulo, vd.RenglonID as linea, vd.Cantidad, ar.Descripcion1 as descripcion FROM VentaD vd LEFT JOIN Art ar ON ar.articulo = vd.articulo WHERE vd.ID = (SELECT ID FROM Venta WHERE MovId = @pedido AND estatus = 'PENDIENTE' AND mov = 'Pedido' )";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<DetallePedidoView>(sql, new { pedido = pedido}, commandType: CommandType.Text).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<DetallePedidoView>();
            }
        }
    }
}
