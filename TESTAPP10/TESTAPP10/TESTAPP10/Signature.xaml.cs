using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TESTAPP10
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Signature : ContentPage
	{
		public Signature ()
		{
			InitializeComponent ();

            var HAWB = Application.Current.Properties.ContainsKey("SignHawb") ? Application.Current.Properties["SignHawb"] as string : "";
            var MoveType = Application.Current.Properties.ContainsKey("SignMtype") ? Application.Current.Properties["SignMtype"] as string : "";
            var ServiceDate = Application.Current.Properties.ContainsKey("SignDate") ? Application.Current.Properties["SignDate"] as string : "";
            var SignRefNo = Application.Current.Properties.ContainsKey("SignRefNo") ? Application.Current.Properties["SignRefNo"] as string : "";
           
            LoadValues(HAWB, MoveType, ServiceDate, SignRefNo);
        }
        public Signature(string HAWB, string MoveType, string ServiceDate,string RefNo)
        {
            InitializeComponent();
            LoadValues(HAWB, MoveType, ServiceDate,RefNo);
        }
        public void LoadValues(string HAWB, string MoveType, string ServiceDate,string RefNo)
        {

            Application.Current.Properties["SignHawb"] = HAWB;
            Application.Current.Properties["SignMtype"] = MoveType;
            Application.Current.Properties["SignDate"] = ServiceDate;
            Application.Current.Properties["SignRefNo"] = RefNo;

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
            var username = Application.Current.Properties["username"];
            lblwlcm.Text = "Welcome " + username;

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

        private async void Btnsave_Clicked(object sender, EventArgs e)
        {
            try
            {

                Stream image = await signature1.GetImageStreamAsync(SignaturePad.Forms.SignatureImageFormat.Jpeg);

                if((image==null)|| (string.IsNullOrEmpty(NAMEid.Text)))
                {
                    await DisplayAlert("", "Signature and Name are required.", "OK");
                    return;
                }

                string name = string.IsNullOrEmpty(NAMEid.Text) ? "" : NAMEid.Text.Trim().TrimStart();
                string emailid = string.IsNullOrEmpty(EMAILentry.Text) ? "" : EMAILentry.Text.Trim().TrimStart();

                byte[] bytes = StreamToByte(image);
                string imgBase64String = Convert.ToBase64String(bytes);

                var HAWB = Application.Current.Properties.ContainsKey("SignHawb") ? Application.Current.Properties["SignHawb"] as string : "";
                var MoveType = Application.Current.Properties.ContainsKey("SignMtype") ? Application.Current.Properties["SignMtype"] as string : "";
                var ServiceDate = Application.Current.Properties.ContainsKey("SignDate") ? Application.Current.Properties["SignDate"] as string : "";
                var SignRefNo = Application.Current.Properties.ContainsKey("SignRefNo") ? Application.Current.Properties["SignRefNo"] as string : "";


                var customer = from s in App.SqlLiteCon().Table<Customer>() select s;

                var username = "";
                var CompanyId = "";
                var InviteCode = "";
                var Url = "";

                foreach (var c in customer)
                {
                    username = c.UserId;
                    InviteCode = c.XCode;
                    CompanyId = c.CompanyID;
                    Url = c.TransactURL;
                    break;
                }


                var Imgresp = App.SOAP_Request.PostSignature(SignRefNo, HAWB, imgBase64String, name, emailid, username.Trim(), CompanyId, InviteCode, Url);

                if (!string.IsNullOrEmpty(Imgresp))
                {
                    Sign_RootObject Btnshipresponse = JsonConvert.DeserializeObject<Sign_RootObject>(Imgresp);
                    foreach (var re in Btnshipresponse.Details)
                    {
                        if (re.Message.ToLower() == "ok")
                        {
                            await DisplayAlert("", "Signature saved.", "OK");
                            await Navigation.PushAsync(new ShipmentDetails());
                        }
                        else
                            await DisplayAlert("", "Signature could not be saved.", "OK");
                    }
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");
            }

        }


        public static byte[] StreamToByte(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private void Btnreset_Clicked(object sender, EventArgs e)
        {
            NAMEid.Text = "";
            EMAILentry.Text = "";
            signature1.Clear();
        }
    }

    public class Sign_Detail
    {
        public string Message { get; set; }
        public string ID { get; set; }
        public string UserId { get; set; }
    }

    public class Sign_RootObject
    {
        public List<Sign_Detail> Details { get; set; }
    }
}