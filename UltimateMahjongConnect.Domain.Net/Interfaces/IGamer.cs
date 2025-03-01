namespace UltimateMahjongConnect.Domain.Interfaces
{
    public interface IGamer
    {
        void AddScore(int score);
        void LevelCompleted();
    }
}