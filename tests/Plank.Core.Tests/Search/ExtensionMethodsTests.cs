using FluentAssertions;
using Plank.Core.Search;
using Plank.Core.Tests.Helpers.Entities;

namespace Plank.Core.Tests.Search
{
    public class ExtensionMethodsTests
    {
        [Fact]
        public void And_FirstNull_ArgumentNullException()
        {
            // Arrange

            // Act
            Action act = () => ExtensionMethods.And<ParentEntity>(null, p => p.IsActive);

            // Assert
            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null. (Parameter 'first')");
        }

        [Fact]
        public void And_SecondNull_ArgumentNullException()
        {
            // Arrange

            // Act
            Action act = () => ExtensionMethods.And<ParentEntity>(p => p.IsActive, null);

            // Assert
            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null. (Parameter 'second')");
        }

        [Fact]
        public void Or_FirstNull_ArgumentNullException()
        {
            // Arrange

            // Act
            Action act = () => ExtensionMethods.Or<ParentEntity>(null, p => p.IsActive);

            // Assert
            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null. (Parameter 'first')");
        }

        [Fact]
        public void Or_SecondNull_ArgumentNullException()
        {
            // Arrange

            // Act
            Action act = () => ExtensionMethods.Or<ParentEntity>(p => p.IsActive, null);

            // Assert
            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null. (Parameter 'second')");
        }
    }
}