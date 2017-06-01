using System;
using System.Collections.Generic;
using System.Data;
using TwinFinder.Matching.Compare.RowCompare;
using TwinFinder.Matching.Compare.RowCompare.Base;
using TwinFinder.Matching.MatchingData.Model;

namespace TwinFinder.Matching.Compare.Algorithm.Base
{
    public abstract class CompareAlgorithm : ICompareAlgorithm
    {
        // ***********************Fields***********************

        private RowComparer rowComparer = new StandardRowComparer();

        private List<string> keyFields1 = new List<string>();

        private List<string> keyFields2 = new List<string>();

        private List<MatchPair<DataRow>> matches = new List<MatchPair<DataRow>>();

        private List<MatchPair<DataRow>> possibleMatches = new List<MatchPair<DataRow>>();

        private int matchLimit = 96;

        private int possibleMatchLimit = 85;

        // ***********************Properties***********************

        public RowComparer RowComparer
        {
            get { return this.rowComparer; }
            set { this.rowComparer = value; }
        }

        public List<string> KeyFields1
        {
            get { return this.keyFields1; }
            set { this.keyFields1 = value; }
        }

        public List<string> KeyFields2
        {
            get { return this.keyFields2; }
            set { this.keyFields2 = value; }
        }

        public int MatchLimit
        {
            get { return this.matchLimit; }
            set { this.matchLimit = value; }
        }

        public List<MatchPair<DataRow>> Matches
        {
            get { return this.matches; }
            private set { this.matches = value; }
        }

        public int PossibleMatchLimit
        {
            get { return this.possibleMatchLimit; }
            set { this.possibleMatchLimit = value; }
        }

        public List<MatchPair<DataRow>> PossibleMatches
        {
            get { return this.possibleMatches; }
            private set { this.possibleMatches = value; }
        }

        /// <summary>
        /// the datasource
        /// </summary>
        public DataTable Table1 { get; set; }

        public DataTable Table2 { get; set; }

        // ***********************Functions***********************

        protected DataView Sort1(string column)
        {
            this.Table1.DefaultView.Sort = column;
            return this.Table1.DefaultView;
        }

        protected DataView Sort2(string column)
        {
            this.Table2.DefaultView.Sort = column;
            return this.Table2.DefaultView;
        }

        public void Execute()
        {
            if (this.Table1 == null)
            {
                throw new ArgumentNullException("Table", "No input table exists.");
            }

            for (int i = 0; i < this.KeyFields1.Count; i++)
            {
                if (this.Table2 == null)
                {
                    this.Execute(this.KeyFields1[i]);
                }
                else
                {
                    this.Execute(this.KeyFields1[i], this.KeyFields2[i]);
                }
            }
        }

        protected abstract void Execute(string keyfield1);

        protected abstract void Execute(string keyfield1, string keyfield2);
    }
}