using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Plank.Core.Models;

namespace Plank.Core.Tests.Helpers.Entities
{
    [HasSelfValidation]
    public class ChildOneEntity : PlankEntity
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Address cannot be longer than 50 characters")]
        public string Address { get; set; } = string.Empty;

        [Required]
        [MaxLength(30, ErrorMessage = "City cannot be longer than 30 characters")]
        public string City { get; set; } = string.Empty;

        public int ParentEntityId { get; set; }

        [ForeignKey("ParentEntityId")]
        public virtual ParentEntity ParentEntity { get; set; } = new();
    }
}