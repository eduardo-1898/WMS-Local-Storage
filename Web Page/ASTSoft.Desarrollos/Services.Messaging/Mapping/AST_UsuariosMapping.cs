using AutoMapper;
using Model.DomainModels;
using Services.Messaging.ViewModels.AST_Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging.Mapping
{
    public static class AST_UsuariosMapping
    {
        public static IEnumerable<AST_UsuariosView> ToAST_UsuariosViewList(this IEnumerable<AST_Usuarios> list, IMapper mapper)
        {
            return mapper.Map<List<AST_UsuariosView>>(list);
        }

        public static AST_UsuariosView ToAST_UsuariosView(this AST_Usuarios model, IMapper mapper)
        {
            return mapper.Map<AST_UsuariosView>(model);
        }

        public static AST_Usuarios ToAST_UsuariosModel(this AST_UsuariosView modelView, IMapper mapper)
        {
            return mapper.Map<AST_Usuarios>(modelView);
        }
    }
}
