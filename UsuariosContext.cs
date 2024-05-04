using Microsoft.EntityFrameworkCore;

namespace TP_Redes.Models.Context;

public class UsuariosContext : DbContext
{
    public UsuariosContext(DbContextOptions<UsuariosContext> options)
        : base(options)
    {
    }

    public DbSet<UsuariosItem> UsuariosItems { get; set; } = null!;
}