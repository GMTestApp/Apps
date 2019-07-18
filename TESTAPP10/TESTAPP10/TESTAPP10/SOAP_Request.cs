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
        public string MBoardData(string uid, string invite, string Type, string CompanyId)
        {
            return ISoapService.MBoardData(uid, invite, Type, CompanyId);
        }
        public string MBoardDataDetails(string uid, string invite, string ManifestNo, string CompanyId)
        {
            
            return ISoapService.MBoardDataDetails(uid, invite, ManifestNo, CompanyId);
            
        }
    }
}
