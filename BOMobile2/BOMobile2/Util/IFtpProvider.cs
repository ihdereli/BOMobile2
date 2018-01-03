using System.IO;

namespace BOMobile2.Util
{
    public interface IFtpProvider
    {
        FtpResult UploadFile(Stream image);
    }

    public class FtpResult
    {
        public string Status { get; set; }
        public int Debug { get; set; }
        public string Error { get; set; }
    }
}
