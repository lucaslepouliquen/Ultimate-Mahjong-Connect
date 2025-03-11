[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public sealed class TileRepetitionAttribute : Attribute
{
    public int Repetitions { get; }

    public TileRepetitionAttribute(int repetitions)
    {
        Repetitions = repetitions;
    }
}
