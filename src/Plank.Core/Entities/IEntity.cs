using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Plank.Core.Entities
{
    public interface IEntity
    {
        int Id { get; set; }

        Guid GlobalId { get; set; }

        DateTime DateCreated { get; set; }

        DateTime DateLastModified { get; set; }

        void Validate(ValidationResults results);
    }
}