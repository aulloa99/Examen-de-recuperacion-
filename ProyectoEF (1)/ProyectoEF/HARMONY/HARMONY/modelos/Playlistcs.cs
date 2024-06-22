namespace HARMONY.modelos
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int IdUsuario { get; set; }


        public override string ToString()
        {
            return Nombre; 
        }
    }
}
