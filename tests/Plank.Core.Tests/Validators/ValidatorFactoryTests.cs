using FluentAssertions;
using Plank.Core.Tests.Helpers.Entities;
using Plank.Core.Tests.Helpers.Validators;
using Plank.Core.Validators;

namespace Plank.Core.Tests.Validators
{
    public class ValidatorFactoryTests
    {
        [Fact]
        public void CreateInstance_FluentValidators()
        {
            // Arrange

            // Act
            var got = ValidatorFactory.CreateInstance<ChildThreeEntity>();

            // Assert
            got.Single().GetType().Name.Should().Be("FluentValidatorAdapter`1");
        }

        [Fact]
        public void CreateInstance_PlankValidators()
        {
            // Arrange

            // Act
            var got = ValidatorFactory.CreateInstance<ChildTwoEntity>();

            // Assert
            got.Should().HaveCount(2);
            got.Where(v => v is PassValidator).Should().ContainSingle();
            got.Where(v => v is FailValidator).Should().ContainSingle();
        }
    }
}