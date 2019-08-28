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

            Application.Current.Properties.Clear();
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
