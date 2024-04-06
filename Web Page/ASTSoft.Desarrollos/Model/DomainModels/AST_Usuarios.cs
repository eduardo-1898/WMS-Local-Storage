using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DomainModels
{
    public partial class AST_Usuarios
    {
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string GrupoTrabajo { get; set; }

        public string Contrasena { get; set; }
        public int? Sucursal { get; set; }
        public bool? Autorizado { get; set; }
        public string Quejas { get; set; }
        public string Encuestas { get; set; }
        public string SolucionQuejas { get; set; }
        public string Configuraciones { get; set; }
        public string Departamento { get; set; }
        public string ModQueja { get; set; }
        public int? CrearSolicitudMant { get; set; }
        public int? SolucionarSolicitudMant { get; set; }
        public bool? VariasListasPrecios { get; set; }
        public string Email { get; set; }
        public bool? TI { get; set; }



    }
}
