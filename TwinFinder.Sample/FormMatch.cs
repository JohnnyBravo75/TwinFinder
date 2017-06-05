﻿using System;
using System.Collections.Generic;
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
        private Address adr1;
        private Address adr2;

        public FormMatch()
        {
            this.InitializeComponent();

            this.adr1 = new Address()
            {
                Firstname = "Peter",
                Surname = "Parker",
                Street = "Hauptstr. 13a",
                Zip = "10500",
                City = "Berlin",
                Phone = "(+49)30 71 39 212"
            };

            this.adr2 = new Address()
            {
                Firstname = "P.",
                Surname = "Parker",
                Street = "Hauptstrasse 13",
                Zip = "10500",
                City = "Berlin-Kreuzberg",
                Phone = "030/7139212"
            };

            this.propertyGrid1.SelectedObject = this.adr1;
            this.propertyGrid2.SelectedObject = this.adr2;
        }

        private void buttonMatch_Click(object sender, EventArgs e)
        {
            string explainPlan = "";
            float result = 0.0f;

            var defGroup = new CompareDefinitionGroup()
            {
                Aggregator = new MaximumAggregator(),

                CompareDefinitions = new List<CompareDefinition>()
                    {
                        new CompareDefinition()
                        {
                            Name = "AddressDefinition",
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
                                },

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
                                },

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

            if (this.chkToogleMode.Checked)
            {
                result = MatchingService.Instance.CompareRecords(this.adr1, this.adr2, null, out explainPlan);
            }
            else
            {
                var record1 = new Dictionary<string, string>()
                {
                    {"Firstname", this.adr1.Firstname },
                    {"Surname", this.adr1.Surname},
                    {"Street", this.adr1.Street},
                    {"Zip", this.adr1.Zip},
                    {"City", this.adr1.City},
                    {"Phone", this.adr1.Phone}
                };

                var record2 = new Dictionary<string, string>()
                {
                    {"Firstname", this.adr2.Firstname },
                    {"Surname", this.adr2.Surname},
                    {"Street", this.adr2.Street},
                    {"Zip", this.adr2.Zip},
                    {"City", this.adr2.City},
                    {"Phone", this.adr2.Phone}
                };

                result = MatchingService.Instance.CompareRecords(record1, record2, defGroup, out explainPlan);
            }

            this.txtSimiliarity.Text = result.ToString();
            this.txtExplainPlan.Text = explainPlan;
        }
    }

    [Matching(Aggregator = typeof(MaximumAggregator))]
    public class Address
    {
        [MatchingField(CompareDefinition = "AddressDefinition", FuzzyComparer = typeof(NameComparer))]
        public string Firstname { get; set; }

        [MatchingField(CompareDefinition = "AddressDefinition", FuzzyComparer = typeof(NameComparer))]
        public string Surname { get; set; }

        [MatchingField(CompareDefinition = "AddressDefinition", FuzzyComparer = typeof(DamerauLevenshteinDistance))]
        public string Street { get; set; }

        [MatchingField(CompareDefinition = "AddressDefinition", FuzzyComparer = typeof(Identity))]
        public string Zip { get; set; }

        [MatchingField(CompareDefinition = "AddressDefinition", FuzzyComparer = typeof(CityComparer))]
        public string City { get; set; }

        [MatchingField(CompareDefinition = "AddressDefinition", FuzzyComparer = typeof(PhoneComparer))]
        public string Phone { get; set; }
    }
}