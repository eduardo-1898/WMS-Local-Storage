using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IUsuariosService
    {
        public AuthRequest SingIn(AuthRequest request);
    }
}
