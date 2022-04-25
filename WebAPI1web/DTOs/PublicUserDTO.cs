using System.ComponentModel.DataAnnotations;

namespace WebAPI1web.DTOs
{
    public class PublicUserDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [StringLength(300)]
        public string About { get; set; }
    }
}
