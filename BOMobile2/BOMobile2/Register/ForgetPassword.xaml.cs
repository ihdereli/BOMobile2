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
    public partial class ForgetPassword : TabbedPage
    {
        public ForgetPassword()
        {
            InitializeComponent();
        }

        private void buttonSendEmail_Clicked(object sender, EventArgs e)
        {
            SendActivation(entryEmail.Text, "Email_FP");
        }

        private void buttonSendSMS_Clicked(object sender, EventArgs e)
        {
            SendActivation(entryGSM.Text, "GSM_FP");
        }

        private async void SendActivation(string verificationIdentity, string type)
        {
            var data = await Global.DataService.Post<int, MemberSendActivationRequest>(new MemberSendActivationRequest
            {
                VerificationIdentity = verificationIdentity,
                Type = type
            });

            if (data.responseStatus == "ERROR")
            {
                UserDialogs.Instance.ShowError(data.errorDefiniton, 3000);
            }
            else
            {
                UserDialogs.Instance.ShowSuccess(TranslateExtension.Translate(74), 1000);

                await Navigation.PushModalAsync(new VerifyActivation(data.data, type));
            }
        }
    }
}