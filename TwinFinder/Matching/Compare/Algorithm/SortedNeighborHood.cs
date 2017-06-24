using System.Diagnostics;
using TwinFinder.Matching.Compare.Algorithm.Base;

namespace TwinFinder.Matching.Compare.Algorithm
{
    using System;
    using System.Data;
    using TwinFinder.Matching.MatchingData.Model;

    /// <summary>
    /// Compares rows by the SortedNeighborHood algorithm.
    /// 1. The algorithm sorts the data by the keyfield
    /// 2. All data in a block (window) of a fixed size gets compared to each other
    /// 3. The window is moved one postion down. Goto step 2 until the end is reached.
    /// </summary>
    public class SortedNeighborHood : CompareAlgorithm
    {
        // ***********************Fields***********************

        private int windowSize = 20;

        // ***********************Properties***********************

        public int WindowSize
        {
            get { return this.windowSize; }
            set { this.windowSize = value; }
        }

        // ***********************Functions***********************

        protected override void Execute(string keyfield1)
        {
            // sort by the keyfield e.g. "Key1" where the matchcode is in
            DataView dataView1 = this.Sort1(keyfield1);

            System.Diagnostics.Debug.WriteLine(Environment.NewLine + "  ----- Intracheck: " + dataView1.Table.TableName + Environment.NewLine);

            int mainIndex = 0;
            int viewCount = dataView1.Count;

            // is window bigger than the number of records?
            if (this.windowSize > viewCount)
            {
                this.windowSize = viewCount;
            }

            int mainCount = viewCount - this.windowSize + 1;

            while (mainIndex < mainCount)
            {
                System.Diagnostics.Debug.WriteLine("--------Row[" + mainIndex + "]");

                this.CompareAllInWindow(dataView1, mainIndex, this.windowSize, this.windowSize - 1);

                mainIndex++;
            }
        }

        protected override void Execute(string keyfield1, string keyfield2)
        {
            // sort by the keyfield e.g. "Key1" where the matchcode is in
            DataView dataView1 = this.Sort1(keyfield1);
            DataView dataView2 = this.Sort2(keyfield2);

            System.Diagnostics.Debug.WriteLine(Environment.NewLine + "  ----- Intercheck: " + dataView1.Table.TableName + "(" + dataView1.Table.Rows.Count + ") and " + dataView2.Table.TableName + "(" + dataView2.Table.Rows.Count + ")" + Environment.NewLine);

            int mainIndex = 0;
            float rowIndex1 = 0;
            float rowIndex2 = 0;

            float lastIndex1 = 0;
            float lastIndex2 = 0;

            bool hasMoved1 = false;
            bool hasMoved2 = false;

            float step1 = 0;
            float step2 = 0;

            int viewCount = 0;

            if (dataView1.Count > dataView2.Count)
            {
                step1 = 1;
                step2 = (float)(dataView2.Count - this.windowSize) / (float)(dataView1.Count - this.windowSize);
                viewCount = dataView1.Count;
            }
            else
            {
                step1 = (float)(dataView1.Count - this.windowSize) / (float)(dataView2.Count - this.windowSize);
                step2 = 1;
                viewCount = dataView2.Count;
            }

            // is window bigger than the number of records?
            if (this.windowSize > viewCount)
            {
                this.windowSize = viewCount;
            }

            int mainCount = viewCount - this.windowSize + 1;

            while (mainIndex < mainCount)
            {
                System.Diagnostics.Debug.WriteLine("--------Row[" + mainIndex + "]");

                hasMoved1 = (((int)rowIndex1 - lastIndex1) > 0) ? true
                                                                : false;

                hasMoved2 = (((int)rowIndex2 - lastIndex2) > 0) ? true
                                                                : false;

                this.CompareAllInWindow(dataView1, (int)rowIndex1, this.windowSize, hasMoved1,
                                        dataView2, (int)rowIndex2, this.windowSize, hasMoved2);

                lastIndex1 = rowIndex1;
                lastIndex2 = rowIndex2;

                mainIndex += 1;
                rowIndex1 += step1;
                rowIndex2 += step2;
            }
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
            int start1 = 0;
            int start2 = 0;
            int index1 = 0;
            int index2 = 0;
            float similarity = 0.0f;
            int percentSimilarity = 0;
            int windowEnd = windowStart + windowSize;
            DataRow row1;
            DataRow row2;

            // Start one window run
            // compare all rows in the window to each  other
            start1 = 0;
            while (start1 < windowSize - 1)
            {
                index1 = windowStart + start1 - 1;

                // loop through the remaining rows and compare against them
                start2 = start1 + 1;
                while (start2 < windowSize)
                {
                    index2 = windowStart + start2 - 1;

                    // Compare only, if the row wasn´t compared.
                    // 1. In the first run (mainIndex=0) everything need to be compared
                    // 2. if the second row (index2) has an index one smaller than the max (the window gets only one row shifted or windowsize minus overlapping),
                    //    it was compared on the last window run
                    if (!(windowStart > 0 && index2 < windowEnd - Math.Abs((windowSize - overlapping))))
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

                        System.Diagnostics.Debug.WriteLine("Compare row[" + (index1) + "] and row [" + (index2) + "] Percent: " + percentSimilarity + "%");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("  --> Skipped row[" + (index1) + "] and row [" + (index2) + "]");
                    }

                    start2++;
                }

                start1++;
            }
        }

        private void CompareAllInWindow(DataView dataView1, int windowStart1, int windowSize1, bool hasMoved1,
                                        DataView dataView2, int windowStart2, int windowSize2, bool hasMoved2)
        {
            int start1 = 0;
            int start2 = 0;
            int index1 = 0;
            int index2 = 0;
            float similarity = 0.0f;
            int percentSimilarity = 0;
            int windowEnd1 = windowStart1 + this.windowSize;
            int windowEnd2 = windowStart2 + this.windowSize;
            DataRow row1;
            DataRow row2;

            // Start one window run
            // compare all rows in the window to each  other
            start1 = windowSize1;
            while (start1 > 0)
            {
                index1 = windowStart1 + start1;

                start2 = windowSize2;
                while (start2 > 0)
                {
                    index2 = windowStart2 + start2;

                    // 1. beide Fenster am Anfang, dann alle Datensätze innerhalb des Fensters vergleichen
                    // 2. oder der letzte (neue) Datensatz in Fenster1 und/oder Fenster2
                    if ((windowStart1 == 0 && windowStart2 == 0)
                        || (index1 == windowEnd1 && hasMoved1)
                        || (index2 == windowEnd2 && hasMoved2))
                    {
                        row1 = dataView1[index1 - 1].Row;
                        row2 = dataView2[index2 - 1].Row;

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
                        break;
                        // System.Diagnostics.Debug.WriteLine("  --> Skipped row[" + (index1) + "] and row [" + (index2) + "]");
                    }

                    start2--;
                }

                start1--;
            }
        }
    }
}