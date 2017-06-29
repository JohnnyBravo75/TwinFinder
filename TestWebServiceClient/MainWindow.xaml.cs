namespace TestWebServiceApplication
{
    using System;
    using System.Windows;
    using System.Collections.Generic;
    using TwinFinder.WebServiceTestClient.TwinFinderWebservice;

    public partial class MainWindow : Window
    {
        private TwinFinderWebServiceClient matchingWebServiceClient = new TwinFinderWebServiceClient();
        private Address adr1;
        private Address adr2;

        public MainWindow()
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
        }

        private void BtnCheckDoublets_Click(object sender, RoutedEventArgs e)
        {
            // create a Comparedefintion for the dynmaic data
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

            this.txtResult.Text = "";

            string sessionKey = this.matchingWebServiceClient.CreateSession("nobody", "123456");
            if (!string.IsNullOrEmpty(sessionKey))
            {
                var result = this.matchingWebServiceClient.CompareRecords(sessionKey, record1, record2, defGroup);
                this.matchingWebServiceClient.CloseSession(sessionKey);

                this.txtResult.Text = this.adr1.ToString() + Environment.NewLine +
                                        Environment.NewLine +
                                        this.adr2.ToString() + Environment.NewLine +
                                        "Result=" + result;
            }
            else
            {
                this.txtResult.Text = "No session.";
            }
        }
    }

    public class Address
    {
        public string Firstname { get; set; }

        public string Surname { get; set; }

        public string Street { get; set; }

        public string Zip { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }

        public override string ToString()
        {
            return
                "Firstname=" + this.Firstname + Environment.NewLine +
                "Surname=" + this.Surname + Environment.NewLine +
                "Street=" + this.Street + Environment.NewLine +
                "Zip=" + this.Zip + Environment.NewLine +
                "City=" + this.City + Environment.NewLine +
                "Phone=" + this.Phone + Environment.NewLine;
        }
    }
}