using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels.Datamatrix
{
    public class DatamatrixView
    {
        public int Id { get; set; }
        public int OrdenID { get; set; }
        public int renglonID { get; set; }
        public string Orden { get; set; }
        public string Articulo { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public string Documento { get; set; }
        public string Qr1 { get; set; }
        public string Qr2 { get; set; }
        public int Cantidad { get; set; }
        public int CantidadRecibida { get; set; }
        public DateTime Fecha { get; set; }
        public string modulo { get; set; }
    }

    public class ArticuloInfo
    {
        public string articulo { get; set; }
        public string codigo { get; set; }
        public string articuloDetalle { get; set; }
        public string trazable { get; set; }
        public int cantidad { get; set; }
        public string lote { get; set; }
        public string expira { get; set; }
        public string Qr { get; set; }
        public string usuario { get; set; }
        public DateTime fechaVencimiento { get; set; }

        public string consecutivos { get; set; }
    }
}
