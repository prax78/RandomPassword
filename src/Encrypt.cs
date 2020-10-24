using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Security.Cryptography;
using System.Collections;
using System.IO;
namespace Encrypt
{
    [Cmdlet(VerbsCommon.Get, "PassWordFromEncryptFile")]
    [OutputType(typeof(Credentials))]
    public class Encrypt : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Enter A Plain TXT File Containing  Password,Rember to delete the file as it is a clear text")]
        [ValidateNotNullOrEmpty]
        public string UserPasswordFile { get; set; }
        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Enter UserName that would like to pass")]
        [ValidateNotNullOrEmpty]

        public string OutPutEncryptedFile { get; set; }
        [Parameter(Mandatory = true, Position = 2, HelpMessage = "Input output FileName.this will be your encrypted file")]
        [ValidateNotNullOrEmpty]
        public string UserName { get; set; }


        protected override void ProcessRecord()
        {
            FileInfo fInfo = new FileInfo(UserPasswordFile);
            string fPath = fInfo.DirectoryName;
            string fName = fPath + "\\"+ OutPutEncryptedFile;
            if (File.Exists(UserPasswordFile))
            {
                string readPass = File.ReadAllText(UserPasswordFile);
                using (Aes myAes = Aes.Create())
                {
                    EncryptStringToBytes_Aes(readPass, myAes.Key, myAes.IV, fName);
                }
            }
            string encPass = File.ReadAllLines(fName)[0];
            string Key = File.ReadAllLines(fName)[1];
            string IV = File.ReadAllLines(fName)[2];
            byte[] encyptedPass = Convert.FromBase64String(encPass);
            byte[] storedKey = Convert.FromBase64String(Key);
            byte[] storedIV = Convert.FromBase64String(IV);
            string result = DecryptStringFromBytes_Aes(encyptedPass, storedKey, storedIV, fName);
            WriteObject(new Credentials { PassWord = result,UserName=UserName, OutPutEncryptedFile= fName });
        }

        static void EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV, string filename)
        {

            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Padding = PaddingMode.PKCS7;
                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            // File.Create(filename);

            System.Threading.Thread.Sleep(150);
            if (!(File.Exists(filename)))
            {
                
                File.AppendAllText(filename, Convert.ToBase64String(encrypted));
                File.AppendAllText(filename, Environment.NewLine);
                File.AppendAllText(filename, Convert.ToBase64String(Key));
                File.AppendAllText(filename, Environment.NewLine);
                File.AppendAllText(filename, Convert.ToBase64String(IV));
            }
            else
            {
                File.Delete(filename);
                File.AppendAllText(filename, Convert.ToBase64String(encrypted));
                File.AppendAllText(filename, Environment.NewLine);
                File.AppendAllText(filename, Convert.ToBase64String(Key));
                File.AppendAllText(filename, Environment.NewLine);
                File.AppendAllText(filename, Convert.ToBase64String(IV));
            }

        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV, string filename)
        {

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Padding = PaddingMode.PKCS7;
                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
    public class Credentials
    {
      public string UserName { get; set; }
        public string PassWord { get; set; }

        public string OutPutEncryptedFile { get; set; }
    }
}