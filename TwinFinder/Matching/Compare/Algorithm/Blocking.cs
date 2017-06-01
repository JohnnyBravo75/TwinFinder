using System.Diagnostics;
using TwinFinder.Matching.Compare.Algorithm.Base;

namespace TwinFinder.Matching.Compare.Algorithm
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using TwinFinder.Matching.MatchingData.Model;

    /// <summary>
    /// Compares rows by the blocking algorithm.
    /// 1. The algorithm sorts the data by the keyfield
    /// 2. He takes all rows with the same key as one block (window).
    /// 3. All data in this block (window) gets compared to each other
    /// </summary>
    public class Blocking : CompareAlgorithm
    {
        // ***********************Fields***********************

        // ***********************Properties***********************

        // ***********************Functions***********************

        protected override void Execute(string keyfield1)
        {
            // sort by the keyfield e.g. "Key1" where the matchcode is in
            DataView dataView1 = this.Sort1(keyfield1);

            int windowStart = 0;

            string newKey = "";
            string oldKey = "";
            int mainIndex = 0;
            int windowSize = 0;
            List<DataRow> block = new List<DataRow>();

            while (mainIndex < dataView1.Count)
            {
                windowStart = mainIndex;
                oldKey = "";
                windowSize = 0;

                // search beginning and end of the block (when the key changes, a block(window) ends, a new starts)
                while ((oldKey == newKey || string.IsNullOrEmpty(oldKey))
                       && mainIndex < dataView1.Count)
                {
                    oldKey = newKey;
                    DataRow row = dataView1[mainIndex].Row;
                    newKey = row.Field<string>(keyfield1);

                    if (oldKey == newKey || string.IsNullOrEmpty(oldKey))
                    {
                        mainIndex++;
                        windowSize++;
                    }
                    else
                    {
                        // when the key changes, the block(window) ends
                        break;
                    }
                }

                // when no data in the window left, cancel
                if (windowSize == 0)
                {
                    break;
                }

                System.Diagnostics.Debug.WriteLine("--------Block Start: " + windowStart + " Anzahl: " + windowSize);
                if (windowSize > 1)
                {
                    // compare all data in the winow to each other
                    this.CompareAllInWindow(dataView1, windowStart, windowSize, 0);
                }
            }
        }

        protected override void Execute(string keyfield1, string keyfield2)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// All data rows in this block (window) gets compared to each other.
        /// when thy match, they are put in the match or the possible match list.
        /// </summary>
        /// <param name="dataView">The sorted data view.</param>
        /// <param name="windowStart">The window start index.</param>
        /// <param name="windowSize">Size of the window.</param>
        /// <param name="overlapping">The overlapping of the last window (prevents double checkings when the window moves over the data).</param>
        private void CompareAllInWindow(DataView dataView, int windowStart, int windowSize, int overlapping)
        {
            int start = 0;
            int i = 0;
            int index1 = 0;
            int index2 = 0;
            float similarity = 0.0f;
            int percentSimilarity = 0;
            DataRow row1;
            DataRow row2;

            // Start one window run
            // compare all rows in the window to each  other
            start = 0;
            while (start < windowSize - 1)
            {
                index1 = windowStart + start;

                // loop through the remaining rows and compare against them
                i = start + 1;
                while (i < windowSize)
                {
                    index2 = windowStart + i;

                    // Compare only, if the row wasn´t compared.
                    // 1. In the first run (mainIndex=0) everything need to be compared
                    // 2. if the second row (index2) has an index one smaller than the max (the window gets only one row shifted),
                    //    it was compared on the last window run
                    if (!(windowStart > 0 && index2 < windowStart + windowSize - Math.Abs((windowSize - overlapping))))
                    {
                        row1 = dataView[index1].Row;
                        row2 = dataView[index2].Row;

                        similarity = this.RowComparer.Compare(row1, row2);
                        percentSimilarity = (int)(similarity * 100);

                        if (percentSimilarity >= this.MatchLimit)
                        {
                            // safe doublet
                            this.Matches.Add(new MatchPair<DataRow>(row1, row2, percentSimilarity));
                        }
                        else if (percentSimilarity >= this.PossibleMatchLimit)
                        {
                            // possible doublet
                            this.PossibleMatches.Add(new MatchPair<DataRow>(row1, row2, percentSimilarity));
                        }

                        Debug.WriteLine("Compare row[" + (index1) + "] and row [" + (index2) + "] Percent: " + percentSimilarity + "%");
                    }
                    else
                    {
                        Debug.WriteLine("  --> Skipped row[" + (index1) + "] and row [" + (index2) + "]");
                    }

                    i++;
                }

                start++;
            }
        }
    }
}