﻿using Acr.UserDialogs;
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

            var dataDepositCurrency = await Global.DataService.Post<List<Currency>, GetDepositCurrenciesRequest>(new GetDepositCurrenciesRequest { });

            pickerDepositCurrency.ItemsSource = dataDepositCurrency.data;

            var dataWithdrawCurrency = await Global.DataService.Post<List<Currency>, GetWithdrawCurrenciesRequest>(new GetWithdrawCurrenciesRequest { MemberId = (int)Global.MemberInfo.Id });

            pickerWithdrawCurrency.ItemsSource = dataWithdrawCurrency.data;

            RefreshBalances();

            base.OnAppearing();
            
            UserDialogs.Instance.HideLoading();
        }

        private async void RefreshBalances()
        {
            var dataBalance = await Global.DataService.Post<List<MemberBalance>, MemberBalancesRequest>(new MemberBalancesRequest { CurrencyId = null });

            MemberBalances.ItemsSource = dataBalance.data;
        }

        private async void pickerDepositCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            var _currency = ((Currency)pickerDepositCurrency.SelectedItem).Id;

            var dataBanka = await Global.DataService.Post<List<Bank>, GetDepositBanksRequest>(new GetDepositBanksRequest { Currency = _currency });

            pickerDepositBanka.ItemsSource = dataBanka.data;
        }

        string transactionCode = "";
        public async void buttonDepositOrder_Clicked(object sender, EventArgs e)
        {
            if (pickerDepositCurrency.SelectedItem == null)
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
                Currency = ((Currency)pickerDepositCurrency.SelectedItem).Id, 
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

                RefreshBalances();
            }
        }

        private async void pickerWithdrawCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            var _currency = ((Currency)pickerWithdrawCurrency.SelectedItem).Id;

            var dataBanka = await Global.DataService.Post<List<Bank>, GetWithdrawBanksRequest>(new GetWithdrawBanksRequest { Currency = _currency, MemberId = (int)Global.MemberInfo.Id });

            pickerWithdrawBanka.ItemsSource = dataBanka.data;
        }
        
        public async void buttonWithdrawOrder_Clicked(object sender, EventArgs e)
        {
            if (pickerWithdrawCurrency.SelectedItem == null)
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
                Currency = ((Currency)pickerWithdrawCurrency.SelectedItem).Id,
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

                RefreshBalances();
            }
        }
    }
}