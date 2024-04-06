using Services.Messaging.ViewModels.AST_Usuarios;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels.Procesos
{
    public class ProcesosView
    {
        public int id { get; set; }
        public string pedido { get; set; }
        public string cliente { get; set; }
        public string nombre { get; set; }
        public string observ { get; set; }
        public string estado { get; set; }
        public string prod { get; set; }
        public string ruta { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fechaInicial { get; set; }
        public DateTime fechaFinal { get; set; }
        public int diferencia { get; set; }
        public string usuario { get; set; }
        public string usuarioBOD1 { get; set; }
        public string usuarioIntelisis { get; set; }
        public string tunidad { get; set; }
        public DateTime hora { get; set; }
        public bool? cancelar { get; set; }
        public int idAlistador { get; set; }
        public string idRuta { get; set; }
        public bool finalizarDespacho { get; set; }
        public bool despachado { get; set; }
        public int bultos { get; set; }
        public string almacen { get; set; }
        public int cantidadLineas { get; set; }
        public string documento { get; set; }
        public int prioridad { get; set; }
        public string canastas { get; set; }
        public List<AST_RutasView> Rutas { get; set; }
        public List<AST_UsuariosView> Usuarios { get; set; }
        public List<Prioridades> Prioridades { get; set; }
    }

    public class Prioridades { 
        public int idPrioridad { get; set; }
        public string prioridad { get; set; }
    }
}
