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
    }
}
