using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using static TESTAPP10.ShipmentDetails;

namespace TESTAPP10
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            //UsernameEntrylogin.Text = "David";
            //PasswordEntrylogin.Text = "1234567";
            //txtuname.Text = "David";
            //txtpass.Text = "1234567";
            //txtinvitecode.Text = "H9i86f";

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
            catch (Exception ex)
            {

            }
        }
        private async void CreateAccount_Clicked(object sender, EventArgs e)
        {
            try
            {
                var userid = txtuname.Text.Trim();
                var password = txtpass.Text.Trim();
                var InviteCode = txtinvitecode.Text.Trim();
                var BackgroundLocationUpdate = "";

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



                var rsds = App.SOAP_Request.NewRegistration(userid.Trim(), TripleDES.Encrypt(password.Trim()), InviteCode.Trim(), BackgroundLocationUpdate);





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
                    BackgroundLocationUpdate = response.BackgroundLocationUpdate;
                }
                catch (Exception f)
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
                    custom.ActiveShipmentNo = "";
                    custom.ActiveShipmentStatus = "";
                    custom.LocationUpdate = BackgroundLocationUpdate;
                    custom.IsBackgroundLocationUpdate="";
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
                    lblmsg2.Text = "User's registration is not accepted, please contact admin.";
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

                if ((string.IsNullOrEmpty(userid)) || (string.IsNullOrEmpty(password)))
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

                var type = "";
                string LocationUpdate = "0";
                var UserId = "";
                var CompanyId = "";
                var XCode = "";
                var lattitude = "0";
                var longtitude = "0";

                try
                {

                    try
                    {
                        var requestsss = new GeolocationRequest(GeolocationAccuracy.Medium);
                        var locationsss = await Geolocation.GetLocationAsync(requestsss);


                        if (locationsss != null)
                        {
                            lattitude = locationsss.Latitude.ToString();
                            longtitude = locationsss.Longitude.ToString();
                        }
                        else
                        {
                            await DisplayAlert("", "Cannot access Location ? Please enable the location.", "OK");
                            return;
                        }

                    }
                    catch
                    {
                        var locationd = await Geolocation.GetLastKnownLocationAsync();

                        if (locationd != null)
                        {
                            lattitude = locationd.Latitude.ToString();
                            longtitude = locationd.Longitude.ToString();
                        }
                        else
                        {
                            await DisplayAlert("", "Cannot access Location ? Please enable the location.", "OK");
                            return;
                        }
                    }

                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    await DisplayAlert("", fnsEx.Message, "OK");
                }
                catch (FeatureNotEnabledException fneEx)
                {
                    await DisplayAlert("", fneEx.Message, "OK");
                }
                catch (PermissionException pEx)
                {
                    await DisplayAlert("", pEx.Message, "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("", ex.Message, "OK");
                }
                foreach (var v in customer)
                {
                    type = v.Type;
                    UserId = v.UserId;
                    CompanyId = v.CompanyID;
                    XCode = v.XCode;
                    LocationUpdate = v.LocationUpdate;

                    // v.IsBackgroundLocationUpdate = "true";
                    //App.SqlLiteCon().Update(v);
                }
                App.SqlLiteCon().Execute("update Customer set IsBackgroundLocationUpdate='true'");

                if ((!string.IsNullOrEmpty(LocationUpdate)) && (Convert.ToInt32(LocationUpdate) >= 20000))
                {



                    Thread t = new Thread(() =>
                    {
                        Console.WriteLine("executing ThreadProc");
                        try
                        {
                            var DDate = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt").Replace("-", "/");
                            ReceiveLocationUpdate(UserId, CompanyId, lattitude, longtitude, DDate, XCode);
                        }
                        finally
                        {
                            // Console.WriteLine("finished executing ThreadProc");
                        }
                    });
                    // t.IsBackground = true;
                    t.Start();
                }

                if (type.ToLower() == "m")
                    await Navigation.PushAsync(new MBoard(userid, password));
                else
                    await Navigation.PushAsync(new SBoardDataDetails());
            }
            catch (Exception ex)
            {

                lblmsg2.Text = ex.Message.ToString();
                lblmsg2.IsVisible = true;
                errorframe.IsVisible = true;
            }
        }

        private async void ReceiveLocationUpdate(string UserId, string CompanyId, string lattitude, string longtitude, string DDate, string XCode)
        {
            try
            {
            start:


                string shipmentid = "";
                var customer = from s in App.SqlLiteCon().Table<Customer>().Where(s => s.UserId == UserId) select s;
                var BackgroundLocationUpdate = "";
                var url = "";
                bool IsContinue = true;
                if ((lattitude != "0") && (longtitude != "0"))
                {

                    foreach (var c in customer)
                    {
                        BackgroundLocationUpdate = c.LocationUpdate;
                        url = c.TransactURL;
                        IsContinue = !string.IsNullOrEmpty(c.IsBackgroundLocationUpdate) ? Convert.ToBoolean(c.IsBackgroundLocationUpdate) : true;
                    }

                    if (!IsContinue)
                        return;
                    DDate = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt").Replace("-","/");
                    var ChangeShipment = App.SOAP_Request.ReceiveLocationUpdate(UserId, CompanyId, lattitude, longtitude, DDate, XCode, url);
                    Btnship_RootObject ChngShipLogin = JsonConvert.DeserializeObject<Btnship_RootObject>(ChangeShipment);
                    foreach (var re in ChngShipLogin.Details)
                    {
                        if (re.Message.ToLower() != "ok")
                        {
                            return;
                        }
                        //await Task.Delay(60000);
                        await Task.Delay(Convert.ToInt32(!String.IsNullOrEmpty(BackgroundLocationUpdate) ? BackgroundLocationUpdate : ""));
                        goto start;
                    }

                }
                else
                    return;
            }
            catch(Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");
                return;
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
