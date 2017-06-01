namespace TwinFinder.Matching.Key
{
    using System;
    using System.Collections.Generic;

    public class KeyDefinition
    {
        // ***********************Fields***********************

        private string targetKeyField = "keyfield_" + new Guid().ToString();

        private List<KeyField> fields = new List<KeyField>();

        // ***********************Properties***********************

        public List<KeyField> Fields
        {
            get { return this.fields; }
            set { this.fields = value; }
        }

        public string TargetKeyField 
        {
            get { return this.targetKeyField; }
            set { this.targetKeyField = value; }
        }
    }
}
