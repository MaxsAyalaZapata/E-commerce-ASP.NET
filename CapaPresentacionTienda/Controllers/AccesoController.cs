using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registrar()
        {
            return View();
        }

        public ActionResult Reestablecer()
        {
            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registrar(Cliente obj)
        {
            int resultado;
            string mensaje = string.Empty;

            ViewData["Nombre"] = string.IsNullOrEmpty(obj.Nombre) ? "" : obj.Nombre;
            ViewData["Apellido"] = string.IsNullOrEmpty(obj.Apellido) ? "" : obj.Apellido;
            ViewData["Correo"] = string.IsNullOrEmpty(obj.Correo) ? "" : obj.Correo;

            if (obj.Clave != obj.ConfirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            
            resultado = new CN_Cliente().Registrar(obj, out mensaje);

            if (resultado > 0)
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
        [HttpPost]
        public ActionResult Index(string correo , string clave)
        {
            Cliente oCliente = null;

            oCliente = new CN_Cliente().Listar().Where(item => item.Correo == correo && item.Clave == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();

            if (oCliente == null)
            {
                ViewBag.Error = "Correo o contraseña no son correctas";
                return View();
            }
            else 
            {
                if (oCliente.ReEstablecer)
                {
                    TempData["IdCliente"] = oCliente.IdCliente;
                    return RedirectToAction("CambiarClave", "Acceso");

                }
                else 
                {
                    FormsAuthentication.SetAuthCookie(oCliente.Correo, false);

                    Session["Cliente"] = oCliente;

                    ViewBag.Error = null;

                    return RedirectToAction("Index", "Tienda");
                }
            }
        }


        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Cliente oCliente = new Cliente();

            oCliente = new CN_Cliente().Listar().Where(item => item.Correo == correo).FirstOrDefault();

            if (oCliente == null)
            {
                ViewBag.Error = "No se Encontro  un Cliente con un Cliente relacionado a ese correo  ";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Cliente().ReestablecerClave(oCliente.IdCliente, oCliente.Correo, out mensaje);

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

        [HttpPost]
        public ActionResult CambiarClave(string idCliente, string claveActual, string nuevaClave, string confirmacionClave)
        {
            Cliente oCliente = new Cliente();

            oCliente = new CN_Cliente().Listar().Where(u => u.IdCliente == int.Parse(idCliente)).FirstOrDefault();

            if (oCliente.Clave != CN_Recursos.ConvertirSha256(claveActual))
            {
                TempData["IdCliente"] = idCliente;
                ViewData["vClave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }
            else if (nuevaClave != confirmacionClave)
            {
                TempData["IdCliente"] = idCliente;
                ViewData["vClave"] = claveActual;
                ViewBag.Error = "Las contraseñas nuevas no coinciden";
                return View();
            }
            ViewData["vClave"] = "";

            nuevaClave = CN_Recursos.ConvertirSha256(nuevaClave);

            string mensaje = string.Empty;

            bool respuesta = new CN_Cliente().CambiarClave(int.Parse(idCliente), nuevaClave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index", "Acceso");

            }
            else
            {
                TempData["IdCliente"] = idCliente;
                ViewBag.Error = mensaje;
                return View();
            }
        }

        public ActionResult CerrarSesion()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }
    }
}