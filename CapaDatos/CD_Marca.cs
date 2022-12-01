using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class CD_Marca
    {

        public List<Marca> Listar()
        {
            List<Marca> lista = new List<Marca>();

            try
            {

                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {

                    string consulta = "SELECT IdMarca,Descripcion,Activo FROM MARCA";

                    SqlCommand commandoSql = new SqlCommand(consulta, oConexion);
                    commandoSql.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dR = commandoSql.ExecuteReader())
                    {

                        while (dR.Read())
                        {

                            lista.Add(new Marca
                            {
                                IdMarca = Convert.ToInt32(dR["IdMarca"]),
                                Descripcion = dR["Descripcion"].ToString(),
                                Activo = Convert.ToBoolean(dR["Activo"]),
                            });
                        }
                    }
                }
            }
            catch
            {

                lista = new List<Marca>();
            }
            return lista;
        }

        public int RegistrarMarca(Marca obj, out string Mensaje)
        {
            int IdAutoGenerado = 0;

            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarMarca", oConexion);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteReader();

                    IdAutoGenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {

                IdAutoGenerado = 0;
                Mensaje = ex.Message;
            }
            return IdAutoGenerado;
        }

        public bool EditarMarca(Marca obj, out string Mensaje)
        {
            bool resultado = false;

            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarMarca", oConexion);
                    cmd.Parameters.AddWithValue("IdMarca", obj.IdMarca);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
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

        public bool EliminarMarca(int id, out string Mensaje)
        {
            bool resultado = false;

            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarMarca", oConexion);
                    cmd.Parameters.AddWithValue("IdMarca", id);
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

        public List<Marca> ListarMarcaPorCategoria(int idCategoria)
        {
            List<Marca> lista = new List<Marca>();

            try
            {

                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("SELECT* FROM PRODUCTO p");
                    sb.AppendLine("INNER JOIN CATEGORIA c ON c.IdCategoria = p.IdCategoria2");
                    sb.AppendLine("INNER JOIN MARCA m ON m.IdMarca = p.IdMarca2 AND m.Activo = 1");
                    sb.AppendLine("WHERE c.IdCategoria = iif(@idcategoria = 0, c.IdCategoria, @idcategoria)");

                    SqlCommand commandoSql = new SqlCommand(sb.ToString(), oConexion);
                    commandoSql.Parameters.AddWithValue("@idcategoria", idCategoria);
                    commandoSql.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dR = commandoSql.ExecuteReader())
                    {

                        while (dR.Read())
                        {

                            lista.Add(new Marca
                            {
                                IdMarca = Convert.ToInt32(dR["IdMarca"]),
                                Descripcion = dR["Descripcion"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {

                lista = new List<Marca>();
            }
            return lista;
        }

    }
}
