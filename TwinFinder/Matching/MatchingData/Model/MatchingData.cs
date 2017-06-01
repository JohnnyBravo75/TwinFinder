namespace TwinFinder.Matching.MatchingData.Model
{
    using System.Collections.Generic;
    using TwinFinder.Matching.Key;
    using TwinFinder.Matching.Compare;

    public class MatchingData
    {
        // ***********************Fields***********************

        private List<KeyDefinition> keyDefinitions = new List<KeyDefinition>();
        private List<CompareDefinition> compareDefinitions = new List<CompareDefinition>();

        // ***********************Properties***********************

        public List<KeyDefinition> KeyDefinitions
        {
            get { return this.keyDefinitions; }
            set { this.keyDefinitions = value; }
        }

        public List<CompareDefinition> CompareDefinitions
        {
            get { return this.compareDefinitions; }
            set { this.compareDefinitions = value; }
        }

    }
}
