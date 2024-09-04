using System.Linq.Expressions;
using FluentAssertions;
using Plank.Core.Search;
using Plank.Core.Tests.Helpers.Entities;

namespace Plank.Core.Tests.Search
{
    public class SearchCriteriaBuilderTests
    {
        [Fact]
        public void Include_ShouldAddIncludeExpression()
        {
            // Arrange
            Expression<Func<ParentEntity, object>> includeExpression = e => e.ChildOne;
            var builder = new SearchCriteriaBuilder<ParentEntity>()
                .Include(includeExpression);

            // Act
            var criteria = builder.Build();

            // Assert
            criteria.Includes.Should().Contain(includeExpression);
        }

        [Fact]
        public void SetFilter_ShouldSetFilterWithAndCombination()
        {
            // Arrange
            Expression<Func<ParentEntity, bool>> filter = e => e.FirstName == "FirstName";
            var builder = new SearchCriteriaBuilder<ParentEntity>()
                .SetFilter(filter, FilterCombination.And);

            // Act
            var criteria = builder.Build();

            // Assert
            criteria.Filter.ToString().Should().Contain("And");
            criteria.Filter.ToString().Should().Contain("FirstName == \"FirstName\"");
            criteria.Filter.ToString().Should().Contain("True"); // Default value
        }

        [Fact]
        public void SetFilter_ShouldSetFilterWithOrCombination()
        {
            // Arrange
            Expression<Func<ParentEntity, bool>> filter = e => e.FirstName == "FirstName";
            var builder = new SearchCriteriaBuilder<ParentEntity>()
                .SetFilter(filter, FilterCombination.Or);

            // Act
            var criteria = builder.Build();

            // Assert
            criteria.Filter.ToString().Should().Contain("Or");
            criteria.Filter.ToString().Should().Contain("FirstName == \"FirstName\"");
            criteria.Filter.ToString().Should().Contain("True"); // Default value
        }

        [Fact]
        public void SetFilter_ShouldThrowArgumentOutOfRangeExceptionForInvalidCombination()
        {
            // Arrange
            Expression<Func<ParentEntity, bool>> filter = e => e.FirstName == "FirstName";
            var invalidCombination = (FilterCombination)999;
            var builder = new SearchCriteriaBuilder<ParentEntity>();

            // Act
            Action act = () => builder.SetFilter(filter, invalidCombination);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("*combination*"); // Ensure the exception message contains the expected text
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