using Dapper;
using Infrastructure.Messaging;
using IServices;
using Microsoft.AspNetCore.Mvc;
using Services.Messaging.ViewModels.Articulos;
using Services.Messaging.ViewModels.AST_Usuarios;
using Services.Messaging.ViewModels.Datamatrix;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AST_UsuariosService : IAST_UsuariosService
    {

        public AST_UsuariosService(ConnectionString connectionString)
        {
            ConnectionString = connectionString;
        }
        public ConnectionString ConnectionString { get; }

        public AST_UsuariosView Login(string usuario, string contrasenna)
        {
            try
            {
                string sql = "SELECT * FROM AST_Usuarios WHERE usuario = @usuario and contrasenna = @contrasenna and AST_WEB = 1 ";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QuerySingleOrDefault<AST_UsuariosView>(sql, new { usuario = usuario, contrasenna = contrasenna }, commandType: CommandType.Text);
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new AST_UsuariosView();
            }
        }

        public List<AST_UsuariosView> UserList()
        {
            try
            {
                string sql = "SELECT usuario, estado, nombre FROM AST_Usuarios WHERE AST_Alisto = 1 ";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<AST_UsuariosView>(sql, commandType: CommandType.Text).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<AST_UsuariosView>();
            }
        }

        public List<AST_UsuariosView> GetUsuarios()
        {
            try
            {
                string sql = "AST_GetUsuarios";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.Query<AST_UsuariosView>(sql, commandType: CommandType.StoredProcedure).ToList();
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new List<AST_UsuariosView>();
            }
        }

        public AST_UsuariosView GetUsuario(int ID)
        {
            try
            {
                string sql = "AST_GetUsuarioWMS";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QueryFirstOrDefault<AST_UsuariosView>(sql, new { ID = ID }, commandType: CommandType.StoredProcedure);
                connection.Close();

                return response;
            }
            catch (Exception ex)
            {
                return new AST_UsuariosView();
            }
        }

        public bool AgregarUsuario(AST_UsuariosView model)
        {
            try
            {
                string sql = "AST_AgregarUsuarioWMS";

                var lista = GetUsuarios();

                if (lista.Find(x => x.usuario == model.usuario) != null)
                {
                    return false;
                }

                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QueryFirstOrDefault<int>(sql,
                new
                {
                    Usuario = model.usuario,
                    Contrasena = model.contrasenna,
                    Estado = model.Estatus == "ALTA" ? 1 : 0,
                    Nombre = model.Nombre,
                    Alisto = model.AST_Alisto,
                    Despacho = model.AST_Despacho,
                    Bultos = model.AST_Bultos,
                    Supervisor = model.AST_Supervisor,
                    Reimpresion = model.AST_Reimpresion,
                    Impresion = model.AST_impresion,
                    WEB = model.AST_WEB,
                    Almacen = model.AlmacenAsignado
                },
                commandType: CommandType.StoredProcedure);
                connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ActualizarUsuario(AST_UsuariosView model)
        {
            try
            {
                string sql = "AST_ActualizarUsuariosWMS";
                using var connection = new SqlConnection(ConnectionString.Value);
                connection.Open();
                var response = connection.QueryFirstOrDefault<int>(sql,
                new
                {
                    Usuario = model.usuario,
                    Contrasena = model.contrasenna,
                    Estado = model.Estatus == "ALTA" ? 1 : 0,
                    Nombre = model.Nombre,
                    Alisto = model.AST_Alisto,
                    Despacho = model.AST_Despacho,
                    Bultos = model.AST_Bultos,
                    Supervisor = model.AST_Supervisor,
                    Reimpresion = model.AST_Reimpresion,
                    Impresion = model.AST_impresion,
                    WEB = model.AST_WEB,
                    Almacen = model.AlmacenAsignado,
                    ID = model.id
                },
                commandType: CommandType.StoredProcedure);

                connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
