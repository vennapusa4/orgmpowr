using MPOWR.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPOWR.Dal
{
    public class SearchResult
    {
        public int FinancialYearID { get; set; }
        public string FinancialYear { get; set; }
        public List<PartnerType>PartnerType { get; set; }
        public string AllocationLevel { get; set; }
        public string CountryOrGeoOrDistrict { get; set; }
        public List<Geo> Geo { get; set; }
        public List<Country> Country { get; set; }
        public List<District> District { get; set; }
        public List<Membership> Membership { get; set; }
    }
    public class SearchCriteria
    {
        public string GeoID { get; set; }
        public string CountryID { get; set; }
        public string DistrictID { get; set; }
        public int PartnerTypeID { get; set; }
        public int FinancialYearID { get; set; }
        public string UserID { get; set; }
    }
    public class ConfigPopup
    {
        public bool IsPopup { get; set; }
        public bool IsActive { get; set; }
        public int Days { get; set; }

    }
    public class PartnerType
    {
        public int PartnerTypeID { get; set; }
        public string PartnerName { get; set; }
    }
    public class District
    {
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
    }
    public class Membership
    {
        public int MembershipGroupID { get; set; }
        public string MembershipName { get; set; }
    }
}
