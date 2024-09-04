using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Plank.Core.Entities;

namespace Plank.Core.Tests.Helpers.Entities
{
    [HasSelfValidation]
    public class ChildTwoEntity : PlankEntity
    {
        public int ParentEntityId { get; set; }

        [ForeignKey("ParentEntityId")]
        public virtual ParentEntity ParentEntity { get; set; } = new();
    }
}