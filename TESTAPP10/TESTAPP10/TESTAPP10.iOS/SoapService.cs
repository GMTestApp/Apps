﻿using System;
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
        public string NewRegistration(string uid, string pass, string invitecode,string BackgroundLocationUpdate)
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
        public string SendProgress(string RefNo, string HAWB, string Lat, string Long, string UserId, string COMPANYID, string InviteCode, string Status, string Url, string TrackDateTime)
        {
            WTDriverTrak obj = new WTDriverTrak();
            obj.Url = Url;
            var res = obj.SendProgress(RefNo, HAWB, Lat, Long, UserId, COMPANYID, InviteCode, Status, TrackDateTime);
            return res;
        }
        public string UpdateDCargo(string RefNo, string HAWB, string DamageCargo, string UserID, string CompanyID, string InviteCode, string Url)
        {
            WTDriverTrak obj = new WTDriverTrak();
            obj.Url = Url;
            var res = obj.UpdateDCargo(RefNo, HAWB, DamageCargo, UserID, CompanyID, InviteCode);
            return res;
        }
        public string Uploadimgs(string RefNo, string HAWB, string StringImages, string UserId, string COMPANYID, string InviteCode, string Url)
        {
            WTDriverTrak obj = new WTDriverTrak();
            obj.Url = Url;
            var res = obj.Uploadimgs(RefNo, HAWB, StringImages, UserId, COMPANYID, InviteCode);
            return res;
        }

        public string UpdateDCargoNotes(string RefNo, string HAWB, string Notes, string UserId, string COMPANYID, string InviteCode, string Url)
        {
            WTDriverTrak obj = new WTDriverTrak();
            obj.Url = Url;
            var res = obj.UpdateDCargoNotes(RefNo, HAWB, Notes, UserId, COMPANYID, InviteCode);
            return res;
        }

        public string ShipmentActionUpdate(string RefNo, string HAWB, string Status, string MType, string UserId, string COMPANYID, string InviteCode, string Url)
        {
            WTDriverTrak obj = new WTDriverTrak();
            obj.Url = Url;
            var res = obj.ShipmentActionUpdate(RefNo, HAWB, Status, MType, UserId, COMPANYID, InviteCode);
            return res;
        }

        public string PostSignature(string RefNo, string HAWB, string Signature, string Name, string EmailId,  string UserId, string COMPANYID, string InviteCode, string Url)
        {
            WTDriverTrak obj = new WTDriverTrak();
            obj.Url = Url;
            var res = obj.PostSignature(RefNo, HAWB, Signature, Name, EmailId, UserId, COMPANYID, InviteCode);
            return res;
        }
         public string ChangeShipment(string OldRefNo, string NewRefNo, string UserID, string CompanyID, string status, string OldRefnoStatus, string DDate, string invitecode, string Url)
        {
            WTDriverTrak obj = new WTDriverTrak();
            obj.Url = Url;
            var res = obj.ChangeShipment(OldRefNo, NewRefNo, UserID, CompanyID, status, OldRefnoStatus, DDate, invitecode);
            return res;
        }
        public string ReceiveLocationUpdate(string UserId, string CompanyId, string Lat, string Lon, string DDate, string XCode, string Url)
        {
            WTDriverTrak obj = new WTDriverTrak();
            obj.Url = Url;
            var res = obj.ReceiveLocationUpdate(UserId, CompanyId, Lat, Lon, DDate, XCode).ToString();
            return res;
        }
    }
}