using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels.Util
{
    public class ModelClasificaciones
    {
        public string clasificacion { get; set; }
        public string id { get; set; }

        public int TallerMecanico { get; set; }
        public int TallerPintura { get; set; }
        public int AutosUsados { get; set; }
        public int AutosNuevos { get; set; }
        public int Repuestos { get; set; }
        public int CreditoCobro { get; set; }
        public int General { get; set; }
        public int Orden { get; set; }





        public List<SelectListItem> ListClasificaciones { get; set; }
        public List<SelectListItem> ListClasificacionesSinFiltro { get; set; }


    }
}
