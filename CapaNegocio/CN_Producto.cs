using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Producto
    {
        private CD_Producto objCapaDatos = new CD_Producto();

        public List<Producto> Listar()
        {
            return objCapaDatos.Listar();
        }

        public int Registrar(Producto obj, out string Mensaje)
        {

            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "La Nombre de la Producto no puede ser vacio";
            }

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La Descripcion de la Producto no puede ser vacio";
            }

            if (obj.oMarca2.IdMarca == 0 )
            {
                Mensaje = "Debe seleccionar una marca para el producto";
            }

            if (obj.oCategoria2.IdCategoria == 0)
            {
                Mensaje = "Debe seleccionar una categoria para el producto";
            }

            if ( obj.Precio ==0)
            {
                Mensaje = "Debe ingresar el precio del producto";
            }

            if (obj.Stock == 0 )
            {
                Mensaje = "Debe ingresar el stock del producto";
            }           

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDatos.RegistrarProducto(obj, out Mensaje);

            }
            else
            {
                return 0;
            }
        }

        public bool EditarProducto(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "La Nombre de la Producto no puede ser vacio";
            }

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La Descripcion de la Producto no puede ser vacio";
            }

            if (obj.oMarca2.IdMarca == 0)
            {
                Mensaje = "Debe seleccionar una marca para el producto";
            }

            if (obj.oCategoria2.IdCategoria == 0)
            {
                Mensaje = "Debe seleccionar una categoria para el producto";
            }

            if (obj.Precio == 0)
            {
                Mensaje = "Debe ingresar el precio del producto";
            }

            if (obj.Stock == 0)
            {
                Mensaje = "Debe ingresar el stock del producto";
            }


            if (string.IsNullOrEmpty(Mensaje))
                return objCapaDatos.EditarProducto(obj, out Mensaje);
            else
                return false;
        }

        public bool GuardarDatosImagen(Producto oProducto, out string Mensaje)
        {
            return objCapaDatos.GuardarDatosImagen(oProducto, out Mensaje);
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDatos.EliminarProducto(id, out Mensaje);
        }


    }
}
