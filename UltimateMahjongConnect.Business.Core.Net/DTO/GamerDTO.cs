namespace UltimateMahjongConnect.Service.DTO
{
    public class GamerDTO
    {
        public int Id { get; set; }
        public string Pseudonyme { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public int? Score { get; set; }
    }
}
