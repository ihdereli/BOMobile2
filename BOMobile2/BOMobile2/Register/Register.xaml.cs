using Acr.UserDialogs;
using BOMobile2.Services.Schema;
using Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BOMobile2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        public Register()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            UserDialogs.Instance.ShowLoading(TranslateExtension.Translate(40) + "...", MaskType.Black);
            
            var data = await Global.DataService.Post<List<Country>, GetCountriesRequest>(new GetCountriesRequest());

            pickerPhoneCode.ItemsSource = data.data;

            pickerPhoneCode.SelectedItem = data.data.Single(s => s.IsoCode == System.Globalization.RegionInfo.CurrentRegion.TwoLetterISORegionName.ToLower());

            base.OnAppearing();

            UserDialogs.Instance.HideLoading();
        }

        private void entryConfirmPassword_Focused(object sender, FocusEventArgs e)
        {
            if (entryPassword.Text.Length < 6)
            {
                UserDialogs.Instance.ShowError(Extensions.TranslateExtension.Translate(54).Replace("{0}", "6"), 2000);

                entryPassword.Focus();
            }
        }

        private bool Validation()
        {
            if (string.IsNullOrWhiteSpace(entryName.Text))
            {
                labelName.TextColor = Color.Red;
                entryName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(entrySurname.Text))
            {
                labelSurname.TextColor = Color.Red;
                entrySurname.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(entryEmail.Text))
            {
                labelEmail.TextColor = Color.Red;
                entryEmail.Focus();
                return false;
            }

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            Match match = regex.Match(entryEmail.Text);

            if (!match.Success)
            {
                labelEmail.TextColor = Color.Red;
                entryEmail.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(entryGsm.Text))
            {
                labelGsm.TextColor = Color.Red;
                entryGsm.Focus();
                return false;
            }

            if (entryPassword.Text.Length < 6)
            {
                entryConfirmPassword.TextColor = Color.Red;
                entryConfirmPassword.Focus();
                return false;
            }

            if (entryPassword.Text != entryConfirmPassword.Text)
            {
                captionConfirmPassword.TextColor = Color.Red;
                entryConfirmPassword.Focus();
                return false;
            }

            return true;
        }

        private async void buttonContinue_Clicked(object sender, EventArgs e)
        {
            if (!Validation()) return;

            var c = (Country)pickerPhoneCode.SelectedItem;

            var r = new RegionInfo(c.IsoCode);
            
            var data = await Global.DataService.Post<MemberLoginInfo, MemberRegisterFirstRequest>(new MemberRegisterFirstRequest
            {
                Language = Global.Language,
                Name = entryName.Text,
                Surname = entrySurname.Text,
                Email = entryEmail.Text,
                Gsm = "+" + c.PhoneCode + entryGsm.Text,
                Password = entryPassword.Text,
                Currency = r.ISOCurrencySymbol,
                Country = c.Id 
            });

            if (data.responseStatus == "ERROR")
            {
                UserDialogs.Instance.ShowError(data.errorDefiniton, 3000);
            }
            else
            {
                UserDialogs.Instance.ShowSuccess(TranslateExtension.Translate(53), 1000);

                await Navigation.PushModalAsync(new RegisterEmailConfirm(data.data));
            }
        }
    }
}