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

    }
}
