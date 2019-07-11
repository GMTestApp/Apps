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
	public partial class ResetPassword : ContentPage
	{

        public ResetPassword()
        {
            InitializeComponent();
            getusername();
        }
        //public ResetPassword(string username)
        //{
        //    InitializeComponent();
        //    getusername();
        //}

        public void getusername()
        {
            var customer = from s in App.SqlLiteCon().Table<Customer>() select s;

            foreach (var c in customer)
            {
                txtuname.Text = c.UserId.Trim();
                txtuname.IsEnabled = false;
                break;
            }

        }

        private async void Resetpwd_Clicked(object sender, EventArgs e)
        {
            try
            {
                var userid = txtuname.Text;
                var password = txtpass.Text;

                if ((string.IsNullOrEmpty(userid)) || (string.IsNullOrEmpty(password)))
                {
                    lblmsg.Text = "Please fill all the fields.";
                    errorframe.IsVisible = true;
                    lblmsg.IsVisible = true;
                    return;
                }
                //if (Convert.ToString(password).Length < 7)
                //{
                //    lblmsg.Text = "Password did not pass our security criteria. Please try another password. Hint: No space, Certain special characters are not allowed.";
                //    return;
                //}

                if ((SecurityCheck.isTampered((Convert.ToString(password) == null ? "" : password))) || (Convert.ToString(password).Length < 7))
                {
                    lblmsg.Text = "Username or Password provided is invalid OR Password did not pass our security criteria. Hint: Needs to be minimum 6 chars long, No Spaces, certain special characters are not allowed.";
                    errorframe.IsVisible = true;
                    lblmsg.IsVisible = true;
                    return;
                }

                var InviteCode = App.SqlLiteCon().Query<Customer>("SELECT * FROM Customer WHERE UserId = ?", userid).Count > 0 ? App.SqlLiteCon().Query<Customer>("SELECT * FROM Customer WHERE UserId = ?", userid)[0].XCode : "";

                // var res = App.SOAP_Request.ValidateLogins(userid.Trim(), TripleDES.Encrypt(password.Trim()), InviteCode.Trim());

                var rsds = App.SOAP_Request.ForgotPassword(userid.Trim(), TripleDES.Encrypt(password.Trim()), InviteCode.Trim());
                //await DisplayAlert("", rsds, "OK");
                if (rsds.ToLower().Contains("success"))
                {
                    App.SqlLiteCon().Execute("update Customer set Password='" + password.Trim() + "' where UserId = ?", userid);
                    login.IsVisible = false;
                    frmfaild.IsVisible = false;
                    frmsuccess.IsVisible = true;
                }
                else
                {
                    login.IsVisible = false;
                    frmfaild.IsVisible = true;
                    frmsuccess.IsVisible = false;

                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");
            }
        }
        private async void BackTosignIn_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private async void Warning_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private async void Tick_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private void Resetuser_Tapped(object sender, EventArgs e)
        {

        }
    }
}