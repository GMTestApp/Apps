using System;
using System.Collections.Generic;
using System.Text;

namespace TESTAPP10
{
    public class SOAP_Request
    {

        ISoapService ISoapService;
        public SOAP_Request(ISoapService service)
        {
            ISoapService = service;
        }
        public string NewRegistration(string uid, string pass, string invitecode)
        {
            return ISoapService.NewRegistration(uid, pass, invitecode);
        }
        public string ForgotPassword(string uid, string pass, string invitecode)
        {
            return ISoapService.ForgotPassword(uid, pass, invitecode);
        }
        public string MBoardData(string uid, string invite, string Type, string CompanyId, string Url)
        {
            return ISoapService.MBoardData(uid, invite, Type, CompanyId, Url);
        }
        public string MBoardDataDetails(string uid, string invite, string ManifestNo, string CompanyId, string Url)
        {
            
            return ISoapService.MBoardDataDetails(uid, invite, ManifestNo, CompanyId, Url);
            
        }
        public string SBoardDataDetails(string uid, string invite, string CompanyId, string Url)
        {
            return ISoapService.SBoardDataDetails(uid, invite, CompanyId, Url);
        }
        public string LoadDetails(string HAWB, string USERID, string MTYPE, string INVITECODE, string COMPANYID, string Url)
        {
            return ISoapService.LoadDetails( HAWB,  USERID,  MTYPE,  INVITECODE,  COMPANYID,  Url);
        }
        public string SendProgress(string RefNo, string HAWB, string Lat, string Long, string UserId, string COMPANYID, string InviteCode, string Status, string Url)
        {
            return ISoapService.SendProgress(RefNo, HAWB, Lat, Long, UserId, COMPANYID, InviteCode, Status, Url);
        }
    }
}
