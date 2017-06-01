using TwinFinder.Matching.StringFuzzyCompare.Base;
using TwinFinder.Matching.StringFuzzyCompare.Common;

namespace TwinFinder.Matching.Compare
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

#if !SILVERLIGHT

    [Serializable]
#endif
    [DataContract]
    public class CompareField
    {
        // ***********************Fields***********************

        private float weight = 1;
        private StringFuzzyComparer fuzzyComparer = new DamerauLevenshteinDistance();
        private string name1 = "";
        private string name2 = "";
        private List<string> fieldConverters = new List<string>();

        // ***********************Constructor***********************

        public CompareField()
        {
        }

        public CompareField(string fieldName)
        {
            this.Name1 = fieldName;
            this.Name2 = fieldName;
        }

        public CompareField(string fieldName1, string fieldName2 = null, StringFuzzyComparer fuzzyComparer = null)
        {
            this.Name1 = fieldName1;
            this.Name2 = fieldName2;
            this.fuzzyComparer = fuzzyComparer;
        }

        // ***********************Properties***********************

        [DataMember]
        public string Name1
        {
            get
            {
                if (string.IsNullOrEmpty(this.name1))
                {
                    return this.name2;
                }

                return this.name1;
            }
            set { this.name1 = value; }
        }

        [DataMember]
        public string Name2
        {
            get
            {
                if (string.IsNullOrEmpty(this.name2))
                {
                    return this.name1;
                }

                return this.name2;
            }
            set { this.name2 = value; }
        }

        /// <summary>
        /// The weight (how important is the field).
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        [DataMember]
        public float Weight
        {
            get
            {
                if (this.weight == 0)
                {
                    this.weight = 1;
                }
                return this.weight;
            }
            set { this.weight = value; }
        }

        [DataMember]
        public StringFuzzyComparer FuzzyComparer
        {
            get { return this.fuzzyComparer; }
            set { this.fuzzyComparer = value; }
        }

        [DataMember]
        public List<string> FieldConverters
        {
            get { return this.fieldConverters; }
            set { this.fieldConverters = value; }
        }

        public override string ToString()
        {
            return string.Format("'{0}' <-> '{1}' (W={2})", this.Name1, this.Name2, this.Weight);
        }
    }
}