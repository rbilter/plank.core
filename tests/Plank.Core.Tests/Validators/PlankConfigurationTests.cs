using System.Reflection;
using FluentAssertions;
using Plank.Core.Tests.Fixtures;
using Plank.Core.Validators;

namespace Plank.Core.Tests.Validators
{
    [Collection(nameof(PlankConfigurationFixture))]
    public sealed class PlankConfigurationTests 
    {
        public PlankConfigurationTests()
        {
            PlankValidatorConfiguration.ClearRegisteredAssemblies();
        }

        [Fact]
        public void GetRegisteredAssemblies_NoAssembliesAreRegistered_EmptyExpected()
        {
            // Act
            var registeredAssemblies = PlankValidatorConfiguration.GetRegisteredAssemblies();

            // Assert
            Assert.Empty(registeredAssemblies);
        }

        [Fact]
        public void RegisterAssembly_AssemblyIsAlreadyRegistered_NotAddedExpected()
        {
            // Arrange
            var assembly = Assembly.GetExecutingAssembly();
            PlankValidatorConfiguration.RegisterAssembly(assembly);

            // Act
            PlankValidatorConfiguration.RegisterAssembly(assembly);
            var registeredAssemblies = PlankValidatorConfiguration.GetRegisteredAssemblies();

            // Assert
            registeredAssemblies.Where(a => a == assembly).Should().ContainSingle();
        }

        [Fact]
        public void RegisterAssembly_AssemblyIsNotRegistered_AddedExpected()
        {
            // Arrange
            var assembly = Assembly.GetExecutingAssembly();
            PlankValidatorConfiguration.RegisterAssembly(assembly);

            // Act
            var registeredAssemblies = PlankValidatorConfiguration.GetRegisteredAssemblies();

            // Assert
            registeredAssemblies.Should().Contain(assembly);
        }

        [Fact]
        public void RegisterAssemblyByPartialName_InvalidPartialName_NoAssembliesRegisteredExpected()
        {
            // Arrange
            var partialName = "Non.Existent.Assembly";

            // Act
            PlankValidatorConfiguration.RegisterAssemblyByPartialName(partialName);
            var registeredAssemblies = PlankValidatorConfiguration.GetRegisteredAssemblies();

            // Assert
            registeredAssemblies.Should().BeEmpty();
        }  

        [Fact]
        public void RegisterAssemblyByPartialName_ValidPartialName_AssembliesRegisteredExpected()
        {
            // Arrange
            var partialName = "Plank.Core.Tests";

            // Act
            PlankValidatorConfiguration.RegisterAssemblyByPartialName(partialName);
            var registeredAssemblies = PlankValidatorConfiguration.GetRegisteredAssemblies();

            // Assert
            registeredAssemblies.Should().NotBeEmpty();
            registeredAssemblies.All(a => a.FullName.Contains(partialName)).Should().BeTrue();
        }      
    }
}