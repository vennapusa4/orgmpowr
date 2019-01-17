using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPOWR.Api.ViewModel
{
   
    public class RoleFeatureViewModelRequest
    {
        public string ShortName { get; set; } 
        public string RoleName { get; set; }
        public string UserID { get; set; }

    }

    public class RoleFeatureActivityRequest
    {
        public RoleUserData[] RoleUser { get; set; }

        public RolePartnerTypeData[] RolePartnerType { get; set; }

        public RoleFeatureActivityData[] RoleFeatureActivity { get; set; }
               
    }

    public class RoleUserData
    {
        public int RoleID { get; set; }
        public string UserID { get; set; }
    }

    public class RolePartnerTypeData
    {        
        public int PartnerTypeID { get; set; }       
        public bool PartnerTypeIsChecked { get; set; }
    }
    public class RoleFeatureActivityData
    {       
        public int FeatureID { get; set; }     
        public int FeatureActionID { get; set; }       
        public bool FeatureActionIsChecked { get; set; }
    }
}