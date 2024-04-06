using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IDespachosService
    {
        public int createNewDespacho(string ruta);
        public DespachoInfo getListDespacho(int id);
        public bool validaForInsert(string ruta);
        public bool InsertNewScan(string pedido, int consecutivo, int bulto);
        public bool InsertNewScanDoc(long doc, int consecutivo);
        public bool finishScanDespacho(int consecutivo, string conductor);
        public bool deleteScanDespacho(int consecutivo);
        public List<RutasInfo> getRoutes();
        public bool updateBultos(string canasta, int bultos);
        public bool ValidateBultoForUpdate(string canasta);
        public bool ValidateBeforeFinish(int consecutivo);
    }
}
