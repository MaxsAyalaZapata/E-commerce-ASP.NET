using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Clientes
    {

        public int RegistrarCliente(Cliente obj, out string Mensaje)
        {
            int IdAutoGenerado = 0;

            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarCliente", oConexion);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Apellido", obj.Apellido);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
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

        public List<Cliente> Listar()
        {
            List<Cliente> lista = new List<Cliente>();

            try
            {

                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {

                    string consulta = "SELECT IdCliente,Nombre,Apellido,Correo,Clave,ReEstablecer FROM Cliente";

                    SqlCommand commandoSql = new SqlCommand(consulta, oConexion);
                    commandoSql.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dR = commandoSql.ExecuteReader())
                    {

                        while (dR.Read())
                        {

                            lista.Add(new Cliente
                            {
                                IdCliente = Convert.ToInt32(dR["IdCliente"]),
                                Nombre = dR["Nombre"].ToString(),
                                Apellido = dR["Apellido"].ToString(),
                                Correo = dR["Correo"].ToString(),
                                Clave = dR["Clave"].ToString(),
                                ReEstablecer = Convert.ToBoolean(dR["ReEstablecer"]),
                            });
                        }
                    }
                }


            }
            catch
            {

                lista = new List<Cliente>();
            }

            return lista;
        }

        public bool CambiarClave(int idCliente, string nuevaClave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Cliente SET Clave = @NuevaClave , Reestablecer = 0 WHERE IdCliente = @Id", oConexion);
                    cmd.Parameters.AddWithValue("@id", idCliente);
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

        public bool ReestablecerClave(int idCliente, string clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Cliente SET Clave = @Clave , Reestablecer = 1 WHERE IdCliente = @Id", oConexion);
                    cmd.Parameters.AddWithValue("@id", idCliente);
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
