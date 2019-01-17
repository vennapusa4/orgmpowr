using MPOWR.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPOWR.Model;
using MPOWR.Dal.Models;
using System.Text.RegularExpressions;
using System.Data.Common;
using System.Data;

namespace MPOWR.Dal
{
    public class GlossaryDal : ClsDispose
    {
        /// <summary>
        /// To Return GlossaryDetails
        /// </summary>
        /// <returns></returns>
        public List<GlossaryViewModel> GetGlossaryDetails()
        {
            try
            {
                MPOWREntities db = new MPOWREntities();
                List<GlossaryViewModel> data = new List<GlossaryViewModel>();

                data = (from g in db.GlossaryScreenDetails
                        where g.IsActive == true
                        select new GlossaryViewModel
                        {
                            pageName = g.ScreenName,
                            ID = g.ID,
                            Description = g.Description,
                            RefinedFormula = g.RefinedFormula,
                            IsChild = g.IsChild,
                            ParentScreenID = g.ParentScreenID,
                            ParameterDetails = (from gp in db.GlossaryScreenParameterDetails
                                                where g.ID == gp.ScreenID && gp.IsActive == true && gp.IsDeleted == false && gp.Description != null && gp.Description != string.Empty
                                                select new ParameterViewModel
                                                {
                                                    RefinedDescription = gp.RefinedDescription,
                                                    RefinedParameter = gp.RefinedParameter,
                                                    DisplayOrder = gp.DisplayOrder,
                                                    ID = gp.ID,
                                                    Icon = gp.Icon
                                                }).OrderBy(x => x.DisplayOrder).ToList()
                        }).ToList();

                return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public List<GlossaryEditModel> GetGlossaryEditDetails()
        {
            try
            {
                MPOWREntities db = new MPOWREntities();
                List<GlossaryEditModel> data = new List<GlossaryEditModel>();

                data = (from g in db.GlossaryScreenDetails
                        join geditScreen in db.GlossaryScreenEditDetails on
                                                g.ID equals geditScreen.ScreenID into screenEdits
                        from gEdit in screenEdits.Where(x => x.IsDeleted == false && x.IsActive == true).DefaultIfEmpty()
                        where g.IsActive == true
                        select new GlossaryEditModel
                        {
                            ID = g.ID,
                            pageName = g.ScreenName,
                            DisplayOrder = g.DisplayOrder,
                            IsChild = g.IsChild,
                            ParentScreenID = g.ParentScreenID,
                            Description = g.Description,
                            Formula = g.Formula,
                            EditScreenID = gEdit.ID,
                            EditedFormula = gEdit.Formula,
                            EditedDesc = gEdit.Description,
                            ModifiedDate = gEdit.ModifiedDate,
                            RefinedFormula = g.RefinedFormula,
                            IsAltered = (((gEdit.Description != null && gEdit.Description != string.Empty) || (gEdit.Formula != null && gEdit.Formula != string.Empty)) ?
                            (((g.Description != null && g.Description != string.Empty) || (g.Formula != null && g.Formula != string.Empty)) ? 2 : 1) : 0),
                            ParameterDetails = (from gp in db.GlossaryScreenParameterDetails
                                                join gpEdit in db.GlossaryScreenParameterEditDetails on
                                                gp.ID equals gpEdit.EditParameterID into paramEdits
                                                from gpEdit in paramEdits.Where(x => x.IsActive == true).DefaultIfEmpty()
                                                where g.ID == gp.ScreenID && gp.IsActive == true
                                                select new ParameterEditModel
                                                {
                                                    ID = gp.ID,
                                                    Description = gp.Description,
                                                    ParameterName = gp.ParameterName,
                                                    DisplayOrder = gp.DisplayOrder,
                                                    EditParamID = gpEdit.ID,
                                                    EditedDesc = gpEdit.Description,
                                                    ModifiedDate = gpEdit.ModifiedDate,
                                                    Icon = gp.Icon,
                                                    RefinedParameter = gp.RefinedParameter,
                                                    RefinedDescription = gp.RefinedDescription,
                                                    IsAltered = ((gpEdit.Description != null && gpEdit.Description != string.Empty) ? ((gp.Description != null && gp.Description != string.Empty) ? 2 : 1) : 0)
                                                }).OrderBy(x => x.DisplayOrder).ToList()
                        }).ToList();

                foreach (GlossaryEditModel glossary in data)
                {
                    if (glossary.IsChild)
                    {
                        data.Find(x => x.ID == glossary.ParentScreenID).IsParent = true;
                    }
                }

                return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public GlossaryReturnModel SaveGlossaryParameter(ParameterEditModel data, string user)
        {
            DateTime currentTime = CommonFunction.GetCurrentTime;
            GlossaryReturnModel retVal = new GlossaryReturnModel();
            using (MPOWREntities db = new MPOWREntities())
            {
                try
                {
                    GlossaryScreenParameterEditDetail parameterData = (from g in db.GlossaryScreenParameterEditDetails where g.EditParameterID == data.ID && g.IsActive == true select g).FirstOrDefault();
                    if (parameterData != null && parameterData.ModifiedDate.ToString("MM/dd/yyyy HH:mm:ss") != data.ModifiedDate.Value.ToString("MM/dd/yyyy HH:mm:ss"))
                    {
                        retVal.ErrorCode = 1001;
                        return retVal;
                    }
                    if (parameterData == null)
                    {
                        parameterData = new GlossaryScreenParameterEditDetail();
                        parameterData.EditParameterID = data.ID;
                        parameterData.CreatedBy = user;
                        parameterData.CreatedDate = currentTime;
                        parameterData.IsActive = true;

                    }
                    parameterData.Description = data.EditedDesc;
                    parameterData.ModifiedBy = user;
                    parameterData.ModifiedDate = currentTime;
                    parameterData.IsDeleted = data.IsDeleted;

                    if (parameterData.ID <= 0)
                        db.GlossaryScreenParameterEditDetails.Add(parameterData);
                    db.SaveChanges();
                    retVal.ID = parameterData.ID;
                    retVal.ModifiedDate = parameterData.ModifiedDate;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return retVal;
        }

        public GlossaryReturnModel SaveGlossaryScreen(GlossaryEditModel data, string user)
        {
            DateTime currentTime = CommonFunction.GetCurrentTime;
            GlossaryReturnModel retVal = new GlossaryReturnModel();
            using (MPOWREntities db = new MPOWREntities())
            {
                try
                {
                    GlossaryScreenEditDetail screenData = (from g in db.GlossaryScreenEditDetails where g.ScreenID == data.ID && g.IsActive == true select g).FirstOrDefault();
                    if (screenData != null && screenData.ModifiedDate.ToString("MM/dd/yyyy HH:mm:ss") != data.ModifiedDate.Value.ToString("MM/dd/yyyy HH:mm:ss"))
                    {
                        retVal.ErrorCode = 1001;
                        return retVal;
                    }
                    if (screenData == null)
                    {
                        screenData = new GlossaryScreenEditDetail();
                        screenData.ScreenID = data.ID;
                        screenData.CreatedBy = user;
                        screenData.CreatedDate = currentTime;

                    }
                    screenData.Description = data.EditedDesc;
                    screenData.Formula = data.EditedFormula;
                    screenData.ModifiedBy = user;
                    screenData.ModifiedDate = currentTime;
                    screenData.IsActive = true;
                    if (screenData.ID <= 0)
                        db.GlossaryScreenEditDetails.Add(screenData);
                    db.SaveChanges();
                    retVal.ID = screenData.ID;
                    retVal.ModifiedDate = screenData.ModifiedDate;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return retVal;
        }

        public GlossaryReturnModel ApproveGlossary(List<GlossaryEditModel> data, string user)
        {
            DateTime currentTime = CommonFunction.GetCurrentTime;
            GlossaryReturnModel retVal = new GlossaryReturnModel();
            MPOWREntities db = new MPOWREntities();
            using (var dbTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (GlossaryEditModel glossaryData in data)
                    {
                        if (glossaryData.IsApproved != null)
                        {
                            GlossaryScreenEditDetail screenData = (from g in db.GlossaryScreenEditDetails where g.ScreenID == glossaryData.ID && g.IsActive == true select g).FirstOrDefault();
                            if (screenData == null || (screenData != null && screenData.ModifiedDate.ToString("MM/dd/yyyy HH:mm:ss") != glossaryData.ModifiedDate.Value.ToString("MM/dd/yyyy HH:mm:ss")))
                            {
                                dbTransaction.Rollback();
                                retVal.ErrorCode = 1001;
                                return retVal;
                            }
                            else
                            {
                                screenData.IsApproved = glossaryData.IsApproved;
                                screenData.ApprovalDate = currentTime;
                                screenData.Approver = user;
                                screenData.IsActive = false;
                                if (glossaryData.IsApproved.Value)
                                {
                                    screenData.GlossaryScreenDetail.Description = screenData.Description;
                                    screenData.GlossaryScreenDetail.Formula = screenData.Formula;
                                    screenData.GlossaryScreenDetail.ModifiedBy = user;
                                    screenData.GlossaryScreenDetail.ModifiedDate = currentTime;
                                }
                                db.SaveChanges();
                            }
                        }
                        foreach (ParameterEditModel paramModel in glossaryData.ParameterDetails)
                        {
                            GlossaryScreenParameterDetail finalData = (from g in db.GlossaryScreenParameterDetails where g.ID == paramModel.ID && g.IsActive == true && g.IsDeleted == false select g).FirstOrDefault();
                            if (finalData.DisplayOrder != paramModel.DisplayOrder)
                            {
                                finalData.DisplayOrder = paramModel.DisplayOrder;
                                finalData.ModifiedBy = user;
                                finalData.ModifiedDate = currentTime;
                            }
                            GlossaryScreenParameterEditDetail parameterData = (from g in db.GlossaryScreenParameterEditDetails where g.EditParameterID == paramModel.ID && g.IsActive == true select g).FirstOrDefault();
                            if ((parameterData == null && paramModel.EditParamID > 0) || (parameterData != null && parameterData.ModifiedDate.ToString("MM/dd/yyyy HH:mm:ss") != paramModel.ModifiedDate.Value.ToString("MM/dd/yyyy HH:mm:ss")))
                            {
                                dbTransaction.Rollback();
                                retVal.ErrorCode = 1001;
                                return retVal;
                            }
                            else if (parameterData != null && paramModel.IsApproved != null)
                            {

                                parameterData.IsApproved = paramModel.IsApproved;
                                parameterData.ApprovalDate = currentTime;
                                parameterData.Approver = user;
                                parameterData.IsActive = false;
                                if (paramModel.IsApproved.Value)
                                {
                                    parameterData.GlossaryScreenParameterDetail.Description = parameterData.Description;
                                    parameterData.GlossaryScreenParameterDetail.ModifiedBy = user;
                                    parameterData.GlossaryScreenParameterDetail.ModifiedDate = currentTime;
                                }
                                db.SaveChanges();
                            }
                            else if (paramModel.IsDeleted)
                            {
                                if (parameterData != null)
                                {
                                    parameterData.IsActive = false;
                                    parameterData.ModifiedBy = user;
                                    parameterData.ModifiedDate = currentTime;
                                }

                                if (finalData != null)
                                {
                                    finalData.Description = null;
                                    finalData.ModifiedBy = user;
                                    finalData.ModifiedDate = currentTime;
                                }
                                db.SaveChanges();
                            }
                        }

                    }
                    db.SaveChanges();
                    dbTransaction.Commit();

                    DbCommand command = db.Database.Connection.CreateCommand();
                    command.CommandText = "usp_RefineGlossaryText";
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                    retVal.ErrorCode = 200;
                    return retVal;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

        }
        public void InsertGlossaryScreenParams(List<GlossaryScreenParameterDetail> List)
        {

            using (MPOWREntities db = new MPOWREntities())
            {
                try
                {
                    foreach (var item in List)
                    {
                        db.GlossaryScreenParameterDetails.Add(item);
                        db.SaveChanges();
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

        }



        public List<AppConfig> GetGlossaryConfiguration()
        {
            List<AppConfig> retVal = new List<AppConfig>();
            try
            {
                using (MPOWREntities db = new MPOWREntities())
                {
                    retVal = db.AppConfig.Where(x => x.ScreenName == "Glossary").OrderBy(x => x.DisplayOrder).ToList();
                }
                return retVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
