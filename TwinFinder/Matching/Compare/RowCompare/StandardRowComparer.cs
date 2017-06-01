using TwinFinder.Matching.Compare.RowCompare.Base;
using TwinFinder.Matching.StringFuzzyCompare.Base;
using TwinFinder.Matching.StringFuzzyCompare.Common;

namespace TwinFinder.Matching.Compare.RowCompare
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// StandardRowComparer for comparing two rows/records.
    /// Loops through all fields and compares each.
    /// Then all similarities are aggregated together (including their weight) to one similarity, which is returned
    /// </summary>
    public class StandardRowComparer : RowComparer
    {
        private StringFuzzyComparer identityComparer = new Identity();

        public override float Compare(DataRow row1, DataRow row2)
        {
            if (this.CompareDefinition.CompareFields.Count == 0)
            {
                return 0.0f;
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
            if (this.CompareDefinition.StopFields != null)
            {
                foreach (var stopField in this.CompareDefinition.StopFields)
                {
                    // check if the stopfields are identically, otherwise, the two rows cannot be identically
                    if (this.identityComparer.Compare(row1.Field<string>(stopField.Name1),
                                                 row2.Field<string>(stopField.Name2)) >= 0.9f)
                    {
                        // further comparision stopped
                        return;
                    }
                }
            }

            if (this.CompareDefinition.CompareFields != null)
            {
                // loop through all fields in the definition and compare them
                int i = 0;
                foreach (var compareField in this.CompareDefinition.CompareFields)
                {
                    var value1 = row1.Field<object>(compareField.Name1);
                    var value2 = row2.Field<object>(compareField.Name2);

                    // first convert
                    //foreach (var fieldConverterName in compareField.FieldConverters)
                    //{
                    //    var fieldConverter = FieldConverterFactory.TryGetInstance(fieldConverterName);

                    //    if (fieldConverter != null)
                    //    {
                    //        value1 = fieldConverter.Convert(value1, countryCode: "");
                    //        value2 = fieldConverter.Convert(value2, countryCode: "");

                    //        fieldConverter.Dispose();
                    //    }
                    //}

                    // compare field1 <-> field2
                    similarities[i] = compareField.FuzzyComparer.Compare(value1 as string, value2 as string);

                    // set the weight (how important is the field)
                    weights[i] = compareField.Weight;
                    i++;
                }
            }
        }

        public override IList<CompareExplaination> Explain(DataRow row1, DataRow row2)
        {
            var result = new List<CompareExplaination>();
            float[] similarities;
            float[] weights;

            this.CompareFields(row1, row2, out similarities, out weights);

            int i = 0;
            foreach (var compareField in this.CompareDefinition.CompareFields)
            {
                result.Add(new CompareExplaination()
                {
                    CompareField = compareField,
                    Similarity = similarities[i],
                    Value1 = row1.Field<object>(compareField.Name1),
                    Value2 = row2.Field<object>(compareField.Name2)
                });
                i++;
            }

            return result;
        }
    }
}