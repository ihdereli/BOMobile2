using Acr.UserDialogs;
using BOMobile2.Services.Schema;
using Extensions;
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
    public partial class RegisterFinish : ContentPage
    {
        private MemberLoginInfo MemberInfo { get; set; }

        public RegisterFinish(MemberLoginInfo _member)
        {
            InitializeComponent();

            MemberInfo = _member;
        }

        protected async override void OnAppearing()
        {
            UserDialogs.Instance.ShowLoading(TranslateExtension.Translate(40) + "...", MaskType.Black);

            datePickerBirthDate.Date = DateTime.Now.AddYears(-20);

            var data = await Global.DataService.Post<List<Country>, GetCountriesRequest>(new GetCountriesRequest());

            pickerCountry.ItemsSource = data.data;

            pickerCountry.SelectedItem = data.data.Single(s => s.Id == MemberInfo.Country);

            base.OnAppearing();

            UserDialogs.Instance.HideLoading();
        }

        private async void pickerCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            var countryId = ((Country)pickerCountry.SelectedItem).Id;

            var data = await Global.DataService.Post<List<City>, GetCitiesRequest>(new GetCitiesRequest { CountryId = countryId });

            pickerCity.ItemsSource = data.data;
        }

        private bool Validation()
        {
            if (entryIdentity.Text == null)
            {
                labelIdentity.TextColor = Color.Red;
                entryIdentity.Focus();
                return false;
            }

            if (datePickerBirthDate.Date == null)
            {
                labelBirthDate.TextColor = Color.Red;
                datePickerBirthDate.Focus();
                return false;
            }

            if (pickerCountry.SelectedItem == null)
            {
                labelCountry.TextColor = Color.Red;
                pickerCountry.Focus();
                return false;
            }

            if (pickerCity.SelectedItem == null)
            {
                labelCity.TextColor = Color.Red;
                pickerCity.Focus();
                return false;
            }

            return true;
        }

        private async void buttonRegisterFinish_Clicked(object sender, EventArgs e)
        {
            if (!Validation()) return;

            var data = await Global.DataService.Post<string, MemberRegisterFinishRequest>(new MemberRegisterFinishRequest
            {
                MemberId = MemberInfo.Id, 
                IdentityNumber = entryIdentity.Text, 
                BirthDate = datePickerBirthDate.Date, 
                Country = ((Country)pickerCountry.SelectedItem).Id, 
                City = ((City)pickerCity.SelectedItem).Name 
            });

            if (data.responseStatus == "ERROR")
            {
                UserDialogs.Instance.ShowError(data.errorDefiniton, 2000);
            }
            else
            {
                UserDialogs.Instance.ShowSuccess(Extensions.TranslateExtension.Translate(53), 2000);

                App.Current.MainPage = new NavigationPage(new Loading());
            }
        }
    }
}