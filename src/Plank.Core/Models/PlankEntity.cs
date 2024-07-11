using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Plank.Core.Models
{
    [HasSelfValidation]
    public abstract class PlankEntity : IEntity, IPopulateComputedColumns
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public Guid GlobalId { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateLastModified { get; set; }

        public void PopulateComputedColumns()
        {
            EntityHelper.PopulateComputedColumns(this);
        }

        [SelfValidation]
        public void Validate(ValidationResults results)
        {
            _ = results ?? throw new ArgumentNullException(nameof(results));

            EntityHelper.Validate(this, results);
        }
    }
}