using System.Linq.Expressions;
using FluentAssertions;
using Plank.Core.Search;
using Plank.Core.Tests.Helpers.Entities;

namespace Plank.Core.Tests.Search
{
    public class SearchCriteriaBuilderTests
    {
        [Fact]
        public void AddInclude_ShouldAddIncludeExpression()
        {
            // Arrange
            Expression<Func<ParentEntity, object>> includeExpression = e => e.ChildOne;
            var builder = new SearchCriteriaBuilder<ParentEntity>()
                .AddInclude(includeExpression);

            // Act
            var criteria = builder.Build();

            // Assert
            criteria.Includes.Should().Contain(includeExpression);
        }

        [Fact]
        public void AddFilterAnd_ShouldAddAndFilterExpression()
        {
            // Arrange
            var builder = new SearchCriteriaBuilder<ParentEntity>()
                .AddFilterAnd(e => e.FirstName.Contains("FirstName"));

            // Act
            var criteria = builder.Build();

            // Assert
            criteria.Filter.ToString().Should().Contain("e => (True And e.FirstName.Contains(\"FirstName\"))");
        }

        [Fact]
        public void AddFilterOr_ShouldAddOrFilterExpression()
        {
            // Arrange
            var builder = new SearchCriteriaBuilder<ParentEntity>()
                .AddFilterOr(e => e.FirstName.Contains("FirstName"));

            // Act
            var criteria = builder.Build();

            // Assert
            criteria.Filter.ToString().Should().Contain("e => (True Or e.FirstName.Contains(\"FirstName\"))");
        }

        [Fact]
        public void SetPageNumber_ShouldSetPageNumber()
        {
            // Arrange
            var builder = new SearchCriteriaBuilder<ParentEntity>()
                .SetPageNumber(2);

            // Act
            var criteria = builder.Build();

            // Assert
            criteria.PageNumber.Should().Be(2);
        }

        [Fact]
        public void SetPageSize_ShouldSetPageSize()
        {
            // Arrange
            var builder = new SearchCriteriaBuilder<ParentEntity>()
                .SetPageSize(20);

            // Act
            var criteria = builder.Build();

            // Assert
            criteria.PageSize.Should().Be(20);
        }
    }
}