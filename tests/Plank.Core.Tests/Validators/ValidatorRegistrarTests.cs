using FluentAssertions;
using FluentValidation;
using log4net;
using Moq;
using Plank.Core.Validators;
using System.Reflection;

namespace Plank.Core.Tests.Validators
{
    public sealed class ValidatorRegistrarTests
    {
        private readonly Mock<ILog> _loggerMock;
        private List<Tuple<string, object>> _validators;
        private ValidatorRegistrar _validatorRegistrar;

        public ValidatorRegistrarTests()
        {
            _loggerMock = new Mock<ILog>();
            InitializeValidators();
        }

        private void InitializeValidators()
        {
            _validators = [];
            _validatorRegistrar = new ValidatorRegistrar(_validators, _loggerMock.Object);
        }

        [Fact]
        public void RegisterValidators_AssemblyHasValidators_ValidatorsRegisteredExpected()
        {
            // Arrange
            var assembly = Assembly.GetExecutingAssembly();
            var assemblies = new List<Assembly> { assembly };

            // Act
            _validatorRegistrar.RegisterValidators(assemblies);

            // Assert
            _validators.Should().NotBeEmpty();
        }

        [Fact]
        public void RegisterValidators_DuplicateValidatorsRegistered_DuplicateNotAddedExpected()
        {
            // Arrange
            var assembly = Assembly.GetExecutingAssembly();
            var assemblies = new List<Assembly> { assembly };

            // Act
            _validatorRegistrar.RegisterValidators(assemblies);
            _validatorRegistrar.RegisterValidators(assemblies);

            // Assert
            var duplicateValidators = _validators.GroupBy(v => v.Item2.GetType())
                                                 .Where(g => g.Count() > 1)
                                                 .SelectMany(g => g)
                                                 .ToList();
            duplicateValidators.Should().BeEmpty();
        }
    }
}