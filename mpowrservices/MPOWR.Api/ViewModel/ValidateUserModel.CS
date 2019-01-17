using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPOWR.Api.ViewModel
{
    public class ValidateUserModel
    {
        public string UserID { get; set; }
        public string TokenID { get; set; }
        public string Environment { get; set; }
        public string Data { get; set; }
        public int ApplicationID { get; set; }
        public List<string> UserList { get; set; }
    }
}