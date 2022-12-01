using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Carrito
    {
        public int IdCarrito { get; set; }
        public Cliente oCliente2 { get; set; }
        public Producto oProducto2 { get; set; }
        public bool Activo { get; set; }
        public int Cantidad { get; set; }

        
    }
}
