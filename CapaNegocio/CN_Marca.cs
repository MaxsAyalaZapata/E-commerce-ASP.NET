﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Marca
    {
        private CD_Marca objCapaDatos = new CD_Marca();

        public List<Marca> Listar()
        {
            return objCapaDatos.Listar();
        }

        public int Registrar(Marca obj, out string Mensaje)
        {

            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La Descripcion de la Marca no puede ser vacio";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDatos.RegistrarMarca(obj, out Mensaje);

            }
            else
            {
                return 0;
            }
        }

        public bool EditarMarca(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripcion de la Marca no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
                return objCapaDatos.EditarMarca(obj, out Mensaje);
            else
                return false;
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDatos.EliminarMarca(id, out Mensaje);
        }

        public List<Marca> ListarMarcaPorCategoria(int idCategoria)
        {
            return objCapaDatos.ListarMarcaPorCategoria(idCategoria);
        }

    }
}
