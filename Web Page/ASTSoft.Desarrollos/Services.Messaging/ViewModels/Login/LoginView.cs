using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.ViewModels.Login
{
    public class LoginView
    {
        [Required(ErrorMessage = " * El usuario es requerido")]
        [DisplayName("Usuario")]
        public string UserName { get; set; }

        [Required(ErrorMessage = " * La contraseña es requerida")]
        [DisplayName("Contraseña")]
        [MinLength(4, ErrorMessage = "Debe contener minimo 4 caracteres")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
