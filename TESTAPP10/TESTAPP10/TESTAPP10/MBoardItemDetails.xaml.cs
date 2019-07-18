using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TESTAPP10
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MBoardItemDetails : ContentPage
	{
		public MBoardItemDetails ()
		{
			InitializeComponent ();
		}
        public MBoardItemDetails(string values)
        {
            InitializeComponent();
            LoadDetails(values);
             
        }

        public async void LoadDetails(string ManifestNo)
        {
            date.Text = DateTime.Now.ToString("MMMM dd yyyy");
            manifestno.Text = "Manifest # " + ManifestNo;
            var customer = from s in App.SqlLiteCon().Table<Customer>() select s;
            var username = "";
            var CompanyId = "";
            var InviteCode = "";

            foreach (var c in customer)
            {
                username = c.UserId;
                //Type = c.Type;
                lblwlcm.Text = "Welcome " + username;
                InviteCode = c.XCode;
                CompanyId = c.CompanyID;
                break;
            }
           var resp = App.SOAP_Request.MBoardDataDetails(username.Trim(), InviteCode, ManifestNo, CompanyId);

            //var resp = "{ \"Manifest\": [{ \"Message\": \"OK\", \"ID\": \"1253001\", \"MNo\": \"1\", \"Date\": \"01 / 01 / 2017\", \"Mtype\": \"pickup\", \"Status\": \"Out For Delivery\", \"SLine1\": \"2020 EXHIBITS\", \"SLine2\": \"10550 S.SAM HOUSTON PKWY W HOUSTON TX,77071\", \"CLine1\": \"36 CS MSG COMM - F1C344\", \"CLine2\": \"ARACELI PATAGUE APO,AP GUAM,96543\" }, { \"Message\": \"OK\", \"ID\": \"1253001\", \"MNo\": \"2\", \"Date\": \"02 / 10 / 2017\", \"Mtype\": \"delivery\", \"Status\": \"Out For Delivery\", \"SLine1\": \"2020 EXHIBITS\", \"SLine2\": \"10550 S.SAM HOUSTON PKWY W HOUSTON TX,77071\", \"CLine1\": \"36 CS MSG COMM - F1C344\", \"CLine2\": \"ARACELI PATAGUE APO,AP GUAM,96543\" }] }";


            if (resp.Contains("\"Manifest\":"))
            {

                MD_RootObject response = JsonConvert.DeserializeObject<MD_RootObject>(resp);

                foreach(var a in response.Manifest)
                {
                    if (a.Mtype.Trim().ToLower() == "pickup")
                        a.MtypeColor = "#B5D6A7";
                    else if (a.Mtype.Trim().ToLower() == "delivery")
                        a.MtypeColor = "#71A6D8";
                    else if (a.Mtype.Trim().ToLower() == "direct")
                        a.MtypeColor = "#FFFFFD";

                }
                MboardData.ItemsSource = response.Manifest;
            }
            else
            {
                await DisplayAlert("", resp, "OK");
            }
        }

        private async void Sigout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private async void Signout_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private async void Home_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private void MboardData_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void Manifest_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MBoard());
        }

        private async void Sline1_Tapped(object sender, EventArgs e)
        {
            Label lblClicked = (Label)sender;
            if (lblClicked != null)
            {
                var clicked = lblClicked.Text;
                openmap(clicked);
            }
        }

        public void openmap(string address)
        {
            Uri url = null;
            Device.OnPlatform(iOS: () =>
            {
                url = new Uri(string.Format("http://maps.apple.com/maps?q={0}", address.Replace(" ","+")));
            },
            Android: () =>
            {
                url = new Uri(string.Format("http://maps.google.com/maps?q={0}", address.Replace(" ","+")));
            });

            Device.OpenUri(url);
        }

        private void Cline1_Tapped(object sender, EventArgs e)
        {
            Label lblClicked = (Label)sender;
            if (lblClicked != null)
            {
                var clicked = lblClicked.Text;
                openmap(clicked);
            }
        }
    }


    public class MD_Manifest
    {
        public string Message { get; set; }
        public string ID { get; set; }
        public string MNo { get; set; }
        public string Date { get; set; }
        public string Mtype { get; set; }
        public string Status { get; set; }
        public string SLine1 { get; set; }
        public string SLine2 { get; set; }
        public string CLine1 { get; set; }
        public string CLine2 { get; set; }
        public string MtypeColor { get; set; }
    }

    public class MD_RootObject
    {
        public List<MD_Manifest> Manifest { get; set; }
    }


}