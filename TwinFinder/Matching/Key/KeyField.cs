using TwinFinder.Base.Model;
using TwinFinder.Matching.StringPhoneticKey.Base;

namespace TwinFinder.Matching.Key
{
    using System;
    using TwinFinder.Matching.StringPhoneticKey;

    public class KeyField : Field
    {
        // ***********************Fields***********************

        private Type datatype = typeof(string);

        private StringPhoneticKeyBuilder generator = new SimpleTextKey();

        // ***********************Constructor***********************

        public KeyField()
        {
        }

        public KeyField(string name)
        {
            this.Name = name;
        }

        // ***********************Properties***********************

        public Type DataType
        {
            get { return this.datatype; }
            set { this.datatype = value; }
        }

        public StringPhoneticKeyBuilder Generator
        {
            get { return this.generator; }
            set { this.generator = value; }
        }
    }
}
