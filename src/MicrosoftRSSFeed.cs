using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Xml;
using System.Xml.Linq;

namespace getrssfeed
{
    [Cmdlet(VerbsCommon.Get, "MicrosoftSupportContentUpdatesRSSFeed")]
    [OutputType(typeof(FormatOutput))]
    public class getrssfeed : PSCmdlet
    {

     
        [Parameter(Mandatory = true, Position = 0)]
        [ValidateSet("2012", "2012R2", "2016", "2019", IgnoreCase = true)]
        public string[] ServerOs { get; set; }



        static IEnumerable<FormatOutput> SetData(string osName, string RSSFeed)
        {
            XElement read = XElement.Load(RSSFeed);

            foreach (var a in read.Descendants("item"))
            {
                yield return new FormatOutput { Title = a.Element("title").Value, Description = a.Element("description").Value, Link = a.Element("link").Value, PubDate = a.Element("pubDate").Value, OS = osName };

            }
        }



        protected override void ProcessRecord()
        {
            
            foreach (string os in ServerOs)
            {
                switch (os)
                {
                    case "2012":
                        var data = SetData("2012", "https://support.microsoft.com/app/content/api/content/feeds/sap/en-in/0cfbf2af-24ea-3e18-17e6-02df7331b571/rss");
                        data.ToList().ForEach(WriteObject);
                        break;
                    case "2012R2":
                        var data1 = SetData("2012R2", "https://support.microsoft.com/app/content/api/content/feeds/sap/en-in/3ec8448d-ebc8-8fc0-e0b7-9e8ef6c79918/rss");
                        data1.ToList().ForEach(WriteObject);
                        break;
                    case "2016":
                        var data2 = SetData("2016", "https://support.microsoft.com/app/content/api/content/feeds/sap/en-in/c3a1be8a-50db-47b7-d5eb-259debc3abcc/rss");
                        data2.ToList().ForEach(WriteObject);
                        break;
                    case "2019":
                        var data3 = SetData("2019", "https://support.microsoft.com/app/content/api/content/feeds/sap/en-in/eb958e25-cff9-2d06-53ca-f656481bb31f/rss");
                        data3.ToList().ForEach(WriteObject);
                        break;


                }
            }


        }



        public class FormatOutput
        {
            public string Title { get; set; }
            public string OS { get; set; }
            public string Description { get; set; }

            public string Link { get; set; }
            public string PubDate { get; set; }
        }
    }
}
