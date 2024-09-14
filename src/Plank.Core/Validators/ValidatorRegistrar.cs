using FluentValidation;
using log4net;
using System.Reflection;

namespace Plank.Core.Validators
{
    public class ValidatorRegistrar
    {
        private readonly List<Tuple<string, object>> _validators;
        private readonly ILog _logger;

        public ValidatorRegistrar(List<Tuple<string, object>> validators, ILog logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public void RegisterValidators(IEnumerable<Assembly> assemblies)
        {
            var allTypes = GetAllTypes(assemblies);

            RegisterEntityValidators(allTypes);
            RegisterFluentValidators(allTypes);
        }

        private void AddValidator(string entityName, object validator)
        {
            if (_validators.Any(v => v.Item2.GetType() == validator.GetType()))
            {
                return;
            }

            _validators.Add(new Tuple<string, object>(entityName, validator));
        }

        private IEnumerable<Type> GetAllTypes(IEnumerable<Assembly> assemblies)
        {
            return assemblies
                .SelectMany(a => a.GetExportedTypes())
                .Where(t => t.IsClass && !t.IsAbstract);
        }

        private void RegisterEntityValidators(IEnumerable<Type> allTypes)
        {
            var entityValidatorTypes = allTypes.Where(t => t.GetInterfaces().Any(i => i.Name == "IEntityValidator`1"));
            foreach (var type in entityValidatorTypes)
            {
                try
                {
                    var instance = Activator.CreateInstance(type);
                    var inter = type.GetInterface("IEntityValidator`1");
                    var entity = inter.GetTypeInfo().GenericTypeArguments[0];

                    AddValidator(entity.Name, instance);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    throw;
                }
            }
        }

        private void RegisterFluentValidators(IEnumerable<Type> allTypes)
        {
            var fluentValidatorTypes = allTypes.Where(t => t.BaseType.IsGenericType
                    && t.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>)
                    && !t.ContainsGenericParameters);

            var fvType = typeof(FluentValidatorAdapter<>);
            foreach (var type in fluentValidatorTypes)
            {
                try
                {
                    var instance = Activator.CreateInstance(type);
                    var abstr = type.BaseType;
                    var entity = abstr.GenericTypeArguments[0];

                    var constructed = fvType.MakeGenericType(entity);
                    var validator = Activator.CreateInstance(constructed, args: instance);

                    AddValidator(entity.Name, validator);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    throw;
                }
            }
        }
    }
}