namespace DMF.Matching.Compare.RowCompare
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using DMF.Base;
    using DMF.Matching.StringFuzzyCompare;
    using DMF.Matching.StringFuzzyCompare.Aggregators;

    /// <summary>
    /// StandardRowComparer for comparing two rows/records.
    /// Loops through all fields and compares each. 
    /// Then all similarities are aggregated together (including their weight) to one similarity, which is returned
    /// </summary>
    public class StandardRowComparerTest : RowComparer
    {
        private StringFuzzyComparer identityComparer = new Identity();

        public override float Compare(DataRow row1, DataRow row2)
        {
            if (this.CompareDefinition.CompareFields.Count == 0
             || this.CompareDefinition.CompareFields2.Count == 0)
            {
                return 0.0f;
            }

            if (this.CompareDefinition.CompareFields.Count != this.CompareDefinition.CompareFields2.Count)
            {
                throw new Exception("The comparedefinitions need the same number of columns");
            }

            float[] similarities;
            float[] weights;

            // compare all fields and return the similarities and the weights
            this.CompareFields(row1, row2, out similarities, out weights);

            // aggregate all single similarities with their weights together to one similarity
            if (this.CompareDefinition.Aggregator == null)
            {
                throw new ArgumentNullException("Aggregator", "Please specify a aggregator for the rowcomparer '" + this.ToString() + "'");
            }
          
            float similarity = this.CompareDefinition.Aggregator.AggregatedSimilarity(similarities, weights);
            
            return similarity;
        }

        private void CompareFields(DataRow row1, DataRow row2, out float[] similarities, out float[] weights)
        {
            similarities = new float[this.CompareDefinition.CompareFields.Count];
            weights = new float[this.CompareDefinition.CompareFields.Count];

            // loop through all stopfields in the definition and check, if one stops the further comparision
            for (int i = 0; i < this.CompareDefinition.StopFields.Count; i++)
            {
                var stopField1 = this.CompareDefinition.StopFields[i];
                var stopField2 = this.CompareDefinition.StopFields2[i];

                // check if the stopfields are identically, otherwise, the two rows cannot be identically
                if (identityComparer.Compare(row1.Field<string>(stopField1.Name),
                                             row2.Field<string>(stopField2.Name)) < 0.9f)
                {
                    // further comparision stopped
                    return;
                }
            }                

            // loop through all fields in the definition and compare them
            for (int i = 0; i < this.CompareDefinition.CompareFields.Count; i++)
            {
                var compareField1 = this.CompareDefinition.CompareFields[i];
                var compareField2 = this.CompareDefinition.CompareFields2[i];

                // compare field1 <-> field2
                similarities[i] = compareField1.Comparer.Compare(row1.Field<string>(compareField1.Name),
                                                                 row2.Field<string>(compareField2.Name));
                // set the weight (how important is the field)
                weights[i] = compareField1.Weight;
                i++;
            }
        }

        public Dictionary<CompareField,float> Explain(DataRow row1, DataRow row2)
        {
            var result = new Dictionary<CompareField, float>();
            float[] similarities;
            float[] weights;

            this.CompareFields(row1, row2, out similarities, out weights);

            int i = 0;
            foreach (CompareField field in this.CompareDefinition.CompareFields)
            {
                result.Add(field, similarities[i]);
                i++;
            }

            return result;
        }
    }
}
