using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace PetShop
{
    class Usuario
    {
        public int id { get; set; }
        public string? nome { get; set; }
        public string? email { get; set; }
    }

    class MinhaBase : DbContext
    {
        public MinhaBase(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; } = null!;
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("Usuarios") ?? "Data Source=Usuarios.db";
            builder.Services.AddSqlite<MinhaBase>(connectionString);

            var app = builder.Build();

            // Listar todos os usuarios
            app.MapGet("/usuarios", (MinhaBase minhaBase) =>
            {
                return minhaBase.Usuarios.ToList();
            });

            // Listar usuario especifico (por id)
            app.MapGet("/usuario/{id}", (MinhaBase minhaBase, int id) =>
            {
                return minhaBase.Usuarios.Find(id);
            });

            // Cadastrar usuario
            app.MapPost("/cadastrar", (MinhaBase minhaBase, Usuario usuario) =>
            {
                minhaBase.Usuarios.Add(usuario);
                minhaBase.SaveChanges();
                return "Usuario adicionado";
            });

            // Atualizar usuario (usando PUT)
            app.MapPut("/usuario/{id}", (MinhaBase minhaBase, Usuario usuarioAtualizado, int id) =>
            {
                var usuario = minhaBase.Usuarios.Find(id);
                if (usuario != null)
                {
                    usuario.nome = usuarioAtualizado.nome;
                    usuario.email = usuarioAtualizado.email;
                    minhaBase.SaveChanges();
                    return "Usuario atualizado";
                }
                else
                {
                    return "Usuario não encontrado";
                }
            });

            // Deletar usuario (usando DELETE)
            app.MapDelete("/usuario/{id}", (MinhaBase minhaBase, int id) =>
            {
                var usuario = minhaBase.Usuarios.Find(id);
                if (usuario != null)
                {
                    minhaBase.Usuarios.Remove(usuario);
                    minhaBase.SaveChanges();
                    return "Usuario removido";
                }
                else
                {
                    return "Usuario não encontrado";
                }
            });

            app.Run();
        }
    }
}
