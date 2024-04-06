using Services.Messaging.ViewModels.Articulos;
using Services.Messaging.ViewModels.AST_Usuarios;
using Services.Messaging.ViewModels.Canasta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels.Etiquetas
{
    public class EtiquetasView
    {
        public string OrdenCompra { get; set; }
        public string Pedido { get; set; }
        public string PedidoConsulta { get; set; }
        public string Alistador { get; set; }
        public string Cliente { get; set; }
        public string Ruta { get; set; }
        public string Observacion { get; set; }
        public int cantidadSinLeer { get; set; }
        public int Bultos { get; set; }
        public bool Fragil { get; set; }
        public string usuario { get; set; }
        public string Filtro { get; set; }
        public int Cantidad { get; set; }
        public bool Finalizado { get; set; }
        public string AST_ObservacionesExtraEtiquetado { get; set; }
        public List<CanastaView> canastas { get; set; }
        public List<ArticulosView> articulos { get; set; }
        public List<AST_UsuariosView> usuarios { get; set; }
    }
}
