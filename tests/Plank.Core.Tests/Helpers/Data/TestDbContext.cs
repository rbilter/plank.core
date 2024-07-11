using Microsoft.EntityFrameworkCore;
using Plank.Core.Data;
using Plank.Core.Tests.Helpers.Entities;

namespace Plank.Core.Tests.Helpers.Data
{
    public class TestDbContext(DbContextOptions options) : PlankDbContext(options)
    {
        public DbSet<ParentEntity> ParentEntity { get; set; } = null!;

        public DbSet<ChildOneEntity> ChildOneEntity { get; set; } = null!;

        public DbSet<ChildTwoEntity> ChildTwoEntity { get; set; } = null!;
    }
}