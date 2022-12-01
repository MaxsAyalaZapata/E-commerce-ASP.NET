using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;
namespace CapaNegocio
{
    public class CN_Carrito
    {
        private CD_Carrito objCapaDatos = new CD_Carrito();

        public bool ExisteCarrito(int idCliente, int idProducto)
        {
            return objCapaDatos.ExisteCarrito(idCliente, idProducto);
        }

        public bool OperacionCarrito(int idCliente, int idProducto, bool sumar, out string Mensaje)
        {
            return objCapaDatos.OperacionCarrito(idCliente, idProducto, sumar, out  Mensaje);
        }

        public int CantidadEnCarrito(int idCliente)
        {
            return objCapaDatos.CantidadEnCarrito(idCliente);
        }
    }
}
