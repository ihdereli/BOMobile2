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

namespace BOMobile2.Wallet
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WalletMain : ContentPage
    {
        public WalletMain()
        {
            InitializeComponent();

            ToolbarItems.Add(new ToolbarItem
            {
                Icon = "accountSettings.png",
                Text = "Menu",
                Command = new Command(this.ShowSettings),
            });

            ToolbarItems.Add(new ToolbarItem
            {
                Icon = "camera.png",
                Text = "Camera",
                Command = new Command(this.CameraTest),
            });
        }

        private async void ShowSettings(object obj)
        {
            await Navigation.PushModalAsync(new MemberDetails());
        }

        private async void CameraTest(object obj)
        {
            await Navigation.PushModalAsync(new Documents());
        }

        protected async override void OnAppearing()
        {
            UserDialogs.Instance.ShowLoading(TranslateExtension.Translate(40) + "...", MaskType.Black);

            RefreshBalances();

            base.OnAppearing();

            UserDialogs.Instance.HideLoading();
        }

        private async void RefreshBalances()
        {
            MemberBalances.ItemsSource = null;

            var dataBalance = await Global.DataService.Post<List<MemberBalance>, MemberBalancesRequest>(new MemberBalancesRequest { CurrencyId = null });

            MemberBalances.ItemsSource = dataBalance.data;
        }

        MemberBalance selected = null;

        private async void MemberBalances_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            selected = e.Item as MemberBalance;

            if (selected.IsCoin)
            {
                slCoin.IsVisible = !slCoin.IsVisible;
            }
            else
            {
                await Navigation.PushModalAsync(new DepositWithdraw(selected.CurrencyId));
            }
        }

        private async void buttonSendRecieve_Clicked(object sender, EventArgs e)
        {
            if (selected.CurrencyId == "BTC")
                await Navigation.PushModalAsync(new BitcoinSendRecieve());
        }

        private async void buttonBuySell_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CoinShop(selected.CurrencyId, selected.CurrencyRate));
        }
    }
}