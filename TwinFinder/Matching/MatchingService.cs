using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using TwinFinder.Base.Extensions;
using TwinFinder.Base.Utils;
using TwinFinder.Matching.Compare;
using TwinFinder.Matching.Compare.Algorithm.Base;

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

        public int CompareRecords(Dictionary<string, string> record1, Dictionary<string, string> record2, CompareDefinitionGroup compareDefinitionGroup, out string explainPlan)
        {
            if (compareDefinitionGroup == null) { throw new ArgumentNullException("compareDefinitionGroup"); }
            if (compareDefinitionGroup.CompareDefinitions == null) { throw new ArgumentNullException("compareDefinitionGroup.CompareDefinitions"); }
            if (record1 == null) { throw new ArgumentNullException("record1"); }
            if (record2 == null) { throw new ArgumentNullException("record2"); }

            var table1 = record1.ToDataTable();
            var table2 = record2.ToDataTable();

            return this.CompareRecords(table1, table2, compareDefinitionGroup, out explainPlan);
        }

        public int CompareRecords(DataTable table1, DataTable table2, CompareDefinitionGroup compareDefinitionGroup, out string explainPlan)
        {
            explainPlan = "";

            if (compareDefinitionGroup == null) { throw new ArgumentNullException("compareDefinitionGroup"); }
            if (compareDefinitionGroup.CompareDefinitions == null) { throw new ArgumentNullException("compareDefinitionGroup.CompareDefinitions"); }
            if (table1 == null) { throw new ArgumentNullException("table1"); }
            if (table2 == null) { throw new ArgumentNullException("table2"); }

            int maxMatchScore = 0;

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

                maxMatchScore = Math.Max(match.Cost, maxMatchScore);

                explainPlan = explainPlan.Append("--------------------------------");
            }

            //Debug.WriteLine("best result: " + maxMatchScore);
            explainPlan = explainPlan.Append("best result: " + maxMatchScore);

            return maxMatchScore;
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
    }
}