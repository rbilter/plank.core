using AutoMapper;

namespace Plank.Core.Mappers
{
    public static class PlankMappingConfiguration
    {
        private static readonly List<Action<IMapperConfigurationExpression>> _profileActions = [];
        private static readonly HashSet<string> _profileNames = [];

        public static void AddProfile(Profile profile)
        {
            if (!_profileNames.Contains(profile.ProfileName))
            {
                _profileActions.Add(cfg => cfg.AddProfile(profile));
                _profileNames.Add(profile.ProfileName);
            }
        }

        public static List<Action<IMapperConfigurationExpression>> ProfileActions => _profileActions;
    }
}