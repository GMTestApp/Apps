﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TESTAPP10
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{

        public Login()
        {
            InitializeComponent();
        }
        public Login(string username)
        {
            InitializeComponent();
            txtuname.Text = username;
        }
        private void Resetpwd_Clicked(object sender, EventArgs e)
        {

            var userid = txtuname.Text;
            var password = txtpass.Text;
            frmfaild.IsVisible = false;
            frmsuccess.IsVisible = false;
            errorframe.IsVisible = false;
           
            if ((string.IsNullOrEmpty(userid)) || (string.IsNullOrEmpty(password)))
            {
                lblmsg.Text = "All fields are required for registration.";
                errorframe.IsVisible = true;
                lblmsg.IsVisible = true;
                return;
            }
            //if (Convert.ToString(password).Length < 7)
            //{
            //    lblmsg.Text = "Password did not pass our security criteria. Please try another password. Hint: No space, Certain special characters are not allowed.";
            //    return;
            //}

            if ((SecurityCheck.isTampered((Convert.ToString(password) == null ? "" : password)))|| (Convert.ToString(password).Length < 7))
            {
                lblmsg.Text = "Password did not pass our security criteria. Hint: Needs to be minimum 6 chars long, No Spaces, certain special characters are not allowed.";
                errorframe.IsVisible = true;
                lblmsg.IsVisible = true;
                return;
            }

            var InviteCode = App.SqlLiteCon().Query<Customer>("SELECT * FROM Customer WHERE UserId = ?", userid).Count > 0 ? App.SqlLiteCon().Query<Customer>("SELECT * FROM Customer WHERE UserId = ?", userid)[0].XCode : "";

           // var res = App.SOAP_Request.ValidateLogins(userid.Trim(), TripleDES.Encrypt(password.Trim()), InviteCode.Trim());


            var rsds = App.SOAP_Request.ForgotPassword(userid.Trim(), TripleDES.Encrypt(password.Trim()), InviteCode.Trim());

            if (rsds.ToLower().Contains("success"))
            {
                frmsuccess.IsVisible = true;
                login.IsVisible = false;
            }
            else
            {
                frmfaild.IsVisible = true;
                login.IsVisible = false;
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

    }
}