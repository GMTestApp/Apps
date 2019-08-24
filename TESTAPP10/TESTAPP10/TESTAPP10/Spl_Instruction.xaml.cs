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
	public partial class Spl_Instruction : ContentPage
	{
		public Spl_Instruction ()
		{
			InitializeComponent ();
            date.Text = DateTime.Now.ToString("MMM dd yyyy");
        }
        public Spl_Instruction(string HAWB, string MoveType,string ServiceDate,string SP)
        {
            InitializeComponent();
            date.Text = DateTime.Now.ToString("MMM dd yyyy");
            lblspmsg.Text = "";
            lblhawb.Text = HAWB;
            lblmovetype.Text = MoveType;
            lblservicedate.Text = ServiceDate;
            lblspmsg.Text = SP;
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

        private async void Signout_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

       
        private async void Sigout_Clicked(object sender, EventArgs e)
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
            //await Navigation.PushAsync(new ShipmentDetails(lblhawb.Text.Replace("Shipment # ", ""), lblmovetype.Text.Replace("Move Type : ", ""), lblservicedate.Text.Replace("Service Date : ", "")));
            await Navigation.PushAsync(new ShipmentDetails());
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShipmentDetails());
        }

        private async void TGlblship_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SBoardDataDetails());
        }
    }
}