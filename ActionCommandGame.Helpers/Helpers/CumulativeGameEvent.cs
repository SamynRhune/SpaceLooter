namespace ActionCommandGame.Helpers
{
    public class CumulativeGameEvent<T>
    {
        public T GameEvent { get; set; }
        public int CumulativeProbability { get; set; }
    }
}
