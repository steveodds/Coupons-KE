using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CoupnsKE.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new Data.CoupnsKEContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<Data.CoupnsKEContext>>()))
            {
                // Look for any movies.
                if (context.Users.Any())
                {
                    return;   // DB has been seeded
                }

                context.Users.Add(
                    new Areas.Identity.Data.CoupnsKEUser
                    {
                        DOB = new DateTime(1996, 1, 18),
                        Email = "steveremdi@gmail.com",
                        EmailConfirmed = true,
                        Gender = Enum.Gender.Male,
                        Name = "Steve",
                        Id = "c05bd4f8-e64c-437c-bc23-2ae3ae8e4e52",
                        PasswordHash = "AQAAAAEAACcQAAAAEOk6MNrqgO6unhJwk9tv3n31ypQdkZ9BlacObKV2nIiDjxIDkvlQML9LchcDXe7cTw==",
                        UserName = "Steve",
                        UserRole = Enum.Roles.Administrator,
                        NormalizedEmail = "STEVEREMDI@GMAIL.COM",
                        NormalizedUserName = "STEVE",
                        LockoutEnabled = false,
                        SecurityStamp = "XW7DFBOXSXKOFKZPAIYRRQCDVEGKWCWP",
                        ConcurrencyStamp = "36628ca2-37ad-472e-a19b-4d83384b7273"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}