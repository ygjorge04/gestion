using Microsoft.AspNetCore.Mvc;
using GestionBackend.Data;
using GestionBackend.Models;
using GestionBackend.Services; // 👈 KafkaProducerService
using Microsoft.EntityFrameworkCore;

namespace GestionBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EspacioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly KafkaProducerService _kafkaProducer;

        public EspacioController(ApplicationDbContext context)
        {
            _context = context;
            _kafkaProducer = new KafkaProducerService(); // inicializamos Kafka
        }

        [HttpGet]
        public async Task<IActionResult> GetEspacios() =>
            Ok(await _context.Espacios.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEspacio(int id)
        {
            var espacio = await _context.Espacios.FindAsync(id);
            return espacio == null ? NotFound("Espacio no encontrado") : Ok(espacio);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEspacio([FromBody] Espacio espacio)
        {
            _context.Espacios.Add(espacio);
            await _context.SaveChangesAsync();

            // Publicar evento Kafka
            await _kafkaProducer.PublishAsync("espacio-creado", espacio);

            return CreatedAtAction(nameof(GetEspacio), new { id = espacio.Id }, espacio);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEspacio(int id, [FromBody] Espacio e)
        {
            var espacio = await _context.Espacios.FindAsync(id);
            if (espacio == null) return NotFound("Espacio no encontrado");

            espacio.Nombre = e.Nombre;
            espacio.Tipo = e.Tipo;
            espacio.Capacidad = e.Capacidad;
            espacio.Ubicacion = e.Ubicacion;
            await _context.SaveChangesAsync();

            // Publicar evento Kafka
            await _kafkaProducer.PublishAsync("espacio-actualizado", espacio);

            return Ok(espacio);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEspacio(int id)
        {
            var espacio = await _context.Espacios.FindAsync(id);
            if (espacio == null) return NotFound("Espacio no encontrado");

            _context.Espacios.Remove(espacio);
            await _context.SaveChangesAsync();

            // Publicar evento Kafka
            await _kafkaProducer.PublishAsync("espacio-eliminado", new { Id = espacio.Id });

            return NoContent();
        }
    }
}
