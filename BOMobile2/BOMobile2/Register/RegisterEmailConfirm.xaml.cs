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

        private async void buttonVerifyEmail_Clicked(object sender, EventArgs e)
        {
            var data = await Global.DataService.Post<MemberLoginInfo, MemberRegisterVerificationRequest>(new MemberRegisterVerificationRequest
            {
                MemberId = MemberInfo.Id, 
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