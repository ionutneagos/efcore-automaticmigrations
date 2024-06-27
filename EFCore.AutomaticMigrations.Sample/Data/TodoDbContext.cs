using EFCore.AutomaticMigrations.Sample.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EFCore.AutomaticMigrations.Sample.Data;

public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
{
    public DbSet<Entities.Todo> Todos => base.Set<Entities.Todo>();
    public DbSet<TodoPoco> TodosPoco => Set<TodoPoco>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //OBS!: you can filter types within the assembly based on context name, usefull on multitenant solutions
        builder.ApplyConfigurationsFromAssembly(typeof(TodoDbContext).Assembly);
    }

}
