namespace UltimateMahjongConnect.Domain.Models
{
    public class MahjongPath
    {
        public MahjongPath()
        {
            IsValid = false;
            PathRows = new List<int>();
            PathColumns = new List<int>();
        }
        public bool IsValid { get; set; }
        public List<int> PathRows { get; set; }
        public List<int> PathColumns
        {
            get; set;
        }
    }
}
