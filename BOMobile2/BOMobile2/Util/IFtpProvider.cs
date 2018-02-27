using System.IO;

namespace BOMobile2.Util
{
    public interface IFtpProvider
    {
        FtpResult<object> UploadFile(Stream image, int documentId);
        FtpResult<Xamarin.Forms.ImageSource> DownloadImage(string fileName);
    }

    public class FtpResult<T>
    {
        public T Data { get; set; }
        public string Status { get; set; }
        public int Debug { get; set; }
        public string Error { get; set; }
    }
}
