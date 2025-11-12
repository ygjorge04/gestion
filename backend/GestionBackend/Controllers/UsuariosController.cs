using Microsoft.AspNetCore.Mvc;
using GestionBackend.Data;
using GestionBackend.Models;
using GestionBackend.Services; // 👈 KafkaProducerService
using Microsoft.EntityFrameworkCore;

namespace GestionBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly KafkaProducerService _kafkaProducer;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
            _kafkaProducer = new KafkaProducerService(); // inicializamos
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound("Usuario no encontrado");
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUsuario([FromBody] Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Publicar evento Kafka
            await _kafkaProducer.PublishAsync("usuario-creado", usuario);

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] Usuario usuarioActualizado)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound("Usuario no encontrado");

            usuario.Nombre = usuarioActualizado.Nombre;
            usuario.NumeroDoc = usuarioActualizado.NumeroDoc;
            usuario.Contrasena = usuarioActualizado.Contrasena;
            usuario.Rol = usuarioActualizado.Rol;

            await _context.SaveChangesAsync();

            // Publicar evento Kafka
            await _kafkaProducer.PublishAsync("usuario-actualizado", usuario);

            return Ok(usuario);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound("Usuario no encontrado");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            // Publicar evento Kafka
            await _kafkaProducer.PublishAsync("usuario-eliminado", new { Id = usuario.Id });

            return NoContent();
        }
        
    }
    
}
