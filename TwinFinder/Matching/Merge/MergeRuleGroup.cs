using System.Collections.Generic;

namespace TwinFinder.Matching.Merge
{
    public class MergeRuleGroup
    {
        private List<MergeRule> mergeRules = new List<MergeRule>();

        public List<MergeRule> MergeRules
        {
            get { return this.mergeRules; }
            set { this.mergeRules = value; }
        }
    }
}