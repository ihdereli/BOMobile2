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
    public partial class DepositWithdraw : TabbedPage
    {
        private string currency;

        private string Currency
        {
            get
            {
                if (currency != null) return currency;

                try
                {
                    if (this.CurrentPage.Title == TranslateExtension.Translate(147))
                    {
                        return ((Currency)pickerDepositCurrency.SelectedItem).Id;
                    }
                    else
                    {
                        return ((Currency)pickerWithdrawCurrency.SelectedItem).Id;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public DepositWithdraw()
        {
            InitializeComponent();

            currency = null;
        }

        public DepositWithdraw(string _currency)
        {
            InitializeComponent();

            currency = _currency;
        }

        protected async override void OnAppearing()
        {
            UserDialogs.Instance.ShowLoading(TranslateExtension.Translate(40) + "...", MaskType.Black);

            if (currency == null)
            {
                var dataDepositCurrency = await Global.DataService.Post<List<Currency>, GetDepositCurrenciesRequest>(new GetDepositCurrenciesRequest { });

                pickerDepositCurrency.ItemsSource = dataDepositCurrency.data;

                var dataWithdrawCurrency = await Global.DataService.Post<List<Currency>, GetWithdrawCurrenciesRequest>(new GetWithdrawCurrenciesRequest { MemberId = (int)Global.MemberInfo.Id });

                pickerWithdrawCurrency.ItemsSource = dataWithdrawCurrency.data;
            }
            else
            {
                pickerDepositCurrency.IsVisible = false;
                pickerWithdrawCurrency.IsVisible = false;
                labelDepositCurrency.IsVisible = false;
                labelWithdrawCurrency.IsVisible = false;

                labelDepositSelectedCurrency.IsVisible = true;
                labelWithdrawSelectedCurrency.IsVisible = true;

                labelDepositSelectedCurrency.Text = currency;
                labelWithdrawSelectedCurrency.Text = currency;

                pickerDepositCurrency_SelectedIndexChanged(null, null);
                pickerWithdrawCurrency_SelectedIndexChanged(null, null);
            }

            base.OnAppearing();

            UserDialogs.Instance.HideLoading();
        }
        
        private async void pickerDepositCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dataBanka = await Global.DataService.Post<List<Bank>, GetDepositBanksRequest>(new GetDepositBanksRequest { Currency = Currency });

            pickerDepositBanka.ItemsSource = dataBanka.data;
        }

        string transactionCode = "";
        public async void buttonDepositOrder_Clicked(object sender, EventArgs e)
        {
            if (Currency == null)
            {
                labelDepositMessage.Text = "Lütfen önce para birimi seçin.";
                return;
            }

            if (pickerDepositBanka.SelectedItem == null)
            {
                labelDepositMessage.Text = "Lütfen önce banka hesabı seçin.";
                return;
            }

            decimal _amount = 0;

            try
            {
                _amount = Convert.ToDecimal(entryDepositAmount.Text);
            }
            catch
            {
                labelDepositMessage.Text = "Lütfen önce bir miktar yazın.";
                return;
            }
            
            var data = await Global.DataService.Post<string, BankOperationInsertRequest>(new BankOperationInsertRequest
            {
                FinancialMethod = 1,
                Currency = Currency,
                BankAccountId = ((Bank)pickerDepositBanka.SelectedItem).Id,
                Amount = _amount,
                NoteText = ""
            });

            if (data.responseStatus == "ERROR")
            {
                UserDialogs.Instance.ShowError(data.errorDefiniton, 3000);
            }
            else
            {
                UserDialogs.Instance.ShowSuccess(TranslateExtension.Translate(53), 1000);

                transactionCode = data.data;
                labelDepositMessage.Text = "Gönderim Kodunuz:" + data.data + "   Lütfen havale açıklamasına bu kodu girin.";
                buttonDepositOrder.IsEnabled = false;
                buttonDepositOrderTest.IsVisible = true;
            }
        }

        public async void buttonDepositOrderTest_Clicked(object sender, EventArgs e)
        {
            decimal _amount = 0;

            try
            {
                _amount = Convert.ToDecimal(entryDepositAmount.Text);
            }
            catch
            {
                labelDepositMessage.Text = "Lütfen önce bir miktar yazın.";
                return;
            }

            var data = await Global.DataService.Post<string, BankOperationUpdateRequest>(new BankOperationUpdateRequest
            {
                TransactionCode = transactionCode,
                AcceptedAmount = _amount
            });

            if (data.responseStatus == "ERROR")
            {
                UserDialogs.Instance.ShowError(data.errorDefiniton, 3000);
            }
            else
            {
                UserDialogs.Instance.ShowSuccess(TranslateExtension.Translate(53), 1000);

                //RefreshBalances();
            }
        }

        private async void pickerWithdrawCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dataBanka = await Global.DataService.Post<List<Bank>, GetWithdrawBanksRequest>(new GetWithdrawBanksRequest { Currency = Currency, MemberId = (int)Global.MemberInfo.Id });

            pickerWithdrawBanka.ItemsSource = dataBanka.data;
        }

        public async void buttonWithdrawOrder_Clicked(object sender, EventArgs e)
        {
            if (Currency == null)
            {
                labelWithdrawMessage.Text = "Lütfen önce para birimi seçin.";
                return;
            }

            if (pickerWithdrawBanka.SelectedItem == null)
            {
                labelWithdrawMessage.Text = "Lütfen önce banka hesabı seçin.";
                return;
            }

            decimal _amount = 0;

            try
            {
                _amount = -Convert.ToDecimal(entryWithdrawAmount.Text);
            }
            catch
            {
                labelWithdrawMessage.Text = "Lütfen önce bir miktar yazın.";
                return;
            }
            
            var data = await Global.DataService.Post<string, BankOperationInsertRequest>(new BankOperationInsertRequest
            {
                FinancialMethod = 2,
                Currency = Currency,
                BankAccountId = ((Bank)pickerWithdrawBanka.SelectedItem).Id,
                Amount = _amount,
                NoteText = ""
            });

            if (data.responseStatus == "ERROR")
            {
                UserDialogs.Instance.ShowError(data.errorDefiniton, 3000);
            }
            else
            {
                UserDialogs.Instance.ShowSuccess(TranslateExtension.Translate(53), 1000);

                transactionCode = data.data;
                labelWithdrawMessage.Text = "Gönderim Kodunuz:" + data.data + "   Bu kod ile para çekme talebinizi takip edebilirsiniz.";
                buttonWithdrawOrder.IsEnabled = false;
                buttonWithdrawOrderTest.IsVisible = true;
            }
        }

        public async void buttonWithdrawOrderTest_Clicked(object sender, EventArgs e)
        {
            decimal _amount = 0;

            try
            {
                _amount = -Convert.ToDecimal(entryWithdrawAmount.Text);
            }
            catch
            {
                labelWithdrawMessage.Text = "Lütfen önce bir miktar yazın.";
                return;
            }

            var data = await Global.DataService.Post<string, BankOperationUpdateRequest>(new BankOperationUpdateRequest
            {
                TransactionCode = transactionCode,
                AcceptedAmount = _amount
            });

            if (data.responseStatus == "ERROR")
            {
                UserDialogs.Instance.ShowError(data.errorDefiniton, 3000);
            }
            else
            {
                UserDialogs.Instance.ShowSuccess(TranslateExtension.Translate(53), 1000);

                //RefreshBalances();
            }
        }
    }
}