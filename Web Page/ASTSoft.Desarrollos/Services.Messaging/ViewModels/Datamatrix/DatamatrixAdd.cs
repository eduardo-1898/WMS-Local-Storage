using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels.Datamatrix
{
    public class DatamatrixAdd
    {
        public int ID { get; set; }
        public string OrdenID { get; set; }
        public int RenglonID { get; set; }
        public string Embarque { get; set; }
        public string Articulo { get; set; }
        public string Qr { get; set; }
        public string Descripcion { get; set; }
        public string Lote { get; set; }
        public string FechaVencimiento { get; set; }
        public bool Etiqueta { get; set; }
        public bool GenerarConsecutivo { get; set; }
        public string UsuarioAlistar { get; set; }
        public int NumeroEtiquetas { get; set; }
        public int CantidadRecibidas { get; set; }
        public int CantidadDisponibles { get; set; }
        public int CantidadImprimir { get; set; }
        public int CantidadA { get; set; }
        public int Imprimir { get; set; }
        public string Tipo { get; set; }
    }
}
