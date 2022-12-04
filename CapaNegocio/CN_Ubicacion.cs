using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Ubicacion
    {
        private CD_Ubicacion objCapaDatos = new CD_Ubicacion();

        public List<Departamento> ObtenerDepartamento() 
        {
            return objCapaDatos.ObtenerDepartamento();
        }

        public List<Provincia> ObtenerProvincias(string idDepartamento)
        { 
            return objCapaDatos.ObtenerProvincias(idDepartamento);
        }
        public List<Distrito> ObtenerDistrito(string idDepartamento, string idProvincia)
        { 
            return objCapaDatos.ObtenerDistrito(idDepartamento, idProvincia);
        }
    }
}
