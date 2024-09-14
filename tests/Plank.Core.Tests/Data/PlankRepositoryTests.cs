using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Plank.Core.Data;
using Plank.Core.Tests.Helpers;
using Plank.Core.Tests.Helpers.Data;
using Plank.Core.Tests.Helpers.Entities;

namespace Plank.Core.Tests.Data
{
    public class PlankRepositoryTests
    {
        private static readonly DbContextOptions<TestDbContext> _options = TestHelper.InitializeContextOptions();
        private readonly TestDbSeeder _configuration;
        private readonly PlankRepository<ParentEntity> _repo;

        public PlankRepositoryTests()
        {
            var context = new TestDbContext(options: _options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            _configuration = new TestDbSeeder();
            _configuration.Seed(context);

            _repo = new PlankRepository<ParentEntity>(context);
        }

        [Fact]
        public async Task Add_EntityValid_EntityCreated()
        {
            // Arrange
            var entity = TestHelper.GetParentEntity();
            entity.ChildOne = [TestHelper.GetChildOne()];

            // Act
            await _repo.Add(entity);
            var includes = new List<Expression<Func<ParentEntity, object>>> { i => i.ChildOne };
            var got = await _repo.Search(r => r.Id == entity.Id, includes);

            // Assert
            got.Single().Id.Should().Be(entity.Id);
            got.Single().ChildOne.First().Id.Should().Be(entity.ChildOne.First().Id);
        }

        [Fact]
        public async Task BulkAdd_EntitiesValid_EntitiesCreated()
        {
            // Arrange
            var entities = new List<ParentEntity>
            {
                TestHelper.GetParentEntity(),
                TestHelper.GetParentEntity()
            };

            // Act
            await _repo.BulkAdd(entities);
            var ids = entities.Select(e => e.Id).ToList();
            var got = await _repo.Search(e => ids.Contains(e.Id));

            // Assert
            got.Should().HaveCount(2);
            entities.Select(e => e.Id == got[0].Id).Should().NotBeEmpty();
            entities.Select(e => e.Id == got[1].Id).Should().NotBeEmpty();
        }

        [Fact]
        public void Constructor_DbContextNull_ArgumentNullException()
        {
            // Arrange

            // Act
            Action act = () =>
            {
                _ = new PlankRepository<ChildOneEntity>(null);
            };

            // Assert
            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null. (Parameter 'context')");
        }

        [Fact]
        public async Task Delete_EntityExists_EntityDeleted()
        {
            // Arrange
            var expected = TestHelper.GetParentEntity();

            // Act
            await _repo.Add(expected);

            await _repo.Delete(expected.Id);
            var got = await _repo.Get(expected.Id);

            // Assert
            got.Should().BeNull();
        }

        [Fact]
        public async Task Get_EntityExists_EntityReturned()
        {
            // Arrange
            var id = TestHelper.GetParentId(_options);

            // Act
            var got = await _repo.Get(id);

            // Assert
            got.Id.Should().Be(id);
            got.FirstName.Should().Be("Luke");
            got.LastName.Should().Be("Skywalker");
        }

        [Fact]
        public async Task Search_EntitiesFound_ListReturned()
        {
            // Arrange

            // Act
            var got = await _repo.Search(i => i.FirstName == "Han" && i.LastName == "Solo");

            // Assert
            got.Single().ChildOne.Should().BeEmpty();
        }

        [Fact]
        public async Task Search_IncludesProvided_ListReturned()
        {
            // Arrange
            var includes = new List<Expression<Func<ParentEntity, object>>> { i => i.ChildOne };

            // Act
            var got = await _repo.Search(i => i.FirstName == "Han" && i.LastName == "Solo", includes);

            // Assert
            got.Single().ChildOne.Should().ContainSingle();
        }

        [Fact]
        public async Task Update_EntityExists_EntityUpdated()
        {
            // Arrange
            var expected = TestHelper.GetParentEntity();

            // Act
            await _repo.Add(expected);

            var firstName = TestHelper.GetRandomString(10);
            var lastName = TestHelper.GetRandomString(20);
            expected.FirstName = firstName;
            expected.LastName = lastName;

            await _repo.Update(expected);
            var got = await _repo.Get(expected.Id);

            // Assert
            got.Id.Should().Be(expected.Id);
            got.FirstName.Should().Be(firstName);
            got.LastName.Should().Be(lastName);
        }
    }
}