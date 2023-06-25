using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;


namespace InmobiliariaVaras.Models
{
    public class RepositorioPropietarios : RepositorioBase
    {
        public RepositorioPropietarios(IConfiguration configuration): base(configuration)
        {
            
        }

        public List<Propietarios> obtener()
        { 
              //lista nueva de propietarios
            var res = new List<Propietarios>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Propietarios.id_Prop)}, {nameof(Propietarios.dni)}, {nameof(Propietarios.nombre)}, {nameof(Propietarios.dom_Prop)}, {nameof(Propietarios.tel)} FROM Propietarios where estado = 1";
                
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    //recorre los datos de la consulta...
                    while (reader.Read())
                    {
                        var e = new Propietarios
                        {
                            id_Prop = reader.GetInt32(0),
                            dni= reader.GetInt32(1),
                            nombre = reader.GetString(2),
                            dom_Prop = reader.GetString(3),
                            tel = reader.GetInt32(4),
                            
                        };
                        //agrega un proietario ala lista
                        res.Add(e);
                    }
                    connection.Close();
                }

            }
            return res;
        }
        public Propietarios Buscar(int id)
        {
            Propietarios Pro;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Propietarios.id_Prop)}, {nameof(Propietarios.dni)}, {nameof(Propietarios.nombre)}, {nameof(Propietarios.dom_Prop)}, {nameof(Propietarios.tel)} FROM Propietarios where id_Prop=@id_Prop and estado = 1";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id_Prop", MySqlDbType.Int32).Value = id;
                    connection.Open();
                    MySqlDataReader res = command.ExecuteReader();
                    Pro = new Propietarios();
                    if(res.Read())
                    {
                        Pro.id_Prop= int.Parse(res["id_Prop"].ToString());
                        Pro.dni = int.Parse(res["dni"].ToString());
                        Pro.nombre = res["nombre"].ToString();
                        Pro.dom_Prop = res["dom_Prop"].ToString();
                        Pro.tel = int.Parse(res["tel"].ToString());
                    }
                    connection.Close();                                       
                }
            }
            return Pro;
        }

        public int Alta(Propietarios p)
        {
            var res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Propietarios(dni, nombre, dom_Prop, tel) VALUES (@dni_Pro,@nombre,@dom_Prop,@tel)";
                
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@dni_Pro", MySqlDbType.Int32);
                    command.Parameters.Add("@nombre", MySqlDbType.VarChar);
                    command.Parameters.Add("@dom_Prop", MySqlDbType.VarChar);
                    command.Parameters.Add("@tel", MySqlDbType.Int32);
                    
                    command.Parameters["@dni_Pro"].Value = p.dni;
                    command.Parameters["@nombre"].Value = p.nombre;
                    command.Parameters["@dom_Prop"].Value = p.dom_Prop;
                    command.Parameters["@tel"].Value = p.tel;
                   
                    connection.Open();
                    command.ExecuteScalar();
                    connection.Close();
                }
                
                string sql_ID = $"SELECT MAX(id_Prop) AS id FROM Propietarios"; // último id insertado
 
                
                using (var command = new MySqlCommand(sql_ID, connection))
                {
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            return res;
        }

        public int Editar(Propietarios p)
        {
            var e = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"update Propietarios set dni=@dni, nombre=@nombre, dom_Prop=@dom_Prop, tel=@tel where id_Prop=@id_Prop";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@dni", MySqlDbType.VarChar);
                    command.Parameters["@dni"].Value = p.dni;
                    command.Parameters.Add("@nombre", MySqlDbType.VarChar);
                    command.Parameters["@nombre"].Value = p.nombre;
                    command.Parameters.Add("@dom_Prop", MySqlDbType.VarChar);
                    command.Parameters["@dom_Prop"].Value = p.dom_Prop;
                    command.Parameters.Add("@tel", MySqlDbType.VarChar);
                    command.Parameters["@tel"].Value = p.tel;
                    
                    command.Parameters.Add("@id_Prop", MySqlDbType.VarChar);
                    command.Parameters["@id_Prop"].Value = p.id_Prop;
                    connection.Open();
                    e = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return e;
        }

        public int Borrar(int id_Prop)
        {
            var e = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"UPDATE Propietarios SET estado = '0' WHERE id_Prop = @id_Prop";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id_Prop", MySqlDbType.UInt32);
                    command.Parameters["@id_Prop"].Value = id_Prop;
                    connection.Open();
                    e = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return e;
        }
    }

}