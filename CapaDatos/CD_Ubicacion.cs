using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Data;
using System.Data.SqlClient;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_Ubicacion
    {
        public List<Departamento> ObtenerDepartamento()
        {
            List<Departamento> lista = new List<Departamento>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string consulta = "SELECT * FROM DEPARTAMENTO";

                    SqlCommand commandoSql = new SqlCommand(consulta, oConexion);
                    commandoSql.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dR = commandoSql.ExecuteReader())
                    {
                        while (dR.Read())
                        {
                            lista.Add(new Departamento
                            {
                                IdDepartamento = dR["IdDepartamento"].ToString(),
                                Descripcion = dR["Descipcion"].ToString(),
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Departamento>();
            }
            return lista;
        }

        public List<Provincia> ObtenerProvincias(string idDepartamento)
        {
            List<Provincia> lista = new List<Provincia>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string consulta = "SELECT * FROM PROVINCIA WHERE IdDepartamento2 = @IdDepartamento";

                    SqlCommand commandoSql = new SqlCommand(consulta, oConexion);
                    commandoSql.Parameters.AddWithValue("@IdDepartamento", idDepartamento);
                    commandoSql.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dR = commandoSql.ExecuteReader())
                    {
                        while (dR.Read())
                        {
                            lista.Add(new Provincia
                            {
                                IdProvincia = dR["IdProvincia"].ToString(),
                                Descripcion = dR["Descipcion"].ToString(),
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Provincia>();
            }
            return lista;
        }

        public List<Distrito> ObtenerDistrito(string idDepartamento, string idProvincia)
        {
            List<Distrito> lista = new List<Distrito>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string consulta = "SELECT * FROM DISTRITO WHERE IdProvincia2 = @IdDepartamento AND IdDepartamento3 = @IdProvincia";

                    SqlCommand commandoSql = new SqlCommand(consulta, oConexion);
                    commandoSql.Parameters.AddWithValue("@IdDepartamento", idDepartamento);
                    commandoSql.Parameters.AddWithValue("@IdProvincia", idProvincia);
                    commandoSql.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dR = commandoSql.ExecuteReader())
                    {
                        while (dR.Read())
                        {
                            lista.Add(new Distrito
                            {
                                IdDistrito = dR["IdDistrito"].ToString(),
                                Descripcion = dR["Descipcion"].ToString(),
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Distrito>();
            }
            return lista;
        }
    }
}
