using AutoMapper;
using FluentAssertions;
using log4net;
using Moq;
using Plank.Core.Mappers;
using System.Reflection;

namespace Plank.Core.Tests.Mappers
{
    public class ProfileRegistrarTests
    {
        private readonly Mock<ILog> _loggerMock;
        private List<Profile> _profiles;
        private ProfileRegistrar _profileRegistrar;

        public ProfileRegistrarTests()
        {
            _loggerMock = new Mock<ILog>();
            InitializeProfiles();
            PlankMapperConfiguration.ClearRegisteredProfiles();
        }

        private void InitializeProfiles()
        {
            _profiles = [];
            _profileRegistrar = new ProfileRegistrar(_profiles, _loggerMock.Object);
        }

        [Fact]
        public void RegisterValidators_AssemblyHasProfiles_ProfilesRegisteredExpected()
        {
            // Arrange
            var assembly = Assembly.GetExecutingAssembly();
            var assemblies = new List<Assembly> { assembly };

            // Act
            _profileRegistrar.RegisterProfiles(assemblies);

            // Assert
            _profiles.Should().NotBeEmpty();
        }

        [Fact]
        public void RegisterProfile_DuplicateProfilesRegistered_DuplicateNotAddedExpected()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblies = new List<Assembly> { assembly };

            // Act
            _profileRegistrar.RegisterProfiles(assemblies);
            _profileRegistrar.RegisterProfiles(assemblies);

            // Assert
            var duplicateProfiles = _profiles.GroupBy(v => v.GetType())
                                                 .Where(g => g.Count() > 1)
                                                 .SelectMany(g => g)
                                                 .ToList();
            duplicateProfiles.Should().BeEmpty();
        }
    }
}