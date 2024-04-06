using AutoMapper;
using Model.DomainModels;
using Services.Messaging.ViewModels.AST_Usuarios;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Messaging
{
    public class AutoMapping : Profile
    {
        public AutoMapping() {

            CreateMap<AST_Usuarios, AST_UsuariosView>().ReverseMap();

        }
    }
}
