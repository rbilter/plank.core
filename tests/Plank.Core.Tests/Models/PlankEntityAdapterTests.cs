using FluentAssertions;
using Plank.Core.Tests.Helpers.Adapters;

namespace Plank.Core.Tests.Models
{
    public class PlankEntityAdapterTests
    {
        [Fact]
        public void Validate_ValidationResultsNull_ArgumentNullException()
        {
            // Arrange

            // Act
            Action act = () =>
                        {
                var entity = new PlankHelperEntityAdapter();
                entity.Validate(null);
            };

            // Assert
            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null. (Parameter 'results')");
        }
    }
}