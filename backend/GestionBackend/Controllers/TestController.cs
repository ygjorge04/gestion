using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace GestionBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("db-connection")]
        public IActionResult TestDbConnection()
        {
            var connectionString = "Server=tcp:sqlserver,1433;Database=Gestion;User Id=sa;Password=Abc123.!;Encrypt=False;TrustServerCertificate=True";

            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                return Ok("✅ Conexión exitosa a SQL Server desde Docker!");
            }
            catch (Exception ex)
            {
                return BadRequest($"❌ Error de conexión: {ex.Message}");
            }
        }
    }
}
