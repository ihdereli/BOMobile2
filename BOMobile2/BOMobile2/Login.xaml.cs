using Acr.UserDialogs;
using BOMobile2.Services;
using BOMobile2.Services.Schema;
using Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BOMobile2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            UserDialogs.Instance.ShowLoading(TranslateExtension.Translate(40) + "...", MaskType.Black);

            var data = await Global.DataService.Post<List<Languages>, GetLanguagesRequest>(new GetLanguagesRequest { Id = null, IsActive = true });

            pickerLanguages.ItemsSource = data.data;

            base.OnAppearing();

            UserDialogs.Instance.HideLoading();
        }

        private void pickerLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            Global.Language = ((Languages)pickerLanguages.SelectedItem).Id;

            App.Current.MainPage = new NavigationPage(new Loading());
        }

        private async void buttonLogin_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading(TranslateExtension.Translate(40) + "...", MaskType.Black);

            try
            {
                var data = await Global.DataService.Post<List<MemberLoginInfo>, MemberLoginRequest>(new MemberLoginRequest { Email = entryEmail.Text, Password = entryPassword.Text });

                if (data.responseStatus == "OK" && data.data.Count > 0)
                {
                    Global.MemberInfo = data.data[0];

                    //App.Current.MainPage = new NavigationPage(new MainPage());
                }
                else
                {
                    captionMessage.Text = TranslateExtension.Translate(13);
                    captionMessage.TextColor = Color.Red;
                    captionMessage.IsVisible = true;
                }

                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.ShowError(ex.ToString(), 2000);

                UserDialogs.Instance.HideLoading();
            }
        }

        private async void buttonRegister_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Register());
        }

        private void buttonForgetPassword_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new ForgetPassword());
        }
    }
}