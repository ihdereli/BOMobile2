using Acr.UserDialogs;
using BOMobile2.Services.Schema;
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

        private async void buttonSendEmail_Clicked(object sender, EventArgs e)
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
            }
        }

        private void buttonSendSMS_Clicked(object sender, EventArgs e)
        {

        }
    }
}