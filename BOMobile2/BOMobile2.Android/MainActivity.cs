using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.Permissions;
using ZXing.Mobile;

namespace BOMobile2.Droid
{
    [Activity(Label = "BOMobile2", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            
            global::Xamarin.Forms.Forms.Init(this, bundle);

            Acr.UserDialogs.UserDialogs.Init(this);

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;

            ZXing.Net.Mobile.Forms.Android.Platform.Init();

            MobileBarcodeScanner.Initialize(Application);

            Global.Ftp = new Util.FtpProvider();
            Global.Encrypt = new Util.StringEncyrption();

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

