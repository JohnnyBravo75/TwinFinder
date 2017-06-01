using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TwinFinder.Matching.Compare;
using TwinFinder.Matching.Key;
using TwinFinder.Matching.StringFuzzyCompare.Aggregators.Base;
using TwinFinder.Matching.StringFuzzyCompare.Base;
using TwinFinder.Matching.StringPhoneticKey.Base;

namespace TwinFinder.Matching.MatchingData
{
    public class MatchingDataManager
    {
        private static MatchingDataManager instance = new MatchingDataManager();

        // ***********************Fields***********************

        private List<KeyDefinition> keyDefinitions = new List<KeyDefinition>();
        private List<CompareDefinition> compareDefinitions = new List<CompareDefinition>();

        // ***********************Constructor***********************

        private MatchingDataManager()
        {
        }

        // ***********************Properties***********************

        public static MatchingDataManager Instance
        {
            get { return instance; }
        }

        public List<KeyDefinition> KeyDefinitions
        {
            get { return this.keyDefinitions; }
        }

        public List<CompareDefinition> CompareDefinitions
        {
            get { return this.compareDefinitions; }
        }

        // ***********************Functions***********************

        public XElement SerializeMatchingData(Model.MatchingData matchingData)
        {
            XElement xMatchingData = new XElement("matchingdata",
                                        new XElement("comparedefinitions", matchingData.CompareDefinitions.Select(cmd =>
                                          this.SerializeCompareDefinition(cmd)
                                      )),
                                        new XElement("keydefinitions", matchingData.KeyDefinitions.Select(cmd =>
                                          this.SerializeKeyDefinition(cmd)
                                      ))
                 );

            return xMatchingData;
        }

        public Model.MatchingData DeserializeMatchingData(XElement xMatchingData)
        {
            var matchingData = new Model.MatchingData();

            if (xMatchingData == null)
            {
                return matchingData;
            }

            foreach (XElement xCompareDef in xMatchingData.Element("comparedefinitions").Elements())
            {
                matchingData.CompareDefinitions.Add(this.DeserializeCompareDefinition(xCompareDef));
            }

            foreach (XElement xKeyDef in xMatchingData.Element("keydefinitions").Elements())
            {
                matchingData.KeyDefinitions.Add(this.DeserializeKeyDefinition(xKeyDef));
            }

            return matchingData;
        }

        //public void LoadKeyDefinitions()
        //{
        //    this.keyDefinitions.Clear();

        //    DirectoryInfo di = new DirectoryInfo(@"C:\Develop\c#\DataMatchingFramework\Resources\Matching\");
        //    foreach (var file in di.GetFiles().Where(x => x.Name.StartsWith("keydefinition") && x.Name.EndsWith(".xml")))
        //    {
        //        KeyDefinition keyDef = this.LoadKeyDefinition(di.FullName + file.Name);

        //        this.keyDefinitions.Add(keyDef);
        //    }
        //}

        //public void LoadCompareDefinitions()
        //{
        //    this.keyDefinitions.Clear();

        //    DirectoryInfo di = new DirectoryInfo(@"C:\Develop\c#\DataMatchingFramework\Resources\Matching\");
        //    foreach (var file in di.GetFiles().Where(x => x.Name.StartsWith("comparedefinition") && x.Name.EndsWith(".xml")))
        //    {
        //        CompareDefinition compareDef = this.LoadCompareDefinition(di.FullName + file.Name);

        //        this.compareDefinitions.Add(compareDef);
        //    }
        //}

        public KeyDefinition LoadKeyDefinition(string fileName)
        {
            XDocument xDocument = XDocument.Load(fileName, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
            XElement xKeyDef = xDocument.Element("keydefinition");

            return this.DeserializeKeyDefinition(xKeyDef);
        }

        public KeyDefinition DeserializeKeyDefinition(XElement xKeyDef)
        {
            try
            {
                var keyDef = new KeyDefinition();

                if (xKeyDef == null)
                {
                    return keyDef;
                }

                // KeyFields
                foreach (XElement xField in xKeyDef.Element("keyfields").Elements())
                {
                    var keyField = new KeyField();
                    keyField.Name = xField.Attribute(XName.Get("name")).Value;

                    // Keygenerator
                    string keyGeneratorName = xField.Attribute(XName.Get("generator")).Value;

                    if (!string.IsNullOrEmpty(keyGeneratorName))
                    {
                        keyField.Generator = StringPhoneticKeyBuilderFactory.GetInstance(keyGeneratorName);
                        keyField.Generator.MaxLength = (int)xField.Attribute(XName.Get("maxlength"));
                    }

                    keyDef.Fields.Add(keyField);
                }

                // Target Field
                keyDef.TargetKeyField = xKeyDef.Element("targetkeyfield")
                                                  .Attribute(XName.Get("name"))
                                                     .Value;

                return keyDef;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Fehler in " + this.GetType().FullName + " Method: [" + System.Reflection.MethodBase.GetCurrentMethod() + "] Data: " + xKeyDef.ToString(), ex);
            }
        }

        public XElement SerializeKeyDefinition(KeyDefinition keyDef)
        {
            XElement xkeyDef = new XElement("keydefinition",
                                      new XElement("keyfields", keyDef.Fields.Select(cmd =>
                                          new XElement("field", new XAttribute("name", cmd.Name), new XAttribute("generator", cmd.Generator.Name), new XAttribute("maxlength", cmd.Generator.MaxLength)
                                          )
                                      )),
                                      new XElement("targetkeyfield", new XAttribute("name", keyDef.TargetKeyField)
                                      )
                                    );
            return xkeyDef;
        }

        public CompareDefinition LoadCompareDefinition(string fileName)
        {
            XDocument xDocument = XDocument.Load(fileName, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
            XElement xCompareDef = xDocument.Element("comparedefinition");

            return this.DeserializeCompareDefinition(xCompareDef);
        }

        public CompareDefinition DeserializeCompareDefinition(XElement xCompareDef)
        {
            try
            {
                var compareDef = new CompareDefinition();

                if (xCompareDef == null)
                {
                    return compareDef;
                }

                // CompareFields
                foreach (XElement xField in xCompareDef.Element("comparefields").Elements())
                {
                    // Name
                    CompareField compareField = new CompareField();
                    compareField.Name1 = xField.Attribute(XName.Get("name1")).Value;

                    // Weight
                    compareField.Weight = (float)xField.Attribute(XName.Get("weight"));

                    // Comparer
                    string comparerName = xField.Attribute(XName.Get("comparer")).Value;

                    if (!string.IsNullOrEmpty(comparerName))
                    {
                        compareField.FuzzyComparer = StringFuzzyComparerFactory.GetInstance(comparerName);
                    }

                    compareDef.CompareFields.Add(compareField);
                }

                // Aggregator
                string aggregatorName = xCompareDef.Element("aggregator")
                                                      .Attribute(XName.Get("name"))
                                                        .Value;

                compareDef.Aggregator = AggregatorFactory.GetInstance(aggregatorName);

                return compareDef;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Fehler in " + this.GetType().FullName + " Method: [" + System.Reflection.MethodBase.GetCurrentMethod() + "] Data: " + xCompareDef.ToString(), ex);
            }
        }

        public XElement SerializeCompareDefinition(CompareDefinition compareDef)
        {
            XElement xCompareDef = new XElement("comparedefinition",
                                      new XElement("comparefields", compareDef.CompareFields.Select(cmd =>
                                          new XElement("field", new XAttribute("name1", cmd.Name1), new XAttribute("comparer", cmd.FuzzyComparer.Name), new XAttribute("weight", cmd.Weight)
                                          )
                                      )),
                                      new XElement("stopfields", compareDef.StopFields.Select(cmd =>
                                          new XElement("field", new XAttribute("name1", cmd.Name1)
                                          )
                                      )),
                                      new XElement("aggregator", new XAttribute("name", compareDef.Aggregator.Name)
                                      )
                                    );
            return xCompareDef;
        }
    }
}