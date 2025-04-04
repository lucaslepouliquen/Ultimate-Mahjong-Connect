using System.ComponentModel.DataAnnotations;

namespace UltimateMahjongConnect.Application.DTO
{
    public class GamerDTO
    {
        [Range(0,int.MaxValue,ErrorMessage = "Id must be 0 or positive")]
        public int Id { get; set; }
        [Required]
        public string Pseudonyme { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Range(1, 110, ErrorMessage = "Age must be within humain range")]
        public int Age { get; set; }
        public int? Score { get; set; }
    }
}
