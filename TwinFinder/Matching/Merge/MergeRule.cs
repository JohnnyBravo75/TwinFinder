namespace TwinFinder.Matching.Merge
{
    public class MergeRule
    {
        private string fieldName = "";
        private MergeActions action = MergeActions.Update;

        public string FieldName
        {
            get { return this.fieldName; }
            set { this.fieldName = value; }
        }

        public MergeActions Action
        {
            get { return this.action; }
            set { this.action = value; }
        }
    }
}