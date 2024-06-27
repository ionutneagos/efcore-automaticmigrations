using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EFCore.AutomaticMigrations.Sample.Data.Entities;

namespace EFCore.AutomaticMigrations.Sample.Data.Configurations
{
    internal sealed class TodoConfiguration : IEntityTypeConfiguration<Entities.Todo>
    {
        public void Configure(EntityTypeBuilder<Entities.Todo> builder)
        {
            builder.ToTable("Todos");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();
            builder.Property(t => t.Title).HasMaxLength(400).IsRequired();
         
            //map other properties here
        }
    }
}
