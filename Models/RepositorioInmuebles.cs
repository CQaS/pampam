using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace InmobiliariaVaras.Models
{
    public class RepositorioInmuebles : RepositorioBase
    {

        public RepositorioInmuebles(IConfiguration configuration) : base(configuration)
        {

        }

        public List<Inmuebles> obtener()
        {
            var res = new List<Inmuebles>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Inmuebles.id_Inm)}, {nameof(Inmuebles.dom_Inm)}, {nameof(Inmuebles.uso)}, {nameof(Inmuebles.tipo)}, {nameof(Inmuebles.ambientes)}, {nameof(Inmuebles.precio)}, {nameof(Inmuebles.prop_Id)}, p.Nombre, {nameof(Inmuebles.imagen)},{nameof(Inmuebles.disponible)} FROM inmuebles i INNER JOIN Propietarios p ON i.prop_Id = p.id_Prop WHERE (SELECT COUNT(c.id_Cont) AS contID FROM Contratos c WHERE c.inm_Id = i.id_Inm AND (c.fecha_fin > CURDATE() AND c.estado = 1)) = 0 AND i.estado = 1";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var b = new Inmuebles
                        {
                            id_Inm = reader.GetInt32(0),
                            dom_Inm = reader.GetString(1),
                            uso = reader.GetString(2),
                            tipo = reader.GetString(3),
                            ambientes = reader.GetInt32(4),
                            precio = reader.GetInt32(5),
                            disponible = reader.GetInt32(9),
                            prop_Id = reader.GetInt32(6),
                            Duenio = new Propietarios
                            {

                                id_Prop = reader.GetInt32(6),
                                nombre = reader.GetString(7),
                            }
                        };
                        res.Add(b);
                    }
                    connection.Close();
                }

            }
            return res;
        }

        public List<Inmuebles> obtenerDisponibles()
        {
            var res = new List<Inmuebles>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Inmuebles.id_Inm)}, {nameof(Inmuebles.dom_Inm)}, {nameof(Inmuebles.uso)}, {nameof(Inmuebles.tipo)}, {nameof(Inmuebles.ambientes)}, {nameof(Inmuebles.precio)}, {nameof(Inmuebles.prop_Id)}, p.Nombre, {nameof(Inmuebles.imagen)},{nameof(Inmuebles.disponible)} FROM inmuebles i INNER JOIN Propietarios p ON i.prop_Id = p.id_Prop WHERE (SELECT COUNT(c.id_Cont) AS contID FROM Contratos c WHERE c.inm_Id = i.id_Inm AND (c.fecha_fin > CURDATE() AND c.estado = 1)) = 0 AND i.estado = 1 AND i.disponible = 1";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var b = new Inmuebles
                        {
                            id_Inm = reader.GetInt32(0),
                            dom_Inm = reader.GetString(1),
                            uso = reader.GetString(2),
                            tipo = reader.GetString(3),
                            ambientes = reader.GetInt32(4),
                            precio = reader.GetInt32(5),
                            disponible = reader.GetInt32(9),
                            prop_Id = reader.GetInt32(6),
                            Duenio = new Propietarios
                            {

                                id_Prop = reader.GetInt32(6),
                                nombre = reader.GetString(7),
                            }
                        };
                        res.Add(b);
                    }
                    connection.Close();
                }

            }
            return res;
        }


        public Inmuebles Buscar(int id)
        {
            Inmuebles Inm;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Inmuebles.id_Inm)}, {nameof(Inmuebles.dom_Inm)}, {nameof(Inmuebles.uso)}, {nameof(Inmuebles.tipo)}, {nameof(Inmuebles.ambientes)}, {nameof(Inmuebles.precio)}, {nameof(Inmuebles.prop_Id)}, {nameof(Inmuebles.imagen)}, {nameof(Inmuebles.disponible)} FROM inmuebles where id_inm = @id_Inm AND estado = 1";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id_Inm", MySqlDbType.Int32).Value = id;
                    connection.Open();
                    MySqlDataReader res = command.ExecuteReader();
                    Inm = new Inmuebles();
                    if (res.Read())
                    {
                        Inm.id_Inm = int.Parse(res["id_Inm"].ToString());
                        Inm.dom_Inm = res["dom_Inm"].ToString();
                        Inm.uso = res["uso"].ToString();
                        Inm.tipo = res["tipo"].ToString();
                        Inm.ambientes = int.Parse(res["ambientes"].ToString()); ;
                        Inm.precio = int.Parse(res["precio"].ToString());
                        Inm.prop_Id = int.Parse(res["prop_Id"].ToString());
                        Inm.imagen = res["imagen"].ToString();
                        Inm.disponible = int.Parse(res["disponible"].ToString());
                    }
                    connection.Close();
                }
            }
            return Inm;
        }

        public int Alta(Inmuebles b)
        {
            var res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"INSERT INTO inmuebles (dom_Inm, uso, tipo, ambientes, precio, prop_Id, imagen) VALUES (@dom_Inm, @uso, @tipo, @ambientes, @precio, @prop_Id, @imagen)";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("@dom_Inm", b.dom_Inm);
                    command.Parameters.AddWithValue("@uso", b.uso);
                    command.Parameters.AddWithValue("@tipo", b.tipo);
                    command.Parameters.AddWithValue("@ambientes", b.ambientes);
                    command.Parameters.AddWithValue("@precio", b.precio);
                    command.Parameters.AddWithValue("@prop_Id", b.prop_Id);
                    command.Parameters.AddWithValue("@imagen", b.imagen);
                    connection.Open();
                    command.ExecuteScalar();
                    connection.Close();
                }

                string sql_ID = $"SELECT MAX(id_Inm) AS idMax FROM inmuebles";
                using (var command = new MySqlCommand(sql_ID, connection))
                {
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            return res;
        }

        public int Editar(Inmuebles b)
        {
            var i = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"update inmuebles set dom_Inm=@dom_Inm, uso=@uso, tipo=@tipo, ambientes=@ambientes, precio=@precio, imagen=@imagen, prop_Id=@prop_id where id_Inm=@id_Inm";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@dom_Inm", MySqlDbType.VarChar);
                    command.Parameters["@dom_Inm"].Value = b.dom_Inm;
                    command.Parameters.Add("@uso", MySqlDbType.VarChar);
                    command.Parameters["@uso"].Value = b.uso;
                    command.Parameters.Add("@tipo", MySqlDbType.VarChar);
                    command.Parameters["@tipo"].Value = b.tipo;
                    command.Parameters.Add("@ambientes", MySqlDbType.VarChar);
                    command.Parameters["@ambientes"].Value = b.ambientes;
                    command.Parameters.Add("@precio", MySqlDbType.VarChar);
                    command.Parameters["@precio"].Value = b.precio;
                    command.Parameters.Add("@imagen", MySqlDbType.VarChar);
                    command.Parameters["@imagen"].Value = b.imagen;
                    command.Parameters.Add("@prop_id", MySqlDbType.VarChar);
                    command.Parameters["@prop_id"].Value = b.prop_Id;

                    command.Parameters.Add("@id_Inm", MySqlDbType.VarChar);
                    command.Parameters["@id_Inm"].Value = b.id_Inm;
                    connection.Open();
                    i = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return i;
        }

        public int Borrar(int idIn)
        {

            var b = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"update inmuebles set estado='0' where id_Inm = @id_Inm";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id_Inm", MySqlDbType.UInt32);
                    command.Parameters["@id_Inm"].Value = idIn;
                    connection.Open();
                    b = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return b;
        }

        public int Disponible(int idIn)
        {
            var D = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"UPDATE inmuebles SET disponible=(SELECT IF((SELECT disponible FROM inmuebles WHERE id_Inm = @id_Inm)= 1, 0, 1)) WHERE id_Inm = @id_Inm";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id_Inm", MySqlDbType.UInt32);
                    command.Parameters["@id_Inm"].Value = idIn;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                string sql_ID = $"SELECT disponible FROM inmuebles WHERE id_Inm = @id_Dis";
                using (var command = new MySqlCommand(sql_ID, connection))
                {
                    command.Parameters.Add("@id_Dis", MySqlDbType.UInt32);
                    command.Parameters["@id_Dis"].Value = idIn;
                    connection.Open();
                    D = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }

            return D;
        }

        public IList<Inmuebles> BuscarInmueblesDisponibles(BuscarPorFecha f)
        {
            var res = new List<Inmuebles>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT i.id_Inm, i.dom_Inm, i.uso, i.tipo, i.ambientes, i.precio, i.prop_Id, p.nombre, i.disponible FROM Inmuebles i JOIN Propietarios p ON i.prop_Id = p.id_Prop WHERE (SELECT COUNT(c.id_Cont) AS contID FROM Contratos c WHERE c.inm_Id = i.id_Inm AND ((c.fecha_In BETWEEN @inicio AND @fin) OR (c.fecha_fin BETWEEN @inicio AND @fin) OR (c.fecha_In < @inicio AND c.fecha_fin > @fin))) = 0 AND i.estado = 1 AND i.disponible = 1";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@inicio", MySqlDbType.Date).Value = f.FechaInicio;
                    command.Parameters.Add("@fin", MySqlDbType.Date).Value = f.FechaFin;
                    command.CommandType = System.Data.CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var e = new Inmuebles
                        {
                            id_Inm = reader.GetInt32(0),
                            dom_Inm = reader.GetString(1),
                            uso = reader.GetString(2),
                            tipo = reader.GetString(3),
                            ambientes = reader.GetInt32(4),
                            precio = reader.GetInt32(5),
                            disponible = reader.GetInt32(8),
                            prop_Id = reader.GetInt32(6),
                            Duenio = new Propietarios
                            {

                                id_Prop = reader.GetInt32(6),
                                nombre = reader.GetString(7),
                            }
                        };
                        res.Add(e);
                    }
                    connection.Close();
                }
            }
            return res;

        }

        public List<Inmuebles> obtenerPorPropietario(int id)
        {
            var res = new List<Inmuebles>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(Inmuebles.id_Inm)}, {nameof(Inmuebles.dom_Inm)}, {nameof(Inmuebles.uso)}, {nameof(Inmuebles.tipo)}, {nameof(Inmuebles.ambientes)}, {nameof(Inmuebles.precio)}, {nameof(Inmuebles.imagen)} FROM inmuebles WHERE prop_Id = @id AND estado = 1";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var e = new Inmuebles
                        {
                            id_Inm = reader.GetInt32(0),
                            dom_Inm = reader.GetString(1),
                            uso = reader.GetString(2),
                            tipo = reader.GetString(3),
                            ambientes = reader.GetInt32(4),
                            precio = reader.GetInt32(5),
                            imagen = reader.GetString(6),
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