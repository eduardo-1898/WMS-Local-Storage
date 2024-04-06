using ASTSoft.Desarrollos.Extensions;
using ASTSoft.Desarrollos.Models;
using IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ASTSoft.Desarrollos.Controllers
{
    [FilterConfig]
    public class HomeController : Controller
    {
        private readonly IAST_UsuariosService _AST_UsuariosService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IAST_UsuariosService AST_UsuariosService, ILogger<HomeController> logger)
        {
            _AST_UsuariosService = AST_UsuariosService;
            _logger = logger;
        }
         
        public IActionResult Index()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
