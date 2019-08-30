using SQLite;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TESTAPP10
{
    public partial class App : Application
    {
        public static SOAP_Request SOAP_Request { get; set; }
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }
        public static SQLiteConnection SqlLiteCon()
        {
            return new SQLiteConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "DriverTrakSQLite.db3"));
        }
        protected override void OnStart()
        {
            // Handle when your app starts
            var StopSend = Application.Current.Properties.ContainsKey("StopSend") ? Application.Current.Properties["StopSend"] as string : "";
            Application.Current.Properties.Clear();
            if (!string.IsNullOrEmpty(StopSend))
                Application.Current.Properties["StopSend"] = StopSend;


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
