using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MEDIT.Models
{
    public class ProgressTrackings
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        public int CompletedSessionsCount { get; set; } = 0;  // <-- NEW field: total sessions completed

        public DateTime? CompletedOn { get; set; }  // Optional: to know when the last session was completed
    }
}
