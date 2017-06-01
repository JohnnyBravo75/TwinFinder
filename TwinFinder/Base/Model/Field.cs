using System;
using TwinFinder.Base.Model.Base;

namespace TwinFinder.Base.Model
{
    public class Field : ModelBase
    {
        private string dataType = "System.String";
        private string function = string.Empty;
        private int length = -1;
        private string name = string.Empty;

        // ***********************Constructors***********************

        /// <summary>
        /// Initializes a new instance of the <see cref="Field" /> class.
        /// </summary>
        public Field()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Field" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public Field(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Field" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="length">The length.</param>
        public Field(string name, int length)
        {
            this.Name = name;
            this.Length = length;
        }

        public Field(string name, int length, Type dataType)
        {
            this.Name = name;
            this.Length = length;
            this.Datatype = dataType.ToString();
        }

        // ***********************Properties***********************

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public int Length
        {
            get
            {
                return this.length;
            }

            set
            {
                if (this.length != value)
                {
                    this.length = value;
                    this.RaisePropertyChanged("Length");
                }
            }
        }

        /// <summary>
        /// Gets or sets the datatype in string form (e.g "System.String")
        /// </summary>
        /// <value>
        /// The datatype.
        /// </value>
        public string Datatype
        {
            get
            {
                return this.dataType;
            }

            set
            {
                if (this.dataType != value)
                {
                    this.dataType = value;
                    this.RaisePropertyChanged("Datatype");
                }
            }
        }

        /// <summary>
        /// Gets or sets the function.
        /// </summary>
        /// <value>
        /// The function.
        /// </value>
        public string Function
        {
            get
            {
                return this.function;
            }

            set
            {
                if (this.function != value)
                {
                    this.function = value;
                    this.RaisePropertyChanged("Function");
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}