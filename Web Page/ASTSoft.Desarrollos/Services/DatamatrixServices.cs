using Dapper;
using IServices;
using Services.Messaging.ViewModels.AST_SerieLoteMov;
using Services.Messaging.ViewModels.Datamatrix;
using Services.Messaging.ViewModels.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Services
{
    public class DatamatrixServices : IDatamatrixServices
    {

        public ConnectionString ConnectionString { get; }

        public DatamatrixServices(ConnectionString connectionString) { 
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Obtiene los datamatrix asociados a un articulo o codigo de barra, ya que valida por ambos
        /// </summary>
        /// <param name="articulo"></param>
        /// <returns></returns>
        public List<DatamatrixView> getDatamatrixInfo(string articulo)
        {
			try
			{
                string sql = "sp_AST_ObtenerRegistroDatamatrix";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<DatamatrixView>(sql, new { codigo = articulo }, commandType: CommandType.StoredProcedure).ToList();
                connection.Close();

                return response;
            }
			catch (Exception)
			{
				return new List<DatamatrixView>();
			}
        }

        public bool ActualizarAlmacenCompra(string id, string renglonID ,string almacen, string modulo) {
            try
            {
                string sql = string.Empty;
                if (modulo == "INV")
                {
                    sql = "UPDATE InvD SET Almacen = @almacen WHERE id = @id AND RenglonID = @renglonID";
                }
                else {
                    sql = "UPDATE CompraD SET Almacen = @almacen WHERE id = @id AND RenglonID = @renglonID";
                }
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, 
                    new { 
                        id = id, 
                        almacen = almacen,
                        renglonID = renglonID
                    }, 
                    commandType: CommandType.Text);
                connection.Close();

                return (response > 0);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Permite obtener la informacion necesaria para llenar el modelo de datos sobre el articulo
        /// </summary>
        /// <param name="articulo"></param>
        /// <param name="id"></param>
        /// <param name="modulo"></param>
        /// <returns></returns>
        public DatamatrixAdd getDatamatrixAsoc(string articulo, int id, string modulo)
        {
            try
            {
                string sql = "sp_AST_ObtenerEtiquetasDatamatrix";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<DatamatrixAdd>(sql, new { ARTICULO = articulo, id = id, modulo = modulo}, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response;
            }
            catch (Exception)
            {
                return new DatamatrixAdd();
            }
        }

        public List<DatamatrixAdd> getArtDatamatrixAsoc(string articulo, int id, string modulo)
        {
            try
            {
                string sql = "sp_AST_ObtenerArtEtiquetasDatamatrix";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<DatamatrixAdd>(sql, new { ARTICULO = articulo, id = id, modulo = modulo }, commandType: CommandType.StoredProcedure).ToList();
                connection.Close();

                return response;
            }
            catch (Exception)
            {
                return new List<DatamatrixAdd>();
            }
        }

        /// <summary>
        /// Permite generar los codigos datamatrix de los articulos solicitados junto con su cantidad respectiva
        /// </summary>
        /// <param name="lote"></param>
        /// <param name="fechaVencimiento"></param>
        /// <param name="etiqueta"></param>
        /// <param name="consecutivo"></param>
        /// <param name="revisador"></param>
        /// <param name="imprimir"></param>
        /// <returns></returns>
        public bool addDatamatrix(string lote, DateTime fechaVencimiento, bool etiqueta, bool consecutivo, string revisador, int imprimir)
        {
            try
            {
                string sql = "sp_AST_AgregarEtiquetasByBarCode";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new { 
                    lote = lote, 
                    fechaVencimiento = fechaVencimiento,
                    etiqueta = etiqueta,
                    consecutivo = consecutivo,
                    revisador = revisador,
                    imprimir = imprimir
                }
                , commandType: CommandType.StoredProcedure);
                connection.Close();

                return response>0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Me permite obtener los usuarios registrados en el sistema para llenar los usuarios que reciben en la pantalla de creacion de datamatrix
        /// </summary>
        /// <returns></returns>
        public List<Usuarios> getUsersDatamatrix()
        {
            try
            {
                string sql = "SELECT * FROM AST_Usuarios";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<Usuarios>(sql, commandType: CommandType.Text).ToList();
                connection.Close();

                return response;
            }
            catch (Exception)
            {
                return new List<Usuarios>();
            }
        }

        public bool InsertarDatamatrix(AST_DatamatrixView model)
        {
            try
            {
                string sql = "sp_AST_InsertarDatamatrixGenerados";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new
                {
                    articulo = model.Articulo,
                    consecutivo = model.Consecutivo,
                    lote = model.Lote,
                    fechaVencimiento = model.FechaVencimiento,
                    idCaja = model.idCaja,
                    Datamatrix = model.Datamatrix,
                    MovId = model.MovID,
                    RenglonID = model.RenglonID,
                }
                , commandType: CommandType.StoredProcedure);
                connection.Close();

                return response > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool InsertarSerieLote(AST_SerieLoteMovView model)
        {
            try
            {
                string sql = "sp_AST_InsertarSerieLoteMov";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new
                {
                    Empresa = model.Empresa,
                    Modulo = model.Modulo,
                    id = model.ID,
                    RenglonId = model.RenglonID,
                    Articulo = model.Articulo,
                    SerieLote = model.SerieLote,
                    Cantidad = model.Cantidad,
                    Datamatrix = model.Datamatrix,
                    Vencimiento = model.Vencimiento
                }
                , commandType: CommandType.StoredProcedure);
                connection.Close();

                return response > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int ObtenerConsecutivo(string articulo, string lote, string vencimiento)
        {
            try
            {
                string sql = "SELECT COUNT(*) as consecutivo FROM AST_SerieLoteMov sl WHERE SL.Articulo = @articulo AND SL.SerieLote = @lote AND FORMAT (sl.Vencimiento, 'yyMMdd') = @vencimiento";
                //string sql = "SELECT MAX(CAST(SUBSTRING(sl.SerieLote, CHARINDEX(sl.Propiedades, sl.serielote)-7,5) AS INT)) as consecutivo FROM SerieLote sl LEFT JOIN SerieLoteProp SLP on SLP.propiedades = sl.propiedades WHERE SL.Articulo = @articulo AND SL.Propiedades = @lote AND FORMAT (slp.Fecha1, 'yyMMdd') = @vencimiento";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<int>(sql, new { articulo, lote, vencimiento }, commandType: CommandType.Text);
                connection.Close();

                return response;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool ObtenerActualizarCantidades(string id, int renglon, int cantidad)
        {
            try
            {
                string sql = "UPDATE CompraD SET CantidadA = ISNULL(CantidadA,0) + @cantidad, AST_CantidadA = ISNULL(AST_CantidadA,0) + @cantidad WHERE id = @id AND RenglonID = @renglon";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new { cantidad, id, renglon }, commandType: CommandType.Text);
                connection.Close();

                return response>0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public AST_DatamatrixCajaView InsertarConsecutivoDatamatrix(int renglon, int renglonID)
        {
            try
            {
                string sql = "SP_AST_CrearCajaConsecutivo";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<AST_DatamatrixCajaView>(sql, new { renglon = renglon, renglonID = renglonID }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response;
            }
            catch (Exception)
            {
                return new AST_DatamatrixCajaView();
            }
        }
        
        public AST_DatamatrixCajaView InsertarConsecutivoDatamatrixRC(int renglon, string renglonCompuesto, int renglonID)
        {
            try
            {
                string sql = "SP_AST_CrearCajaConsecutivoRC";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<AST_DatamatrixCajaView>(sql, new { renglon = renglon, renglonID = renglonID, renglonCompuesto = renglonCompuesto }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response;
            }
            catch (Exception)
            {
                return new AST_DatamatrixCajaView();
            }
        }

        public List<EtiquetasModel> ObtenerDatosImprimir(string serieLote, string articulo, int inicio, int final)
        {
            try
            {
                string sql = "SELECT datamatrix, consecutivo FROM AST_Datamatrix WHERE lote = @serielote AND Articulo = @articulo AND consecutivo >= @inicio AND consecutivo <= @final order by consecutivo asc";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<EtiquetasModel> (sql, new { serieLote = serieLote, articulo = articulo, inicio = inicio, final = final }, commandType: CommandType.Text).ToList();
                connection.Close();

                return response;
            }
            catch (Exception)
            {
                return new List<EtiquetasModel>();
            }
        }

        public List<Almacenes> ObtenerAlmacenes()
        {
            try
            {
                string sql = "SELECT Almacen, Nombre FROM Alm WHERE Exclusivo <> 'Compra' and Exclusivo IS NOT NULL ";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<Almacenes>(sql, commandType: CommandType.Text).ToList();
                connection.Close();

                return response;
            }
            catch (Exception)
            {
                return new List<Almacenes>();
            }
        }

        public ArticuloInfo ObtenerInformacionEtiqueta(int idCaja)
        {
            try
            {
                string sql = "sp_AST_ObtenerArticulosICaja";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<ArticuloInfo>(sql, new { idCaja = idCaja }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response;
            }
            catch (Exception)
            {
                return new ArticuloInfo();
            }
        }

        public bool ActualizarCantidad(int cantidad, int id, int renglonID)
        {
            try
            {
                string sql = "sp_ActualizarCantidadA";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Execute(sql, new { cantidad, id, renglonID }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response>0;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
