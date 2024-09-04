using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Plank.Core.Entities
{
    internal static class EntityHelper
    {
        public static void PopulateComputedColumns(IEntity item)
        {
            item.DateCreated = item.DateCreated == DateTime.MinValue
                ? DateTime.UtcNow
                : item.DateCreated;
            item.DateLastModified = DateTime.UtcNow;

            item.GlobalId = item.GlobalId == Guid.Empty
                ? Guid.NewGuid()
                : item.GlobalId;
        }

        public static void Validate(IEntity item, ValidationResults results)
        {
            var inverseProperties = item.GetType()
                .GetProperties()
                .Where(e => e.IsDefined(typeof(InversePropertyAttribute), false))
                .ToList();

            ValidateWithCustomValidators(results, item);

            foreach (var property in inverseProperties)
            {
                if (property.GetValue(item) is IEnumerable collection)
                {
                    foreach (var entity in collection)
                    {
                        var validator = ValidationFactory.CreateValidator(entity.GetType());

                        var result = validator.Validate(entity);
                        results.AddAllResults(result);

                        if (result.IsValid)
                        {
                            ValidateWithCustomValidators(results, entity);
                        }
                    }
                }
                else
                {
                    var validator = ValidationFactory.CreateValidator(property.GetType());

                    var result = validator.Validate(property);
                    results.AddAllResults(result);

                    if (result.IsValid)
                    {
                        ValidateWithCustomValidators(results, property);
                    }
                }
            }
        }

        private static void ValidateWithCustomValidators(ValidationResults results, object entity)
        {
            var validators = Validators.ValidatorFactory.CreateInstance(entity.GetType());
            foreach (var v in validators)
            {
                var result = v.Validate(entity);
                results.AddAllResults(result);

                if (!result.IsValid)
                {
                    break;
                }
            }
        }
    }
}