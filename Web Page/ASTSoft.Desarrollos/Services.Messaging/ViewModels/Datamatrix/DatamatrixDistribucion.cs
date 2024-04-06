using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels.Datamatrix
{
    /// <summary>
    /// Me permite obtener los consecutivos y las cajas a procesar dentro de la generacion de los codigos datamatrix
    /// </summary>
    public class DatamatrixDistribucion
    {
        public int IdCaja { get; set; }
        public int ConsecutivoInicio { get; set; }
        public int ConsecutivoFinal { get; set; }
    }
}
