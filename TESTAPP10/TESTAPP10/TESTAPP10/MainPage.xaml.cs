using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
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
            //try
            //{
            //    App.SqlLiteCon().Execute("drop table Customer;");
            //}
            //catch
            //{ }
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
                //if (Convert.ToString(txtpass.Text).Length < 7)
                //{
                //    lblmsg2.Text = "Password did not pass our security criteria. Please try another password. Hint: Enter Minimum 6 Charactes.";
                //    lblmsg2.IsVisible = true;
                //    errorframe.IsVisible = true;
                //    return;
                //}

                if ((SecurityCheck.isTampered((Convert.ToString(txtpass.Text) == null ? "" : txtpass.Text))) || (Convert.ToString(txtpass.Text).Length < 7))
                {
                    lblmsg2.Text = "Password did not pass our security criteria. Hint: Needs to be minimum 6 chars long, No Spaces, certain special characters are not allowed.";
                    lblmsg2.IsVisible = true;
                    errorframe.IsVisible = true;
                   
                    return;
                }

                var rsds = App.SOAP_Request.NewRegistration(userid.Trim(), TripleDES.Encrypt(password.Trim()), InviteCode.Trim());

                rsds = !rsds.EndsWith("}}") ? rsds + "}" : rsds;

                if (!rsds.Contains("\"CompanyID\":"))
                {
                    lblmsg2.Text = rsds;
                    lblmsg2.IsVisible = true;
                    errorframe.IsVisible = true;
                    return;
                }

                SOAPResponse response = JsonConvert.DeserializeObject<SOAPResponse>(rsds);

                if (response.Valid.ToString().ToLower() == "y")
                {

                    Customer custom = new Customer();
                    custom.UserId = userid;
                    custom.Password = password;
                    custom.XCode = InviteCode;
                    custom.CompanyID = response.Type != null ? response.Type.CompanyID : "";
                    custom.TransactURL = response.Type != null ? response.Type.URL : "";

                    App.SqlLiteCon().CreateTable<Customer>();

                    App.SqlLiteCon().Insert(custom);

                    if (App.SqlLiteCon().Table<Customer>().Count() == 0)
                    {
                        lblmsg2.Text = "Invite Code Not Stored.";
                        lblmsg2.IsVisible = true;
                        errorframe.IsVisible = true;
                        return;
                    }

                    await Navigation.PushAsync(new SuccessPage((!string.IsNullOrEmpty(Convert.ToString(response.Type.Type)) ? response.Type.Type : "D")));
                }
                else
                {
                    lblmsg2.Text = "Registration Faild.";
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
                    lblmsg2.Text = "Username or Password provided is invalid. Please try again.";
                    errorframe.IsVisible = true;
                    lblmsg2.IsVisible = true;
                    return;
                }
                if ((SecurityCheck.isTampered((Convert.ToString(password) == null ? "" : password)))|| (Convert.ToString(password).Length < 7))
                {
                    lblmsg2.Text = "Password did not pass our security criteria. Please try another password. Hint: No space, Certain special characters are not allowed.";
                    errorframe.IsVisible = true;
                    lblmsg2.IsVisible = true;
                    return;
                }

                var InviteCode = App.SqlLiteCon().Query<Customer>("SELECT * FROM Customer WHERE UserId = ?", userid).Count > 0 ? App.SqlLiteCon().Query<Customer>("SELECT * FROM Customer WHERE UserId = ?", userid)[0].XCode : "";

                var res = App.SOAP_Request.ValidateLogins(userid.Trim(), TripleDES.Encrypt(password.Trim()), InviteCode.Trim());

                if (!res.Contains("\"CompanyID\":"))
                {
                    lblmsg2.Text = res;
                    errorframe.IsVisible = true;
                    lblmsg2.IsVisible = true;
                    return;
                }

                Re_Type response = JsonConvert.DeserializeObject<Re_Type>(res);

                if (response.Valid.ToLower() == "n")
                {
                    lblmsg2.Text = "Username or Password provided is invalid. Please try again.";
                    lblmsg2.IsVisible = true;
                    errorframe.IsVisible = true;
                    return;
                }

                await Navigation.PushAsync(new SuccessPage((!string.IsNullOrEmpty(Convert.ToString(response.Type)) ? response.Type : "D")));
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
            await Navigation.PushAsync(new Login());
        }

        private async void Forgotpwd_Tapped(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(UsernameEntrylogin.Text)))
                await Navigation.PushAsync(new Login(Convert.ToString(UsernameEntrylogin.Text)));
            else
                await Navigation.PushAsync(new Login());
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            txtpass.IsPassword = true;
        }
    }
}
