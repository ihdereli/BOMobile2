using Xamarin.Forms;

namespace BOMobile2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            Global.Language = Global.GetLanguage();
            Global.DataService = new Services.MobileDataService();

            //MainPage = new NavigationPage(new Camera());
            MainPage = new NavigationPage(new Loading());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
