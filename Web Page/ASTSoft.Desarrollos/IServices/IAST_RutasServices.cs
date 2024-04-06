using Services.Messaging.ViewModels;
using Services.Messaging.ViewModels.Procesos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public  interface IAST_RutasServices
    {
        public List<AST_RutasView> getListRutas();
        public bool aprovOut(ProcesosView request);
        public bool volverDespachar(ProcesosView request);
    }
}
