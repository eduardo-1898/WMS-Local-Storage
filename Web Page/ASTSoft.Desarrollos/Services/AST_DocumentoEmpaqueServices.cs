using Dapper;
using IServices;
using Services.Messaging.ViewModels.AST_DocumentosEmpaque;
using Services.Messaging.ViewModels.AST_DocumentosEmpaqueD;
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
    public class AST_DocumentoEmpaqueServices : IAST_DocumentosEmpaqueServices
    {
        public ConnectionString ConnectionString { get; }

        public AST_DocumentoEmpaqueServices(ConnectionString connectionString)
        {
            ConnectionString = connectionString;
        }

        public AST_DocumentosEmpaqueView createDocument(string usuario)
        {
            try
            {
                string sql = "sp_AST_CrearDocumentoEmpaque";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<AST_DocumentosEmpaqueView>(sql, new { usuario } , commandType: CommandType.StoredProcedure);
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new AST_DocumentosEmpaqueView();
            }
        }

        public List<AST_DocumentosEmpaqueView> getDocuments()
        {
            try
            {
                string sql = "sp_AST_ObtenerDocumentos";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<AST_DocumentosEmpaqueView>(sql, commandType: CommandType.StoredProcedure).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<AST_DocumentosEmpaqueView>();
            }
        }

        public List<AST_DocumentosEmpaqueDView> getPedidos(long idDocumento) {
            try
            {
                string sql = "SELECT pedido, de.fechaAsociacion, V.AST_Bultos as cantidadBultos, DE.id FROM AST_DocumentosEmpaqueD DE LEFT JOIN Venta V ON v.MovID = de.pedido AND v.Mov = 'Pedido' WHERE De.idDocumento = @idDocumento";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<AST_DocumentosEmpaqueDView>(sql, new { idDocumento }, commandType: CommandType.Text).ToList();
                connection.Close();

                return response;
            }
            catch (Exception)
            {
                return new List<AST_DocumentosEmpaqueDView>();
            }
        }

        public AST_DocumentosEmpaqueView getDocument(long id)
        {
            try
            {
                string sql = "SELECT *, (SELECT COUNT(*) FROM AST_DocumentosEmpaqueD WHERE idDocumento = AST_DocumentosEmpaque.id ) as cantidadBultos FROM AST_DocumentosEmpaque WHERE id = @id";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<AST_DocumentosEmpaqueView>(sql, new { id } ,commandType: CommandType.Text);
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new AST_DocumentosEmpaqueView();
            }
        }

        public bool addNewOrder(AST_DocumentosEmpaqueDView document)
        {
            try
            {
                string sql = "sp_AST_AgregarPedido";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new { document.idDocumento, document.pedido }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response>0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool deleteOrder(long id) {
            try
            {
                string sql = "DELETE FROM AST_DocumentosEmpaque WHERE id = @id";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new { id }, commandType: CommandType.Text);
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
