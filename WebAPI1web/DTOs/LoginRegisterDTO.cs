using System.ComponentModel.DataAnnotations;

namespace WebAPI1web.DTOs
{
    public class LoginRegisterDTO
    {
        [Required]
        [StringLength(20,MinimumLength =4)]
        [RegularExpression(@"^[a-zA-Z0-9]+$")]
        public string username { get; set; }
        [Required]
        [StringLength(25,MinimumLength =4)]
        [RegularExpression(@"^[a-zA-Z0-9%~!@#=&_:;,`\$\^\*\+\|]+$")]
        public string password { get; set; }
        //Special characters (~!@#$%^&*_-+=`|\(){}[]:;'<>,.?/)
    }
}
