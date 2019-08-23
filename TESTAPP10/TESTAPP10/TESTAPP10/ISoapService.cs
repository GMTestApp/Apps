using System;
using System.Collections.Generic;
using System.Text;

namespace TESTAPP10
{
    public interface ISoapService
    {
        //string validate(string uid, string pass, string invitecode);
        string NewRegistration(string uid, string pass, string invitecode);
        string ForgotPassword(string uid, string pass, string invitecode);
        string MBoardData(string uid, string invite, string Type, string CompanyId, string Url);
        string MBoardDataDetails(string uid, string invite, string ManifestNo, string CompanyId, string Url);
        string SBoardDataDetails(string uid, string invite, string CompanyId, string Url);
        string LoadDetails(string HAWB, string USERID, string MTYPE, string INVITECODE, string COMPANYID, string Url);

        string SendProgress(string RefNo, string HAWB, string Lat, string Long, string UserId, string COMPANYID, string InviteCode, string Status, string Url);
        string UpdateDCargo(string RefNo, string HAWB, string DamageCargo, string UserId, string COMPANYID, string InviteCode, string Url);
        string Uploadimgs(string RefNo, string HAWB, string StringImages, string UserId, string COMPANYID, string InviteCode, string Url);
        string UpdateDCargoNotes(string RefNo, string HAWB, string Notes, string UserId, string COMPANYID, string InviteCode, string Url);
        string ShipmentActionUpdate(string RefNo, string HAWB, string Status, string MType, string UserId, string COMPANYID, string InviteCode, string Url);
        string PostSignature(string RefNo, string HAWB, string Signature, string Name, string EmailId, string UserId, string COMPANYID, string InviteCode, string Url);

    }
}
