namespace TwinFinder.Matching.Key
{
    using System;
    using System.Data;

    public class KeyGenerator
    {
        // ***********************Fields***********************

        private KeyDefinition keyDefinition = new KeyDefinition();

        // ***********************Properties***********************

        public DataTable Table { get; set; }

        public KeyDefinition KeyDefinition
        {
            get { return this.keyDefinition; }
            set { this.keyDefinition = value; }
        }

        // ***********************Functions***********************

        public void GenerateKey()
        {
            if (this.Table == null)
            {
                throw new ArgumentNullException("Table", "Table must not be empty");
            }

            if (string.IsNullOrEmpty(this.KeyDefinition.TargetKeyField))
            {
                return;
            }

            // add keyfield (if not exists)
            if (!this.Table.Columns.Contains(this.KeyDefinition.TargetKeyField))
            {
                this.Table.Columns.Add(this.KeyDefinition.TargetKeyField);
            }

            // loop through all rows
            foreach (DataRow row in this.Table.Rows)
            {
                // build key from all fields
                string key = string.Empty;

                foreach (KeyField field in this.KeyDefinition.Fields)
                {
                    if (row.Table.Columns.Contains(field.Name))
                    {
                        string value = row.Field<string>(field.Name);

                        if (field.DataType == typeof(string))
                        {
                            // build a stringkey / matchcode from the field value e.g. "Müller" -> "MLR"
                            key += field.Generator.BuildKey(value);
                        }
                    }
                    else
                    {
                        throw new ArgumentException("The source key field '" + field.Name + "' was not found in the datasource.");
                    }
                }

                // set the key in the tgt keyfield
                row[this.KeyDefinition.TargetKeyField] = key;
            }
        }
    }
}