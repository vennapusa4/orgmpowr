using MPOWR.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPOWR.Model;
using MPOWR.Dal;
using MPOWR.Dal.Models;

namespace MPOWR.Bal
{
    public class versionBL : ClsDispose
    {
        public List<NewVersionAdd> Getversions(VersionMDF version)
        {
            List<NewVersionAdd> model = null;

            try
            {

                using (VersionDal dal = new VersionDal())
                {
                    model = dal.GetVersions(version);
                }
            }

            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }

            return model;
        }
        public List<GetFinancial> GetFinancialYears(GetFinancial Financial, bool isSearch = false)
        {
            List<GetFinancial> model = null;

            try
            {

                using (VersionDal dal = new VersionDal())
                {
                    model = dal.GetFinancialyear(Financial, isSearch);
                }
            }

            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }

            return model;
        }
        public NewVersionAdd CreateVersion(VersionMDF data)
        {
            NewVersionAdd model = new NewVersionAdd();
            try
            {
                using (VersionDal dal = new VersionDal())
                {
                    model= dal.CreateVersion(data);
                }
               
            }

            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }

           return model;
        }

        public NewVersionAdd CopyVersion(CopyVersion data)
        {
            try
            {
                NewVersionAdd model = new NewVersionAdd();
                using (VersionDal dal = new VersionDal())
                {
                    model = dal.CopyVersion(data);
                }
                return model;
            }

            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }

            // return model;
        }
        public void DelVersion(VersionMDF data)
        {
            try
            {

                using (VersionDal dal = new VersionDal())
                {
                    dal.DelFinancialVersion(data);

                }
            }

            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }

            // return model;
        }

        public void UpdateIsFinal(VersionMDF version)
        {
            try
            {
                using (VersionDal dal = new VersionDal())
                {
                    dal.UpdateIsFinal(version);
                }

            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
            }
        }
        public List<SearchResult> GetSearchResult(SearchCriteria version)
        {
            try
            {
                using (VersionDal dal = new VersionDal())
                {
                    var searchresultList = dal.GetSearchResult(version);
                    return searchresultList;
                }
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }
        public void UpdateVersionName(VersionMDF version)
        {
            try
            {
                using (VersionDal dal = new VersionDal())
                {
                    dal.UpdateVersionName(version);
                }
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
            }
        }
        public int CheckIsActiveFlag(int VersionID)
        {
            try
            {
                using (VersionDal dal = new VersionDal())
                {
                    return dal.CheckIsActiveFlag(VersionID);
                }
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return 0;
            }
        }

        public bool CheckIsActiveFlagByVersion(VersionMDF version)
        {
            try
            {
                using (VersionDal dal = new VersionDal())
                {
                    return dal.CheckIsActiveFlagByVersion(version);
                }
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return false;
            }
        }

        public List<DataLoadRefresh> GetPopup()
        {
            try
            {
                using (VersionDal dal = new VersionDal())
                {
                    var searchresultList = dal.GetPopup();
                    return searchresultList;
                }
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }
        public List<DataLoadRefresh> SetPopup(ConfigPopup config)
        {
            try
            {

                using (VersionDal dal = new VersionDal())
                {
                    var searchresultList = dal.ConfigPopup(config);
                    return searchresultList;
                }
            }

            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                throw;
            }


        }
    }
}
