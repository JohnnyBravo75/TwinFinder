using System;
using TwinFinder.Matching.StringFuzzyCompare.Aggregators.Base;
using TwinFinder.Matching.StringFuzzyCompare.Base;

namespace TwinFinder.Matching.Compare
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using TwinFinder.Matching.StringFuzzyCompare.Aggregators;

    /// <summary>
    /// Comparedefinition, which fields need to be compared, and how a aggregated similarity is build
    /// </summary>

#if !SILVERLIGHT

    [Serializable]
#endif
    [DataContract]
    public class CompareDefinition
    {
        // ***********************Fields***********************

        private List<CompareField> compareFields = new List<CompareField>();

        private List<CompareField> stopFields = new List<CompareField>();

        private Aggregator aggregator = new AverageAggregator();

        private string name = "Default";

        // ***********************Constructors***********************

        public CompareDefinition()
        {
        }

        public CompareDefinition(Aggregator fieldAggregator, StringFuzzyComparer fuzzyComparer = null)
        {
            this.aggregator = fieldAggregator;
            foreach (var compareField in this.compareFields)
            {
                compareField.FuzzyComparer = fuzzyComparer;
            }
        }

        // ***********************Properties***********************

        /// <summary>
        /// Fields, which get compared
        /// </summary>
        [DataMember]
        public List<CompareField> CompareFields
        {
            get
            {
                return this.compareFields;
            }
            private set { this.compareFields = value; }
        }

        /// <summary>
        /// Stopfields which prevent to be a doublet, e.g. language code
        /// </summary>
        [DataMember]
        public List<CompareField> StopFields
        {
            get
            {
                return this.stopFields;
            }
            private set { this.stopFields = value; }
        }

        /// <summary>
        /// Aggregator for aggregating the similarities of each compared field to one aggregated similarity
        /// </summary>
        [DataMember]
        public Aggregator Aggregator
        {
            get
            {
                if (this.aggregator == null)
                {
                    this.aggregator = new AverageAggregator();
                }

                return this.aggregator;
            }
            set { this.aggregator = value; }
        }

        [DataMember]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
    }
}