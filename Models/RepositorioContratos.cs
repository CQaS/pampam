using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
 
namespace InmobiliariaVaras.Models
{
    public class RepositorioContratos : RepositorioBase
    {

        public RepositorioContratos(IConfiguration configuration): base(configuration)
        {

        }
        
        public List<Contratos> obtener()
        {
            var res = new List<Contratos>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT c.id_Cont, c.fecha_In, c.fecha_Fin, c.inq_Id , i.nombre_Inq, c.inm_Id, e.dom_Inm, e.prop_Id FROM Contratos c JOIN Inquilinos i ON i.id_Inq = c.inq_Id JOIN Inmuebles e ON e.id_Inm = c.inm_Id WHERE c.fecha_fin >= Curdate() AND c.estado = 1";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var e = new Contratos
                        {
                            id_Cont = reader.GetInt32(0),
                            fecha_In = reader.GetDateTime(1),
                            fecha_Fin = reader.GetDateTime(2),
                            Inquilinos = new Inquilinos
                            {
                                nombre_Inq = reader.GetString(4),
                            },
                            Inmuebles = new Inmuebles
                            {
                                dom_Inm = reader.GetString(6)
                            }

                        };
                        res.Add(e);
                    }
                    connection.Close();
                }

            }
            return res;
        }

        public Contratos Buscar(int id)
        {
            Contratos con;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT c.id_Cont, c.fecha_In, c.fecha_Fin, c.valor, inq.nombre_Inq, i.dom_Inm, c.inm_Id, c.inq_Id FROM Contratos c JOIN Inmuebles i ON c.inm_Id = i.id_Inm JOIN Inquilinos inq ON c.inq_Id = inq.id_Inq WHERE c.id_Cont = @id AND c.estado = 1";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    connection.Open();
                    MySqlDataReader res = command.ExecuteReader();
                    con = new Contratos();
                    if(res.Read())
                    {
                        con.id_Cont = int.Parse(res["id_Cont"].ToString());
                        con.fecha_In = DateTime.Parse(res["fecha_In"].ToString());
                        con.fecha_Fin = DateTime.Parse(res["fecha_Fin"].ToString());
                        con.valor = int.Parse(res["valor"].ToString());
                        con.inm_Id = int.Parse(res["inm_Id"].ToString());
                        con.inq_Id = int.Parse(res["inq_Id"].ToString());
                    }
                    connection.Close();                                       
                }
            }
            return con;
        }


        public int Alta(Contratos e)
        {
            var res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"INSERT INTO contratos (fecha_In, fecha_Fin, valor, inm_Id, inq_Id) VALUES (@fecha_In, @fecha_Fin, @valor, @inm_Id, @inq_Id)";
                
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
					command.Parameters.AddWithValue("@fecha_In", e.fecha_In);
                    command.Parameters.AddWithValue("@fecha_Fin", e.fecha_Fin);
                    command.Parameters.AddWithValue("@valor", e.valor);
                    command.Parameters.AddWithValue("@inm_Id", e.inm_Id);
                    command.Parameters.AddWithValue("@inq_Id", e.inq_Id);
                    connection.Open();
                    command.ExecuteScalar();
                    connection.Close();
                }                
                string sql_ID = $"SELECT MAX(id_Cont) AS idUltimo FROM contratos";
                
                using (var command = new MySqlCommand(sql_ID, connection))
                {
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            return res;
        }

        public int Editar(Contratos e)
        {
            var i = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"update contratos set fecha_In=@fecha_In, fecha_Fin=@fecha_Fin, inm_Id=@inm_id, inq_Id=@inq_id, valor=@valor where id_Cont=@id_Cont";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@fecha_In", e.fecha_In);
					command.Parameters.AddWithValue("@fecha_Fin", e.fecha_Fin);
                    command.Parameters.AddWithValue("@valor", e.valor);
					command.Parameters.AddWithValue("@inm_Id", e.inm_Id);
					command.Parameters.AddWithValue("@inq_Id", e.inq_Id);
					command.Parameters.AddWithValue("@id_Cont", e.id_Cont);
                    connection.Open();
                    i = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return i;
        }

        public int Borrar(int id)
        {
            var i = 0;
            var m = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"update contratos set estado='0' where id_Cont = @id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.UInt32);
                    command.Parameters["@id"].Value = id;
                    connection.Open();
                    i = command.ExecuteNonQuery();
                    connection.Close();
                }

                string multa = $"Select Multa(@id)";

                using (var command = new MySqlCommand(multa, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.UInt32);
                    command.Parameters["@id"].Value = id;
                    connection.Open();
                    m =  Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }

            }
            return m;
        }

        public List<Contratos> VerContratosDelInmueble(int id)
        {
            var res = new List<Contratos>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT c.id_Cont, c.fecha_In, c.fecha_Fin, i.nombre_Inq, e.dom_Inm FROM Contratos c JOIN Inquilinos i ON i.id_Inq = c.inq_Id JOIN Inmuebles e ON e.id_Inm = c.inm_Id WHERE c.inm_Id = @id_inmueble";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id_inmueble", MySqlDbType.Int32).Value = id;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var e = new Contratos
                        {
                            id_Cont = reader.GetInt32(0),
                            fecha_In = reader.GetDateTime(1),
                            fecha_Fin = reader.GetDateTime(2),
                            Inquilinos = new Inquilinos
                            {
                                nombre_Inq = reader.GetString(3),
                            },
                            Inmuebles = new Inmuebles
                            {
                                dom_Inm = reader.GetString(4)
                            }
                        };
                        res.Add(e);
                    }
                    connection.Close();                                       
                }
            }
            return res;
        }
    }
}