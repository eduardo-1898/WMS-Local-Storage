using Dapper;
using IServices;
using Services.Messaging.ViewModels;
using Services.Messaging.ViewModels.Articulos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ArticulosServices : IArticulosServices
    {

        public ConnectionString ConnectionString { get; }

        public ArticulosServices(ConnectionString connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<ArticulosView> getArticulos(string pedido)
        {
            try
            {
                string sql = "SELECT v.id, renglon, D.articulo as idArticulo, ISNULL(a.Descripcion1,'') AS Descripcion, ISNULL(d.CantidadA,0) AS CantidadEscaneado, ISNULL(d.Cantidad, 0) as CantidadTotal FROM VentaD d LEFT JOIN Art a ON a.Articulo = d.Articulo LEFT JOIN Venta V ON v.id = d.id  WHERE v.MovID = @pedido";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<ArticulosView>(sql, new { pedido = pedido} ,commandType: CommandType.Text).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<ArticulosView>();
            }
        }
    }
}
