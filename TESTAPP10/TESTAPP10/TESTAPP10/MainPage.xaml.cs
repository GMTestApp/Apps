using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace TESTAPP10
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            loginvisibility();
        }

        private void loginvisibility()
        {
            try
            {
                bool dataexist = false;
                try
                {
                    if (App.SqlLiteCon().Table<Customer>().Count() > 0)
                        dataexist = true;
                }
                catch
                {

                }
                if (dataexist)
                {
                    login.IsVisible = true;
                    registration.IsVisible = false;
                    lblmsg2.IsVisible = false;
                    loginscreen.Text = "Log-In Screen";
                }
                else
                {
                   
                    login.IsVisible = false;
                    registration.IsVisible = true;
                    loginscreen.Text = "New User Registration";
                }
            }
            catch(Exception ex)
            {

            }
        }
        private async void CreateAccount_Clicked(object sender, EventArgs e)
        {
            try
            {
                var userid = txtuname.Text;
                var password = txtpass.Text;
                var InviteCode = txtinvitecode.Text;

                if ((string.IsNullOrEmpty(txtuname.Text)) || (string.IsNullOrEmpty(txtpass.Text)) || (string.IsNullOrEmpty(txtinvitecode.Text)))
                {
                    lblmsg2.Text = "All fields are required for registration.";
                    lblmsg2.IsVisible = true;
                    errorframe.IsVisible = true;
                    return;
                }
                if ((SecurityCheck.isTampered((Convert.ToString(txtpass.Text) == null ? "" : txtpass.Text))) || (Convert.ToString(txtpass.Text).Length < 7))
                {
                    lblmsg2.Text = "Password did not pass our security criteria. Hint: Needs to be minimum 6 chars long, No Spaces, certain special characters are not allowed.";
                    lblmsg2.IsVisible = true;
                    errorframe.IsVisible = true;
                    return;
                }
                var rsds = App.SOAP_Request.NewRegistration(userid.Trim(), TripleDES.Encrypt(password.Trim()), InviteCode.Trim());
                if (!rsds.Contains("\"CompanyID\":"))
                {
                    lblmsg2.Text = rsds;
                    lblmsg2.IsVisible = true;
                    errorframe.IsVisible = true;
                    return;
                }
                Re_Type response = null;
                try
                {
                    response = JsonConvert.DeserializeObject<Re_Type>(rsds);
                }
                catch(Exception f)
                {
                    lblmsg2.Text = f.Message;
                    lblmsg2.IsVisible = true;
                    errorframe.IsVisible = true;
                    return;
                }

                if (response.Valid.ToString().ToLower() == "y")
                {

                    Customer custom = new Customer();
                    custom.UserId = userid;
                    custom.Password = password;
                    custom.XCode = InviteCode;
                    custom.Type = response.Type != null ? response.Type : "";
                    custom.CompanyID = response.Type != null ? response.CompanyID : "";
                    custom.TransactURL = response.Type != null ? response.URL : "";

                    App.SqlLiteCon().CreateTable<Customer>();

                    App.SqlLiteCon().Insert(custom);

                    if (App.SqlLiteCon().Table<Customer>().Count() == 0)
                    {
                        lblmsg2.Text = "Invite Code Not Stored.";
                        lblmsg2.IsVisible = true;
                        errorframe.IsVisible = true;
                        return;
                    }

                    await DisplayAlert("", "Registered Successfully.", "OK");
                    await Navigation.PushAsync(new MainPage());
                }
                else
                {
                    lblmsg2.Text = "Invalid Credentials";
                    lblmsg2.IsVisible = true;
                    errorframe.IsVisible = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                lblmsg2.Text = ex.Message.ToString();
                lblmsg2.IsVisible = true;
                errorframe.IsVisible = true;
            }
        }
        private async void Signin_Clicked(object sender, EventArgs e)
        {
            try
            {
                var userid = UsernameEntrylogin.Text;
                var password = PasswordEntrylogin.Text;

                if ((string.IsNullOrEmpty(userid)) || (string.IsNullOrEmpty(password)) )
                {
                    lblmsg2.Text = "Please fill all the fields.";
                    errorframe.IsVisible = true;
                    lblmsg2.IsVisible = true;
                    return;
                }
                var customer = from s in App.SqlLiteCon().Table<Customer>().Where(s => s.UserId == userid).Where(s => s.Password == password) select s;

                if (customer.Count() == 0)
                {
                    lblmsg3.IsVisible = true;
                    errorframe.IsVisible = true;
                    lblmsg2.IsVisible = false;
                    return;
                }
                await Navigation.PushAsync(new MBoard( userid,password));
            }
            catch(Exception ex)
            {

                lblmsg2.Text = ex.Message.ToString();
                lblmsg2.IsVisible = true;
                errorframe.IsVisible = true;
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(UsernameEntrylogin.Text)))
                await Navigation.PushAsync(new ResetPassword());
            else
                await Navigation.PushAsync(new ResetPassword());
        }

        private async void Forgotpwd_Tapped(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(UsernameEntrylogin.Text)))
                await Navigation.PushAsync(new ResetPassword());
            else
                await Navigation.PushAsync(new ResetPassword());
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            txtpass.IsPassword = true;
           
        }

        private async void Forgotpwd_Tapped_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(UsernameEntrylogin.Text)))
                await Navigation.PushAsync(new ResetPassword());
            else
                await Navigation.PushAsync(new ResetPassword());
        }

        private async void Resetuser_Tapped(object sender, EventArgs e)
        {
            try
            {
                var action = await DisplayAlert("", "Are you sure, you want to delete the existing profile?", "Yes", "No");
                if (action)
                {
                    App.SqlLiteCon().Execute("drop table Customer;");
                    await DisplayAlert("", "Profile successful removed.", "OK");
                    await Navigation.PushAsync(new MainPage());
                }
                else
                {
                    await DisplayAlert("", "No changes made to existing profile.", "OK");
                }
            }
            catch(Exception f)
            {
                await DisplayAlert("",f.Message, "OK");
            }
           
        }
    }
   
}
