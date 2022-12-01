using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;

using System.Web.Security;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }

        public ActionResult Reestablecer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index( string correo , string clave )
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo && u.Clave == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();

            if (oUsuario == null)
            {
                ViewBag.Error = "Correo o contraseña no correcta";

                return View();
            }
            else
            {
                if (oUsuario.ReEstablecer)
                {
                    TempData["IdUsuario"] = oUsuario.IdUsuario; 

                    FormsAuthentication.SetAuthCookie(oUsuario.Correo, false); 

                    return RedirectToAction("CambiarClave");
                }
                return RedirectToAction("Index","Home"); 
            }
      
        }

        [HttpPost]
        public ActionResult CambiarClave(string idUsuario, string claveActual, string nuevaClave,string confirmacionClave)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.IdUsuario == int.Parse(idUsuario)).FirstOrDefault();

            if (oUsuario.Clave != CN_Recursos.ConvertirSha256(claveActual))
            {
                TempData["IdUsuario"] = idUsuario;
                ViewData["vClave"]= "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }
            else if (nuevaClave != confirmacionClave)
            {
                TempData["IdUsuario"] = idUsuario;
                ViewData["vClave"] = claveActual;
                ViewBag.Error = "Las contraseñas nuevas no coinciden";
                return View();
            }
            ViewData["vClave"] = "";

            nuevaClave = CN_Recursos.ConvertirSha256(nuevaClave);

            string mensaje = string.Empty;

            bool respuesta = new CN_Usuarios().CambiarClave(int.Parse(idUsuario), nuevaClave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index","Acceso");

            }
            else
            {
                TempData["IdUsuario"] = idUsuario;
                ViewBag.Error = mensaje;
                return View();
            }

        }

        [HttpPost]

        public ActionResult Reestablecer(string correo)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(item => item.Correo == correo).FirstOrDefault();

            if (oUsuario == null)
            {
                ViewBag.Error = "No se Encontro  un usuario con un usuario relacionado a ese correo  ";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().ReestablecerClave(oUsuario.IdUsuario, oUsuario.Correo, out mensaje);

            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        public ActionResult CerraSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");

        }
    }
}