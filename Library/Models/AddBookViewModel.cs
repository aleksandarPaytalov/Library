using System.ComponentModel.DataAnnotations;
using Library.Data;

namespace Library.Models
{
    public class AddBookViewModel
    {
        [Required]
        [StringLength(DataConstants.BookTitleMaxLength,
            MinimumLength = DataConstants.BookTitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(DataConstants.AuthorNameMaxLength, 
            MinimumLength = DataConstants.AuthorNameMinLength)]
        public string Author { get; set; } = null!;

        [Required]
        public string Url { get; set; } = null!;

        [Required]
        [Range(DataConstants.RatingMinValue, DataConstants.RatingMaxValue)]
        public decimal Rating { get; set; }

        [Required]
        [StringLength(DataConstants.BookDescriptionMaxLength, 
            MinimumLength = DataConstants.BookDescriptionMinLength)]
        public string Description { get; set; } = null!;

        public int CategoryId { get; set; }

        public ICollection<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }
}
