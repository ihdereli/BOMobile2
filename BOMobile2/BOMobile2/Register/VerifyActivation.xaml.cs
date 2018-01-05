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
    public partial class VerifyActivation : ContentPage
    {
        int memberId;
        private string type = "";

        public VerifyActivation(int _memberId, string _type)
        {
            memberId = _memberId;
            type = _type;

            InitializeComponent();

            if (type == "Email_FP")
            {
                labelActivationConfirm.Text = TranslateExtension.Translate(58);
                buttonVerifyActivation.Text = TranslateExtension.Translate(60);
            }
            else
            {
                labelActivationConfirm.Text = TranslateExtension.Translate(65);
                buttonVerifyActivation.Text = TranslateExtension.Translate(61);
            }
        }

        private async void buttonVerifyActivation_Clicked(object sender, EventArgs e)
        {
            var data = await Global.DataService.Post<MemberLoginInfo, MemberRegisterVerificationRequest>(new MemberRegisterVerificationRequest
            {
                MemberId = memberId,
                Code = entryActivationConfirm.Text,
                Type = type
            });

            if (data.responseStatus == "ERROR")
            {
                UserDialogs.Instance.ShowError(data.errorDefiniton, 3000);
            }
            else
            {
                UserDialogs.Instance.ShowSuccess(TranslateExtension.Translate(53), 1000);

                await Navigation.PushModalAsync(new CreatePassword(data.data));
            }
        }
    }
}