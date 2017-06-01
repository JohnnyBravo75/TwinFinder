namespace TwinFinder.Base.Extensions
{
    /// <summary>
    /// Extends any instance of System.Math
    /// </summary>
    public static class MathExtension
    {
        public static float Max(params float[] p)
        {
            if (p == null) { return float.MaxValue; }

            float max = float.MinValue;
            for (int i = 0; i < p.Length; i++)
            {
                if (max < p[i])
                {
                    max = p[i];
                }
            }

            return max;
        }

        public static int Max(params int[] p)
        {
            if (p == null) { return int.MaxValue; }

            int max = int.MinValue;
            for (int i = 0; i < p.Length; i++)
            {
                if (max < p[i])
                {
                    max = p[i];
                }
            }

            return max;
        }

        public static float Min(params float[] p)
        {
            if (p == null) { return float.MinValue; }

            float min = float.MaxValue;
            for (int i = 0; i < p.Length; i++)
            {
                if (min > p[i])
                {
                    min = p[i];
                }
            }

            return min;
        }

        public static int Min(params int[] p)
        {
            if (p == null) { return int.MinValue; }

            int min = int.MaxValue;
            for (int i = 0; i < p.Length; i++)
            {
                if (min > p[i])
                {
                    min = p[i];
                }
            }
            return min;
        }

        public static int Square(int p)
        {
            return p * p;
        }

        public static float Square(float p)
        {
            return p * p;
        }
    }
}