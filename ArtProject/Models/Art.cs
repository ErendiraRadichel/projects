using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtProject.Models
{
    public class Art
    {
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Title { get; set; } = string.Empty;

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Artist { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        public string? Technique { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(30, 2)")]
        public decimal? Price { get; set; }

        public string? Movement { get; set; }

        public string? History { get; set; }

        public string ImageName { get; set; }

        [Required]
        [Display(Name = "Picture")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
