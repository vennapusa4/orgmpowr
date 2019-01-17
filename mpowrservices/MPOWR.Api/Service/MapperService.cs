using MPOWR.Dal.Models;
using MPOWR.Api.ViewModel;
using MPOWR.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPOWR.Api.Service
{
    public class MapperService
    {
        #region User table mapper methods
        /// <summary>
        /// Mapping method for UsersViewModel to User table.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        internal static UsersViewModel Map(User src)
        {
            var dest = new UsersViewModel();
            dest.UserID = src.UserID;
            dest.EmailID = src.EmailID;
            dest.FirstName = src.FirstName;
            dest.LastName = src.LastName;
            dest.Password = src.Password;
            dest.IsActive = src.IsActive;
            dest.CreatedBy = src.CreatedBy;
            dest.CreatedDate = src.CreatedDate;
            dest.ModifiedBy = src.ModifiedBy;
            dest.ModifiedDate = src.ModifiedDate;
            return dest;
        }

        /// <summary>
        ///  Mapping method for User table to UsersViewModel
        /// </summary>
        /// <param name="src"></param>
        /// <returns>User</returns>
        internal static User Map(UsersViewModel src)
        {
            var dest = new User();
            dest.UserID = src.UserID;
            dest.EmailID = src.EmailID;
            dest.FirstName = src.FirstName;
            dest.LastName = src.LastName;
            dest.Password = src.Password;
            dest.IsActive = src.IsActive;
            dest.CreatedBy = src.CreatedBy;
            dest.CreatedDate = src.CreatedDate;
            dest.ModifiedBy = src.ModifiedBy;
            dest.ModifiedDate = src.ModifiedDate;
            return dest;
        }

        #endregion


        #region   UserRoleUserType table mapper methods
        /// <summary>
        /// Mapping method for UserRoleUsertypeViewModel to UserRoleUserType table.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        internal static UserRoleUsertypeViewModel Map(UserRoleUserType src)
        {
            var dest = new UserRoleUsertypeViewModel();
            dest.UserRoleUserTypeID = src.UserRoleUserTypeID;
            dest.UserTypeID = src.UserTypeID;
            dest.UserID = src.UserID;
            dest.RoleID = src.RoleID;
            dest.IsAdmin = src.IsAdmin;
            dest.CreatedBy = src.CreatedBy;
            dest.CreatedDate = src.CreatedDate;
            dest.ModifiedBy = src.ModifiedBy;
            dest.ModifiedDate = src.ModifiedDate;
            return dest;
        }

        /// <summary>
        /// Mapping method for UserRoleUserType table to UserRoleUsertypeViewModel
        /// </summary>
        /// <param name="src"></param>
        /// <returns>UserRoleUserType</returns>
        internal static UserRoleUserType Map(UserRoleUsertypeViewModel src)
        {
            var dest = new UserRoleUserType();
            dest.UserRoleUserTypeID = src.UserRoleUserTypeID;
            dest.UserTypeID = src.UserTypeID;
            dest.UserID = src.UserID;
            dest.RoleID = src.RoleID;
            dest.IsAdmin = src.IsAdmin;
            dest.GlossaryApprover = src.GlossaryApprover;
            dest.CreatedBy = src.CreatedBy;
            dest.CreatedDate = src.CreatedDate;
            dest.ModifiedBy = src.ModifiedBy;
            dest.ModifiedDate = src.ModifiedDate;
            return dest;
        }

        #endregion


        #region UserType table mapper methods
        /// <summary>
        /// Mapping method for UsertypeViewModel to UserType table.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        internal static UsertypeViewModel Map(UserType src)
        {
            var dest = new UsertypeViewModel();
            dest.UserTypeID = src.UserTypeID;
            dest.DisplayName = src.DisplayName;
            dest.ShortName = src.ShortName;            
            dest.CreatedBy = src.CreatedBy;
            dest.CreatedDate = src.CreatedDate;
            dest.ModifiedBy = src.ModifiedBy;
            dest.ModifiedDate = src.ModifiedDate;
            return dest;
        }
        #endregion


        #region   UserBusinessUnit table mapper methods
        /// <summary>
        /// Mapping method for UserBusinessUnitViewModel to UserBusinessUnit table.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        internal static UserBusinessUnitViewModel Map(UserBusinessUnit src)
        {
            var dest = new UserBusinessUnitViewModel();
            dest.UserBusinessUnitID = src.UserBusinessUnitID;
            dest.UserRoleUserTypeID = src.UserRoleUserTypeID;
            dest.BusinessUnitID = src.BusinessUnitID;          
            dest.CreatedBy = src.CreatedBy;
            dest.CreatedDate = src.CreatedDate;
            dest.ModifiedBy = src.ModifiedBy;
            dest.ModifiedDate = src.ModifiedDate;
            return dest;
        }

        /// <summary>
        /// Mapping method for UserBusinessUnit table  to UserBusinessUnitViewModel .
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        internal static UserBusinessUnit Map(UserBusinessUnitViewModel src)
        {
            var dest = new UserBusinessUnit();
            dest.UserBusinessUnitID = src.UserBusinessUnitID;
            dest.UserRoleUserTypeID = src.UserRoleUserTypeID;
            dest.BusinessUnitID = src.BusinessUnitID;
            dest.CreatedBy = src.CreatedBy;
            dest.CreatedDate = src.CreatedDate;
            dest.ModifiedBy = src.ModifiedBy;
            dest.ModifiedDate = src.ModifiedDate;
            return dest;
        }

        /// <summary>
        /// Mapping method for UserBusinessUnit table  to UserBusinessUnitViewModel .
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        internal static UserRTM Map(UserPartnerTypeViewModel src)
        {
            var dest = new UserRTM();
            dest.UserRTMID = src.UserRTMID;
            dest.UserRoleUserTypeID = src.UserRoleUserTypeID;
            dest.PartnerTypeID = src.PartnerTypeID;
            dest.CreatedBy = src.CreatedBy;
            dest.CreatedDate = src.CreatedDate;
            dest.ModifiedBy = src.ModifiedBy;
            dest.ModifiedDate = src.ModifiedDate;
            return dest;
        }

        /// <summary>
        /// Mapping method for UserBusinessUnitViewModel to UserBusinessUnit table.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        internal static UserPartnerTypeViewModel Map(UserRTM src)
        {
            var dest = new UserPartnerTypeViewModel();
            dest.UserRTMID = src.UserRTMID;
            dest.UserRoleUserTypeID = src.UserRoleUserTypeID;
            dest.PartnerTypeID = src.PartnerTypeID;
            dest.CreatedBy = src.CreatedBy;
            dest.CreatedDate = src.CreatedDate;
            dest.ModifiedBy = src.ModifiedBy;
            dest.ModifiedDate = src.ModifiedDate;
            return dest;
        }

        /// <summary>
        /// Mapper method to map IList<UserBusinessUnit> to  IList<UserBusinessUnitViewModel>
        /// </summary>
        /// <param name="input"></param>
        /// <returns>IList<InstitutionsModel> </returns>
        internal static IList<UserBusinessUnitViewModel> Map(IList<UserBusinessUnit> input)
        {
            return input.Select(x => Map(x)).ToList();
        }


        #endregion


        #region   UserDistrict table mapper methods
        /// <summary>
        /// Mapping method for UserDistrictViewModel to UserDistrict table.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        internal static UserDistrictViewModel Map(UserDistrict src)
        {
            var dest = new UserDistrictViewModel();
            dest.UserDistrictID = src.UserDistrictID;
            dest.UserRoleUserTypeID = src.UserRoleUserTypeID;
            dest.DistrictID = src.DistrictID;
            dest.CreatedBy = src.CreatedBy;
            dest.CreatedDate = src.CreatedDate;
            dest.ModifiedBy = src.ModifiedBy;
            dest.ModifiedDate = src.ModifiedDate;
            return dest;
        }

        /// <summary>
        /// Mapping method for UserDistrict table  to UserDistrictViewModel .
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        internal static UserDistrict  Map(UserDistrictViewModel src)
        {
            var dest = new UserDistrict();
            dest.UserDistrictID = src.UserDistrictID;
            dest.UserRoleUserTypeID = src.UserRoleUserTypeID;
            dest.DistrictID = src.DistrictID;
            dest.CreatedBy = src.CreatedBy;
            dest.CreatedDate = src.CreatedDate;
            dest.ModifiedBy = src.ModifiedBy;
            dest.ModifiedDate = src.ModifiedDate;
            return dest;
        }
        //internal static UserGeo Map(UserGeoViewModel src)
        //{
        //    var dest = new UserGeo();
        //    dest.UserGeoID = src.UserGEOID;
        //    dest.UserRoleUserTypeID = src.UserRoleUserTypeID;
        //    dest.GeoID = src.GEOID;
        //    dest.CreatedBy = src.CreatedBy;
        //    dest.CreatedDate = src.CreatedDate;
        //    dest.ModifiedBy = src.ModifiedBy;
        //    dest.ModifiedDate = src.ModifiedDate;
        //    return dest;
        //}
        //internal static UserSubRegion Map(UserSubRegionViewModel src)
        //{
        //    var dest = new UserSubRegion();
        //    dest.UserSubRegionID = src.UserSubRegionID;
        //    dest.UserRoleUserTypeID = src.UserRoleUserTypeID;
        //    dest.SubRegionID = src.SubregionID;
        //    dest.CreatedBy = src.CreatedBy;
        //    dest.CreatedDate = src.CreatedDate;
        //    dest.ModifiedBy = src.ModifiedBy;
        //    dest.ModifiedDate = src.ModifiedDate;
        //    return dest;
        //}
        /// <summary>
        /// Mapper method to map IList<UserBusinessUnit> to  IList<UserBusinessUnitViewModel>
        /// </summary>
        /// <param name="input"></param>
        /// <returns>IList<InstitutionsModel> </returns>
        internal static IList<UserDistrictViewModel> Map(IList<UserDistrict> input)
        {
            return input.Select(x => Map(x)).ToList();
        }


        #endregion


        #region   UserCountry table mapper methods
        /// <summary>
        /// Mapping method for UserCountryViewModel to UserCountry table.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        internal static UserCountryViewModel Map(UserCountry src)
        {
            var dest = new UserCountryViewModel();
            dest.UserCountryID = src.UserCountryID;
            dest.UserRoleUserTypeID = src.UserRoleUserTypeID;
            dest.CountryID = src.CountryID;
            dest.CreatedBy = src.CreatedBy;
            dest.CreatedDate = src.CreatedDate;
            dest.ModifiedBy = src.ModifiedBy;
            dest.ModifiedDate = src.ModifiedDate;
            return dest;
        }

        /// <summary>
        /// Mapping method for UserCountry table  to UserCountryViewModel.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        internal static UserCountry  Map(UserCountryViewModel src)
        {
            var dest = new UserCountry();
            dest.UserCountryID = src.UserCountryID;
            dest.UserRoleUserTypeID = src.UserRoleUserTypeID;
            dest.CountryID = src.CountryID;
            dest.CreatedBy = src.CreatedBy;
            dest.CreatedDate = src.CreatedDate;
            dest.ModifiedBy = src.ModifiedBy;
            dest.ModifiedDate = src.ModifiedDate;
            return dest;
        }

        /// <summary>
        /// Mapper method to map IList<UserBusinessUnit> to  IList<UserBusinessUnitViewModel>
        /// </summary>
        /// <param name="input"></param>
        /// <returns>IList<InstitutionsModel> </returns>
        internal static IList<UserCountryViewModel> Map(IList<UserCountry> input)
        {
            return input.Select(x => Map(x)).ToList();
        }


        #endregion


    }
}