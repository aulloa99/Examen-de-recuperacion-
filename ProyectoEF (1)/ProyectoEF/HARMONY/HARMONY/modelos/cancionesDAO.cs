using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace HARMONY.modelos
{
    public class DaoCanciones
    {
        private string connectionString = basededatosql.connectionString;

     

        public bool InsertarCancion(string nombre, string artista, byte[] imagen)
        {
            string query = "INSERT INTO Canciones (nombre, artista, imagen) VALUES (@Nombre, @Artista, @Imagen)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Artista", artista);
                    cmd.Parameters.AddWithValue("@Imagen", imagen);

                    try
                    {
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al insertar canción: " + ex.Message);
                        return false;
                    }
                }
            }
        }


        public bool ActualizarCancion(int id, string nombre, string artista, byte[] imagen)
        {
            string query = "UPDATE Canciones SET nombre = @Nombre, artista = @Artista, imagen = @Imagen WHERE id = @Id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Artista", artista);
                    cmd.Parameters.AddWithValue("@Imagen", imagen);
                    cmd.Parameters.AddWithValue("@Id", id);

                    try
                    {
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al actualizar canción: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool EliminarCancion(int id)
        {
            string query = "DELETE FROM Canciones WHERE id = @Id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    try
                    {
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al eliminar canción: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public List<Cancion> ObtenerTodasLasCanciones()
        {
            List<Cancion> canciones = new List<Cancion>();
            string query = "SELECT id, nombre, artista, imagen FROM Canciones";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Cancion cancion = new Cancion()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                    Artista = reader.GetString(reader.GetOrdinal("artista")),
                                    Imagen = (byte[])reader["imagen"]
                                };
                                canciones.Add(cancion);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al obtener canciones: " + ex.Message);
                    }
                }
            }
            return canciones;
        }
    }

   
}
