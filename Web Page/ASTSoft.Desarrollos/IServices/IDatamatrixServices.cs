using Services.Messaging.ViewModels.AST_SerieLoteMov;
using Services.Messaging.ViewModels.Datamatrix;
using Services.Messaging.ViewModels.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IDatamatrixServices
    {
        public List<DatamatrixView> getDatamatrixInfo(string articulo);
        public DatamatrixAdd getDatamatrixAsoc(string articulo, int id, string modulo);
        public bool addDatamatrix(string lote, DateTime fechaVencimiento, bool etiqueta, bool consecutivo, string revisador, int imprimir);
        public List<Usuarios> getUsersDatamatrix();
        public bool InsertarDatamatrix(AST_DatamatrixView model);
        public bool InsertarSerieLote(AST_SerieLoteMovView model);
        public int ObtenerConsecutivo(string articulo, string lote, string vencimiento);
        public AST_DatamatrixCajaView InsertarConsecutivoDatamatrix(int renglon, int renglonID);
        public List<EtiquetasModel> ObtenerDatosImprimir(string serieLote, string articulo, int inicio, int final);
        public List<Almacenes> ObtenerAlmacenes();
        public bool ActualizarAlmacenCompra(string id, string renglonID, string almacen, string modulo);
        public ArticuloInfo ObtenerInformacionEtiqueta(int idCaja);
        public bool ObtenerActualizarCantidades(string id, int renglon, int cantidad);
        public bool ActualizarCantidad(int cantidad, int id, int renglonID);
        public List<DatamatrixAdd> getArtDatamatrixAsoc(string articulo, int id, string modulo);
        public AST_DatamatrixCajaView InsertarConsecutivoDatamatrixRC(int renglon, string renglonCompuesto, int renglonID);
    }
}
