using System.ComponentModel.DataAnnotations;

namespace MEDIT.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Full Name")]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        // Uncomment and implement these if needed
        public ICollection<Subscription>? Subscriptions { get; set; }
        public ICollection<ProgressTrackings>? ProgressTrackings { get; set; }
    }
}