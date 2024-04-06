using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    /// <summary>
    /// Clase que me permite obtener la solicitud por parte del cliente.
    /// </summary>
    public class AuthRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Key { get; set; }
        public bool IsLogued { get; set; }
    }
}
