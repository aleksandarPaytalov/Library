using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    [Comment("Books categories in the library")]
    public class Category
    {
        [Key]
        [Comment("Category identifier")]
        public int Id { get; set; }

        [Required]
        [Comment("Category name")]
        [MaxLength(DataConstants.CategoryNameMaxLength)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
