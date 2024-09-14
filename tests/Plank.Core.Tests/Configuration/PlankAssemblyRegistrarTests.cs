using System.Reflection;
using FluentAssertions;
using Plank.Core.Configuration;
using Plank.Core.Tests.Fixtures;

namespace Plank.Core.Tests.Configuration
{
    [Collection(nameof(PlankConfigurationFixture))]
    public sealed class PlankAssemblyRegistrarTests
    {
        public PlankAssemblyRegistrarTests()
        {
            PlankAssemblyRegistrar.ClearRegisteredAssemblies();
        }

        [Fact]
        public void GetRegisteredAssemblies_NoAssembliesAreRegistered_EmptyExpected()
        {
            // Act
            var registeredAssemblies = PlankAssemblyRegistrar.GetRegisteredAssemblies();

            // Assert
            Assert.Empty(registeredAssemblies);
        }

        [Fact]
        public void RegisterAssembly_AssemblyIsAlreadyRegistered_NotAddedExpected()
        {
            // Arrange
            var assembly = Assembly.GetExecutingAssembly();
            PlankAssemblyRegistrar.RegisterAssembly(assembly);

            // Act
            PlankAssemblyRegistrar.RegisterAssembly(assembly);
            var registeredAssemblies = PlankAssemblyRegistrar.GetRegisteredAssemblies();

            // Assert
            registeredAssemblies.Where(a => a == assembly).Should().ContainSingle();
        }

        [Fact]
        public void RegisterAssembly_AssemblyIsNotRegistered_AddedExpected()
        {
            // Arrange
            var assembly = Assembly.GetExecutingAssembly();
            PlankAssemblyRegistrar.RegisterAssembly(assembly);

            // Act
            var registeredAssemblies = PlankAssemblyRegistrar.GetRegisteredAssemblies();

            // Assert
            registeredAssemblies.Should().Contain(assembly);
        }

        [Fact]
        public void RegisterAssemblyByPartialName_InvalidPartialName_NoAssembliesRegisteredExpected()
        {
            // Arrange
            var partialName = "Non.Existent.Assembly";

            // Act
            PlankAssemblyRegistrar.RegisterAssemblyByPartialName(partialName);
            var registeredAssemblies = PlankAssemblyRegistrar.GetRegisteredAssemblies();

            // Assert
            registeredAssemblies.Should().BeEmpty();
        }

        [Fact]
        public void RegisterAssemblyByPartialName_ValidPartialName_AssembliesRegisteredExpected()
        {
            // Arrange
            var partialName = "Plank.Core.Tests";

            // Act
            PlankAssemblyRegistrar.RegisterAssemblyByPartialName(partialName);
            var registeredAssemblies = PlankAssemblyRegistrar.GetRegisteredAssemblies();

            // Assert
            registeredAssemblies.Should().NotBeEmpty();
            registeredAssemblies.All(a => a.FullName.Contains(partialName)).Should().BeTrue();
        }
    }
}