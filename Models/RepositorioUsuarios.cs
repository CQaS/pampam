using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
 
namespace InmobiliariaVaras.Models
{
    public class RepositorioUsuarios : RepositorioBase
	{
		public RepositorioUsuarios(IConfiguration configuration) : base(configuration)
		{

		}

        public int Alta(Usuarios e)
		{
			string avatarDefault = "/img/default.jpg";
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Usuarios (nombre, apellido, avatar, email, contraseña, pregunta, rol) VALUES (@nombre, @apellido, @avatar, @email, @contraseña, @pregunta, @rol)";

				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", e.nombre);
					command.Parameters.AddWithValue("@apellido", e.apellido);
					if(String.IsNullOrEmpty(e.avatar))
							command.Parameters.AddWithValue("@avatar", avatarDefault);
					else
                    	command.Parameters.AddWithValue("@avatar", e.avatar);
					command.Parameters.AddWithValue("@email", e.email);
					command.Parameters.AddWithValue("@contraseña", e.contraseña);
					command.Parameters.AddWithValue("@pregunta", e.pregunta);
					command.Parameters.AddWithValue("@rol", e.rol);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					e.id_Us = res;
					connection.Close();
				}

                string sql_ID = $"SELECT MAX(id_Us) AS id FROM Usuarios";
                
                using (var command = new MySqlCommand(sql_ID, connection))
                {
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
			}
			return res;
		}

		public int Baja(int id)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"UPDATE Usuarios SET estado = 0 WHERE id_Us = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public int agregaAvatar(Usuarios e)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"UPDATE Usuarios SET nombre=@nombre, apellido=@apellido, avatar=@avatar, rol=@rol WHERE id_Us = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", e.nombre);
					command.Parameters.AddWithValue("@apellido", e.apellido);
					command.Parameters.AddWithValue("@avatar", e.avatar);
					command.Parameters.AddWithValue("@rol", e.rol);
					command.Parameters.AddWithValue("@id", e.id_Us);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public int Modifica(Usuarios e)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				
				string sql = "UPDATE Usuarios SET nombre=@nombre, apellido=@apellido, contraseña=@contraseña, rol=@rol, avatar=@avatar WHERE id_Us = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", e.nombre);
					command.Parameters.AddWithValue("@apellido", e.apellido);
					command.Parameters.AddWithValue("@rol", e.rol);
					command.Parameters.AddWithValue("@contraseña", e.contraseña);
					command.Parameters.AddWithValue("@avatar", e.avatar);
					command.Parameters.AddWithValue("@id", e.id_Us);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public int ModificarPass(string m, string p)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"UPDATE Usuarios SET contraseña=@contraseña WHERE email = @email";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
			
					command.Parameters.AddWithValue("@contraseña", p);
					command.Parameters.AddWithValue("@email", m);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Usuarios> ObtenerTodos()
		{
			var res = new List<Usuarios>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT id_Us, nombre, apellido, avatar, email, contraseña, rol FROM Usuarios WHERE estado = 1";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Usuarios e = new Usuarios
						{
							id_Us = reader.GetInt32(0),
							nombre = reader.GetString(1),
							apellido = reader.GetString(2),							
                            avatar = reader.GetString(3),
							email = reader.GetString(4),
							contraseña = reader.GetString(5),
							rol = reader.GetInt32(6),
						};
						res.Add(e);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Usuarios ObtenerPorId(int id)
		{
			Usuarios e = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT id_Us, nombre, apellido, avatar, email, contraseña, rol FROM Usuarios WHERE id_Us=@id AND estado = 1";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						e = new Usuarios
						{
							id_Us = reader.GetInt32(0),
							nombre = reader.GetString(1),
							apellido = reader.GetString(2),							
                            avatar = reader.GetString(3),
							email = reader.GetString(4),
							contraseña = reader.GetString(5),
							rol = reader.GetInt32(6),
						};
					}
					connection.Close();
				}
			}
			return e;
		}

		public Usuarios ObtenerPorEmail(string email)
		{
			Usuarios e = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT id_Us, nombre, apellido, avatar, email, contraseña, rol, pregunta FROM Usuarios WHERE email=@email";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = System.Data.CommandType.Text;
					command.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						e = new Usuarios
						{
							id_Us = reader.GetInt32(0),
							nombre = reader.GetString(1),
							apellido = reader.GetString(2),
							avatar = reader.GetString(3),
							email = reader.GetString(4),
							contraseña = reader.GetString(5),
							rol = reader.GetInt32(6),
							pregunta = reader.GetString(7),
						};
					}
					connection.Close();
				}
			}
			return e;
		}   
    }
}