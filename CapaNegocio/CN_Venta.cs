using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Venta
    {
        private CD_Venta objCapaDatos = new CD_Venta();
       
        public bool Registrar(Venta obj, DataTable detalleVenta, out string Mensaje) 
        {
            return objCapaDatos.Registrar(obj, detalleVenta, out Mensaje);
        }
    }
}
