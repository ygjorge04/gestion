using Xunit;
using Microsoft.EntityFrameworkCore;
using GestionBackend.Data;
using GestionBackend.Controllers;
using GestionBackend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestionBackend.Tests
{
    public class UsuariosControllerTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateUsuario_ShouldAddUsuario()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new UsuarioController(context);
            var nuevoUsuario = new Usuario { Nombre = "Juan", NumeroDoc = "123", Contrasena = "abc" };

            // Act
            var result = await controller.CreateUsuario(nuevoUsuario);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);

            // Assert
            var usuario = Assert.IsType<Usuario>(createdResult.Value);
            Assert.Equal("Juan", usuario.Nombre);
            Assert.Single(context.Usuarios);
        }

        [Fact]
        public async Task GetUsuario_ShouldReturnNotFound_WhenNotExists()
        {
            var context = GetInMemoryDbContext();
            var controller = new UsuarioController(context);

            var result = await controller.GetUsuario(999);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
