using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Plank.Core.Models;

namespace Plank.Core.Tests.Helpers.Entities
{
    [HasSelfValidation]
    [NotNullValidator(MessageTemplate = "ParentEntity cannot be null.")]
    public class ParentEntity : PlankEntity
    {
        [Required(ErrorMessage = "First name must be entered.", AllowEmptyStrings = false)]
        [MaxLength(30, ErrorMessage = "First name cannot be greater than 30 characters.")]
        public string FirstName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Last name must be entered.", AllowEmptyStrings = false)]
        [MaxLength(50, ErrorMessage = "Last name cannot be greater than 50 characters.")]
        public string LastName { get; set; } = string.Empty;

        [InverseProperty("ParentEntity")]
        public virtual ICollection<ChildOneEntity> ChildOne { get; set; } = [];

        [InverseProperty("ParentEntity")]
        public virtual ICollection<ChildTwoEntity> ChildTwo { get; set; } = [];
    }
}