using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TESTAPP10
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SBoardDataDetails : ContentPage
    {
        public SBoardDataDetails()
        {
            InitializeComponent();
            LoadData();
          
            ListSBoard.ItemTapped += async (sender, e) =>
            {

                SBoard_Shipment details = e.Item as SBoard_Shipment;
                string value = details.ID.ToString();
                string Mtype = details.Mtype.ToString();
                string Date = details.Date.ToString();
                await Navigation.PushAsync(new ShipmentDetails(value, Mtype, Date));
            };
        }

        public async void LoadData()
        {
            try
            {
                var customer = from s in App.SqlLiteCon().Table<Customer>() select s;
                var username = "";
                var password = "";
                var InviteCode = "";
                var Type = "";
                var CompanyId = "";
                var Url = "";

                foreach (var c in customer)
                {
                    username = c.UserId;
                    password = c.Password;
                    Type = c.Type;
                    InviteCode = c.XCode;
                    CompanyId = c.CompanyID;
                    Url = c.TransactURL;
                    break;
                }
                date.Text = DateTime.Now.ToString("MMM dd yyyy");
                lblwlcm.Text = "Welcome " + username;

                var resp = App.SOAP_Request.SBoardDataDetails(username.Trim(), InviteCode, CompanyId, Url);

                //var  resp = "{ \"Shipments\": [{ \"Message\": \"OK\", \"ID\": \"1253001\",  \"Date\": \"01 / 01 / 2017\", \"Mtype\": \"pickup\", \"Status\": \"Out For Delivery\", \"SLine1\": \"2020 EXHIBITS\", \"SLine2\": \"10550 S.SAM HOUSTON PKWY W HOUSTON TX,77071\", \"CLine1\": \"36 CS MSG COMM - F1C344\", \"CLine2\": \"ARACELI PATAGUE APO,AP GUAM,96543\" }, { \"Message\": \"OK\", \"ID\": \"1253001\", \"Date\": \"02 / 10 / 2017\", \"Mtype\": \"delivery\", \"Status\": \"Out For Delivery\", \"SLine1\": \"2020 EXHIBITS\", \"SLine2\": \"10550 S.SAM HOUSTON PKWY W HOUSTON TX,77071\", \"CLine1\": \"36 CS MSG COMM - F1C344\", \"CLine2\": \"ARACELI PATAGUE APO,AP GUAM,96543\" }] }";

                if (resp.ToLower().Contains("manifest"))
                    resp = resp.Replace("Manifest", "Shipments");

                if (resp.ToLower().Contains("\"shipment\":"))
                {
                    SBoard_RootObject response = JsonConvert.DeserializeObject<SBoard_RootObject>(resp);
                    
                    bool isdataexist = true;
                    foreach (var a in response.Shipment)
                    {
                        if(a.Message.ToLower()!="ok")
                        {
                            isdataexist = false;
                            break;
                        }

                        if (a.Mtype.Trim().ToLower() == "pickup")
                            a.MtypeColor = "#B5D6A7";
                        else if (a.Mtype.Trim().ToLower() == "delivery")
                            a.MtypeColor = "#71A6D8";
                        else if (a.Mtype.Trim().ToLower() == "recovery")
                            a.MtypeColor = "#B5D6A7";
                        else if (a.Mtype.Trim().ToLower() == "drop")
                            a.MtypeColor = "#71A6D8";
                        else if (a.Mtype.Trim().ToLower() == "direct")
                            a.MtypeColor = "#EEE";
                        else
                            a.MtypeColor = "#EEE";


                        if (!string.IsNullOrEmpty(Convert.ToString(a.Date)))
                            a.Date = a.Date.Split(' ')[0] ;

                        a.CLine2 = a.CLine2.Trim().Trim(',').Trim(' ');
                        a.SLine2 = a.SLine2.Trim().Trim(',').Trim(' ');
                       
                    }
                    if(isdataexist)
                      ListSBoard.ItemsSource = response.Shipment;
                   
                }
                else
                {
                    await DisplayAlert("", resp, "OK");
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");
            }
        }

        private async void Sigout_Clicked(object sender, EventArgs e)
        {
            App.CloseLoginBackThread();
            await Navigation.PushAsync(new MainPage());
        }

       

        private async void Signout_Tapped(object sender, EventArgs e)
        {
            App.CloseLoginBackThread();
            await Navigation.PushAsync(new MainPage());
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
           // address= Regex.Replace(address, @"[^0-9a-zA-Z\,_]", "+");
            Uri url = null;
            Device.OnPlatform(iOS: () =>
            {
                url = new Uri(string.Format("http://maps.apple.com/maps?q={0}", (address.Replace(" ", "+").Replace("\r", "+").Replace("\n", "+").Replace("{", "+").Replace("}", "+")).Trim('+')));
            },
            Android: () =>
            {
                url = new Uri(string.Format("http://maps.google.com/maps?q={0}", (address.Replace(" ", "+").Replace("\r", "+").Replace("\n", "+").Replace("{", "+").Replace("}", "+")).Trim('+')));
            });

            Device.OpenUri(url);
        }
        private async void Home_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
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

        public class SBoard_Shipment
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

        public class SBoard_RootObject
        {
            public List<SBoard_Shipment> Shipment { get; set; }
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
                    if(!string.IsNullOrEmpty(clicked))
                        openmap(clicked.Trim());
                }
            }
        }

        
    }
}