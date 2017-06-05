using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TwinFinder.Base.Extensions;
using TwinFinder.Base.Utils;
using TwinFinder.Matching.Compare;
using TwinFinder.Matching.Compare.Algorithm.Base;
using TwinFinder.Matching.StringFuzzyCompare.Aggregators.Base;
using TwinFinder.Matching.StringFuzzyCompare.Base;

namespace TwinFinder.Matching
{
    public class MatchingService
    {
        private static MatchingService instance = new MatchingService();

        private MatchingService()
        {
        }

        public static MatchingService Instance
        {
            get { return instance; }
        }

        public void SaveCompareDefinitionGroup(string fileName, CompareDefinitionGroup compareDefinitionGroup)
        {
            var serializer = new XmlSerializerHelper<CompareDefinitionGroup>();
            serializer.FileName = fileName;
            serializer.Save(compareDefinitionGroup);
        }

        public CompareDefinitionGroup LoadCompareDefinitionGroup(string fileName)
        {
            var serializer = new XmlSerializerHelper<CompareDefinitionGroup>();
            serializer.FileName = fileName;
            return serializer.Load();
        }

        public float CompareRecords(object obj1, object obj2, CompareDefinitionGroup compareDefinitionGroup, out string explainPlan)
        {
            if (obj1 == null) { throw new ArgumentNullException("obj1"); }
            if (obj2 == null) { throw new ArgumentNullException("obj2"); }

            var record1 = new Dictionary<string, string>();
            var record2 = new Dictionary<string, string>();

            if (compareDefinitionGroup == null)
            {
                // build a group from annotations
                compareDefinitionGroup = this.GenerateCompareDefinitionGroup(obj1);
            }

            // get values
            foreach (var compareDefinition in compareDefinitionGroup.CompareDefinitions)
            {
                foreach (var compareField in compareDefinition.CompareFields)
                {
                    var value1 = ReflectionUtil.GetPropertyValue(obj1, compareField.Name1);
                    record1.Add(compareField.Name1, value1.ToStringOrEmpty());

                    var value2 = ReflectionUtil.GetPropertyValue(obj2, compareField.Name2);
                    record2.Add(compareField.Name2, value2.ToStringOrEmpty());
                }
            }

            return this.CompareRecords(record1, record2, compareDefinitionGroup, out explainPlan);
        }

        public float CompareRecords(Dictionary<string, string> record1, Dictionary<string, string> record2, CompareDefinitionGroup compareDefinitionGroup, out string explainPlan)
        {
            if (record1 == null) { throw new ArgumentNullException("record1"); }
            if (record2 == null) { throw new ArgumentNullException("record2"); }

            var table1 = record1.ToDataTable();
            var table2 = record2.ToDataTable();

            return this.CompareRecords(table1, table2, compareDefinitionGroup, out explainPlan);
        }

        public float CompareRecords(DataTable table1, DataTable table2, CompareDefinitionGroup compareDefinitionGroup, out string explainPlan)
        {
            explainPlan = "";

            if (table1 == null) { throw new ArgumentNullException("table1"); }
            if (table2 == null) { throw new ArgumentNullException("table2"); }
            if (compareDefinitionGroup == null) { throw new ArgumentNullException("compareDefinitionGroup"); }
            if (compareDefinitionGroup.CompareDefinitions == null) { throw new ArgumentNullException("compareDefinitionGroup.CompareDefinitions"); }

            var matchScores = new List<float>();
            float finalMatchScore = 0;

            table1.Columns.Add("Key");
            table2.Columns.Add("Key");

            var compareAlgorithm = GenericFactory.GetInstance<CompareAlgorithm>("SortedNeighborHood");
            compareAlgorithm.Table1 = table1;
            compareAlgorithm.Table2 = table2;

            // no comparedefinition, generate a comparedefinition with all fields
            if (!compareDefinitionGroup.CompareDefinitions.Any())
            {
                var compareDefinition = this.GenerateDefaultCompareDefinition(table1, table2);
                compareDefinitionGroup.CompareDefinitions.Add(compareDefinition);
            }

            foreach (var compareDefinition in compareDefinitionGroup.CompareDefinitions)
            {
                compareAlgorithm.Matches.Clear();
                compareAlgorithm.PossibleMatches.Clear();

                compareAlgorithm.RowComparer.CompareDefinition = compareDefinition;

                compareAlgorithm.KeyFields1.Clear();
                compareAlgorithm.KeyFields1.Add("Key");

                compareAlgorithm.KeyFields2.Clear();
                compareAlgorithm.KeyFields2.Add("Key");

                compareAlgorithm.MatchLimit = 0;
                compareAlgorithm.PossibleMatchLimit = 0;
                compareAlgorithm.Execute();

                var match = compareAlgorithm.Matches.First();

                explainPlan = explainPlan.Append("-----Group: " + compareDefinition.Name + "------");

                var explainations = compareAlgorithm.RowComparer.Explain(match.From.Value, match.To.Value);

                explainPlan = explainPlan.Append(string.Join(Environment.NewLine, explainations));
                explainPlan = explainPlan.Append("Result = " + match.Cost + " (" + compareDefinition.Aggregator.ToStringOrEmpty() + ")");
                explainPlan = explainPlan.Append("----------------------------------------");

                matchScores.Add((float)match.Cost);
            }

            finalMatchScore = compareDefinitionGroup.Aggregator.AggregatedSimilarity(matchScores.ToArray());
            explainPlan = explainPlan.Append("final result = " + finalMatchScore + " (" + compareDefinitionGroup.Aggregator.ToStringOrEmpty() + ")");

            return finalMatchScore;
        }

        private CompareDefinition GenerateDefaultCompareDefinition(DataTable table1, DataTable table2)
        {
            var compareDefinition = new CompareDefinition();

            // take all fields
            var fields1 = table1.Columns
                                   .Cast<DataColumn>()
                                   .Select(x => x.ColumnName)
                                   .ToList();
            var fields2 = table2.Columns
                                   .Cast<DataColumn>()
                                   .Select(x => x.ColumnName)
                                   .ToList();
            for (int i = 0; i < fields1.Count; i++)
            {
                compareDefinition.CompareFields.Add(new CompareField(fields1[i], fields2[i]));
            }

            return compareDefinition;
        }

        private CompareDefinitionGroup GenerateCompareDefinitionGroup(object obj1)
        {
            var compareDefinitionGroup = new CompareDefinitionGroup();

            // read atrribute on Class
            var matchingAttr = obj1.GetType().GetCustomAttributes(typeof(MatchingAttribute), false).FirstOrDefault() as MatchingAttribute;
            if (matchingAttr != null)
            {
                compareDefinitionGroup.Aggregator = (Aggregator)Activator.CreateInstance(matchingAttr.Aggregator);
            }

            foreach (var prop in obj1.GetType().GetProperties())
            {
                var fieldAttr = prop.GetCustomAttributes(typeof(MatchingFieldAttribute), false).FirstOrDefault() as MatchingFieldAttribute;
                if (fieldAttr != null)
                {
                    // CompareDefinition
                    var compareDef = compareDefinitionGroup.CompareDefinitions.FirstOrDefault(x => x.Name == fieldAttr.CompareDefinition);
                    if (compareDef == null)
                    {
                        compareDef = new CompareDefinition() { Name = fieldAttr.CompareDefinition };

                        compareDefinitionGroup.CompareDefinitions.Add(compareDef);
                    }

                    // CompareField
                    var compareField = compareDef.CompareFields.FirstOrDefault(x => x.Name1 == prop.Name);
                    if (compareField == null)
                    {
                        compareField = new CompareField()
                        {
                            Name1 = prop.Name,
                            Name2 = prop.Name,
                            FuzzyComparer = (StringFuzzyComparer)Activator.CreateInstance(fieldAttr.FuzzyComparer)
                        };
                        compareDef.CompareFields.Add(compareField);
                    }
                }
            }
            return compareDefinitionGroup;
        }
    }

    public class MatchingFieldAttribute : Attribute
    {
        public string CompareDefinition { get; set; }
        public Type FuzzyComparer { get; set; }
    }

    public class MatchingAttribute : Attribute
    {
        public Type Aggregator { get; set; }
    }
}