using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPOWR.Api.ViewModel
{
    public class GlossaryViewModel
    {
        public string pageName { get; set; }
        public List<ParameterDetails> ParameterDetails { get; set; }

    }
    public class ParameterDetails
    {
        public string ParameterName { get; set; }
        public string Description { get; set; }
    }

}