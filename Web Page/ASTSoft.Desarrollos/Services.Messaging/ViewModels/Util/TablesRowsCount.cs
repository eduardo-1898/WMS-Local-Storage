using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels.Util
{
    //Clase creada con la finalidad de guardar el total de facturas y las facturas creadas para mostrarlo en el label del footer de la tabla en la vista index, indexTM y indexcxc
    public class TablesRowsCount
    {
        public int cantidadFacturas { get; set; }
        public int cantidadEncuestas { get; set; }
        public int cantidadTotal { get; set; }

    }
}
