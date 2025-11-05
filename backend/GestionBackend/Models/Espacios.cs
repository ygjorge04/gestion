namespace GestionBackend.Models
{
    public class Espacio
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int Capacidad { get; set; }
        public string? Ubicacion { get; set; }
    }
}