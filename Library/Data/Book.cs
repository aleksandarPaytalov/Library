using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    [Comment("Books for the library")]
    public class Book
    {
        [Key]
        [Comment("Book identifier")]
        public int Id { get; set; }

        [Required]
        [Comment("Book Title")]
        [MaxLength(DataConstants.BookTitleMaxLength)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Comment("Author of the Book")]
        [MaxLength(DataConstants.AuthorNameMaxLength)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [Comment("Book Description")]
        [MaxLength(DataConstants.BookDescriptionMaxLength)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Comment("Book ImageUrl path")]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [Comment("Rating of the book")]
        public decimal Rating { get; set; }

        [Required]
        [Comment("Category identifier")]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        [Required]
        [Comment("Category of the book")]
        public Category Category { get; set; } = null!;

        public ICollection<IdentityUserBook> UsersBooks { get; set; } = new List<IdentityUserBook>();
    }

}
