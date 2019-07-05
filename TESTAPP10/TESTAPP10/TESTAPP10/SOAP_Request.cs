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

        public string ValidateLogins(string uid, string pass, string invitecode)
        {
            return ISoapService.validate(uid, pass, invitecode);
        }
        public string NewRegistration(string uid, string pass, string invitecode)
        {
            return ISoapService.NewRegistration(uid, pass, invitecode);
        }
        public string ForgotPassword(string uid, string pass, string invitecode)
        {
            return ISoapService.ForgotPassword(uid, pass, invitecode);
        }
    }
}
