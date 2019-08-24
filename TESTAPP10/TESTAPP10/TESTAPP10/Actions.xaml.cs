using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static TESTAPP10.ShipmentDetails;

namespace TESTAPP10
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Actions : ContentPage
    {
        public Actions()
        {
            InitializeComponent();
            // date.Text = DateTime.Now.ToString("MMM dd yyyy");

            var Response = Application.Current.Properties.ContainsKey("AcLoadResponse") ? Application.Current.Properties["AcLoadResponse"] as string : "";

            var HAWB = Application.Current.Properties.ContainsKey("AcHawb") ? Application.Current.Properties["AcHawb"] as string : "";
            var MoveType = Application.Current.Properties.ContainsKey("AcMtype") ? Application.Current.Properties["AcMtype"] as string : "";
            var ServiceDate = Application.Current.Properties.ContainsKey("AcDate") ? Application.Current.Properties["AcDate"] as string : "";

            LoadValues(HAWB, MoveType, ServiceDate, Response);

        }
        public Actions(string HAWB, string MoveType, string ServiceDate, string Response)
        {
            InitializeComponent();
            LoadValues(HAWB, MoveType, ServiceDate, Response);


        }

        public async void LoadValues(string HAWB, string MoveType, string ServiceDate, string Response)
        {
            try
            {
                Application.Current.Properties["AcHawb"] = HAWB;
                Application.Current.Properties["AcMtype"] = MoveType;
                Application.Current.Properties["AcDate"] = ServiceDate;

                Application.Current.Properties["AcLoadResponse"] = Response;

                date.Text = DateTime.Now.ToString("MMM dd yyyy");

                lblhawb.Text = "Shipment # " + HAWB;
                lblmovetype.Text = "Move Type : " + MoveType;
                lblservicedate.Text = "Service Date : " + ServiceDate;

                string Type = Application.Current.Properties["Type"].ToString();
                if (Type.ToLower() == "m")
                {
                    lblmanif.IsVisible = true;
                    lblship.IsVisible = false;
                }
                else
                {
                    lblmanif.IsVisible = false;
                    lblship.IsVisible = true;
                }

                var customer = from s in App.SqlLiteCon().Table<Customer>() select s;

                foreach (var c in customer)
                {
                    lblwlcm.Text = "Welcome " + c.UserId;
                    break;
                }
                
                LoadActionList( Response);
            }
            catch(Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");
            }

        }

        public async void LoadActionList(string Response)
        {

            try
            {
                List<string> act = new List<string>();
                SD_RootObject response = JsonConvert.DeserializeObject<SD_RootObject>(Response);

                string status = "";
                foreach (var li in response.Details)
                {
                    act = li.Action.Split(',').Length > 0 ? li.Action.Split(',').ToList() : null;
                    status = li.Status.Trim().TrimStart();
                }
                List<ACtionList> ActionList = new List<ACtionList>();
               
                bool isselect = false;
                foreach (var a in act)
                {
                    if (string.IsNullOrEmpty(a.Trim()))
                        continue;

                    ACtionList ad = new ACtionList();
                    ad.Action = a.Trim().TrimStart();

                    if ((!isselect) &&(ad.Action==status))
                    {
                        ad.ImageName = "radio.png";
                        isselect = true;
                    }
                    else
                        ad.ImageName = "unradio.png";

                    ActionList.Add(ad);
                }

                if (ActionList.Count > 0)
                    lvaclist.ItemsSource = ActionList;
                else
                {
                    btnsave.IsEnabled = false;
                    btnreset.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");
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

        private async void Signin_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private async void Manifests_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MBoardItemDetails());
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShipmentDetails());
        }

        private async void TGlblship_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SBoardDataDetails());
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShipmentDetails());
        }
        
        private async void TapGestureRecognizer_Action_Tapped_1(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Grid;
                if (button != null)
                {
                    var childlbl = (Label)button.Children[1];
                    if (childlbl != null)
                    {
                        var clicked = childlbl.Text.Trim(); ;

                        var Response = Application.Current.Properties.ContainsKey("AcLoadResponse") ? Application.Current.Properties["AcLoadResponse"] as string : "";

                        List<string> act = new List<string>();
                        SD_RootObject response = JsonConvert.DeserializeObject<SD_RootObject>(Response);

                        foreach (var li in response.Details)
                            act = li.Action.Split(',').Length > 0 ? li.Action.Split(',').ToList() : null;

                        bool isselected=false;
                        List<ACtionList> ActionList = new List<ACtionList>();
                        foreach (var a in act)
                        {
                            if (string.IsNullOrEmpty(a.Trim()))
                                continue;
                            ACtionList ad = new ACtionList();
                            ad.Action = a.Trim();

                            if((!isselected)&&(a.ToLower().TrimStart() == clicked.ToLower().TrimStart()))
                            {
                                ad.ImageName = "radio.png";
                                isselected = true;
                            }
                            else
                                ad.ImageName = "unradio.png";

                            ActionList.Add(ad);
                        }

                        lvaclist.ItemsSource = ActionList;

                        Application.Current.Properties["ActionClicked"] = clicked;

                    }
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");
            }
        }

        private async void Btnsave_Clicked(object sender, EventArgs e)
        {

            try
            {
                var ActionClicked = Application.Current.Properties.ContainsKey("ActionClicked") ? Application.Current.Properties["ActionClicked"] as string : "";

                var HAWB = Application.Current.Properties.ContainsKey("AcHawb") ? Application.Current.Properties["AcHawb"] as string : "";
                var MoveType = Application.Current.Properties.ContainsKey("AcMtype") ? Application.Current.Properties["AcMtype"] as string : "";
                var ServiceDate = Application.Current.Properties.ContainsKey("AcDate") ? Application.Current.Properties["AcDate"] as string : "";
                var customer = from s in App.SqlLiteCon().Table<Customer>() select s;
                var username = "";
                var CompanyId = "";
                var InviteCode = "";
                var Url = "";


                //var DGargo = "";
                var RefNo = "";
                foreach (var c in customer)
                {
                    username = c.UserId;
                    InviteCode = c.XCode;
                    CompanyId = c.CompanyID;
                    Url = c.TransactURL;
                    break;
                }

                var resp = Application.Current.Properties.ContainsKey("AcLoadResponse") ? Application.Current.Properties["AcLoadResponse"] as string : "";

                SD_RootObject response = JsonConvert.DeserializeObject<SD_RootObject>(resp);

                foreach (var a in response.Details)
                {
                    //DGargo = a.DCargo;
                    RefNo = a.RefNo;
                    if (string.IsNullOrEmpty(ActionClicked))
                        ActionClicked = a.Status.Trim().TrimStart();
                }


                var ACresp = App.SOAP_Request.ShipmentActionUpdate(RefNo, HAWB, ActionClicked, MoveType, username.Trim(), CompanyId, InviteCode, Url);

                Action_RootObject Btnshipresponse = JsonConvert.DeserializeObject<Action_RootObject>(ACresp);

                if (Btnshipresponse.Details[0].Message.ToLower().Contains("ok"))
                {
                    await DisplayAlert("", "Driver Actions Updated.", "OK");
                    var Acresp = App.SOAP_Request.LoadDetails(HAWB, username.Trim(), MoveType, InviteCode, CompanyId, Url);
                    Application.Current.Properties["AcLoadResponse"] = Acresp;
                }
                else
                    await DisplayAlert("", "Driver Actions could not be Updated.", "OK");

                   
            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");
            }
        }

        private void Btnreset_Clicked(object sender, EventArgs e)
        {
            var Response = Application.Current.Properties.ContainsKey("AcLoadResponse") ? Application.Current.Properties["AcLoadResponse"] as string : "";
            Application.Current.Properties["ActionClicked"] = "";
            LoadActionList(Response);

        }
    }


    public class ACtionList
    {
        public string Action { get; set; }
        public string ImageName { get; set; }
    }



    public class Action_Detail
    {
        public string Message { get; set; }
        public string ID { get; set; }
        public string UserId { get; set; }
      
    }

    public class Action_RootObject
    {
        public List<Action_Detail> Details { get; set; }
    }

}