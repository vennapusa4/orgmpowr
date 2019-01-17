using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPOWR.ViewModel
{
    public class UsersViewModel
    {
        public string UserID { get; set; }
        //public string UserName { get; set; }
        public string EmailID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pasword { get; set; }
        public string MiddleName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public byte[] Password { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string TokenID { get; set; }
        public string FinancialYear { get; set; }
        public short? UserTypeID { get; set; }
        public int FinancialYearID { get; set; }

    }

    public class UsersVM
    {
        public string UserID { get; set; }
        public string TokenID { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? GlossaryApprover { get; set; }
        public string FirstName { get; set; }
        public string FinancialYear { get; set; }
        public short? UserTypeID { get; set; }
        public int FinancialYearID { get; set; }
        public int CurrentFinancialYearID { get; set; }
        public int? RoleID { get; set; }
        public List<short> Geos { get; set; }
        public List<short> Countries { get; set; }
        public List<int> PartnerTypes { get; set; }
        public IEnumerable<FeatureModel> Features { get; set; }
        public List<short> BUs { get; set; }
        public string AppName { get; set; }
        public string ApplicationID { get; set; }
        public bool DataUpload { get; set; }
        public bool DataUploadAccess { get; set; }
    }
    public class FeatureModel
    {
        public bool? IsFeatureActionChecked { get; set; }
        public int FeatureID { get; set; }
        public int FeatureActionID { get; set; }
        public int? FeatureActionTypeID { get; set; }
        public string FeatureActionType { get; set; }
        public IEnumerable<ActionModel> Actions { get; set; }
    }
    public class ActionModel
    {
        public string IsActive { get; set; }
        public int FeatureActionID { get; set; }
        public int? FeatureActionTypeID { get; set; }
        public bool? IsFeatureActionChecked { get; set; }
    }
}