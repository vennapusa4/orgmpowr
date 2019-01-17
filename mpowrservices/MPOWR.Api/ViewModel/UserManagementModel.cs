
using MPOWR.Dal.Models;
using MPOWR.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace MPOWR.Api.ViewModel
{

    public class UserModel
    {
        public UserManagementModel user { get; set; }
    }
    public class UserManagementModel
    {

        public string EmailID { get; set; }
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LoggedUser { get; set; }
        public bool IsAdmin { get; set; }
        public bool BUAccess { get; set; }
        public bool exists { get; set; }
        public UserRoleUserType UserRoleUserType { get; set; }
        public Role role { get; set; }
        public UserType userTypeDetails { get; set; }
        //Changed for Geo implementation - 9th Jan, 2018
        public List<UserGeo> GeoDetails { get; set; }
        public List<UserBusinessUnit> BusinessUnitDetails { get; set; }
        public string IsActive { get; set; }
        public List<UserDistrict> districts { get; set; }
        public List<UserCountry> countries { get; set; }
        public List<UserRTM> partnerType { get; set; }
        public bool GlossaryApprover { get; set; }
        public bool DataUpload { get; set; }

    }

    public class UserPartnerTypeViewModel
    {
        public int UserRTMID { get; set; }
        public Nullable<int> UserRoleUserTypeID { get; set; }
        public int PartnerTypeID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class UserBusinessUnitViewModel
    {
        public int UserBusinessUnitID { get; set; }
        public Nullable<int> UserRoleUserTypeID { get; set; }
        public short BusinessUnitID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    public class UserCountryViewModel
    {
        public int UserCountryID { get; set; }
        public Nullable<int> UserRoleUserTypeID { get; set; }
        public short CountryID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    public class UserDistrictViewModel
    {
        public int UserDistrictID { get; set; }
        public Nullable<int> UserRoleUserTypeID { get; set; }
        public short DistrictID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    //public class UserGeoViewModel
    //{
    //    public int UserGEOID { get; set; }
    //    public Nullable<int> UserRoleUserTypeID { get; set; }
    //    public short GEOID { get; set; }
    //    public string CreatedBy { get; set; }
    //    public DateTime CreatedDate { get; set; }
    //    public string ModifiedBy { get; set; }
    //    public DateTime ModifiedDate { get; set; }
    //}
    //public class UserSubRegionViewModel
    //{
    //    public int UserSubRegionID { get; set; }
    //    public Nullable<int> UserRoleUserTypeID { get; set; }
    //    public short SubregionID { get; set; }
    //    public DateTime CreatedDate { get; set; }
    //    public string CreatedBy { get; set; }
    //    public DateTime ModifiedDate { get; set; }
    //    public string ModifiedBy { get; set; }

    //}
    public class UserRoleUsertypeViewModel
    {
        public int UserRoleUserTypeID { get; set; }
        public string UserID { get; set; }
        public Nullable<int> RoleID { get; set; }
        public Nullable<short> UserTypeID { get; set; }
        public Nullable<short> RegionID { get; set; }
        public Nullable<short> SubRegionID { get; set; }
        public Nullable<bool> IsAdmin { get; set; }
        public Nullable<bool> GlossaryApprover { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    public class UsertypeViewModel
    {
        public short UserTypeID { get; set; }
        public string ShortName { get; set; }
        public string DisplayName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public enum UserConstants
    {
        [Description("User exists")]
        user_exists,
        [Description("New User Created")]
        new_user_created,
        [Description("usp_User_Mangement_View")]
        userview_storedProcedure,
        [Description("UserBusinessUnitID")]
        UserBusinessUnitID,
        [Description("BusinessUnitID")]
        BusinessUnitID,
        [Description("DisplayName")]
        DisplayName,

    }


}