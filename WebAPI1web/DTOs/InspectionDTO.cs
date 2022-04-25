using System.ComponentModel.DataAnnotations;

namespace WebAPI1web.DTOs
{
    public class InspectionDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [RegularExpression("^(SAT|UNSAT)$")]
        public string Status { get; set; }
        [Required]
        [StringLength(200)]
        public string Comment { get; set; }
        [Required]
        public int InspectionTypeId { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
