using FluentValidation;
using log4net;
using Plank.Core.Configuration;
using Plank.Core.Entities;

namespace Plank.Core.Validators
{
    public static class ValidatorFactory
    {
        private static readonly ILog _logger = LogManager.GetLogger(nameof(ValidatorFactory));
        private static readonly List<Tuple<string, object>> _validators = new List<Tuple<string, object>>();

        static ValidatorFactory()
        {
            var validatorRegistrar = new ValidatorRegistrar(_validators, _logger);
            validatorRegistrar.RegisterValidators(PlankAssemblyRegistrar.GetRegisteredAssemblies());
        }

        public static IEnumerable<IValidator> CreateInstance(Type type)
        {
            return _validators
                .Where(v => v.Item1 == type.Name)
                .Select(v => (IValidator)v.Item2)
                .OrderBy(v => v.Priority);
        }

        public static IEnumerable<IEntityValidator<TEntity>> CreateInstance<TEntity>() where TEntity : IEntity
        {
            return _validators
                .Where(v => v.Item1 == typeof(TEntity).Name)
                .Select(v => (IEntityValidator<TEntity>)v.Item2)
                .OrderBy(v => v.Priority);
        }
    }
}