using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPOWR.Api.ViewModel
{
    public class UserManualViewModel
    {
        public int id { get; set; }
        public string type { get; set; }
        public string shortname { get; set; }
        public string displayname { get; set; }
        public string url { get; set; }

    }
}