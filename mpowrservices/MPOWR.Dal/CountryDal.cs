using MPOWR.Core;
using MPOWR.Dal.Models;
using MPOWR.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPOWR.Dal
{
    public class CountryDAL : ClsDispose
    {
        /// <summary>
        /// Gets the list of countries and partner type details
        /// </summary>
        /// <param name="userid"></param>
        /// <returns>CountryViewModel</returns>
        public CountryViewModel GetCountries(string userid)
        {
            try
            {
                MPOWREntities db = new MPOWREntities();
                CountryViewModel countryInfo = new CountryViewModel();
                var data = (from dat in db.UserRoleUserTypes.AsNoTracking()
                            join rtm in db.UserRTMs.AsNoTracking() on dat.UserRoleUserTypeID equals rtm.UserRoleUserTypeID
                            where dat.UserID == userid
                            select new
                            {
                                partnerId = rtm.PartnerTypeID,
                                UserTypeID = dat.UserTypeID
                            }).ToList();
                if (!data.Any())
                    return countryInfo;

                if (data[0].UserTypeID == (short?)EnumUserType.District)
                {
                    countryInfo.Countries = (from dat in db.UserRoleUserTypes
                                            join usercontry in db.UserCountries.AsNoTracking() on dat.UserRoleUserTypeID equals usercontry.UserRoleUserTypeID
                                            join userdist in db.UserDistricts.AsNoTracking() on dat.UserRoleUserTypeID equals userdist.UserRoleUserTypeID
                                            join contry in db.Countries.AsNoTracking() on usercontry.CountryID equals contry.CountryID
                                            where dat.UserID == userid && contry.IsActive == true
                                            select new
                                            {
                                                CountryID = contry.CountryID,
                                                CountryName = contry.DisplayName,
                                                IsActive = contry.IsActive
                                            }).Distinct().OrderBy(x => x.CountryName).AsQueryable();
                }
                if (data[0].UserTypeID == (short?)EnumUserType.Country)
                {
                    countryInfo.Countries = (from dat in db.UserRoleUserTypes
                                            join usercontry in db.UserCountries.AsNoTracking() on dat.UserRoleUserTypeID equals usercontry.UserRoleUserTypeID
                                            join contry in db.Countries.AsNoTracking() on usercontry.CountryID equals contry.CountryID
                                            where dat.UserID == userid && contry.IsActive == true

                                            select new

                                            {
                                                CountryID = contry.CountryID,
                                                CountryName = contry.DisplayName.Trim(),
                                                IsActive = contry.IsActive
                                            }).Distinct().OrderBy(x => x.CountryName).AsQueryable();
                }
                else if (data[0].UserTypeID == (short?)EnumUserType.Geo)
                {
                    countryInfo.Countries = (from dat in db.UserRoleUserTypes.AsNoTracking()
                                            join ugeo in db.UserGeos.AsNoTracking() on dat.UserRoleUserTypeID equals ugeo.UserRoleUserTypeID
                                            join contry in db.Countries.AsNoTracking() on ugeo.GeoID equals contry.GeoID
                                            where dat.UserID == userid && contry.IsActive == true

                                            select new
                                            {
                                                CountryID = contry.CountryID,
                                                CountryName = contry.DisplayName.Trim(),
                                                IsActive = contry.IsActive
                                            }).Distinct().OrderBy(x => x.CountryName).AsQueryable();
                }
                else if (data[0].UserTypeID == (short?)EnumUserType.WorldWide)
                {
                    countryInfo.Countries = (from dat in db.Countries.AsNoTracking()
                                            where dat.IsActive == true
                                            select new 
                                            {
                                                CountryID = dat.CountryID,
                                                CountryName = dat.DisplayName.Trim(),
                                                IsActive = dat.IsActive
                                            }).Distinct().OrderBy(x => x.CountryName).AsQueryable();
                }
                var dist_data = data.Where(x => x.partnerId == 2).ToList();
                if (dist_data.Any())
                {
                    if (dist_data[0].UserTypeID == (short?)EnumUserType.WorldWide || dist_data[0].UserTypeID == (short?)EnumUserType.Geo || dist_data[0].UserTypeID == (short?)EnumUserType.Country)
                    {
                        countryInfo.Districts = (from dist in db.Districts.AsNoTracking()
                                                where dist.CountryID == (short?)EnumUserType.US && dist.ShortName != countryInfo.NoMap //&& dist.IsActive == true
                                                orderby dist.DisplayName
                                                select new
                                                {
                                                    DistrictID = dist.DistrictID,
                                                    DistrictName = dist.DisplayName.Trim(),
                                                    IsActive = dist.IsActive
                                                }).AsQueryable();
                    }
                    else
                    {
                        countryInfo.Districts = (from dat in db.UserRoleUserTypes.AsNoTracking()
                                                join userdist in db.UserDistricts.AsNoTracking() on dat.UserRoleUserTypeID equals userdist.UserRoleUserTypeID
                                                join dist in db.Districts.AsNoTracking() on userdist.DistrictID equals dist.DistrictID
                                                where dist.CountryID == (short?)EnumUserType.US && dat.UserID == userid && dist.ShortName != countryInfo.NoMap //&& dist.IsActive == true
                                                orderby dist.DisplayName
                                                select new
                                                {
                                                    DistrictID = dist.DistrictID,
                                                    DistrictName = dist.DisplayName.Trim(),
                                                    IsActive = dist.IsActive
                                                }).AsQueryable();
                    }
                }


                countryInfo.Partners = (from dat in db.UserRoleUserTypes.AsNoTracking()
                                       join rolepartner in db.UserRTMs.AsNoTracking() on dat.UserRoleUserTypeID equals rolepartner.UserRoleUserTypeID
                                       join partner in db.PartnerTypes.AsNoTracking() on rolepartner.PartnerTypeID equals partner.PartnerTypeID
                                       where dat.UserID == userid //&& partner.IsActive == true
                                       orderby partner.DisplayName
                                       select new
                                       {
                                           PartnerTypeID = partner.PartnerTypeID,
                                           PartnerName = partner.DisplayName.Trim(),
                                           IsActive = partner.IsActive
                                       }).AsQueryable().OrderBy(y => y.PartnerName);

                countryInfo.Memberships = (from member in db.MembershipGroups.AsNoTracking()
                                           select new
                                           {
                                               MembershipGroupID = member.MembershipGroupID,
                                               MembershipName = member.DisplayName,
                                               IsActive = member.IsActive
                                           }).AsQueryable();

                return countryInfo;

            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw ex;
            }


        }
        /// <summary>
        /// Returns the list of business units for the country list
        /// </summary>
        /// <param name="ComputeCountryList"></param>
        /// <param name="CountriesList"></param>
        /// <returns>list of businessunits</returns>
        public List<buunit> GetBUFromCountries(List<dynamic> ComputeCountryList, List<dynamic> CountriesList)
        {
            MPOWREntities db = new MPOWREntities();
            try
            {
                var BUList = db.BusinessUnits.Where(i=>i.IsActive == true).Select(i => new buunit { BusinessUnitID = i.BusinessUnitID, DisplayName = i.DisplayName }).AsNoTracking().AsQueryable();
                if (!BUList.Any())
                    return null;
                if (!(ComputeCountryList.Intersect(CountriesList).Any()))
                    BUList = BUList.Where(x => x.DisplayName != MPOWRConstants.Compute);
                else if (ComputeCountryList.Intersect(CountriesList).Count() == CountriesList.Count)
                    BUList = BUList.Where(x => x.DisplayName != MPOWRConstants.ComputeVolume && x.DisplayName != MPOWRConstants.ComputeValue);
                return BUList.ToList();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                throw new MPOWRException(message, ex);
            }
        }
    }
}
