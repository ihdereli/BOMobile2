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
    public partial class RegisterGSMConfirm : ContentPage
    {
        private MemberLoginInfo MemberInfo { get; set; }

        public RegisterGSMConfirm(MemberLoginInfo _member)
        {
            InitializeComponent();

            MemberInfo = _member;
        }

        private async void buttonSendVerificationGSM_Clicked(object sender, EventArgs e)
        {
            var data = await Global.DataService.Post<string, MemberRegisterSendVerificationSMSRequest>(new MemberRegisterSendVerificationSMSRequest
            {
                MemberId = MemberInfo.Id,
                GSM = entryGsm.Text 
            });

            if (data.responseStatus == "ERROR")
            {
                UserDialogs.Instance.ShowError(data.errorDefiniton, 3000);
            }
            else
            {
                UserDialogs.Instance.ShowSuccess(TranslateExtension.Translate(53), 1000);

                slGSM.IsVisible = false;
                slSMS.IsVisible = true;
            }
        }

        private async void buttonVerificateSMS_Clicked(object sender, EventArgs e)
        {
            var data = await Global.DataService.Post<string, MemberRegisterVerificationRequest>(new MemberRegisterVerificationRequest
            {
                MemberId = MemberInfo.Id,
                Code = entrySMSConfirm.Text,
                Type = "GSM"
            });

            if (data.responseStatus == "ERROR")
            {
                UserDialogs.Instance.ShowError(data.errorDefiniton, 3000);
            }
            else
            {
                UserDialogs.Instance.ShowSuccess(TranslateExtension.Translate(53), 1000);

                await Navigation.PushModalAsync(new RegisterFinish(MemberInfo));
            }
        }

        private async void buttonVerifyLater_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new RegisterFinish(MemberInfo));
        }
    }
}