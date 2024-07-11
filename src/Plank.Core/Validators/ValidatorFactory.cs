using System.Reflection;
using FluentValidation;
using log4net;
using Plank.Core.Models;

namespace Plank.Core.Validators
{
    public static class ValidatorFactory
    {
        private static readonly ILog _logger = LogManager.GetLogger(nameof(ValidatorFactory));
        private static readonly List<Tuple<string, object>> _validators = [];

        static ValidatorFactory()
        {
            LoadValidators();
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

        private static void LoadValidators()
        {
            var filter = new List<string> { "microsoft", "system", "mscorlib", "xunit" };
            var asm = AppDomain.CurrentDomain.GetAssemblies().Where(a => !filter.Any(f => a.FullName.IndexOf(f, StringComparison.OrdinalIgnoreCase) >= 0) && !a.IsDynamic);
            var allTypes = asm.SelectMany(a => a.GetExportedTypes()).Where(t => t.IsClass && !t.IsAbstract);

            var types = allTypes.Where(t => t.GetInterface("IEntityValidator`1") != null && !t.IsAbstract).ToList();

            foreach (var type in types)
            {
                try
                {
                    var instance = Activator.CreateInstance(type);
                    var inter = type.GetInterface("IEntityValidator`1");
                    var entity = inter.GetTypeInfo().GenericTypeArguments[0];

                    _validators.Add(new Tuple<string, object>(entity.Name, instance));
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    throw;
                }
            }

            types = allTypes.Where(t => t.BaseType.IsGenericType 
                    && t.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>)
                    && !t.ContainsGenericParameters).ToList();

            var fvType = typeof(FluentValidatorAdapter<>);
            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var abstr = type.BaseType;
                var entity = abstr.GenericTypeArguments[0];


                var constructed = fvType.MakeGenericType(entity);
                var validator = Activator.CreateInstance(constructed, args: instance);

                _validators.Add(new Tuple<string, object>(entity.Name, validator));
            }
        }
    }
}