using TwinFinder.Base.Graph;

namespace TwinFinder.Matching.MatchingData.Model
{
    public class MatchPair<T> : Edge<T>
    {
        protected MatchPair()
        {
        }

        public MatchPair(T from, T to, int cost) : base(from, to, cost, Direction.Undirected)
        {
        }
    }
}
