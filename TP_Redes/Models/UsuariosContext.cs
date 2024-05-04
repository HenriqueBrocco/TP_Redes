using Microsoft.EntityFrameworkCore;

namespace TP_Redes.Models;

public class UsuariosContext : DbContext
{
    public UsuariosContext(DbContextOptions<UsuariosContext> options)
        : base(options)
    {
    }

    public DbSet<Usuarios> UsuariosItems { get; set; } = null!;
}