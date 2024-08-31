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
            PlankConfiguration.ClearRegisteredAssemblies();
        }

        [Fact]
        public void GetRegisteredAssemblies_NoAssembliesAreRegistered_EmptyExpected()
        {
            // Act
            var registeredAssemblies = PlankConfiguration.GetRegisteredAssemblies();

            // Assert
            Assert.Empty(registeredAssemblies);
        }

        [Fact]
        public void RegisterAssembly_AssemblyIsAlreadyRegistered_NotAddedExpected()
        {
            // Arrange
            var assembly = Assembly.GetExecutingAssembly();
            PlankConfiguration.RegisterAssembly(assembly);

            // Act
            PlankConfiguration.RegisterAssembly(assembly);
            var registeredAssemblies = PlankConfiguration.GetRegisteredAssemblies();

            // Assert
            registeredAssemblies.Where(a => a == assembly).Should().ContainSingle();
        }

        [Fact]
        public void RegisterAssembly_AssemblyIsNotRegistered_AddedExpected()
        {
            // Arrange
            var assembly = Assembly.GetExecutingAssembly();
            PlankConfiguration.RegisterAssembly(assembly);

            // Act
            var registeredAssemblies = PlankConfiguration.GetRegisteredAssemblies();

            // Assert
            registeredAssemblies.Should().Contain(assembly);
        }

        [Fact]
        public void RegisterAssemblyByPartialName_InvalidPartialName_NoAssembliesRegisteredExpected()
        {
            // Arrange
            var partialName = "Non.Existent.Assembly";

            // Act
            PlankConfiguration.RegisterAssemblyByPartialName(partialName);
            var registeredAssemblies = PlankConfiguration.GetRegisteredAssemblies();

            // Assert
            registeredAssemblies.Should().BeEmpty();
        }  

        [Fact]
        public void RegisterAssemblyByPartialName_ValidPartialName_AssembliesRegisteredExpected()
        {
            // Arrange
            var partialName = "Plank.Core.Tests";

            // Act
            PlankConfiguration.RegisterAssemblyByPartialName(partialName);
            var registeredAssemblies = PlankConfiguration.GetRegisteredAssemblies();

            // Assert
            registeredAssemblies.Should().NotBeEmpty();
            registeredAssemblies.All(a => a.FullName.Contains(partialName)).Should().BeTrue();
        }      
    }
}