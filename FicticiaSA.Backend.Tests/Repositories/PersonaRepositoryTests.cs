using FicticiaBackend.Data;
using FicticiaBackend.Models;
using FicticiaBackend.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;

namespace FicticiaSA.Backend.Tests.Repositories
{
    public class PersonaRepositoryTests
    {
        private FicticiaContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<FicticiaContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new FicticiaContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddPersona()
        {
            var context = GetInMemoryDbContext();
            var repo = new PersonaRepository(context);

            var persona = new Persona
            {
                NombreCompleto = "Test Persona",
                Identificacion = "12345",
                Edad = 30,
                Genero = "Masculino",
                EstadoActivo = true,
                Maneja = false,
                UsaLentes = false,
                Diabetico = false,
                EnfermedadOtra = null
            };

            await repo.AddAsync(persona);

            var personas = await repo.GetAllAsync();
            personas.Should().ContainSingle(p => p.NombreCompleto == "Test Persona");
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemovePersona()
        {
            var context = GetInMemoryDbContext();
            var repo = new PersonaRepository(context);

            var persona = new Persona
            {
                NombreCompleto = "Eliminar Persona",
                Identificacion = "54321",
                Edad = 40,
                Genero = "Femenino",
                EstadoActivo = true,
                Maneja = true,
                UsaLentes = false,
                Diabetico = false,
                EnfermedadOtra = null
            };

            // Agregar la persona (EF Core genera el Id automáticamente)
            await repo.AddAsync(persona);

            // Borrar usando el Id generado automáticamente
            await repo.DeleteAsync(persona.Id);

            // Verificar que la tabla está vacía
            var personas = await repo.GetAllAsync();
            personas.Should().BeEmpty();
        }

    }
}
