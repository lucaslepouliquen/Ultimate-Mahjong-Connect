namespace UltimateMahjongConnect.Business.Models
{
    public class ValidatedPath
    {
        public ValidatedPath()
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
