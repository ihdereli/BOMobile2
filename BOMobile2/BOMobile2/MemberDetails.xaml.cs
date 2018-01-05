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
    public partial class MemberDetails : ContentPage
    {
        public MemberDetails()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            UserDialogs.Instance.ShowLoading(TranslateExtension.Translate(40) + "...", MaskType.Black);

            var data = await Global.DataService.Post<List<MemberLoginInfo>, MemberInfoRequest>(new MemberInfoRequest {  });

            if (data.responseStatus != "ERROR")
            {
                if (data.data != null)
                {
                    if (data.data.Count > 0)
                    {
                        MemberLoginInfo mli = data.data[0];

                        entryName.Text = mli.Name;
                        entrySurname.Text = mli.Surname;
                        entryEmail.Text = mli.EMail;
                        entryGsm.Text = mli.Gsm;
                    }
                }
            }

            base.OnAppearing();

            UserDialogs.Instance.HideLoading();
        }
    }
}