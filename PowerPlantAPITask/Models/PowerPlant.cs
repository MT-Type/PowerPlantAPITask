using System;
using System.ComponentModel.DataAnnotations;

namespace PowerPlantAPITask.Models
{
    public class PowerPlant
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-zÀ-ž]+\s[A-Za-zÀ-ž]+$", ErrorMessage = "Given \"owner\" value must consist of two words (text-only characters) separated by a whitespace")]
        public string Owner { get; set; }

        [Required]
        [Range(0, 200, ErrorMessage = "Given power value must not be below 0 or exceed 200")]
        public decimal Power { get; set; }

        [Required]
        public DateOnly ValidFrom { get; set; }

        public DateOnly? ValidTo { get; set; }
    }
}
