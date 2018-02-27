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
    public partial class RegisterEmailConfirm : ContentPage
    {
        private MemberLoginInfo MemberInfo { get; set; }

        public RegisterEmailConfirm(MemberLoginInfo _member)
        {
            InitializeComponent();

            MemberInfo = _member;

            if (MemberInfo.GsmVerified)
            {
                buttonVerifyByGsm.IsVisible = false;
            }
        }

        protected async override void OnAppearing()
        {
            UserDialogs.Instance.ShowLoading(TranslateExtension.Translate(40) + "...", MaskType.Black);

            if (Global.IsTest)
            {
                var data = await Global.DataService.Post<string, TestGetActivationRequest>(new TestGetActivationRequest { MemberId = (int)MemberInfo.Id });

                labelEmailShowKey.IsVisible = true;
                labelEmailShowKey.Text = data.data;
            }

            base.OnAppearing();

            UserDialogs.Instance.HideLoading();
        }

        private async void buttonVerifyEmail_Clicked(object sender, EventArgs e)
        {
            var data = await Global.DataService.Post<MemberLoginInfo, MemberRegisterVerificationRequest>(new MemberRegisterVerificationRequest
            {
                MemberId = (int)MemberInfo.Id, 
                Code = entryEmailConfirm.Text,
                Type = "Email" 
            });

            if (data.responseStatus == "ERROR")
            {
                UserDialogs.Instance.ShowError(data.errorDefiniton, 3000);
            }
            else
            {
                UserDialogs.Instance.ShowSuccess(TranslateExtension.Translate(53), 1000);

                await Navigation.PushModalAsync(new RegisterGSMConfirm(data.data));
            }
        }

        private async void buttonVerifyByGsm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new RegisterGSMConfirm(MemberInfo));
        }

        private async void buttonVerifyLater_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new RegisterFinish(MemberInfo));
        }
    }
}