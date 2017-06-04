using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwinFinder.Matching;
using TwinFinder.Matching.Compare;
using TwinFinder.Matching.StringFuzzyCompare.AddressSpecific;
using TwinFinder.Matching.StringFuzzyCompare.Aggregators;
using TwinFinder.Matching.StringFuzzyCompare.Common;

namespace TwinFinder.Sample
{
    public partial class FormMatch : Form
    {
        public FormMatch()
        {
            this.InitializeComponent();

            this.txtFirstname1.Text = "Thomas";
            this.txtSurname1.Text = "Lauzi";
            this.txtStreet1.Text = "Daimlerstr. 26";
            this.txtZip1.Text = "65197";
            this.txtCity1.Text = "Wiesbaden";
            this.txtPhone1.Text = "(+49)6722 72 39 208";

            this.txtFirstname2.Text = "T.";
            this.txtSurname2.Text = "Lauzi";
            this.txtStreet2.Text = "Daimlerstr. 26";
            this.txtZip2.Text = "65197";
            this.txtCity2.Text = "Wiesbaden-Dotzheim";
            this.txtPhone2.Text = "06722/7239208";
        }

        private void buttonMatch_Click(object sender, EventArgs e)
        {
            var defGroup = new CompareDefinitionGroup()
            {
                CompareDefinitions = new List<CompareDefinition>()
                {
                    new CompareDefinition()
                    {
                        Name = "NameDefinition",
                        Aggregator = new AverageAggregator(),
                        CompareFields = new List<CompareField>()
                        {
                            new CompareField()
                            {
                                Name1 = "Firstname",
                                Name2 = "Firstname",
                                FuzzyComparer = new NameComparer()
                            },

                            new CompareField()
                            {
                                Name1 = "Surname",
                                Name2 = "Surname",
                                FuzzyComparer = new NameComparer()
                            }
                        }
                    },

                    new CompareDefinition()
                    {
                        Name = "AddressDefinition",
                        Aggregator = new AverageAggregator(),
                        CompareFields = new List<CompareField>()
                        {
                            new CompareField()
                            {
                                Name1 = "Street",
                                Name2 = "Street",
                                FuzzyComparer = new DamerauLevenshteinDistance()
                            },

                            new CompareField()
                            {
                                Name1 = "Zip",
                                Name2 = "Zip",
                                FuzzyComparer = new Identity()
                            },

                            new CompareField()
                            {
                                Name1 = "City",
                                Name2 = "City",
                                FuzzyComparer = new CityComparer()
                            }
                        }
                    },

                    new CompareDefinition()
                    {
                        Name = "PhoneDefinition",
                        Aggregator = new AverageAggregator(),
                        CompareFields = new List<CompareField>()
                        {
                            new CompareField()
                            {
                                Name1 = "Phone",
                                Name2 = "Phone",
                                FuzzyComparer = new PhoneComparer()
                            }
                        }
                    }
                }
            };

            var record1 = new Dictionary<string, string>()
            {
                {"Firstname",this.txtFirstname1.Text},
                {"Surname",this.txtSurname1.Text},
                {"Street",this.txtStreet1.Text},
                {"Zip",this.txtZip1.Text},
                {"City",this.txtCity1.Text},
                {"Phone",this.txtPhone1.Text}
            };

            var record2 = new Dictionary<string, string>()
            {
                {"Firstname",this.txtFirstname2.Text},
                {"Surname",this.txtSurname2.Text},
                {"Street",this.txtStreet2.Text},
                {"Zip",this.txtZip2.Text},
                {"City",this.txtCity2.Text},
                {"Phone",this.txtPhone2.Text}
            };

            string explainPlan = "";
            var result = MatchingService.Instance.CompareRecords(record1, record2, defGroup, out explainPlan);
            this.txtSimiliarity.Text = result.ToString();
            this.txtExplainPlan.Text = explainPlan;
        }
    }
}