using Infrastructure.Messaging;
using Services.Messaging.ViewModels.AST_Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IAST_UsuariosService
    {
        AST_UsuariosView Login(string usuario, string contrasenna);
        List<AST_UsuariosView> UserList();
        List<AST_UsuariosView> GetUsuarios();
        AST_UsuariosView GetUsuario(int ID);
        bool AgregarUsuario(AST_UsuariosView model);
        bool ActualizarUsuario(AST_UsuariosView model);
    }
}
