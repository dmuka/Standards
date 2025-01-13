using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.ModelBuilderExtensions;

internal static class SeedExtension
{
    public static void Seed<T>(this ModelBuilder modelBuilder, object[] seedData) where T : BaseEntity
    {
        var index = 1;
            
        foreach (var entity in seedData)
        {
            Console.WriteLine($"Seeding {typeof(T).Name} {index++}.");
            modelBuilder.Entity<T>().HasData(entity);
        }
    }
}