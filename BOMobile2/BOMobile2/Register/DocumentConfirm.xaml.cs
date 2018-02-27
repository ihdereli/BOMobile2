using Acr.UserDialogs;
using BOMobile2.Services.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BOMobile2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DocumentConfirm : ContentPage
    {
        public delegate void Refresh(object sender, EventArgs e);
        public event Refresh OnRefresh;

        Stream image;
        Plugin.Media.Abstractions.MediaFile photo { get; set; }
        int type { get; set; }

        public DocumentConfirm(Plugin.Media.Abstractions.MediaFile _photo, int _type)
        {
            InitializeComponent();

            image = _photo.GetStream();
            photo = _photo;
            type = _type;
        }

        protected async override void OnAppearing()
        {
            PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
            
            base.OnAppearing();
        }

        private async void AcceptButton_Clicked(object sender, EventArgs e)
        {
            progressBar.IsVisible = true;

            await progressBar.ProgressTo(1, 10000, Easing.Linear);

            var data = await Global.DataService.Post<int, MemberDocumentAddRequest>(new MemberDocumentAddRequest { Type = type });

            if (data.responseStatus != "OK")
            {
                UserDialogs.Instance.ShowError(data.errorDefiniton, 3000);
                return;
            }

            var documentId = data.data;

            var result = Global.Ftp.UploadFile(image, documentId);

            if (result.Status != "OK")
            {
                UserDialogs.Instance.ShowError(result.Error, 3000);
                return;
            }

            var data2 = await Global.DataService.Post<string, MemberDocumentUpdateRequest>(new MemberDocumentUpdateRequest { DocumentId = documentId, Status = 1 });

            if (data2.responseStatus != "OK")
            {
                UserDialogs.Instance.ShowError(data2.errorDefiniton, 3000);
                return;
            }

            progressBar.IsVisible = false;

            await Navigation.PopModalAsync();

            OnRefresh(this, e);
        }
    }
}