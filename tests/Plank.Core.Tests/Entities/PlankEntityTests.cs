using FluentAssertions;
using Plank.Core.Tests.Helpers.Entities;

namespace Plank.Core.Tests.Entities
{
    public class PlankEntityTests
    {
        [Fact]
        public void Validate_ValidationResultsNull_ArgumentNullException()
        {
            // Arrange

            // Act
            Action act = () =>
            {
                var entity = new PlankHelperEntity();
                entity.Validate(null);
            };

            // Assert
            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null. (Parameter 'results')");
        }
    }
}