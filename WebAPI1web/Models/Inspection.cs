using System.ComponentModel.DataAnnotations;

namespace WebAPI1web.Models
{
    public class Inspection
    {
        /*
        public Inspection(int id, string status, string comment, int inspectionTypeId, InspectionType inspectionType)
        {
            Id = id;
            Status = status;
            Comment = comment;
            InspectionTypeId = inspectionTypeId;
            InspectionType = inspectionType;
        }
        */

        [Key]
        public int Id { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        [StringLength(200)]
        public string Comment { get; set; }

        //veza sa InspectionType
        public int InspectionTypeId { get; set; }
        public InspectionType? InspectionType { get; set; }
    }
}
