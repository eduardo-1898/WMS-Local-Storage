using Dapper;
using IServices;
using Services.Messaging.ViewModels.Alm;
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
    public class ReciboMercaderiaService : IReciboMercaderiaService
    {
        public ConnectionString ConnectionString { get; }

        public ReciboMercaderiaService(ConnectionString connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<ArticulosView> ObtenerArtOC(string MovID)
        {
            try
            {
                string sql = "AST_ObtenerArtOC";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<ArticulosView>(sql, new { MovID = MovID }, commandType: CommandType.StoredProcedure).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<ArticulosView>();
            }
        }

        public int ObtenerCantidadEscaneados(string OrdenCompra)
        {
            try
            {
                string sql = "SELECT COUNT(Articulo) Cantidad FROM AST_ReciboMercaderia WHERE IDOrdenCompra = @OrdenCompra AND CodigoBarras IS NULL AND ISNULL(CantidadRecibida,0) > 0";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QueryFirstOrDefault<int>(sql, new { OrdenCompra = OrdenCompra }, commandType: CommandType.Text);
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public List<ArticulosView> ObtenerArtOCCodigoBarras(string OrdenCompra, string CodigoBarras)
        {
            try
            {
                string sql = "AST_ObtenerArtOCCodigoBarras";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<ArticulosView>(sql, new { MovID = OrdenCompra, CodigoBarras = CodigoBarras }, commandType: CommandType.StoredProcedure).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<ArticulosView>();
            }
        }

        public List<ArticulosView> ObtenerArt(string Filtro)
        {
            try
            {
                string sql = "AST_ObtenerArtFiltro";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<ArticulosView>(sql, new { Filtro = Filtro }, commandType: CommandType.StoredProcedure).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<ArticulosView>();
            }
        }

        public bool GuardarCantidad(List<ArticulosView> articulos)
        {
            try
            {
                string sql = "AST_GuardarReciboMerca";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                foreach (var item in articulos)
                {
                    var response = connection.Execute(sql, new
                    {
                        IDOrdenCompra = item.id,
                        Articulo = item.idArticulo,
                        Cantidad = item.Cantidad,
                        CantidadRecibida = item.CantidadTotal
                    }, commandType: CommandType.StoredProcedure);

                }

                connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool GuardarCodigoInex(string OrdenCompra, string CodigoBarras)
        {
            try
            {
                string sql = "AST_GuardarReciboMercaInexistente";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new
                {
                    IDOrdenCompra = OrdenCompra,
                    CodigoBarras = CodigoBarras
                }, commandType: CommandType.StoredProcedure);

                connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int FinalizarOC(string OrdenCompra, int Tipo)
        {
            try
            {
                string sql = "AST_UpdateOCFinalizaEscaneo";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QueryFirstOrDefault<int>(sql, new
                {
                    MovID = OrdenCompra,
                    Tipo = Tipo
                }, commandType: CommandType.StoredProcedure);

                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public List<ArticulosView> ObtenerDiferenciasOC(string OrdenCompra)
        {
            try
            {
                string sql = "ObtenerDiferenciasOC";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<ArticulosView>(sql, new { MovID = OrdenCompra }, commandType: CommandType.StoredProcedure).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<ArticulosView>();
            }
        }

        public bool EnviarCorreo(string OrdenCompra, string email, string ruta)
        {
            try
            {
                string sql = "AST_EnviarCorreoDifMerca";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QueryFirstOrDefault<bool>(sql, new { Adjunto = ruta, OrdenCompra = OrdenCompra, email = email }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
