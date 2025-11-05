using Microsoft.AspNetCore.Mvc;
using GestionBackend.Models;
using GestionBackend.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReservaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReserva([FromBody] Reserva reserva)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Solo usamos los IDs, no los objetos completos
            reserva.Usuario = null;
            reserva.Espacio = null;

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReserva), new { id = reserva.Id }, reserva);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReserva(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Espacio)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
                return NotFound();

            return Ok(reserva);
        }
        // GET /Reserva
[HttpGet]
public async Task<IActionResult> GetAllReservas()
{
    var reservas = await _context.Reservas
        .Include(r => r.Usuario)
        .Include(r => r.Espacio)
        .ToListAsync();
    return Ok(reservas);
}

// PUT /Reserva/{id}
[HttpPut("{id}")]
public async Task<IActionResult> UpdateReserva(int id, [FromBody] Reserva r)
{
    var reserva = await _context.Reservas.FindAsync(id);
    if (reserva == null) return NotFound();

    reserva.IdUsuario = r.IdUsuario;
    reserva.IdEspacio = r.IdEspacio;
    reserva.Fecha = r.Fecha;
    reserva.HoraInicio = r.HoraInicio;
    reserva.DuracionMinutos = r.DuracionMinutos;
    reserva.Estado = r.Estado;

    await _context.SaveChangesAsync();
    return Ok(reserva);
}

// DELETE /Reserva/{id}
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteReserva(int id)
{
    var reserva = await _context.Reservas.FindAsync(id);
    if (reserva == null) return NotFound();

    _context.Reservas.Remove(reserva);
    await _context.SaveChangesAsync();
    return NoContent();
}
    }
}
