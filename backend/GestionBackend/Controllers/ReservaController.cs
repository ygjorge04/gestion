using Microsoft.AspNetCore.Mvc;
using GestionBackend.Models;
using GestionBackend.Data;
using GestionBackend.Services; // 👈 agregar esto
using Microsoft.EntityFrameworkCore;
using System.Text.Json; // 👈 para serializar el objeto

namespace GestionBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly KafkaProducerService _kafkaProducer;

        public ReservaController(ApplicationDbContext context)
        {
            _context = context;
            _kafkaProducer = new KafkaProducerService();
        }

        [HttpPost]
        public async Task<IActionResult> CreateReserva([FromBody] Reserva reserva)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var inicioNueva = reserva.Fecha.Date + reserva.HoraInicio;
            var finNueva = inicioNueva.AddMinutes(reserva.DuracionMinutos);

            var existeConflicto = _context.Reservas
                .AsEnumerable()
                .Any(r =>
                    r.IdEspacio == reserva.IdEspacio &&
                    r.Fecha.Date == reserva.Fecha.Date &&
                    (inicioNueva < r.Fecha.Date + r.HoraInicio + TimeSpan.FromMinutes(r.DuracionMinutos) &&
                     finNueva > r.Fecha.Date + r.HoraInicio)
                );

            if (existeConflicto)
                return BadRequest("❌ El espacio ya está reservado en ese horario.");

            reserva.Usuario = null;
            reserva.Espacio = null;

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            // Publicar evento Kafka
            await _kafkaProducer.PublishAsync("reserva-creada", reserva);

            return CreatedAtAction(nameof(GetReserva), new { id = reserva.Id }, reserva);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReserva(int id, [FromBody] Reserva r)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null) return NotFound();

            var inicioNueva = r.Fecha.Date + r.HoraInicio;
            var finNueva = inicioNueva.AddMinutes(r.DuracionMinutos);

            var conflicto = _context.Reservas
                .AsEnumerable()
                .Any(res =>
                    res.Id != id &&
                    res.IdEspacio == r.IdEspacio &&
                    res.Fecha.Date == r.Fecha.Date &&
                    (inicioNueva < res.Fecha.Date + res.HoraInicio + TimeSpan.FromMinutes(res.DuracionMinutos) &&
                     finNueva > res.Fecha.Date + res.HoraInicio)
                );

            if (conflicto)
                return BadRequest("❌ El espacio ya está reservado en ese horario.");

            reserva.IdUsuario = r.IdUsuario;
            reserva.IdEspacio = r.IdEspacio;
            reserva.Fecha = r.Fecha;
            reserva.HoraInicio = r.HoraInicio;
            reserva.DuracionMinutos = r.DuracionMinutos;
            reserva.Estado = r.Estado;

            await _context.SaveChangesAsync();

            // Publicar evento Kafka
            await _kafkaProducer.PublishAsync("reserva-actualizada", reserva);

            return Ok(reserva);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null) return NotFound();

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            // Publicar evento Kafka
            await _kafkaProducer.PublishAsync("reserva-eliminada", new { Id = reserva.Id });

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReserva(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Espacio)
                .FirstOrDefaultAsync(r => r.Id == id);

            return reserva == null ? NotFound() : Ok(reserva);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReservas()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Espacio)
                .ToListAsync();
            return Ok(reservas);
        }
    }
}
