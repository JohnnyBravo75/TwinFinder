namespace TwinFinder.Matching.Compare
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

#if !SILVERLIGHT

    [Serializable]
#endif
    [DataContract]
    public class CompareDefinitionGroup
    {
        private List<CompareDefinition> compareDefinitions = new List<CompareDefinition>();

        [DataMember]
        public List<CompareDefinition> CompareDefinitions
        {
            get { return this.compareDefinitions; }
            set { this.compareDefinitions = value; }
        }
    }
}