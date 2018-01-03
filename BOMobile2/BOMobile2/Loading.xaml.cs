using BOMobile2.Services;
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
    public partial class Loading : ContentPage
    {
        public Loading()
        {
            InitializeComponent();
        }
        
        protected async override void OnAppearing()
        {
            var data = await Global.DataService.Post<Dictionary<int, string>, GetTextTranslationsByLang>(new GetTextTranslationsByLang { Language = Global.Language });

            Global.Translates = data.data;

            await progressBar.ProgressTo(.8, 1000, Easing.Linear);

            await Navigation.PushModalAsync(new Login());
        }
    }
}