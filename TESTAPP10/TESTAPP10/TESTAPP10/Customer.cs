using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TESTAPP10
{
    public class Customer
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public string XCode { get; set; }
        public string CompanyID { get; set; }
        public string TransactURL { get; set; }
    }
}
