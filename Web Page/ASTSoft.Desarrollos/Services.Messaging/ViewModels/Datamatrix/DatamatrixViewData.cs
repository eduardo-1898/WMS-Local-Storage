using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels.Datamatrix
{
    /// <summary>
    /// Para la vista index de datamatrix que contiene una lista deplegable de usuarios
    /// </summary>
    public class DatamatrixViewData
    {
        public List<Usuarios> Usuarios { get; set; }
        public List<Almacenes> Almacenes { get; set; }
    }

    /// <summary>
    /// Clase necesesaria para la creacion de usuarios.
    /// </summary>
    public class Usuarios {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Contrasenna { get; set; }
        public bool Estado { get; set; }
        public string Nombre { get; set; }
    }

    public class Almacenes { 
        public string Almacen { get; set; }
        public string Nombre { get; set; }
    }
}
