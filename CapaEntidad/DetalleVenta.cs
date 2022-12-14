using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class DetalleVenta
    {

        public int IdDetalleVenta { get; set; }
        public int IdVenta2 { get; set; }
        public Producto oProducto3 { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public string IdTrasaccion { get; set; }                
    }
}
