﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Categoria
    {
        private CD_Categoria objCapaDatos = new CD_Categoria();

        public List<Categoria> Listar()
        {
            return objCapaDatos.Listar();
        }

        public int Registrar(Categoria obj, out string Mensaje)
        {

            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La Descripcion de la categoria no puede ser vacio";
            }
            

            if (string.IsNullOrEmpty(Mensaje))
            {  
                return objCapaDatos.RegistrarCategoria(obj, out Mensaje);

            }
            else
            {
                return 0;
            }
        }

        public bool EditarCategoria(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripcion de la categoria no puede ser vacio";
            }
     
            if (string.IsNullOrEmpty(Mensaje))
                return objCapaDatos.EditarCategoria(obj, out Mensaje);
            else
                return false;
        }


        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDatos.EliminarCategoria(id, out Mensaje);
        }

    }

 
}
