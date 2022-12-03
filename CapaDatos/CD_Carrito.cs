﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using System.Data;
using System.Data.SqlClient;


namespace CapaDatos
{
    public class CD_Carrito
    {

        public bool ExisteCarrito(int idCliente, int idProducto)
        {
            bool resultado = true;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ExisteCarrito", oConexion);
                    cmd.Parameters.AddWithValue("IdCliente", idCliente);
                    cmd.Parameters.AddWithValue("IdProducto", idProducto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteReader();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }
            }
            catch (Exception ex)
            {

                resultado = true;
            }
            return resultado;
        }

        public bool OperacionCarrito(int idCliente, int idProducto, bool sumar, out string Mensaje)
        {
            bool resultado = true;

            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_OperacionCarrito", oConexion);
                    cmd.Parameters.AddWithValue("IdCliente", idCliente);
                    cmd.Parameters.AddWithValue("IdProducto", idProducto);
                    cmd.Parameters.AddWithValue("Sumar", sumar);

                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteReader();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {

                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        
        public int CantidadEnCarrito(int idCliente)
        {
            int resultado = 0;        

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM CARRITO WHERE IdCliente2 =@idCliente", oConexion);
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);
                    cmd.CommandType = CommandType.Text;
                    oConexion.Open();
                    resultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
            }
            return resultado;
        }

        public List<Carrito> ListarProducto(int IdCliente)
        {
            List<Carrito> lista = new List<Carrito>();

            try
            {

                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {

                    string query = "SELECT * FROM fn_obtenerCarritoCliente(@IdCliente) ";


                    SqlCommand commandoSql = new SqlCommand(query, oConexion);
                    commandoSql.Parameters.AddWithValue("@IdCliente", IdCliente);
                    commandoSql.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dR = commandoSql.ExecuteReader())
                    {

                        while (dR.Read())
                        {

                            lista.Add(new Producto
                            {
                                IdProducto = Convert.ToInt32(dR["IdProducto"]),
                                Nombre = dR["Nombre"].ToString(),
                                Descripcion = dR["Descripcion"].ToString(),
                                oMarca2 = new Marca()
                                {
                                    IdMarca = Convert.ToInt32(dR["IdMarca"]),
                                    Descripcion = dR["DesMarca"].ToString(),

                                },
                                oCategoria2 = new Categoria()
                                {
                                    IdCategoria = Convert.ToInt32(dR["IdCategoria"]),
                                    Descripcion = dR["DesCategoria"].ToString(),
                                },
                                Precio = Convert.ToDecimal(dR["Precio"], new CultureInfo("es-CL")),
                                Stock = Convert.ToInt32(dR["Stock"]),
                                RutaImagen = dR["RutaImagen"].ToString(),
                                NombreImagen = dR["NombreImagen"].ToString(),
                                Activo = Convert.ToBoolean(dR["Activo"]),
                            });
                        }
                    }
                }
            }
            catch
            {

                lista = new List<Producto>();
            }
            return lista;
        }
    }
}
