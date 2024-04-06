using ASTSoft.Desarrollos.Models;
using ASTSoft.Desarrollos.Utils;
using AutoMapper;
using IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Services.Messaging.ViewModels.AST_Usuarios;
using Services.Messaging.ViewModels.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASTSoft.Desarrollos.Controllers
{
    public class AST_UsuariosController : Controller
    {
        private readonly ILogger<AST_UsuariosController> _logger;
        private readonly IAST_UsuariosService _AST_UsuariosService;
        private readonly IAlmService _AlmService;
        private readonly IMapper _mapper;

        public AST_UsuariosController(ILogger<AST_UsuariosController> logger, IAST_UsuariosService AST_UsuariosService, IMapper mapper, IAlmService IAlmService)
        {
            _logger = logger;
            _AST_UsuariosService = AST_UsuariosService;
            _mapper = mapper;
            _AlmService = IAlmService;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Agregar()
        {
            var model = new AST_UsuariosView();
            model.Almacenes = _AlmService.ObtenerAlmacenes().Select(x => new SelectListItem
            {
                Value = x.Almacen,
                Text = x.Nombre
            }).ToList();

            return View(model);
        }

        public IActionResult Editar(int ID)
        {
            var model = new AST_UsuariosView();
            model = _AST_UsuariosService.GetUsuario(ID);
            model.Almacenes = _AlmService.ObtenerAlmacenes().Select(x => new SelectListItem
            {
                Value = x.Almacen,
                Text = x.Nombre
            }).ToList();

            return View(model);
        }

        public IActionResult Detalles(int ID)
        {
            var model = new AST_UsuariosView();
            model = _AST_UsuariosService.GetUsuario(ID);
            model.Almacenes = _AlmService.ObtenerAlmacenes().Select(x => new SelectListItem
            {
                Value = x.Almacen,
                Text = x.Nombre
            }).ToList();

            return View(model);
        }

        public IActionResult AgregarUsuario(AST_UsuariosView model)
        {
            var response = _AST_UsuariosService.AgregarUsuario(model);
            if (response)
            {
                ViewBag.Mensaje = "ExitoA";
                return View("Index");
            }
            ViewBag.Mensaje = "Error";
            model.Almacenes = _AlmService.ObtenerAlmacenes().Select(x => new SelectListItem
            {
                Value = x.Almacen,
                Text = x.Nombre
            }).ToList();
            return View("Agregar", model);
        }

        public IActionResult ModificarUsuario(AST_UsuariosView model)
        {
            var response = _AST_UsuariosService.ActualizarUsuario(model);
            if (response)
            {
                ViewBag.Mensaje = "ExitoE";
                return View("Index");
            }
            ViewBag.Mensaje = "Error";
            model.Almacenes = _AlmService.ObtenerAlmacenes().Select(x => new SelectListItem
            {
                Value = x.Almacen,
                Text = x.Nombre
            }).ToList();
            return View("Agregar", model);
        }

        public IActionResult List()
        {
            try
            {
                var list = _AST_UsuariosService.GetUsuarios();

                var response = new DataTableResponseViewModel<AST_UsuariosView>
                {
                    Data = list.ToList(),
                    RecordsFiltered = list.Count(),
                    RecordsTotal = list.Count()
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new DataTableResponseViewModel<AST_UsuariosView>
                {
                    Data = new List<AST_UsuariosView>(),
                    RecordsFiltered = 0,
                    RecordsTotal = 0,
                    Error = $"Error obtiendo los registros: {ex.Message}"
                };

                return Json(response);
            }
        }

    }
}

