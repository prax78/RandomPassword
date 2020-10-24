using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RandomPassword
{

    [Cmdlet(VerbsCommon.Get, "MSSecurityUpdates")]
    [OutputType(typeof(WindowsUpdate))]
    public class GetMicrosoftSecurityUpdates:Cmdlet
    {

        
        [Parameter(Mandatory = true, Position = 0,HelpMessage ="Use YYYY-MM example 2020-May")]
         public string MSUpdateName { get; set; }

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Provide MS DevKey, Get it from https://portal.msrc.microsoft.com/en-us/developer")]
        public string DevKey { get; set; }

        protected override void ProcessRecord()
        {
            if(!CheckMsUpdateName(MSUpdateName))
            {
                WriteObject("Please use input like 2020-May [YYYY-MMM]");
            }
            else
            {
                WriteVerbose($"MSUpdate Name {MSUpdateName}");
            }
            
        }


        static bool CheckMsUpdateName(string inputString)
        {
            Regex match = new Regex(@"^\d{4}(-)[a-zA-z][a-zA-z][a-zA-z]$");
            return match.IsMatch(inputString);
        }

    }
    internal class WindowsUpdate
    {
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string CVE { get; set; }
        public string KBName { get; set; }
        public string DownloadURl { get; set; }
        public string SubType { get; set; }
        internal string DocumentTitle { get; set; }

        public string Supercedence { get; set; }
    }
}
