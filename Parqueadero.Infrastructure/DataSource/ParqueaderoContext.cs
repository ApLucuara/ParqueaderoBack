using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Parqueadero.Infrastructure.DataSource.ModelConfig;

namespace Parqueadero.Infrastructure.DataSource;

public partial class ParqueaderoContext : DbContext
{
    private readonly IConfiguration _config;
    public ParqueaderoContext(DbContextOptions<ParqueaderoContext> options, IConfiguration config) : base(options)
    {
        _config = config;
    }

    public async Task CommitAsync()
    {
        await SaveChangesAsync().ConfigureAwait(false);
    }

    //public virtual DbSet<Domain.Entities.Parametrizacion> Parametrizacions { get; set; }
    //public virtual DbSet<Domain.Entities.Parqueadero> Parqueaderos { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.ApplyConfiguration(new ParqueaderoConfig());
        //modelBuilder.ApplyConfiguration(new ParametrizacionConfig());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ParqueaderoContext).Assembly);

        //if (modelBuilder is null)
        //{
        //    return;
        //}
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ParqueaderoContext).Assembly);
        //modelBuilder.Entity<Domain.Entities.Parqueadero>();
        //modelBuilder.Entity<Domain.Entities.Parametrizacion>();
        //base.OnModelCreating(modelBuilder);
    }


}
