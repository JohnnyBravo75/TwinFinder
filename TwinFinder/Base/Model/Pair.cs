namespace TwinFinder.Base.Model
{
    public class Pair<T, U>
    {
        public Pair()
        {
        }

        public Pair(T value1, U value2)
        {
            this.Value1 = value1;
            this.Value2 = value2;
        }

        public T Value1 { get; set; }
        public U Value2 { get; set; }
    };
}