using MPOWR.Bal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPOWR.Core;
using MPOWR.Dal;
using MPOWR.Model;
using MPOWR.Dal.Models;

namespace MPOWR.Bal
{
    public class GlossaryBL
    {
        public List<GlossaryViewModel> GetGlossaryDetails()
        {

            try
            {
                using (GlossaryDal Dal = new GlossaryDal())
                {

                    return Dal.GetGlossaryDetails();
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

        public List<GlossaryEditModel> GetGlossaryEditDetails()
        {

            try
            {
                using (GlossaryDal Dal = new GlossaryDal())
                {

                    return Dal.GetGlossaryEditDetails();
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


        public GlossaryReturnModel SaveGlossaryParameter(ParameterEditModel data, string user)
        {
            GlossaryReturnModel retVal = new GlossaryReturnModel();
            try
            {
                using (GlossaryDal Dal = new GlossaryDal())
                {

                    retVal = Dal.SaveGlossaryParameter(data, user);
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

            return retVal;

        }

        public GlossaryReturnModel SaveGlossaryScreen(GlossaryEditModel data, string user)
        {
            GlossaryReturnModel retVal = new GlossaryReturnModel();
            try
            {
                using (GlossaryDal Dal = new GlossaryDal())
                {

                    retVal = Dal.SaveGlossaryScreen(data, user);
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

            return retVal;

        }


        public GlossaryReturnModel ApproveGlossary(List<GlossaryEditModel> data, string user)
        {
            GlossaryReturnModel retVal = new GlossaryReturnModel();
            try
            {
                using (GlossaryDal Dal = new GlossaryDal())
                {

                    retVal = Dal.ApproveGlossary(data, user);
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

            return retVal;

        }

        public List<AppConfig> GetGlossaryConfiguration()
        {
            List<AppConfig> retVal = new List<AppConfig>();
            try
            {
                using (GlossaryDal Dal = new GlossaryDal())
                {

                    retVal = Dal.GetGlossaryConfiguration();
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

            return retVal;

        }
        public void InsertGlossaryScreenParams(List<GlossaryScreenParameterDetail> data)
        {
            try
            {
                using (GlossaryDal Dal = new GlossaryDal())
                {

                     Dal.InsertGlossaryScreenParams(data);
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
