using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace TwinFinder.Matching.Compare.RowCompare.Base
{
    /// <summary>
    /// Rowcomparer for comparing two rows/records
    /// </summary>
#if !SILVERLIGHT

    [Serializable]
#endif
    [DataContract]
    public abstract class RowComparer : IRowComparer
    {
        // ***********************Fields***********************

        private CompareDefinition compareDefinition = new CompareDefinition();

        // ***********************Properties***********************

        public CompareDefinition CompareDefinition
        {
            get { return this.compareDefinition; }
            set { this.compareDefinition = value; }
        }

        // ***********************Functions***********************

        public abstract float Compare(DataRow row1, DataRow row2);

        public abstract IList<CompareExplaination> Explain(DataRow row1, DataRow row2);
    }
}