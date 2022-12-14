using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using CapaEntidad;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Venta
    {
        public bool Registrar(Venta obj, DataTable detalleVenta, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarVenta", oConexion);
                    cmd.Parameters.AddWithValue("IdCliente", obj.IdCliente3);
                    cmd.Parameters.AddWithValue("TotalProducto", obj.TotalProducto);
                    cmd.Parameters.AddWithValue("MontoTotal", obj.MontoTotal);
                    cmd.Parameters.AddWithValue("Contacto", obj.Contacto);
                    cmd.Parameters.AddWithValue("IdDistrito", obj.IdDistrito);
                    cmd.Parameters.AddWithValue("Telefono", obj.Telefono);
                    cmd.Parameters.AddWithValue("Direccion", obj.Direccion);
                    cmd.Parameters.AddWithValue("IdTransaccion", obj.IdTrasaccion);
                    cmd.Parameters.AddWithValue("DetalleVenta", detalleVenta);

                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {

                respuesta = false;
                Mensaje = ex.Message;
            }
            return respuesta;
        }

        public List<DetalleVenta> ListarCompras(int idCliente)
        {
            List<DetalleVenta> lista = new List<DetalleVenta>();

            try
            {

                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {


                    string query = "SELECT * FROM fn_ListarCompra(@IdCliente) ";


                    SqlCommand commandoSql = new SqlCommand(query, oConexion);
                    commandoSql.Parameters.AddWithValue("@IdCliente", idCliente);
                    commandoSql.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dR = commandoSql.ExecuteReader())
                    {

                        while (dR.Read())
                        {

                            lista.Add(new DetalleVenta
                            {

                                oProducto3 = new Producto()
                                {
                                    Nombre = dR["Nombre"].ToString(),
                                    Precio = Convert.ToDecimal(dR["Precio"], new CultureInfo("es-CL")),
                                    RutaImagen = dR["RutaImagen"].ToString(),
                                    NombreImagen = dR["NombreImagen"].ToString(),
                                },
                                Cantidad = Convert.ToInt32(dR["Cantidad"]),
                                Total = Convert.ToDecimal(dR["Precio"], new CultureInfo("es-CL")),
                                IdTrasaccion = dR["IdTrasaccion"].ToString(),
                            });
                        }
                    }
                }
            }
            catch
            {

                lista = new List<DetalleVenta>();
            }
            return lista;
        }

    }
}
