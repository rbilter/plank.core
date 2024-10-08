using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Newtonsoft.Json;
using Plank.Core.Contracts;
using Plank.Core.Mappers;

namespace Plank.Core.Entities
{
    internal static class ExtensionMethods
    {
        public static List<PropertyInfo> GetProperties(this IEntity item)
        {
            return item.GetType()
                       .GetProperties()
                       .Where(p => p.DeclaringType == item.GetType() && !p.IsDefined(typeof(InversePropertyAttribute), false))
                       .ToList();
        }

        public static string ToJson(this object item, TypeNameHandling handling = TypeNameHandling.Auto)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = handling
            };

            return JsonConvert.SerializeObject(item, settings);
        }

        public static PlankValidationResultCollection Validate<TEntity>(this TEntity item) where TEntity : IEntity
        {
            var validator = ValidationFactory.CreateValidator<TEntity>();
            var result = validator.Validate(item);

            return InternalMapper<TEntity>.Mapper.Map<PlankValidationResultCollection>(result);
        }

        public static IEnumerable<(TEntity Item, PlankValidationResultCollection ValidationResults)> Validate<TEntity>(this IEnumerable<TEntity> items) where TEntity : IEntity
        {
            var results = new List<(TEntity, PlankValidationResultCollection)>();
            items.ForEach(i => results.Add((i, i.Validate())));

            return results;
        }
    }
}