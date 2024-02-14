using System.ComponentModel.DataAnnotations;
using Library.Data;

namespace Library.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(DataConstants.CategoryNameMaxLength,
            MinimumLength = DataConstants.CategoryNameMinLength,
            ErrorMessage = DataConstants.StringLengthErrorMessage)]
        public string Name { get; set; } = string.Empty;
    }
}
