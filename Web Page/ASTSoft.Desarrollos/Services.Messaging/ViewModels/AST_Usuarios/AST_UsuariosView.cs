using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels.AST_Usuarios
{
    public class AST_UsuariosView
    {
        public int id { get; set; }
        public string usuario { get; set; }
        public string contrasenna { get; set; }
        public bool Estado { get; set; }
        public string Nombre { get; set; }
        public bool AST_Alisto { get; set; }
        public bool AST_Despacho { get; set; }
        public bool AST_Bultos { get; set; }
        public bool AST_Supervisor { get; set; }
        public bool AST_Reimpresion { get; set; }
        public bool AST_impresion { get; set; }
        public bool AST_WEB { get; set; }
        public bool AST_Admin { get; set; }
        public string AlmacenAsignado { get; set; }
        public string Estatus { get; set; }
        public List<SelectListItem> Almacenes { get; set; }
    }
}
