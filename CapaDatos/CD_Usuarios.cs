
using System;
using System.Collections.Generic;

using System.Data;
using System.Data.SqlClient;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_Usuarios
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            try
            {

                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {

                    string consulta = "SELECT IdUsuario,Nombres,Apellidos,Correo,Clave,ReEstablecer,Activo FROM USUARIO";

                    SqlCommand commandoSql = new SqlCommand(consulta, oConexion);
                    commandoSql.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dR = commandoSql.ExecuteReader())
                    {

                        while (dR.Read())
                        {

                            lista.Add(new Usuario
                            {
                                IdUsuario = Convert.ToInt32(dR["IdUsuario"]),
                                Nombres = dR["Nombres"].ToString(),
                                Apellidos = dR["Apellidos"].ToString(),
                                Correo = dR["Correo"].ToString(),
                                Clave = dR["Clave"].ToString(),
                                ReEstablecer = Convert.ToBoolean(dR["ReEstablecer"]),
                                Activo = Convert.ToBoolean(dR["Activo"]),
                            });
                        }
                    }
                }


            }
            catch
            {

                lista = new List<Usuario>();
            }

            return lista;
        }

        public int RegistrarUsuario(Usuario obj, out string Mensaje )
        {
            int IdAutoGenerado = 0;

            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", oConexion);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
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

        public bool EditarUsuario(Usuario obj, out string Mensaje)
        {
            bool resultado = false;

            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarUsuario", oConexion);
                    cmd.Parameters.AddWithValue("IdUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
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

        public bool EliminarUsuario(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                { 
                    SqlCommand cmd = new SqlCommand("DELETE TOP(1) FROM USUARIO WHERE IdUsuario = @id", oConexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    oConexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        public bool CambiarClave(int idUsuario, string nuevaClave ,out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE USUARIO SET Clave = @NuevaClave , Reestablecer = 0 WHERE IdUsuario = @Id", oConexion);
                    cmd.Parameters.AddWithValue("@id", idUsuario);
                    cmd.Parameters.AddWithValue("@NuevaClave", nuevaClave);

                    cmd.CommandType = CommandType.Text;
                    oConexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }


        public bool ReestablecerClave(int idUsuario, string clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE USUARIO SET Clave = @Clave , Reestablecer = 1 WHERE IdUsuario = @Id", oConexion);
                    cmd.Parameters.AddWithValue("@id", idUsuario);
                    cmd.Parameters.AddWithValue("@Clave", clave);
                    cmd.CommandType = CommandType.Text;
                    oConexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
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
