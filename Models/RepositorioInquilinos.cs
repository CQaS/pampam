using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;


namespace InmobiliariaVaras.Models
{
    public class RepositorioInquilinos : RepositorioBase{
        public RepositorioInquilinos(IConfiguration configuration): base(configuration)
        {
            
        }

        public List<Inquilinos> obtener()
        {   
            var res = new List<Inquilinos>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Inquilinos.id_Inq)}, {nameof(Inquilinos.dni_Inq)}, {nameof(Inquilinos.nombre_Inq)}, {nameof(Inquilinos.email)}, {nameof(Inquilinos.dom_Inq)}, {nameof(Inquilinos.tel_Inq)}, {nameof(Inquilinos.domicilio_Lab)}, {nameof(Inquilinos.nombre_Garante)}, {nameof(Inquilinos.dni_Garante)}, {nameof(Inquilinos.tel_Garante)} FROM Inquilinos where estado = 1";
                
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var e = new Inquilinos
                        {
                            id_Inq = reader.GetInt32(0),
                            dni_Inq = reader.GetInt32(1),
                            nombre_Inq = reader.GetString(2),
                            email = reader.GetString(3),
                            dom_Inq = reader.GetString(4),
                            tel_Inq = reader.GetInt32(5),
                            domicilio_Lab = reader.GetString(6),
                            nombre_Garante = reader.GetString(7),
                            dni_Garante = reader.GetInt32(8),
                            tel_Garante = reader.GetInt32(9),
                        };
                        res.Add(e);
                    }
                    connection.Close();
                }

            }
            return res;
        }
        
        public List<Inquilinos> search(string Name)
        {   
            var res = new List<Inquilinos>();
            Name = "%" + Name + "%";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Inquilinos.id_Inq)}, {nameof(Inquilinos.dni_Inq)}, {nameof(Inquilinos.nombre_Inq)}, {nameof(Inquilinos.email)}, {nameof(Inquilinos.dom_Inq)}, {nameof(Inquilinos.tel_Inq)}, {nameof(Inquilinos.domicilio_Lab)}, {nameof(Inquilinos.nombre_Garante)}, {nameof(Inquilinos.dni_Garante)}, {nameof(Inquilinos.tel_Garante)} FROM Inquilinos WHERE estado = 1 AND nombre_Inq LIKE @name";
                
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = Name;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var e = new Inquilinos
                        {
                            id_Inq = reader.GetInt32(0),
                            dni_Inq = reader.GetInt32(1),
                            nombre_Inq = reader.GetString(2),
                            email = reader.GetString(3),
                            dom_Inq = reader.GetString(4),
                            tel_Inq = reader.GetInt32(5),
                            domicilio_Lab = reader.GetString(6),
                            nombre_Garante = reader.GetString(7),
                            dni_Garante = reader.GetInt32(8),
                            tel_Garante = reader.GetInt32(9),
                        };
                        res.Add(e);
                    }
                    connection.Close();
                }

            }
            return res;
        }

        public Inquilinos Buscar(int id)
        {
            Inquilinos Inq;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Inquilinos.id_Inq)}, {nameof(Inquilinos.dni_Inq)}, {nameof(Inquilinos.nombre_Inq)}, {nameof(Inquilinos.email)}, {nameof(Inquilinos.dom_Inq)}, {nameof(Inquilinos.tel_Inq)}, {nameof(Inquilinos.domicilio_Lab)}, {nameof(Inquilinos.nombre_Garante)}, {nameof(Inquilinos.dni_Garante)}, {nameof(Inquilinos.tel_Garante)}  FROM Inquilinos where id_inq=@id_Inq and estado = 1";
                
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id_Inq", MySqlDbType.Int32).Value = id;
                    connection.Open();
                    MySqlDataReader res = command.ExecuteReader();
                    Inq = new Inquilinos();
                    if(res.Read())
                    {
                        Inq.id_Inq = int.Parse(res["id_Inq"].ToString());
                        Inq.dni_Inq = int.Parse(res["dni_inq"].ToString());
                        Inq.nombre_Inq = res["nombre_inq"].ToString();
                        Inq.email = res["email"].ToString();
                        Inq.dom_Inq = res["dom_Inq"].ToString();
                        Inq.tel_Inq = int.Parse(res["tel_inq"].ToString());
                        Inq.domicilio_Lab = res["domicilio_Lab"].ToString();
                        Inq.nombre_Garante = res["nombre_Garante"].ToString();
                        Inq.dni_Garante = int.Parse(res["dni_Garante"].ToString());
                        Inq.tel_Garante = int.Parse(res["tel_Garante"].ToString());
                    }
                    connection.Close();                                       
                }
            }
            return Inq;
        }

        public int Alta(Inquilinos i)
        {
            var res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Inquilinos(dni_Inq, nombre_Inq, email, dom_Inq, tel_Inq, domicilio_Lab, nombre_Garante, dni_Garante, tel_Garante) VALUES (@dni_Inq,@nombre_Inq,@email,@dom_Inq,@tel_Inq, @domicilio_Lab, @nombre_Garante, @dni_Garante,@tel_Garante)";
                
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@dni_Inq", MySqlDbType.Int32);
                    command.Parameters.Add("@nombre_Inq", MySqlDbType.VarChar);
                    command.Parameters.Add("@email", MySqlDbType.VarChar);
                    command.Parameters.Add("@dom_Inq", MySqlDbType.VarChar);
                    command.Parameters.Add("@tel_Inq", MySqlDbType.Int32);
                    command.Parameters.Add("@domicilio_Lab", MySqlDbType.VarChar);
                    command.Parameters.Add("@nombre_Garante", MySqlDbType.VarChar);
                    command.Parameters.Add("@dni_Garante", MySqlDbType.Int32);
                    command.Parameters.Add("@tel_Garante", MySqlDbType.Int32);
                    command.Parameters["@dni_Inq"].Value = i.dni_Inq;
                    command.Parameters["@nombre_Inq"].Value = i.nombre_Inq;
                    command.Parameters["@email"].Value = i.email;
                    command.Parameters["@dom_Inq"].Value = i.dom_Inq;
                    command.Parameters["@tel_Inq"].Value = i.tel_Inq;
                    command.Parameters["@domicilio_Lab"].Value = i.domicilio_Lab;
                    command.Parameters["@nombre_Garante"].Value = i.nombre_Garante;
                    command.Parameters["@dni_Garante"].Value = i.dni_Garante;
                    command.Parameters["@tel_Garante"].Value = i.tel_Garante;
                    connection.Open();
                    command.ExecuteScalar();
                    connection.Close();
                }
                
                string sql_ID = $"SELECT MAX(id) AS id FROM Inquilinos";
                
                using (var command = new MySqlCommand(sql_ID, connection))
                {
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            return res;
        }

        public int Editar(Inquilinos i)
        {
            var e = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"update Inquilinos set dni_Inq=@dni_Inq, nombre_Inq=@nombre_Inq, email=@email, dom_Inq=@dom_Inq, tel_Inq=@tel_Inq, domicilio_Lab=@domicilio_Lab, nombre_Garante=@nombre_Garante, dni_Garante=@dni_Garante, tel_Garante=@tel_Garante where id_Inq=@id_Inq";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@dni_Inq", MySqlDbType.VarChar);
                    command.Parameters["@dni_Inq"].Value = i.dni_Inq;
                    command.Parameters.Add("@nombre_Inq", MySqlDbType.VarChar);
                    command.Parameters["@nombre_Inq"].Value = i.nombre_Inq;
                    command.Parameters.Add("@email", MySqlDbType.VarChar);
                    command.Parameters["@email"].Value = i.email;
                    command.Parameters.Add("@dom_Inq", MySqlDbType.VarChar);
                    command.Parameters["@dom_Inq"].Value = i.dom_Inq;
                    command.Parameters.Add("@tel_Inq", MySqlDbType.VarChar);
                    command.Parameters["@tel_Inq"].Value = i.tel_Inq;
                    command.Parameters.Add("@domicilio_Lab", MySqlDbType.VarChar);
                    command.Parameters["@domicilio_Lab"].Value = i.domicilio_Lab;
                    command.Parameters.Add("@nombre_Garante", MySqlDbType.VarChar);
                    command.Parameters["@nombre_Garante"].Value = i.nombre_Garante;
                    command.Parameters.Add("@dni_Garante", MySqlDbType.VarChar);
                    command.Parameters["@dni_Garante"].Value = i.dni_Garante;
                    command.Parameters.Add("@tel_Garante", MySqlDbType.VarChar);
                    command.Parameters["@tel_Garante"].Value = i.tel_Garante;
                    command.Parameters.Add("@id_Inq", MySqlDbType.VarChar);
                    command.Parameters["@id_Inq"].Value = i.id_Inq;
                    connection.Open();
                    e = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return e;
        }

        public int Borrar(int id_Inq)
        {
            var e = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"UPDATE Inquilinos SET estado = '0' WHERE id_Inq = @id_Inq";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id_Inq", MySqlDbType.UInt32);
                    command.Parameters["@id_Inq"].Value = id_Inq;
                    connection.Open();
                    e = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return e;
        }
    }

}