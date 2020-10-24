using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Security.Cryptography;
using System.Collections;
using System.Net.NetworkInformation;
using System.Threading;

namespace PingServersFromFile
{
    [Cmdlet(VerbsCommon.Get,"PingServersFromFile")]
    [OutputType(typeof(ResultofPing))]
    public class PingServersFromFile:PSCmdlet

    {
        [Parameter(Mandatory =true,Position =0,HelpMessage ="Example C:\\file.txt, input servers one line at time")]
        [ValidateNotNullOrEmpty]
        public string FileNameWithFullPath { get; set; }


       

        protected override void ProcessRecord()
        {
          
            List<string> input = new List<string>();
            
            
            System.IO.StreamReader read = new System.IO.StreamReader(FileNameWithFullPath);
            do
            {
                input.Add(read.ReadLine());
            } while (!(read.EndOfStream));
            read.Close();
            //LegacyPingCheck(pingTarget);
            // ParallelPingCheck(pingTarget);
            List<Task<ResultofPing>> l = new List<Task<ResultofPing>>();
            foreach (string a in input)
            {
                l.Add(Task.Run(() => PingDo(a)));
            }
            Task.WaitAll(l.ToArray());
            l.Select(s => s.Result).ToList().ForEach(WriteObject);
            
        }

        static ResultofPing PingDo(string a)
        {
           
            try
            {
                return new ResultofPing { ServerName = a, PingResult = new Ping().Send(a).Status.ToString(),RoundTripTime= new Ping().Send(a).RoundtripTime,IPAddress= new Ping().Send(a).Address };
            }
            catch (PingException e)
            {
                return new ResultofPing { ServerName = a, PingResult = e.Message};

            }
        }



    }

    public class ResultofPing
    {
        long defaultRoundTrip = 0;
        static byte[] b = new byte[] { 0, 0, 0, 0 };
        System.Net.IPAddress address = new System.Net.IPAddress(b);
        

        public string ServerName { get; set; }
        public string PingResult { get; set; }
        public long RoundTripTime
        {
            get
            {
                return this.defaultRoundTrip;

            }
            set
            {
                this.defaultRoundTrip = value;

            }
        }
        public System.Net.IPAddress IPAddress
        {
            get
            {
                return address;
            }
            set
            {
                if (value != null)
                {
                    this.address = value;
                }
                else
                {
                    this.address = new System.Net.IPAddress(b);
                }
                    
               
            }
        }
        
    }


}