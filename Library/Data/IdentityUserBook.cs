using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    public class IdentityUserBook
    {
        [Required]
        [Comment("Collector identifier")]
        [ForeignKey(nameof(Collector))]
        public string CollectorId { get; set; } = string.Empty;

        public IdentityUser Collector { get; set; } = null!;

        [Required]
        [Comment("Book identifier")]
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }

        public Book Book { get; set; } = null!;
    }
}
