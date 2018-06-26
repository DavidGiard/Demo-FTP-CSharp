using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

namespace DemoFtp
{
    class Program
    {
        static void Main(string[] args)
        {
            string ftpUrl = @"ftp://blah.ftp.blah.com";
            // Note: I stored some values in app.config
            // I had to set a reference to System.Configuration to read these values
            ftpUrl = ConfigurationManager.AppSettings["ftpUri"];
            string userName = ConfigurationManager.AppSettings["userName"];
            string password = ConfigurationManager.AppSettings["password"];

            string ftpDestinationFolder = "site/wwwroot/content/Giard";
            string localFolderSource = @"C:\Test\_source";
            string fileName = "testfile.txt";

            string sourcePath = Path.Combine(localFolderSource, fileName);
            string destinationPath = Path.Combine(ftpUrl, ftpDestinationFolder, fileName);

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(destinationPath);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            request.Credentials = new NetworkCredential(userName, password);

            byte[] fileContents;
            using (StreamReader sourceStream = new StreamReader(sourcePath))
            {
                fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            }

            request.ContentLength = fileContents.Length;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                var responseText = response.StatusDescription;
                Console.WriteLine("Completed uploading file {0}!", fileName);
                Console.WriteLine("Status={0}", responseText);
            }

            Console.ReadLine();
        }
    }
}
