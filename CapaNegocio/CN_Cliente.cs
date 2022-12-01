using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Cliente
    {
        private CD_Clientes objCapaDatos = new CD_Clientes();

        public List<Cliente> Listar()
        {
            return objCapaDatos.Listar();
        }

        public int Registrar(Cliente obj, out string Mensaje)
        {

            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del Cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellido) || string.IsNullOrWhiteSpace(obj.Apellido))
            {
                Mensaje = "El Apellido del Cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El Apellido del Cliente no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {

                    obj.Clave = CN_Recursos.ConvertirSha256(obj.Clave);
                    return objCapaDatos.RegistrarCliente(obj, out Mensaje);
            }
            else
            {
                return 0;
            }
        }



        public bool CambiarClave(int idCliente, string nuevaClave, out string Mensaje)
        {
            return objCapaDatos.CambiarClave(idCliente, nuevaClave, out Mensaje);
        }

        public bool ReestablecerClave(int idCliente, string correo, out string Mensaje)
        {

            Mensaje = string.Empty;
            string nuevaClave = CN_Recursos.GenerarClave();
            bool resultado = objCapaDatos.ReestablecerClave(idCliente, CN_Recursos.ConvertirSha256(nuevaClave), out Mensaje);


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
