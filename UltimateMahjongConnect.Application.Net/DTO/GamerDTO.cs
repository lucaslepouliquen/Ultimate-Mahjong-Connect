using System.ComponentModel.DataAnnotations;

namespace UltimateMahjongConnect.Application.DTO
{
    public class GamerDTO
    {
        public int Id { get; set; }
        [Required]
        public string Pseudonyme { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int Age { get; set; }
        public int? Score { get; set; }
    }
}
