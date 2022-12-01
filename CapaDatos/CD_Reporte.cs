using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Reporte
    {
        public List<Reporte> Ventas(string fechaInicio, string fechaFin, string idTransaccion)
        {
            List<Reporte> lista = new List<Reporte>();

            try
            {

                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {

                    SqlCommand comandoSql = new SqlCommand("sp_ReporteVentas", oConexion);
                    comandoSql.Parameters.AddWithValue("FechaInicio", fechaInicio);
                    comandoSql.Parameters.AddWithValue("FechaFin", fechaFin);
                    comandoSql.Parameters.AddWithValue("IdTransaccion", idTransaccion);

                    comandoSql.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    using (SqlDataReader dR = comandoSql.ExecuteReader())
                    {
                        while (dR.Read())
                        {
                            lista.Add(new Reporte
                            {

                                FechaVenta = dR["FechaVenta"].ToString(),
                                Cliente = dR["Cliente"].ToString(),
                                Producto = dR["Producto"].ToString(),
                                Precio = Convert.ToInt32(dR["Precio"], new CultureInfo("es-CH")),
                                Cantidad = Convert.ToInt32(dR["Cantidad"]),
                                Total = Convert.ToInt32(dR["Total"], new CultureInfo("es-CH")),
                                IdTransaccion = dR["IdTransaccion"].ToString(),
                            });
                        }
                    }
                }


            }
            catch(Exception ex)
            {
                var e = ex.Message;
                lista = new List<Reporte>();
            }

            return lista;
        }

        public DashBoard VerDashBoards()
        {
            DashBoard objeto = new DashBoard();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {

                    SqlCommand commandoSql = new SqlCommand("sp_ReporteDashboard", oConexion);
                    commandoSql.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    using (SqlDataReader dR = commandoSql.ExecuteReader())
                    {

                        while (dR.Read())
                        {

                            objeto = new DashBoard()
                            {
                                TotalCliente = Convert.ToInt32(dR["TotalCliente"]),
                                TotalVentas = Convert.ToInt32(dR["TotalVenta"]),
                                TotalProductos = Convert.ToInt32(dR["TotalProducto"])
                            };


                        }
                    }
                }


            }
            catch
            {

                objeto = new DashBoard();
            }

            return objeto;
        }

    }
}
