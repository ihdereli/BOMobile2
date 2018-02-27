using Acr.UserDialogs;
using BOMobile2.Util;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BOMobile2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Camera : ContentPage
    {
        public System.IO.Stream image { get; set; }

        public Camera()
        {
            InitializeComponent();

            CameraButton.Clicked += CameraButton_Clicked;
            DeviceButton.Clicked += DeviceButton_Clicked;
            SendButton.Clicked += SendButton_Clicked;
        }

        private async void CameraButton_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            if (photo != null)
            {
                image = photo.GetStream();
                PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });

                SendButton.IsEnabled = true;
            }
        }

        private async void DeviceButton_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            var photo = await Plugin.Media.CrossMedia.Current.PickPhotoAsync();

            if (photo != null)
            {
                image = photo.GetStream();
                PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });

                SendButton.IsEnabled = true;
            }
        }

        private void SendButton_Clicked(object sender, EventArgs e)
        {
            var result = Global.Ftp.UploadFile(image, 0);

            UserDialogs.Instance.ShowError(result.Status + ";" + result.Debug + (result.Status == "ERROR" ? (";" + result.Error) : ""), 3000);
        }
    }
}