using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Plank.Core.Controllers;
using Plank.Core.Search;
using Plank.Core.Tests.Helpers;
using Plank.Core.Tests.Helpers.Data;
using Plank.Core.Tests.Helpers.Entities;

namespace Plank.Core.Tests.Controllers
{
    public class PlankControllerTests
    {
        private static readonly DbContextOptions<TestDbContext> _options = TestHelper.InitializeContextOptions();
        private readonly Configuration _configuration;
        private readonly PlankController<ParentEntity> _controller;

        public PlankControllerTests()
        {
            var context = new TestDbContext(options: _options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            _configuration = new Configuration();
            _configuration.Seed(context);

            _controller = new PlankController<ParentEntity>(context);
        }

        [Fact]
        public async Task Add_ValidEntity_Created()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();

            // Act
            var response = await _controller.Add(item);

            // Assert
            response.ValidationResults.IsValid.Should().BeTrue();
            response.ValidationResults.Should().BeEmpty();
            response.Item.Id.Should().Be(item.Id);
        }

        [Fact]
        public async Task BulkAdd_ValidEntities_Created()
        {
            // Arrange
            var items = new List<ParentEntity>
            {
                TestHelper.GetParentEntity(),
                TestHelper.GetParentEntity()
            };

            // Act
            var response = await _controller.BulkAdd(items);

            // Assert
            response.Items.Should().HaveCount(2);
            response.Items.Where(i => i.ValidationResults.IsValid).Should().HaveCount(2);
            response.Items.Where(i => i.Item.Id == items[0].Id).Should().HaveCount(1);
            response.Items.Where(i => i.Item.Id == items[1].Id).Should().HaveCount(1);
        }

        [Fact]
        public async Task Delete_EntityExists_Deleted()
        {
            // Arrange
            var entity = TestHelper.GetParentEntity();

            // Act
            var created = await _controller.Add(entity);
            created.ValidationResults.IsValid.Should().BeTrue();

            var deleted = await _controller.Delete(created.Item.Id);

            // Assert
            deleted.ValidationResults.IsValid.Should().BeTrue();
            deleted.Id.Should().Be(created.Item.Id);
        }

        [Fact]
        public void Search_BuilderNull_ArgumentNullException()
        {
            // Arrange

            // Act
            Func<Task> act = SearchWithNullBuilder;

            // Assert
            act.Should()
               .ThrowAsync<ArgumentNullException>()
               .WithMessage("Value cannot be null. (Parameter 'builder')");

            Task SearchWithNullBuilder()
            {
                return _controller.Search(null);
            }
        }

        [Fact]
        public async Task Search_EntitiesExist_PageReturned()
        {
            // Arrange
            var builder = new Mock<ISearchCriteriaBuilder<ParentEntity>>();
            builder.Setup(m => m.Build()).Returns(new SearchCriteria<ParentEntity>
            {
                PageNumber = 1,
                PageSize = 10
            });

            // Act
            var response = await _controller.Search(builder.Object);

            // Assert
            response.Items.Should().NotBeEmpty();
            response.PageNumber.Should().Be(1);
            response.PageSize.Should().Be(10);
            response.TotalItemCount.Should().BeGreaterOrEqualTo(response.Items.Count());
            response.IsValid.Should().BeTrue();
            response.IsFirstPage.Should().BeTrue();
            response.HasPreviousPage.Should().BeFalse();
            (response.TotalItemCount <= response.PageSize ? response.IsLastPage == true : response.IsLastPage == false).Should().BeTrue();
            (response.TotalItemCount <= response.PageSize ? response.HasNextPage == false : response.HasNextPage == true).Should().BeTrue();

            builder.Verify(m => m.Build(), Times.Once());
        }

        [Fact]
        public async Task Update_EntityValid_Updated()
        {
            // Arrange
            var firstName = TestHelper.GetRandomString(10);
            var lastName  = TestHelper.GetRandomString(20);
            var add       = TestHelper.GetParentEntity();

            // Act
            var response = await _controller.Add(add);
            response.ValidationResults.IsValid.Should().BeTrue();

            add.FirstName = firstName;
            add.LastName  = lastName;
            response = await _controller.Update(add);

            var updated = await _controller.Get(add.Id);

            // Assert
            response.ValidationResults.IsValid.Should().BeTrue();
            updated.Item.FirstName.Should().Be(firstName);
            updated.Item.LastName.Should().Be(lastName);
        }
    }
}