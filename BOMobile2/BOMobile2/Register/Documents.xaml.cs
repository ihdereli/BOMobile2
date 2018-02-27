using Acr.UserDialogs;
using BOMobile2.Services.Schema;
using Extensions;
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
    public partial class Documents : ContentPage
    {
        public Documents()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            UserDialogs.Instance.ShowLoading(TranslateExtension.Translate(40) + "...", MaskType.Black);

            RefreshList();

            base.OnAppearing();

            UserDialogs.Instance.HideLoading();
        }

        private async void RefreshList()
        {
            var data = await Global.DataService.Post<List<DocumentConfirmation>, MemberDocumentsListRequest>(new MemberDocumentsListRequest { });

            DocumentConfirmationList.ItemsSource = data.data;
        }

        private async void ButtonFromCamera_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            if (photo != null)
            {
                DocumentConfirm dc = new DocumentConfirm(photo, Convert.ToInt32(((Button)sender).CommandParameter));
                dc.OnRefresh += Dc_OnRefresh;
                await Navigation.PushModalAsync(dc);
            }
        }

        private async void ButtonFromGallery_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            var photo = await Plugin.Media.CrossMedia.Current.PickPhotoAsync();

            if (photo != null)
            {
                DocumentConfirm dc = new DocumentConfirm(photo, Convert.ToInt32(((Button)sender).CommandParameter));
                dc.OnRefresh += Dc_OnRefresh;
                await Navigation.PushModalAsync(dc);
            }
        }

        private void Dc_OnRefresh(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading(TranslateExtension.Translate(40) + "...", MaskType.Black);

            RefreshList();

            UserDialogs.Instance.HideLoading();
        }

        private async void AddPhoto(System.IO.Stream image, int type)
        {
            UserDialogs.Instance.ShowLoading(TranslateExtension.Translate(40) + "...", MaskType.Black);

            /*Documents.Accepted = false;

            if (!Documents.Accepted) return;
            */
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

            RefreshList();

            UserDialogs.Instance.HideLoading();
        }
    }
}