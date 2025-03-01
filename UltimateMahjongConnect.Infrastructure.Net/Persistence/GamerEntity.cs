namespace UltimateMahjongConnect.Infrastructure.Persistence
{
    public class GamerEntity
    {
        public int Id { get; set; }
        public string Pseudonyme { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string BankDetails { get; set; }

        public int Age { get; set; } 
        public int? Score { get; set; } 
    }
}
