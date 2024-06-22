using System;
using MySql.Data.MySqlClient;

namespace HARMONY.modelos
{
    public class DaoAutenticacion
    {
        private string connectionString;

        public DaoAutenticacion()
        {
            connectionString = basededatosql.connectionString;
        }


        public bool RegistrarUsuario(string username, string password)
        {
            string query = "INSERT INTO usuarios (usuario, contrasena) VALUES (@Username, @Password)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    try
                    {
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al registrar usuario: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public Usuario Login(string username, string password)
        {
            string query = "SELECT id, usuario FROM Usuarios WHERE usuario = @Username AND contrasena = @Password";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Usuario usuario1 = new Usuario()
                                {
                                       id = reader.GetInt32(reader.GetOrdinal("id")),
                                       usuario = reader.GetString(reader.GetOrdinal("usuario"))
                                };
                               
                                return usuario1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al intentar iniciar sesión: " + ex.Message);
                        return null;
                    }
                }
            }
            return null;
        }
    
}
}

