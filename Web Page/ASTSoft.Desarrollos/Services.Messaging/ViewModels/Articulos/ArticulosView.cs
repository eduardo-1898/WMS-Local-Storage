using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels.Articulos
{
    public class ArticulosView
    {
        public int id { get; set; }
        public int renglon { get; set; }
        public string idArticulo { get; set; }
        public string Descripcion { get; set; }
        public int CantidadEscaneado { get; set; }
        public int CantidadTotal { get; set; }
        public int Cantidad { get; set; }
        public int CantidadRecibida { get; set; }
        public int Diferencia { get; set; }
        public string AST_CodigoBarras { get; set; }
        public string AST_CodigoBarrasAnt { get; set; }
        public string OrdenCompra { get; set; }
        public string Tipo { get; set; }
        public int Bonificado { get; set; }
        public int Finalizado { get; set; }
    }

    public class ArticulosViewDocument
    {
        public string Articulo { get; set; }
        public string Descripcion { get; set; }
        public string CodigoBarras { get; set; }
        public string CodigoBarrasAnt { get; set; }
        public int Cantidad { get; set; }
        public int CantidadRecibida { get; set; }
        public int Diferencia { get; set; }
    }
}
