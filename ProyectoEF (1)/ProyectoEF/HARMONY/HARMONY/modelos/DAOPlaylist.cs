using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace HARMONY.modelos
{
    public class DaoPlaylist
    {
        private string connectionString = basededatosql.connectionString;

        

        public bool InsertarPlaylist(string nombre, int idUsuario)
        {
            string sql = "INSERT INTO Playlists (nombre, id_usuario) VALUES (@Nombre, @IdUsuario)";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("playlist creada correctamente");
                            return true;
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Error al insertar playlist: " + ex.Message);
                    }
                }
            }

            return false;
        }

        public void AgregarCancionAPlaylist(int idPlaylist, int idCancion)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO Playlists_Canciones (id_playlist, id_cancion) VALUES (@idPlaylist, @idCancion)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idPlaylist", idPlaylist);
                    command.Parameters.AddWithValue("@idCancion", idCancion);

                    connection.Open();
                    
                    int filas  = command.ExecuteNonQuery();
                    if (filas > 0)
                    {
                        MessageBox.Show("cancion agregada");
                    }
                }
                  
            }
        }
        public List<Cancion> ObtenerCancionesDePlaylist(int idPlaylist)
        {
            List<Cancion> canciones = new List<Cancion>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT c.id, c.nombre, c.artista FROM Canciones c " +
                               "JOIN Playlists_Canciones pc ON c.id = pc.id_cancion " +
                               "WHERE pc.id_playlist = @idPlaylist";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@idPlaylist", idPlaylist);

                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Cancion cancion = new Cancion
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Artista = reader.GetString(2)
                    };
                    canciones.Add(cancion);
                }
                reader.Close();
            }
            return canciones;
        }
    
    public List<Playlist> ObtenerPlaylistsPorUsuario(int idUsuario)
        {
            List<Playlist> playlists = new List<Playlist>();


            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                string consulta = "SELECT * FROM playlists WHERE id_usuario = @IdUsuario";

                using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    try
                    {
                        using (MySqlDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Playlist playlist = new Playlist()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                    IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario"))
                                };
                                
                                playlists.Add(playlist);
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine("Error al obtener playlists por usuario: " + ex.Message);
                    }catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            return playlists;
        }

        public bool EliminarPlaylist(int idPlaylist)
        {
            string sql = "DELETE FROM Playlists WHERE id = @IdPlaylist";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdPlaylist", idPlaylist);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return true;
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Error al eliminar playlist: " + ex.Message);
                    }
                }
            }

            return false;
        }
    }
}
