using System;
using System.Collections.Generic;
using System.Text;

namespace TESTAPP10
{
    public class SOAPResponse
    {
        public string Valid { get; set; }
        public Re_Type Type { get; set; }
    }

    public class Re_Type
    {
        public string Valid { get; set; }
        public string Type { get; set; }
        public string CompanyID { get; set; }
        public string URL { get; set; }
    }

}
