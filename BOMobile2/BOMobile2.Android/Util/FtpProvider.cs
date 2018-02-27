using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BOMobile2.Util;
using System.Net;
using System.IO;

namespace BOMobile2.Droid.Util
{
    public class FtpProvider : IFtpProvider
    {
        public FtpProvider()
        {

        }

        public FtpResult<object> UploadFile(Stream image, int documentId)
        {
            FtpResult<object> result = new FtpResult<object>();
            result.Status = "Processing";
            result.Debug = 0;
            result.Error = "";

            try
            {
                string ftpUser = "newartzone.uqqsvjr7dj8cngkz9pnz";
                string ftpPassword = "asdzxc123";
                string ftpfullpath = "ftp://ftp.newartzone.uqqsvjr7dj8cngkz9pnz.netdna-cdn.com/documents/" + documentId.ToString() + ".png";

                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);

                result.Debug = 1;

                ftp.Credentials = new NetworkCredential(ftpUser, ftpPassword);

                result.Debug = 2;

                ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;

                result.Debug = 3;

                byte[] buffer = new byte[image.Length];
                image.Read(buffer, 0, buffer.Length);

                result.Debug = 4;

                image.Close();

                result.Debug = 5;

                Stream ftpstream = ftp.GetRequestStream();

                result.Debug = 6;

                ftpstream.Write(buffer, 0, buffer.Length);

                result.Debug = 7;

                ftpstream.Close();

                result.Debug = 8;

                ftpstream.Flush();

                result.Debug = 99;

                result.Status = "OK";
            }
            catch (Exception ex)
            {
                ex.ToString();

                result.Status = "ERROR";
                result.Error = ex.Message;
            }

            return result;
        }
        
        public FtpResult<Xamarin.Forms.ImageSource> DownloadImage(string fileName)
        {
            FtpResult<Xamarin.Forms.ImageSource> result = new FtpResult<Xamarin.Forms.ImageSource>();
            result.Status = "Processing";
            result.Debug = 0;
            result.Error = "";
            result.Data = null;

            try
            {
                string ftpfullpath = "http://newartsolutions-uqqsvjr7dj8cngkz9pnz.netdna-ssl.com/assets/" + fileName;

                WebClient wc = new WebClient();

                Stream s = new MemoryStream();
                byte[] data = wc.DownloadData(ftpfullpath);
                s.Write(data, 0, data.Length);
                result.Data = Xamarin.Forms.ImageSource.FromStream(() => { return s; });

                result.Debug = 99;

                result.Status = "OK";
            }
            catch (Exception ex)
            {
                ex.ToString();

                result.Status = "ERROR";
                result.Error = ex.Message;
            }

            return result;
        }
    }
}