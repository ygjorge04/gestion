using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionBackend.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("id_espacio")]
        public int IdEspacio { get; set; }

        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public int DuracionMinutos { get; set; }
        public string Estado { get; set; } = "pendiente";

        [ForeignKey("IdUsuario")]
        public Usuario? Usuario { get; set; }

        [ForeignKey("IdEspacio")]
        public Espacio? Espacio { get; set; }
    }
}
