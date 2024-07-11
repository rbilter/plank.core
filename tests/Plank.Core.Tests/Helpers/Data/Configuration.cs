using Plank.Core.Tests.Helpers.Entities;

namespace Plank.Core.Tests.Helpers.Data
{
    public sealed class Configuration
    {
        public void Seed(TestDbContext context)
        {
            if(!context.ParentEntity.Any())
            {
                var parent1 = new ParentEntity
                {
                    FirstName = "Luke",
                    LastName  = "Skywalker",
                    DateCreated = DateTime.UtcNow,
                    DateLastModified = DateTime.UtcNow,
                    ChildOne  =
                    [
                        new() 
                        {
                            Address = "Luke Skywalker Address",
                            City    = "Skywalker City",
                            DateCreated = DateTime.UtcNow,
                            DateLastModified = DateTime.UtcNow
                        }
                    ]
                };

                var parent2 = new ParentEntity
                {
                    FirstName = "Han",
                    LastName  = "Solo",
                    DateCreated = DateTime.UtcNow,
                    DateLastModified = DateTime.UtcNow,
                    ChildOne  =
                    [
                        new() 
                        {
                            Address = "Han Solo Address",
                            City    = "Solo City",
                            DateCreated = DateTime.UtcNow,
                            DateLastModified = DateTime.UtcNow
                        }
                    ]
                };

                context.ParentEntity.AddRange([parent1, parent2]);
                context.SaveChanges();
            }
        }
    }
}