using Services.Messaging.ViewModels.Procesos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels.Despachos
{
    public class DespachosView
    {
        public int Id { get; set; } 
        public string NumeroDespacho { get; set; }
        public string Ruta { get; set; }
        public List<ProcesosView> Pedidos { get; set; }
    }
}
