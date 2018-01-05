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
    public partial class CreatePassword : ContentPage
    {
        MemberLoginInfo MemberInfo;

        public CreatePassword(MemberLoginInfo _memberInfo)
        {
            MemberInfo = _memberInfo;

            InitializeComponent();
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

        private async void buttonChangePassword_Clicked(object sender, EventArgs e)
        {
            if (!Validation()) return;
            
            var data = await Global.DataService.Post<string, MemberUpdatePasswordRequest>(new MemberUpdatePasswordRequest
            {
                MemberId = (int)MemberInfo.Id, 
                Password = entryPassword.Text,
            });

            if (data.responseStatus == "ERROR")
            {
                UserDialogs.Instance.ShowError(data.errorDefiniton, 3000);
            }
            else
            {
                UserDialogs.Instance.ShowSuccess(TranslateExtension.Translate(82), 1000);

                await Navigation.PushModalAsync(new Loading());
            }
        }
    }
}