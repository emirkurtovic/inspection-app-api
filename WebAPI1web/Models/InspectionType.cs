using System.ComponentModel.DataAnnotations;

namespace WebAPI1web.Models
{
    public class InspectionType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        //veza sa Inspection
        public List<Inspection>? Inspections {get; set;}
    }
}
