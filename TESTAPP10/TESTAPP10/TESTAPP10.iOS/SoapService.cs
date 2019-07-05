using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using TESTAPP10.iOS.DriverTrak_WebReference;



namespace TESTAPP10.iOS
{
    public class SoapService: ISoapService
    {
        public string validate(string uid, string pass, string invitecode)
        {
            ValidateLogin ValidateLogin = new ValidateLogin();
            var res = ValidateLogin.isLoginValid(uid, pass, invitecode);
            return res;
        }
        public string NewRegistration(string uid, string pass, string invitecode)
        {
            ValidateLogin ValidateLogin = new ValidateLogin();
            var res = ValidateLogin.NewRegistration(uid, pass, invitecode);

            return res;

        }
        public string ForgotPassword(string uid, string pass, string invitecode)
        {
            ValidateLogin ValidateLogin = new ValidateLogin();
            var res = ValidateLogin.ForgotPassword(uid, pass, invitecode);

            return res;

        }
    }
}