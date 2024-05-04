using TP_Redes.Models;

namespace TP_Redes
{
    public class Usuarios
    {
        public int id { get; set; }
        public string login { get; set; }
        public string senha { get; set; }
        public string nome { get; set; }
        public int roleId { get; set; }
    }
}
