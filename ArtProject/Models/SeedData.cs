using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ArtProject.Data;
using System;
using System.Linq;

namespace ArtProject.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ArtProjectContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ArtProjectContext>>()))
            {
                // Look for any movies.
                if (context.Art.Any())
                {
                    return;   // DB has been seeded
                }

                context.Art.AddRange(
                    new Art
                    {
                        Title = "Starry Night",
                        Artist = "Vincent Van Gogh",
                        Date = DateTime.Parse("1989"),
                        Technique = "Oil Painting",
                        Price = 100000000M,
                        Movement = "Post-Impressionism, Modern Art"
                    },

                    new Art
                    {
                        Title = "The Scream",
                        Artist = "Edvard Munch",
                        Date = DateTime.Parse("1893"),
                        Technique = "Oil paints and oil paints thickened with beeswax and also oil crayons containing beeswax and Japan wax, as well as casein pastels, a paraffin wax crayon and at least one gum-bound paint",
                        Price = 119000000M,
                        Movement = "Expressionism"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
