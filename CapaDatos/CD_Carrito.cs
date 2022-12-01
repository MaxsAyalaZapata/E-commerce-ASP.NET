using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
    }
}
