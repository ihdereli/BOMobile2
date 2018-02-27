using Acr.UserDialogs;
using BOMobile2.Services.Schema;
using Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace BOMobile2.Wallet
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BitcoinSendRecieve : TabbedPage
    {
        public BitcoinSendRecieve()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            UserDialogs.Instance.ShowLoading(TranslateExtension.Translate(40) + "...", MaskType.Black);

            var dataAddress = await Global.DataService.Post<string, MemberGetBitcoinAddressRequest>(new MemberGetBitcoinAddressRequest {  });

            labelRecieveAddress.Text = dataAddress.data;

            ZXingBarcodeImageView b = new ZXingBarcodeImageView
            {
                 HorizontalOptions = LayoutOptions.FillAndExpand, 
                 VerticalOptions = LayoutOptions.FillAndExpand 
            };

            b.BarcodeFormat = BarcodeFormat.QR_CODE;
            b.BarcodeOptions.Width = 300;
            b.BarcodeOptions.Height = 300;
            b.BarcodeOptions.Margin = 10;
            b.BarcodeValue = dataAddress.data;

            fBarcode.Content = b;

            base.OnAppearing();

            UserDialogs.Instance.HideLoading();
        }

        private async void buttonScan_Clicked(object sender, EventArgs e)
        {
#if __ANDROID__
	// Initialize the scanner first so it can track the current context
	MobileBarcodeScanner.Initialize (Application);
#endif

            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            
            var result = await scanner.Scan(new MobileBarcodeScanningOptions { PossibleFormats = new List<BarcodeFormat>() { BarcodeFormat.QR_CODE } });

            if (result != null)
                entrySendAddress.Text = result.Text;
        }

        private async void buttonSend_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (entrySendAddress.Text == "")
                {
                    labelSendMessage.Text = "Lütfen önce bir adres yazın.";
                    return;
                }

                decimal _amount = 0;

                try
                {
                    _amount = Convert.ToDecimal(entrySendCoinAmount.Text);
                }
                catch
                {
                    labelSendMessage.Text = "Lütfen önce bir miktar yazın.";
                    return;
                }

                var data = await Global.DataService.Post<string, MemberSendCoinRequest>(new MemberSendCoinRequest
                {
                    CoinCurrency = "BTC",
                    Address = entrySendAddress.Text,
                    Amount = _amount
                });

                if (data.responseStatus == "OK")
                {
                    labelSendMessage.Text = "Bitcoin gönderme işlemi tamamlandı.";
                }
                else
                {
                    labelSendMessage.Text = data.errorDefiniton;
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.ShowError(ex.ToString(), 2000);
            }
        }

        private async void buttonGive_Clicked(object sender, EventArgs e)
        {
            try
            {
                var data = await Global.DataService.Post<string, MemberGiveCoinRequest>(new MemberGiveCoinRequest
                {
                    CoinCurrency = "BTC",
                    Address = "",
                    Amount = 1
                });

                if (data.responseStatus == "OK")
                {
                    labelGiveMessage.Text = "Bitcoin verme işlemi tamamlandı.";
                }
                else
                {
                    labelGiveMessage.Text = data.errorDefiniton;
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.ShowError(ex.ToString(), 2000);
            }
        }
    }
}