using ASTSoft.Desarrollos.Utils;
using AutoMapper;
using IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services.Messaging.ViewModels.AST_Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASTSoft.Desarrollos.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAST_UsuariosService _AST_UsuariosService;
        private readonly ILogger<AST_UsuariosController> _logger;
        private readonly IMapper _mapper;

        public LoginController(ILogger<LoginController> logger, IAST_UsuariosService AST_UsuariosService, IMapper mapper)
        {
            _AST_UsuariosService = AST_UsuariosService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Login(AST_UsuariosView model)
        {
            var user = _AST_UsuariosService.Login(model.usuario, model.contrasenna);
            if (user != null)
            {
                HttpContext.Session.SetString("userInfo",JsonConvert.SerializeObject(user));
                return RedirectToAction(nameof(Index), "Home");
            }
            else
            {
                TempData["MessageError"] = "El usuario no se encuentra registrado en el sistema o bien no cuenta con los pemisos necesarios para ingresar!";
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult LogOutAction()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

    }
}
