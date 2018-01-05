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
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            
            BarBackgroundColor = Color.Black;
            BarTextColor = Color.White;

            ToolbarItems.Add(new ToolbarItem
            {
                Icon = "accountSettings.png", 
                Text = "Menu", 
                Command = new Command(this.ShowSettings), 
            });
        }
        
        private async void ShowSettings(object obj)
        {
            await Navigation.PushModalAsync(new MemberDetails());
        }

        protected async override void OnAppearing()
        {
            UserDialogs.Instance.ShowLoading(TranslateExtension.Translate(40) + "...", MaskType.Black);

            var data = await Global.DataService.Post<List<MemberBalance>, MemberBalancesRequest>(new MemberBalancesRequest { CurrencyId = null });
            
            MemberBalances.ItemsSource = data.data;

            base.OnAppearing();

            UserDialogs.Instance.HideLoading();
        }
    }
}