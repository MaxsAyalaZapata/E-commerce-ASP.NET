using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Reporte
    {
        private  CD_Reporte objCapaDatos = new CD_Reporte();

        public DashBoard VerDashBoards()
        {
            return objCapaDatos.VerDashBoards();
        }

        public List<Reporte> Ventas(string fechaInicio, string fechaFin, string idTransaccion)
        {
           return objCapaDatos.Ventas(fechaInicio, fechaFin, idTransaccion);
        }

    }
}
