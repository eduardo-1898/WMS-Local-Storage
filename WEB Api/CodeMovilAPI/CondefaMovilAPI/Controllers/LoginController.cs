using CondefaMovilAPI.Security;
using IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CondefaMovilAPI.Controllers
{
    [Route("Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        IConfiguration _configuration;
        private readonly IUsuariosService _usuariosService;
        private readonly IUtils _utils;

        public LoginController(IConfiguration configuration, IUsuariosService usuariosService, IUtils utils) 
        { 
            _configuration = configuration;
            _usuariosService = usuariosService;
            _utils = utils;
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult login(string username, string key) {
            try
            {
                var arr = username.Split('/');
                try
                {
                    var user = arr[0];
                    var password = arr[1];
                    if (key != _configuration["AppSettings:secret"].ToString())
                        return Unauthorized("La key proporcionada no es valida, favor validar con el administrador del sistema");

                    var request = generateModel(user, password, key);
                    var response = _usuariosService.SingIn(request);
                    if (response != null)
                        request.IsLogued = true;

                    if (request.IsLogued)
                    {
                        request.Key = _utils.GenerarToken(request.Username, key);
                        return Ok(request);
                    }
                    return Unauthorized("Usuario o contraseña incorrecto");
                }
                catch (Exception)
                {
                    return Unauthorized("Usuario o contraseña incorrecto");
                }
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al procesar la solicitud");
            }
        }

        /// <summary>
        /// Permite la separacion de la informacion recibida por medio del codigo QR al realizar el login
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private AuthRequest generateModel(string username, string password, string key) {
            try
            {
                var model = new AuthRequest { Username = username, Password= password, Key = key };
                return model;
            }
            catch (Exception)
            {
                return new AuthRequest();
            }
        }
    }
}
