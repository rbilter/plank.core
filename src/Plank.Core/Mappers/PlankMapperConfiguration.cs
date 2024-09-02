using AutoMapper;

namespace Plank.Core.Mappers
{
    public static class PlankMapperConfiguration
    {
        private static readonly List<Profile> _profiles = [];
        private static readonly HashSet<string> _profileNames = [];

        public static List<Profile> Profiles => _profiles;

        public static void ClearRegisteredProfiles()
        {
            _profiles.Clear();
            _profileNames.Clear();
        }

        public static void RegisterProfile<TProfile>() where TProfile : Profile, new()
        {
            var profile = new TProfile();
            RegisterProfile(profile);
        }

        public static void RegisterProfile(Profile profile)
        {
            if (!_profileNames.Contains(profile.ProfileName))
            {
                _profiles.Add(profile);
                _profileNames.Add(profile.ProfileName);
            }
        }
    }
}