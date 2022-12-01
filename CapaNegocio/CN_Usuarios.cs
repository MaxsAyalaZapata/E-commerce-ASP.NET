using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuarios
    {

        private CD_Usuarios objCapaDatos = new CD_Usuarios();

        public List<Usuario> Listar()
        {
            return objCapaDatos.Listar();
        }

        public int Registrar(Usuario obj, out string Mensaje)
        {

            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            { 
                Mensaje = "El Apellidos del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El Apellidos del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {

                string clave = CN_Recursos.GenerarClave();

                string asunto = "Creacion de cuenta ";
                string mensaje_correo = "<h3>Su cuenta fue creada  corrrectamente</h3> </br> <p> Su contraseña para acceder es: !clave!</p> ";
                mensaje_correo = mensaje_correo.Replace("!clave!", clave);


                bool respuesta = CN_Recursos.Enviarcorreo(obj.Correo, asunto, mensaje_correo);

                if (respuesta)
                {
                    obj.Clave = CN_Recursos.ConvertirSha256(clave);
                    return objCapaDatos.RegistrarUsuario(obj, out Mensaje);

                }
                else
                {
                    Mensaje = "No se ha podido enviar el mensaje con exito";
                    return 0;
                }

            }
            else
            {
                return 0;
            }                    
        }

        public bool EditarUsuario(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El Apellidos del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El Apellidos del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
                return objCapaDatos.EditarUsuario(obj, out Mensaje);
            else
                return false;
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDatos.EliminarUsuario(id, out Mensaje);
        }

        public bool CambiarClave(int idUsuario, string nuevaClave, out string Mensaje)
        {
            return objCapaDatos.CambiarClave(idUsuario, nuevaClave, out Mensaje );
        }

        public bool ReestablecerClave(int idUsuario,string correo ,out string Mensaje)
        {

            Mensaje = string.Empty;
            string nuevaClave = CN_Recursos.GenerarClave();
            bool resultado = objCapaDatos.ReestablecerClave(idUsuario,CN_Recursos.ConvertirSha256(nuevaClave),out Mensaje);


            if (resultado)
            {
                string asunto = "Contraseña Reestablecida ";
                string mensaje_correo = "<h3>Su contraseña fue reestablecida  corrrectamente</h3> </br> <p> Su contraseña para acceder ahora es: !clave!</p> ";
                mensaje_correo = mensaje_correo.Replace("!clave!", nuevaClave);

                bool respuesta = CN_Recursos.Enviarcorreo(correo, asunto, mensaje_correo);
                if (respuesta)
                {
                    return true;

                }
                else
                {
                    Mensaje = "No se pudo enviar el correo electronico";
                    return false;
                }
            }
            else
            {
                Mensaje = "No se pudo reestablecer la contraseña";

                return false;
            }
                

        }
    }
}
