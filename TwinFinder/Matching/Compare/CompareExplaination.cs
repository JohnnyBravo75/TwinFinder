namespace TwinFinder.Matching.Compare
{
    public class CompareExplaination
    {
        private object value1;
        private object value2;
        private CompareField compareField;
        private float similarity;

        public CompareField CompareField
        {
            get { return this.compareField; }
            set { this.compareField = value; }
        }

        public object Value1
        {
            get { return this.value1; }
            set { this.value1 = value; }
        }

        public object Value2
        {
            get { return this.value2; }
            set { this.value2 = value; }
        }

        public float Similarity
        {
            get { return this.similarity; }
            set { this.similarity = value; }
        }

        public override string ToString()
        {
            return string.Format("{0}: '{1}' <-> {2}: '{3}' = {4}", this.CompareField.Name1, this.Value1, this.CompareField.Name2, this.Value2, this.Similarity);
            //return this.CompareField.ToStringOrEmpty() + string.Format(" '{0}' <-> '{1}' = {2}", this.Value1, this.Value2, this.Similarity);
        }
    }
}