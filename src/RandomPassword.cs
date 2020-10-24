using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Security.Cryptography;
using System.Collections;


namespace RandomPassword
{

    [Cmdlet(VerbsCommon.Get, "RandomPassWord")]
    public class RandomPassword : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Input the length of Required Password")]
        [ValidateNotNullOrEmpty]
        public int PasswordLength { get; set; }
        

        protected override void ProcessRecord()
        {
           WriteObject(String.Join("", RandomChar(PasswordLength)));
            WriteObject($"Please find a Random Password of Length: {RandomChar(PasswordLength).Count()}");

        }

        public static string[] RandomChar(int input)
        {

            
                RNGCryptoServiceProvider r = new RNGCryptoServiceProvider();


                byte[] b = new byte[1];
                ArrayList a = new ArrayList();
                for (int i = 0; i < input; i++)
                {
                    r.GetBytes(b);
                var data = b[0] / 2;
                if (data < 33)
                {
                    char o = Convert.ToChar(data + 33);
                    a.Add(o.ToString());

                }
                else
                {
                    char o = Convert.ToChar(data );
                    a.Add(o.ToString());
                }

                }
                //copies the elements of the arralist a to a the string array s
                string[] s = (string[])a.ToArray(typeof(string));
                return s;
                       
        }
    }
}
