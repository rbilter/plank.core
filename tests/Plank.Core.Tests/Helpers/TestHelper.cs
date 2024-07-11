using Microsoft.EntityFrameworkCore;
using Plank.Core.Tests.Helpers.Data;
using Plank.Core.Tests.Helpers.Entities;

namespace Plank.Core.Tests.Helpers
{
    public static class TestHelper
    {
        private static readonly Random _random = new();

        public static int GetParentId(DbContextOptions<TestDbContext> options)
        {
            using (var context = new TestDbContext(options: options))
            {
                return context.ParentEntity.First().Id;
            }
        }

        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public static ParentEntity GetParentEntity(int id = 0)
        {
            return new ParentEntity()
            {
                Id = id,
                FirstName = GetRandomString(10),
                LastName = GetRandomString(20)
            };
        }

        public static ChildOneEntity GetChildOne()
        {
            return new ChildOneEntity()
            {
                Address = GetRandomString(30),
                City = GetRandomString(20)
            };
        }

        public static ChildTwoEntity GetChildTwo()
        {
            return new ChildTwoEntity()
            {
            };
        }

        public static DbContextOptions<TestDbContext> InitializeContextOptions()
        {
            var db = $"Plank.Core.Tests_{Guid.NewGuid()}";
            var builder = new DbContextOptionsBuilder<TestDbContext>();
            builder.UseSqlite($"DataSource=file:{db}?mode=memory&cache=shared");
            return builder.Options;
        }
    }
}