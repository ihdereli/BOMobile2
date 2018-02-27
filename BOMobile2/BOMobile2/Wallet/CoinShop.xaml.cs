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
    public partial class CoinShop : TabbedPage
    {
        private string coin;
        private decimal coinRate;
        private string currency;
        private decimal currencyRate = 1;

        public CoinShop(string _coin, decimal _rate)
        {
            InitializeComponent();

            coin = _coin;
            coinRate = _rate;
        }

        protected async override void OnAppearing()
        {
            UserDialogs.Instance.ShowLoading(TranslateExtension.Translate(40) + "...", MaskType.Black);

            var dataCurrency = await Global.DataService.Post<List<Currency>, GetUserCurrenciesRequest>(new GetUserCurrenciesRequest { });

            pickerBuyCurrency.ItemsSource = dataCurrency.data;
            pickerSellCurrency.ItemsSource = dataCurrency.data;

            base.OnAppearing();

            UserDialogs.Instance.HideLoading();

        }
        
        private void pickerBuyCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            currency = ((Currency)pickerBuyCurrency.SelectedItem).Id;
            currencyRate = ((Currency)pickerBuyCurrency.SelectedItem).CurrencyRate;

            entryBuyCurrencyAmount_TextChanged(null, null);
        }

        private void entryBuyCurrencyAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (currency == null) return;

            entryBuyCoinAmount.TextChanged -= entryBuyCoinAmount_TextChanged;

            try
            {
                entryBuyCoinAmount.Text = (((1 / currencyRate) * Convert.ToDecimal(entryBuyCurrencyAmount.Text)) * coinRate).ToString();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            entryBuyCoinAmount.TextChanged += entryBuyCoinAmount_TextChanged;
        }

        private void entryBuyCoinAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (currency == null) return;

            entryBuyCurrencyAmount.TextChanged -= entryBuyCurrencyAmount_TextChanged;

            try
            {
                entryBuyCurrencyAmount.Text = ((Convert.ToDecimal(entryBuyCoinAmount.Text) / coinRate) * currencyRate).ToString();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            entryBuyCurrencyAmount.TextChanged += entryBuyCurrencyAmount_TextChanged;
        }

        public async void buttonBuy_Clicked(object sender, EventArgs e)
        {

        }

        private void pickerSellCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            currency = ((Currency)pickerSellCurrency.SelectedItem).Id;
            currencyRate = ((Currency)pickerSellCurrency.SelectedItem).CurrencyRate;

            entrySellCurrencyAmount_TextChanged(null, null);
        }

        private void entrySellCurrencyAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (currency == null) return;

            entrySellCoinAmount.TextChanged -= entrySellCoinAmount_TextChanged;

            try
            {
                entrySellCoinAmount.Text = (((1 / currencyRate) * Convert.ToDecimal(entrySellCurrencyAmount.Text)) * coinRate).ToString();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            entrySellCoinAmount.TextChanged += entrySellCoinAmount_TextChanged;
        }

        private void entrySellCoinAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (currency == null) return;

            entrySellCurrencyAmount.TextChanged -= entrySellCurrencyAmount_TextChanged;

            try
            {
                entrySellCurrencyAmount.Text = ((Convert.ToDecimal(entrySellCoinAmount.Text) / coinRate) * currencyRate).ToString();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            entrySellCurrencyAmount.TextChanged += entrySellCurrencyAmount_TextChanged;
        }

        public async void buttonSell_Clicked(object sender, EventArgs e)
        {

        }
    }
}