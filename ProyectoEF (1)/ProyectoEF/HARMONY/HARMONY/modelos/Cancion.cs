using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARMONY.modelos
{
    public class Cancion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Artista { get; set; }
        public byte[] Imagen { get; set; }
    }
}
