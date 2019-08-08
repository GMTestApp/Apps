using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using TESTAPP10.iOS.DriverTrak_WebReference;
using TESTAPP10.iOS.WTDriverTrak_WebReference;


namespace TESTAPP10.iOS
{
    public class SoapService: ISoapService
    {
        //public string validate(string uid, string pass, string invitecode)
        //{
        //    ValidateLogin ValidateLogin = new ValidateLogin();
        //    var res = ValidateLogin.isLoginValid(uid, pass, invitecode);
        //    return res;
        //}
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
        public string MBoardData(string uid, string invite, string Type, string CompanyId,string Url)
        {
            //ValidateLogin ValidateLogin = new ValidateLogin();
            //var res = ValidateLogin.MBoardData(uid, invite, Type, CompanyId);

            WTDriverTrak obj = new WTDriverTrak();
            obj.Url = Url;
            var res = obj.MBoardData(uid, invite, Type, CompanyId);

            return res;
        }
        public string MBoardDataDetails(string uid, string invite, string ManifestNo, string CompanyId, string Url)
        {
            //ValidateLogin ValidateLogin = new ValidateLogin();
            //var res = ValidateLogin.MBoardDataDetails(uid, invite, ManifestNo, CompanyId);

           WTDriverTrak obj = new WTDriverTrak();
            obj.Url = Url;
            var res = obj.MBoardDataDetails(uid, invite, ManifestNo, CompanyId);
            return res;
        }
        public string SBoardDataDetails(string uid, string invite, string CompanyId,string Url)
        {
            WTDriverTrak obj = new WTDriverTrak();
            obj.Url = Url;
            var res = obj.SBoardDataDetails(uid, invite, CompanyId);
            return res;
        }
        public string LoadDetails(string HAWB, string USERID, string MTYPE, string INVITECODE, string COMPANYID, string Url)
        {
            WTDriverTrak obj = new WTDriverTrak();
            obj.Url = Url;
            var res = obj.LoadDetails(HAWB, USERID, MTYPE, INVITECODE, COMPANYID);
            return res;
        }
        public string SendProgress(string RefNo, string HAWB, string Lat, string Long, string UserId, string COMPANYID, string InviteCode, string Status, string Url)
        {
            WTDriverTrak obj = new WTDriverTrak();
            obj.Url = Url;
            var res = obj.SendProgress(RefNo, HAWB, Lat, Long, UserId, COMPANYID, InviteCode, Status);
            return res;
        }
    }
}