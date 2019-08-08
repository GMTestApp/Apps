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

            var ManifestNo = Application.Current.Properties.ContainsKey("ManifestNo") ? Application.Current.Properties["ManifestNo"] as string : "";

            LoadDetails(ManifestNo);
        }
        public MBoardItemDetails(string values)
        {
            InitializeComponent();
            LoadDetails(values);
            MboardData.ItemTapped += async (sender, e) =>
            {
                MD_Manifest details = e.Item as MD_Manifest;
                string value = details.ID.ToString();
                string Mtype = details.Mtype.ToString();
                string Date = details.Date.ToString();
                await Navigation.PushAsync(new ShipmentDetails(value, Mtype, Date));

            };
        }

        public async void LoadDetails(string ManifestNo)
        {
            Application.Current.Properties["ManifestNo"] = ManifestNo;
            date.Text = DateTime.Now.ToString("MMM dd yyyy");
            manifestno.Text = "Manifest # " + ManifestNo;
            var customer = from s in App.SqlLiteCon().Table<Customer>() select s;
            var username = "";
            var CompanyId = "";
            var InviteCode = "";
            var Url = "";

            foreach (var c in customer)
            {
                username = c.UserId;
                //Type = c.Type;
                lblwlcm.Text = "Welcome " + username;
                InviteCode = c.XCode;
                CompanyId = c.CompanyID;
                Url = c.TransactURL;
                break;
            }
           var resp = App.SOAP_Request.MBoardDataDetails(username.Trim(), InviteCode, ManifestNo, CompanyId, Url);

            //var resp = "{ \"Manifest\": [{ \"Message\": \"OK\", \"ID\": \"1253001\", \"MNo\": \"1\", \"Date\": \"01 / 01 / 2017\", \"Mtype\": \"pickup\", \"Status\": \"Out For Delivery\", \"SLine1\": \"2020 EXHIBITS\", \"SLine2\": \"10550 S.SAM HOUSTON PKWY W HOUSTON TX,77071\", \"CLine1\": \"36 CS MSG COMM - F1C344\", \"CLine2\": \"ARACELI PATAGUE APO,AP GUAM,96543\" }, { \"Message\": \"OK\", \"ID\": \"1253001\", \"MNo\": \"2\", \"Date\": \"02 / 10 / 2017\", \"Mtype\": \"delivery\", \"Status\": \"Out For Delivery\", \"SLine1\": \"2020 EXHIBITS\", \"SLine2\": \"10550 S.SAM HOUSTON PKWY W HOUSTON TX,77071\", \"CLine1\": \"36 CS MSG COMM - F1C344\", \"CLine2\": \"ARACELI PATAGUE APO,AP GUAM,96543\" }] }";


            if (resp.Contains("\"Manifest\":"))
            {

                MD_RootObject response = JsonConvert.DeserializeObject<MD_RootObject>(resp);
                bool isdataexist = true;
                foreach (var a in response.Manifest)
                {
                    if (a.Message.ToLower() != "ok")
                    {
                        isdataexist = false;
                        break;
                    }
                    if (a.Mtype.Trim().ToLower() == "pickup")
                        a.MtypeColor = "#B5D6A7";
                    else if (a.Mtype.Trim().ToLower() == "delivery")
                        a.MtypeColor = "#71A6D8";
                    else if (a.Mtype.Trim().ToLower() == "direct")
                        a.MtypeColor = "#FFFFFD";
                    else
                        a.MtypeColor = "#FFFFFD";
                    a.Date = !string.IsNullOrEmpty(Convert.ToString(a.Date)) ? Convert.ToDateTime(a.Date).Date.ToString("MM/dd/yyyy") : "";
                }
                if (isdataexist)
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

        
        private async void Manifest_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MBoard());
        }

       

        public void openmap(string address)
        {
            Uri url = null;
            Device.OnPlatform(iOS: () =>
            {
                url = new Uri(string.Format("http://maps.apple.com/maps?q={0}", address.Replace(" ", "+").Replace("\r", "+").Replace("\n", "+")));
            },
            Android: () =>
            {
                url = new Uri(string.Format("http://maps.google.com/maps?q={0}", address.Replace(" ", "+").Replace("\r", "+").Replace("\n", "+")));
            });

            Device.OpenUri(url);
        }

      

        private void Grid_Tapped(object sender, EventArgs e)
        {
            var button = sender as Grid;
            if (button != null)
            {
                Label label = (Label)button.Children[0];
                if (label != null)
                {
                    var clicked = label.Text;
                    if (!string.IsNullOrEmpty(clicked))
                        openmap(clicked.Trim());
                }
            }
        }

        private void Grid1_Tapped(object sender, EventArgs e)
        {
            var button = sender as Grid;
            if (button != null)
            {
                Label label = (Label)button.Children[0];
                if (label != null)
                {
                    var clicked = label.Text.Trim();
                    if (!string.IsNullOrEmpty(clicked))
                        openmap(clicked.Trim());
                }
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