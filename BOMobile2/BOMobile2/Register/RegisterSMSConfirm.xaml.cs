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
    public partial class RegisterSMSConfirm : ContentPage
    {
        private int MemberId { get; set; }

        public RegisterSMSConfirm(int _memberId)
        {
            InitializeComponent();

            MemberId = _memberId;
        }

        private async void buttonVerificateSMS_Clicked(object sender, EventArgs e)
        {
            var data = await Global.DataService.Post<string, MemberRegisterVerificationRequest>(new MemberRegisterVerificationRequest
            {
                MemberId = MemberId, 
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

                await Navigation.PushModalAsync(new RegisterFinish());
            }
        }
    }
}