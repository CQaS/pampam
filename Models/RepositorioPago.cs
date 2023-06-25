using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
 
namespace InmobiliariaVaras.Models
{
    public class RepositorioPagos: RepositorioBase
    {

        public RepositorioPagos(IConfiguration configuration): base(configuration)
        {

        }
        
        public List<Pagos> obtener()
        {
            var res = new List<Pagos>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT p.id_Pagos, p.num_Pago, p.fecha , p.importe, p.contrato_id, c.inq_Id , c.inm_Id, inq.nombre_Inq FROM Pagos p JOIN Contratos c ON p.contrato_id = c.id_Cont JOIN inquilinos inq ON inq.id_Inq = c.inq_Id WHERE p.estado = 1";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var e = new Pagos
                        {
                            id_Pagos = reader.GetInt32(0),
                            num_Pago = reader.GetInt32(1),
                            fecha = reader.GetDateTime(2),
                            importe = reader.GetDecimal(3),
                            contrato_id = reader.GetInt32(4),
                            Contratos = new Contratos
                            {
                                inq_Id = reader.GetInt32(5),
							    inm_Id = reader.GetInt32(6),
                            },
                            inquilinos = new Inquilinos
                            {
                                nombre_Inq = reader.GetString(7),
                            }
                        };
                        res.Add(e);
                    }
                    connection.Close();
                }

            }
            return res;
        }

        public Pagos Buscar(int id)
        {
            Pagos p = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT p.id_Pagos, p.num_Pago, p.fecha, p.importe, p.contrato_Id, c.inq_Id, c.inm_Id FROM Pagos p JOIN Contratos c ON p.contrato_Id = c.id_Cont WHERE p.contrato_Id= @id AND p.estado = 1";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    p = new Pagos();
                    if(reader.Read())
                    {
                        p.id_Pagos = int.Parse(reader["id_Pagos"].ToString());
                        p.num_Pago = int.Parse(reader["num_Pago"].ToString());
                        p.fecha = DateTime.Parse(reader["fecha"].ToString());
                        p.importe = int.Parse(reader["importe"].ToString());
                    }
                    connection.Close();                                       
                }
            }
            return p;
        }

        public Pagos BuscarPorPagos(int id)
        {
            Pagos p = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT p.id_Pagos, p.num_Pago, p.fecha, p.importe FROM Pagos p WHERE p.id_Pagos= @id AND p.estado = 1";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    p = new Pagos();
                    if(reader.Read())
                    {
                        p.id_Pagos = int.Parse(reader["id_Pagos"].ToString());
                        p.num_Pago = int.Parse(reader["num_Pago"].ToString());
                        p.fecha = DateTime.Parse(reader["fecha"].ToString());
                        p.importe = int.Parse(reader["importe"].ToString());
                    }
                    connection.Close();                                       
                }
            }
            return p;
        }

        public IList<Pagos> BuscarPorContrato(int id)
		{
			List<Pagos> res = new List<Pagos>();
			Pagos entidad = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
                string sql = "SELECT p.id_Pagos, p.num_pago, p.Fecha, p.importe, p.contrato_id, c.inq_Id, c.inm_Id, inq.nombre_Inq, inm.dom_Inm FROM pagos p JOIN contratos c ON p.contrato_id = c.id_Cont JOIN inquilinos inq ON c.inq_Id = inq.id_Inq JOIN inmuebles inm ON c.inm_Id = inm.id_Inm WHERE c.id_Cont = @id AND p.estado = 1";
				//string sql = $"SELECT p.id_Pagos, p.num_pago, p.Fecha, p.importe, p.contrato_id, c.inq_Id, c.inm_Id FROM pagos p JOIN contratos c ON p.contrato_id = c.id_Cont WHERE c.id_Cont = @id AND p.estado = 1";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
					command.CommandType = System.Data.CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						entidad = new Pagos
						{
							id_Pagos = reader.GetInt32(0),
							num_Pago =reader.GetInt32(1),
							fecha = reader.GetDateTime(2),
							importe = reader.GetDecimal(3),
							contrato_id = reader.GetInt32(4),
							Contratos = new Contratos
							{
								id_Cont = reader.GetInt32(4),
								inq_Id = reader.GetInt32(5),
								inm_Id = reader.GetInt32(6),
							},
                            inquilinos = new Inquilinos
                            {
                                nombre_Inq = reader.GetString(7)
                            },
                            Inmuebles = new Inmuebles
                            {
                                dom_Inm = reader.GetString(8)
                            }
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}

        public int Alta(Pagos e)
        {
            
            var res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"INSERT INTO pagos (num_Pago, fecha, importe, contrato_Id) VALUES (@num_Pago,@fecha, @importe, @contrato_Id)";
                
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
					command.Parameters.AddWithValue("@num_Pago", e.num_Pago);
					command.Parameters.AddWithValue("@fecha", e.fecha);
					command.Parameters.AddWithValue("@importe", e.importe);
					command.Parameters.AddWithValue("@contrato_Id", e.contrato_id);
                    connection.Open();
                    command.ExecuteScalar();
                    connection.Close();
                }                
                string sql_ID = $"SELECT MAX(id_Pagos) AS idUltimo FROM pagos";
                
                using (var command = new MySqlCommand(sql_ID, connection))
                {
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            return res;
        }

        public int Editar(Pagos e)
        {
            var i = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"update pagos set num_Pago=@num_Pago,  fecha=@fecha, importe=@importe WHERE id_Pagos = @id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@num_Pago",e.num_Pago);
					command.Parameters.AddWithValue("@fecha", e.fecha);
					command.Parameters.AddWithValue("@importe", e.importe);
					command.Parameters.AddWithValue("@id", e.id_Pagos);
                    connection.Open();
                    i = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return i;
        }

        public int Borrar(int idcon)
        {
            var i = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"update pagos set estado='0' where id_Pagos = @id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.UInt32);
                    command.Parameters["@id"].Value = idcon;
                    connection.Open();
                    i = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return i;
        }

        public int numSigPago(int id)
        {
            int res = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                //Procedimiento almacenado.
                //retorna el siguiente numero de Paga para el siguiente cobro
                string sql = $"SELECT numSigPago(@id)";

                using (var command = new MySqlCommand(sql, connection))
                {
					command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            
            return res;
        }
    }
}