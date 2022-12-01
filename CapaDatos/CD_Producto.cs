using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using CapaEntidad;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Producto
    {
        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();

            try
            {

                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {

                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("SELECT p.IdProducto, p.Nombre, p.Descripcion,");
                    sb.AppendLine("m.IdMarca, m.Descripcion[DesMarca],");
                    sb.AppendLine("c.IdCategoria, c.Descripcion[DesCategoria],");
                    sb.AppendLine("p.Precio, p.Stock, p.RutaImagen, p.NombreImagen, p.Activo");
                    sb.AppendLine("FROM PRODUCTO p");
                    sb.AppendLine("INNER JOIN MARCA m ON m.IdMarca = p.IdMarca2");
                    sb.AppendLine("INNER JOIN CATEGORIA c ON c.IdCategoria = p.IdCategoria2");

                    SqlCommand commandoSql = new SqlCommand(sb.ToString(), oConexion);
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
                            }) ;
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

        public int RegistrarProducto(Producto obj, out string mensaje)
        {
            int IdAutoGenerado = 0;

            mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {

                    SqlCommand cmd = new SqlCommand("sp_RegistrarProducto", oConexion);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", obj.oMarca2.IdMarca);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.oCategoria2.IdCategoria);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteReader();

                    IdAutoGenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {

                IdAutoGenerado = 0;
                mensaje = ex.Message;
            }
            return IdAutoGenerado;
        }

        public bool EditarProducto(Producto obj, out string Mensaje)
        {
            bool resultado = false;

            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarProducto", oConexion);
                    cmd.Parameters.AddWithValue("IdProducto", obj.IdProducto);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", obj.oMarca2.IdMarca);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.oCategoria2.IdCategoria);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

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

        public bool GuardarDatosImagen(Producto oProducto, out string Mensaje )           
        {

            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {

                    string consulta = "UPDATE PRODUCTO SET RutaImagen = @RutaImagen, NombreImagen = @NombreImagen WHERE IdProducto = @IdProducto";

                    SqlCommand cmd = new SqlCommand(consulta, oConexion);     
                    cmd.Parameters.AddWithValue("@RutaImagen", oProducto.RutaImagen);
                    cmd.Parameters.AddWithValue("@NombreImagen", oProducto.NombreImagen);
                    cmd.Parameters.AddWithValue("@IdProducto", oProducto.IdProducto);

                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        Mensaje = "No se pudo actualizar la imagen ";
                    }
                   
                }
            }
            catch (Exception ex)
            {

                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }



        public bool EliminarProducto(int id, out string Mensaje)
        {
            bool resultado = false;

            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarProducto", oConexion);
                    cmd.Parameters.AddWithValue("IdProducto", id);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

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
    }
}
