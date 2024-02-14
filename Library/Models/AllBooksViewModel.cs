using Library.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class AllBooksViewModel
    {
       
        public int Id { get; set; }

        [Required]
        [StringLength(DataConstants.BookTitleMaxLength,
            MinimumLength = DataConstants.BookTitleMinLength)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(DataConstants.AuthorNameMaxLength,
            MinimumLength = DataConstants.AuthorNameMinLength)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [StringLength(DataConstants.BookDescriptionMaxLength,
            MinimumLength = DataConstants.BookDescriptionMinLength)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [Range(DataConstants.RatingMinValue, DataConstants.RatingMaxValue)]
        public decimal Rating { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

    }
}
