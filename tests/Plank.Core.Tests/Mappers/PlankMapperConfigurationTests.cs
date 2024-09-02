using AutoMapper;
using FluentAssertions;
using Plank.Core.Mappers;

namespace Plank.Core.Tests.Mappers
{
    public class PlankMapperConfigurationTests
    {
        public PlankMapperConfigurationTests()
        {
            PlankMapperConfiguration.ClearRegisteredProfiles();
        }

        [Fact]
        public void RegisterProfile_ProfileExists_NotAddedExpected()
        {
            // Arrange
            var profile = new TestProfile();
            PlankMapperConfiguration.RegisterProfile(profile);

            // Act
            PlankMapperConfiguration.RegisterProfile(profile);
            var profiles = PlankMapperConfiguration.Profiles;

            // Assert
            profiles.Should().ContainSingle();
        }

        [Fact]
        public void RegisterProfile_ProfileNotExists_AddedExpected()
        {
            // Arrange
            var profile = new TestProfile();
            PlankMapperConfiguration.RegisterProfile(profile);

            // Act
            var profiles = PlankMapperConfiguration.Profiles;

            // Assert
            profiles.Should().ContainSingle(p => p.ProfileName.Contains(nameof(TestProfile)));
        }

        [Fact]
        public void RegisterProfile_GenericProfileExists_NotAddedExpected()
        {
            // Arrange
            PlankMapperConfiguration.RegisterProfile<TestProfile>();

            // Act
            PlankMapperConfiguration.RegisterProfile<TestProfile>();
            var profiles = PlankMapperConfiguration.Profiles;

            // Assert
            profiles.Should().ContainSingle();
        }        

        [Fact]
        public void RegisterProfile_GenericProfileNotExists_AddedExpected()
        {
            // Arrange
            PlankMapperConfiguration.RegisterProfile<TestProfile>();

            // Act
            var profiles = PlankMapperConfiguration.Profiles;

            // Assert
            profiles.Should().ContainSingle(p => p.ProfileName.Contains(nameof(TestProfile)));
        }

        [Fact]
        public void RegisterProfile_GenericCalledFirst_OneAddedExpected()
        {
            // Arrange
            PlankMapperConfiguration.RegisterProfile<TestProfile>();
            PlankMapperConfiguration.RegisterProfile(new TestProfile());

            // Act
            var profiles = PlankMapperConfiguration.Profiles;

            // Assert
            profiles.Should().ContainSingle();
            profiles.Should().ContainSingle(p => p.ProfileName.Contains(nameof(TestProfile)));
        }

        [Fact]
        public void RegisterProfile_NonGenericCalledFirst_OneAddedExpected()
        {
            // Arrange
            PlankMapperConfiguration.RegisterProfile(new TestProfile());
            PlankMapperConfiguration.RegisterProfile<TestProfile>();

            // Act
            var profiles = PlankMapperConfiguration.Profiles;

            // Assert
            profiles.Should().ContainSingle();
            profiles.Should().ContainSingle(p => p.ProfileName.Contains(nameof(TestProfile)));
        }              

        private class TestProfile : Profile
        {
            public TestProfile()
            {
                CreateMap<object, object>();
            }
        }
    }
}