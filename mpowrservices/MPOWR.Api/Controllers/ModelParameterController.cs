using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using MPOWR.Dal.Models;
using MPOWR.Api.ViewModel;
using System.Data.Entity.Infrastructure;
using System;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Data.Common;
using Newtonsoft.Json.Linq;
using MPOWR.Core;
using MPOWR.Api.App_Start;
using MPOWR.Model;

namespace MPOWR.Api.Controllers
{
    [AuthorizeUser]
    public class ModelParameterController : ApiController
    {
        // private MPOWREntities db = new MPOWREntities();
        ModelParameterInputmodel MPVM = new ModelParameterInputmodel();


        /// <summary>
        /// To set the default values on the SetModelParameter Page.
        /// </summary>
        /// <returns></returns>
        [Route("api/ModelParameter/GetModelParameterDefault")]
        [HttpGet]
        public dynamic GetModelParameterDefault(int? VersionID, string UserID, bool frmStep1 = false)
        {
            try
            {
                using (MPOWREntities dbContext = new MPOWREntities())
                {
                    ModelParameterTable MP_Count = null;
                   
                    MP_Count = (from MP in dbContext.ModelParameterTables.AsNoTracking() where (MP.VersionID == VersionID) select MP).FirstOrDefault();
                   
                    IEnumerable<dynamic> modelResult = null;
                    IEnumerable<dynamic> modelBuresult = null;

                    HistoricPerformanceController History = new HistoricPerformanceController();
                    List<buunit> lstBU = new List<buunit>();
                    lstBU = History.GetBusinessUnitByVersion(VersionID??0);
                    bool isComputeBUonly = false;
                    bool isAllBU = false;
                    bool isOtherBU = false;
                    foreach (var item in lstBU)
                    {
                        //BusinessUnit bu = new BusinessUnit();
                        if (item.Name == MPOWRConstants.Compute)
                            isComputeBUonly = true;
                        if (item.Name == MPOWRConstants.ComputeVolume|| item.Name == MPOWRConstants.ComputeValue)
                            isOtherBU = true;
                    }
                    if (isComputeBUonly == true && isOtherBU == true)
                        isAllBU = true;
                    if (MP_Count == null)
                    {
                        //var WMGOVeryHigh = 5;
                        //var WMGOHigh = 3;
                        //var WMGOVeryLow = -5;
                        //var WMGOLow = -3;
                        modelResult = (from input in dbContext.ModelParameterTables.AsNoTracking()
                                           //join mdf in dbContext.MDFPlannings on input.VersionID equals mdf.ID
                                       where (input.ModelParameterID == 1)
                                       select new
                                       {
                                           ModelParameterID = input.ModelParameterID,
                                           //FinancialYearID = mdf.FinancialYearID,
                                           //PartnerTypeID = mdf.PartnerTypeID,
                                           Max_Sellout_HighDecline_Max_MDF = input.Max_Sellout_HighDecline_Max_MDF,
                                           Max_Sellout_HighDecline_Min_MDF = input.Max_Sellout_HighDecline_Min_MDF,
                                           Max_Sellout_ModerateDecline_Max_MDF = input.Max_Sellout_ModerateDecline_Max_MDF,
                                           Max_Sellout_ModerateDecline_Min_MDF = input.Max_Sellout_ModerateDecline_Min_MDF,
                                           Max_Sellout_Steady_Max_MDF = input.Max_Sellout_Steady_Max_MDF,
                                           Max_Sellout_Steady_Min_MDF = input.Max_Sellout_Steady_Min_MDF,
                                           Max_Sellout_ModerateGrowth_Max_MDF = input.Max_Sellout_ModerateGrowth_Max_MDF,
                                           Max_Sellout_ModerateGrowth_Min_MDF = input.Max_Sellout_ModerateGrowth_Min_MDF,
                                           Max_Sellout_HighGrowth_Max = input.Max_Sellout_HighGrowth_Max,
                                           Max_Sellout_HighGrowth_Min = input.Max_Sellout_HighGrowth_Min,
                                           Target_accomplish_HighPrecision_Score = input.Target_accomplish_HighPrecision_Score,
                                           Target_accomplish_MediumPrecision_Score = input.Target_accomplish_MediumPrecision_Score,
                                           Max_Target_Accomplish_percentage = input.Max_Target_Accomplish_percentage,
                                           Min_Target_Accomplish_percentage = input.Min_Target_Accomplish_percentage,
                                           JPB_Max = input.JPB_Max,
                                           JPB_Min = input.JPB_Min,
                                           Prediction_High_Max = input.Prediction_High_Max,
                                           Prediction_High_Min = input.Prediction_High_Min,
                                           Prediction_Low_Max = input.Prediction_Low_Max,
                                           Prediction_Low_Min = input.Prediction_Low_Min,
                                           Weights_applied_t_1H = input.Weights_applied_t_1H,
                                           Weights_applied_t_2H = input.Weights_applied_t_2H,
                                           Weights_applied_t_3H = input.Weights_applied_t_3H,
                                           Min_Target_Productivity = input.Min_Target_Productivity,
                                           Last_Quarter_Sellout_Scale_Factor = input.Last_Quarter_Sellout_Scale_Factor,
                                           partner_size_threshold = input.partner_size_threshold,
                                           VersionID = VersionID,
                                           WMGO_Perf_Acc_VeryHigh = input.WMGO_Perf_Acc_VeryHigh,
                                           WMGO_Perf_Acc_High = input.WMGO_Perf_Acc_High,
                                           WMGO_Perf_Acc_VeryLow = input.WMGO_Perf_Acc_VeryLow,
                                           WMGO_Perf_Acc_Low = input.WMGO_Perf_Acc_Low,
                                           WMGO_Perf_Acc_Medium = input.WMGO_Perf_Acc_Medium                                         
                                       }).ToList();


                        if (isAllBU)
                        {
                            modelBuresult = (from input in dbContext.ModelBUParameterTables.AsNoTracking()
                                             where input.ModelParameterID == 1
                                             select new
                                             {
                                                 ModelBUParameterID = input.ModelBUParameterID,
                                                 ModelParameterID = input.ModelParameterID,
                                                 BusinessUnitID = input.BusinessUnitID,
                                                 High_Performance = input.High_Performance,
                                                 Medium_Performance = input.Medium_Performance,
                                                 Low_Performance = input.Low_Performance,
                                                 Min_Partner_Investment = input.Min_Partner_Investment,
                                                 New_Partner_RampUp_Scale = input.New_Partner_RampUp_Scale,
                                                 Preferred_Partner_Cut_Off_Percentage = input.Preferred_Partner_Cut_Off_Percentage,
                                                 Dist_cust_membership_weight_Silver_and_Below = input.Dist_cust_membership_weight_Silver_and_Below,
                                                 Dist_cust_membership_weight_Platinum_and_Gold = input.Dist_cust_membership_weight_Platinum_and_Gold,
                                                 //Sustain_Revenue = input.Sustain_Revenue,
                                                 Growth_Revenue = input.Growth_Revenue
                                             }).ToList();
                        }
                        else if (isOtherBU)
                        {
                            modelBuresult = (from input in dbContext.ModelBUParameterTables.AsNoTracking()
                                             where input.ModelParameterID == 1 && (input.BusinessUnit.DisplayName != MPOWRConstants.Compute)
                                             select new
                                             {
                                                 ModelBUParameterID = input.ModelBUParameterID,
                                                 ModelParameterID = input.ModelParameterID,
                                                 BusinessUnitID = input.BusinessUnitID,
                                                 High_Performance = input.High_Performance,
                                                 Medium_Performance = input.Medium_Performance,
                                                 Low_Performance = input.Low_Performance,
                                                 Min_Partner_Investment = input.Min_Partner_Investment,
                                                 New_Partner_RampUp_Scale = input.New_Partner_RampUp_Scale,
                                                 Preferred_Partner_Cut_Off_Percentage = input.Preferred_Partner_Cut_Off_Percentage,
                                                 Dist_cust_membership_weight_Silver_and_Below = input.Dist_cust_membership_weight_Silver_and_Below,
                                                 Dist_cust_membership_weight_Platinum_and_Gold = input.Dist_cust_membership_weight_Platinum_and_Gold,
                                                 //Sustain_Revenue = input.Sustain_Revenue,
                                                 Growth_Revenue = input.Growth_Revenue
                                             }).ToList();
                        }
                        else
                        {
                            modelBuresult = (from input in dbContext.ModelBUParameterTables.AsNoTracking()
                                             where input.ModelParameterID == 1 && (input.BusinessUnit.DisplayName != MPOWRConstants.ComputeValue && input.BusinessUnit.DisplayName != MPOWRConstants.ComputeVolume)
                                             select new
                                             {
                                                 ModelBUParameterID = input.ModelBUParameterID,
                                                 ModelParameterID = input.ModelParameterID,
                                                 BusinessUnitID = input.BusinessUnitID,
                                                 High_Performance = input.High_Performance,
                                                 Medium_Performance = input.Medium_Performance,
                                                 Low_Performance = input.Low_Performance,
                                                 Min_Partner_Investment = input.Min_Partner_Investment,
                                                 New_Partner_RampUp_Scale = input.New_Partner_RampUp_Scale,
                                                 Preferred_Partner_Cut_Off_Percentage = input.Preferred_Partner_Cut_Off_Percentage,
                                                 Dist_cust_membership_weight_Silver_and_Below = input.Dist_cust_membership_weight_Silver_and_Below,
                                                 Dist_cust_membership_weight_Platinum_and_Gold = input.Dist_cust_membership_weight_Platinum_and_Gold,
                                                 //Sustain_Revenue = input.Sustain_Revenue,
                                                 Growth_Revenue = input.Growth_Revenue
                                             }).ToList();
                        }
                    }
                    else
                    {
                        modelResult = (from input in dbContext.ModelParameterTables.AsNoTracking()
                                       join mdf in dbContext.MDFPlannings on input.VersionID equals mdf.ID
                                       where (input.VersionID == VersionID)
                                       select new
                                       {
                                           ModelParameterID = input.ModelParameterID,
                                           FinancialYearID = mdf.FinancialYearID,
                                           PartnerTypeID = mdf.PartnerTypeID,
                                           Max_Sellout_HighDecline_Max_MDF = (input.Max_Sellout_HighDecline_Max_MDF) ?? 0,
                                           Max_Sellout_HighDecline_Min_MDF = input.Max_Sellout_HighDecline_Min_MDF ?? 0,
                                           Max_Sellout_ModerateDecline_Max_MDF = input.Max_Sellout_ModerateDecline_Max_MDF ?? 0,
                                           Max_Sellout_ModerateDecline_Min_MDF = input.Max_Sellout_ModerateDecline_Min_MDF ?? 0,
                                           Max_Sellout_Steady_Max_MDF = input.Max_Sellout_Steady_Max_MDF ?? 0,
                                           Max_Sellout_Steady_Min_MDF = input.Max_Sellout_Steady_Min_MDF ?? 0,
                                           Max_Sellout_ModerateGrowth_Max_MDF = input.Max_Sellout_ModerateGrowth_Max_MDF ?? 0,
                                           Max_Sellout_ModerateGrowth_Min_MDF = input.Max_Sellout_ModerateGrowth_Min_MDF ?? 0,
                                           Max_Sellout_HighGrowth_Max = input.Max_Sellout_HighGrowth_Max ?? 0,
                                           Max_Sellout_HighGrowth_Min = input.Max_Sellout_HighGrowth_Min ?? 0,
                                           Target_accomplish_HighPrecision_Score = input.Target_accomplish_HighPrecision_Score ?? 0,
                                           Target_accomplish_MediumPrecision_Score = input.Target_accomplish_MediumPrecision_Score ?? 0,
                                           Max_Target_Accomplish_percentage = input.Max_Target_Accomplish_percentage ?? 0,
                                           Min_Target_Accomplish_percentage = input.Min_Target_Accomplish_percentage ?? 0,
                                           JPB_Max = input.JPB_Max ?? 0,
                                           JPB_Min = input.JPB_Min ?? 0,
                                           Prediction_High_Max = input.Prediction_High_Max ?? 0,
                                           Prediction_High_Min = input.Prediction_High_Min ?? 0,
                                           Prediction_Low_Max = input.Prediction_Low_Max ?? 0,
                                           Prediction_Low_Min = input.Prediction_Low_Min ?? 0,
                                           Weights_applied_t_1H = input.Weights_applied_t_1H ?? 0,
                                           Weights_applied_t_2H = input.Weights_applied_t_2H ?? 0,
                                           Weights_applied_t_3H = input.Weights_applied_t_3H ?? 0,
                                           Min_Target_Productivity = input.Min_Target_Productivity,
                                           Last_Quarter_Sellout_Scale_Factor = input.Last_Quarter_Sellout_Scale_Factor ?? 0,
                                           partner_size_threshold = input.partner_size_threshold ?? 0,
                                           WMGO_Perf_Acc_VeryHigh=input.WMGO_Perf_Acc_VeryHigh??0,
                                           WMGO_Perf_Acc_High = input.WMGO_Perf_Acc_High ?? 0,
                                           WMGO_Perf_Acc_VeryLow = input.WMGO_Perf_Acc_VeryLow ?? 0,
                                           WMGO_Perf_Acc_Low = input.WMGO_Perf_Acc_Low ?? 0,
                                           WMGO_Perf_Acc_Medium = input.WMGO_Perf_Acc_Medium ??0,
                                           VersionID = input.VersionID
                                       }).ToList();

                        int ModelParameterID = modelResult.FirstOrDefault().ModelParameterID;
                        if (isAllBU)
                        {
                            var buBudget = (from BU in dbContext.BUBudgets where BU.VersionID == VersionID select BU).ToList();
                            if (buBudget.Count > 0)
                            {
                                modelBuresult = (from input in dbContext.ModelBUParameterTables.AsNoTracking()
                                                 join bu in dbContext.BusinessUnits on input.BusinessUnitID equals bu.BusinessUnitID
                                                 join budget in dbContext.BUBudgets on bu.BusinessUnitID equals budget.BusinessUnitID
                                                 where input.ModelParameterID == ModelParameterID && budget.VersionID == VersionID //&& bu.IsActive == true
                                                 select new
                                                 {
                                                     ModelBUParameterID = input.ModelBUParameterID,
                                                     ModelParameterID = input.ModelParameterID,
                                                     BusinessUnitID = input.BusinessUnitID,
                                                     High_Performance = input.High_Performance ?? 0,
                                                     Medium_Performance = input.Medium_Performance ?? 0,
                                                     Low_Performance = input.Low_Performance ?? 0,
                                                     Min_Partner_Investment = input.Min_Partner_Investment ?? 0,
                                                     New_Partner_RampUp_Scale = input.New_Partner_RampUp_Scale ?? 0,
                                                     Preferred_Partner_Cut_Off_Percentage = input.Preferred_Partner_Cut_Off_Percentage ?? 0,
                                                     Dist_cust_membership_weight_Silver_and_Below = input.Dist_cust_membership_weight_Silver_and_Below ?? 0,
                                                     Dist_cust_membership_weight_Platinum_and_Gold = input.Dist_cust_membership_weight_Platinum_and_Gold ?? 0,
                                                     //Sustain_Revenue = input.Sustain_Revenue ?? 0,
                                                     Growth_Revenue = input.Growth_Revenue ?? 0
                                                 }).ToList();
                            }
                            else
                            {
                                modelBuresult = (from input in dbContext.ModelBUParameterTables.AsNoTracking()
                                                 join bu in dbContext.BusinessUnits on input.BusinessUnitID equals bu.BusinessUnitID
                                                 where input.ModelParameterID == ModelParameterID && bu.IsActive == true
                                                 select new
                                                 {
                                                     ModelBUParameterID = input.ModelBUParameterID,
                                                     ModelParameterID = input.ModelParameterID,
                                                     BusinessUnitID = input.BusinessUnitID,
                                                     High_Performance = input.High_Performance ?? 0,
                                                     Medium_Performance = input.Medium_Performance ?? 0,
                                                     Low_Performance = input.Low_Performance ?? 0,
                                                     Min_Partner_Investment = input.Min_Partner_Investment ?? 0,
                                                     New_Partner_RampUp_Scale = input.New_Partner_RampUp_Scale ?? 0,
                                                     Preferred_Partner_Cut_Off_Percentage = input.Preferred_Partner_Cut_Off_Percentage ?? 0,
                                                     Dist_cust_membership_weight_Silver_and_Below = input.Dist_cust_membership_weight_Silver_and_Below ?? 0,
                                                     Dist_cust_membership_weight_Platinum_and_Gold = input.Dist_cust_membership_weight_Platinum_and_Gold ?? 0,
                                                     //Sustain_Revenue = input.Sustain_Revenue ?? 0,
                                                     Growth_Revenue = input.Growth_Revenue ?? 0
                                                 }).ToList();
                            }

                                
                        }
                        else if (isOtherBU)
                        {
                            var buBudget = (from BU in dbContext.BUBudgets where BU.VersionID == VersionID select BU).ToList();
                            if (buBudget.Count > 0)
                            {
                                modelBuresult = (from input in dbContext.ModelBUParameterTables.AsNoTracking()
                                                 join bu in dbContext.BusinessUnits on input.BusinessUnitID equals bu.BusinessUnitID
                                                 join budget in dbContext.BUBudgets on bu.BusinessUnitID equals budget.BusinessUnitID
                                                 where input.ModelParameterID == ModelParameterID && budget.VersionID == VersionID && (input.BusinessUnit.DisplayName != MPOWRConstants.Compute) //&& bu.IsActive == true
                                                 select new
                                                 {
                                                     ModelBUParameterID = input.ModelBUParameterID,
                                                     ModelParameterID = input.ModelParameterID,
                                                     BusinessUnitID = input.BusinessUnitID,
                                                     High_Performance = input.High_Performance ?? 0,
                                                     Medium_Performance = input.Medium_Performance ?? 0,
                                                     Low_Performance = input.Low_Performance ?? 0,
                                                     Min_Partner_Investment = input.Min_Partner_Investment ?? 0,
                                                     New_Partner_RampUp_Scale = input.New_Partner_RampUp_Scale ?? 0,
                                                     Preferred_Partner_Cut_Off_Percentage = input.Preferred_Partner_Cut_Off_Percentage ?? 0,
                                                     Dist_cust_membership_weight_Silver_and_Below = input.Dist_cust_membership_weight_Silver_and_Below ?? 0,
                                                     Dist_cust_membership_weight_Platinum_and_Gold = input.Dist_cust_membership_weight_Platinum_and_Gold ?? 0,
                                                     //Sustain_Revenue = input.Sustain_Revenue ?? 0,
                                                     Growth_Revenue = input.Growth_Revenue ?? 0
                                                 }).ToList();
                            }
                            else {
                                modelBuresult = (from input in dbContext.ModelBUParameterTables.AsNoTracking()
                                                 join bu in dbContext.BusinessUnits on input.BusinessUnitID equals bu.BusinessUnitID
                                                 where input.ModelParameterID == ModelParameterID && (input.BusinessUnit.DisplayName != MPOWRConstants.Compute) && bu.IsActive == true
                                                 select new
                                                 {
                                                     ModelBUParameterID = input.ModelBUParameterID,
                                                     ModelParameterID = input.ModelParameterID,
                                                     BusinessUnitID = input.BusinessUnitID,
                                                     High_Performance = input.High_Performance ?? 0,
                                                     Medium_Performance = input.Medium_Performance ?? 0,
                                                     Low_Performance = input.Low_Performance ?? 0,
                                                     Min_Partner_Investment = input.Min_Partner_Investment ?? 0,
                                                     New_Partner_RampUp_Scale = input.New_Partner_RampUp_Scale ?? 0,
                                                     Preferred_Partner_Cut_Off_Percentage = input.Preferred_Partner_Cut_Off_Percentage ?? 0,
                                                     Dist_cust_membership_weight_Silver_and_Below = input.Dist_cust_membership_weight_Silver_and_Below ?? 0,
                                                     Dist_cust_membership_weight_Platinum_and_Gold = input.Dist_cust_membership_weight_Platinum_and_Gold ?? 0,
                                                     //Sustain_Revenue = input.Sustain_Revenue ?? 0,
                                                     Growth_Revenue = input.Growth_Revenue ?? 0
                                                 }).ToList();
                            }
                                
                        }
                        else
                        {
                            var buBudget = (from BU in dbContext.BUBudgets where BU.VersionID == VersionID select BU).ToList();
                            if (buBudget.Count > 0)
                            {
                                modelBuresult = (from input in dbContext.ModelBUParameterTables
                                                 join bu in dbContext.BusinessUnits on input.BusinessUnitID equals bu.BusinessUnitID
                                                 join budget in dbContext.BUBudgets on bu.BusinessUnitID equals budget.BusinessUnitID
                                                 where input.ModelParameterID == ModelParameterID && budget.VersionID == VersionID && (input.BusinessUnit.DisplayName != MPOWRConstants.ComputeValue && input.BusinessUnit.DisplayName != MPOWRConstants.ComputeVolume)
                                                 //&& bu.IsActive == true
                                                 select new
                                                 {
                                                     ModelBUParameterID = input.ModelBUParameterID,
                                                     ModelParameterID = input.ModelParameterID,
                                                     BusinessUnitID = input.BusinessUnitID,
                                                     High_Performance = input.High_Performance ?? 0,
                                                     Medium_Performance = input.Medium_Performance ?? 0,
                                                     Low_Performance = input.Low_Performance ?? 0,
                                                     Min_Partner_Investment = input.Min_Partner_Investment ?? 0,
                                                     New_Partner_RampUp_Scale = input.New_Partner_RampUp_Scale ?? 0,
                                                     Preferred_Partner_Cut_Off_Percentage = input.Preferred_Partner_Cut_Off_Percentage ?? 0,
                                                     Dist_cust_membership_weight_Silver_and_Below = input.Dist_cust_membership_weight_Silver_and_Below ?? 0,
                                                     Dist_cust_membership_weight_Platinum_and_Gold = input.Dist_cust_membership_weight_Platinum_and_Gold ?? 0,
                                                     //Sustain_Revenue = input.Sustain_Revenue ?? 0,
                                                     Growth_Revenue = input.Growth_Revenue ?? 0
                                                 }).ToList();
                            }
                            else
                            {
                                modelBuresult = (from input in dbContext.ModelBUParameterTables
                                                 join bu in dbContext.BusinessUnits on input.BusinessUnitID equals bu.BusinessUnitID
                                                 where input.ModelParameterID == ModelParameterID && (input.BusinessUnit.DisplayName != MPOWRConstants.ComputeValue && input.BusinessUnit.DisplayName != MPOWRConstants.ComputeVolume)
                                                 && bu.IsActive == true
                                                 select new
                                                 {
                                                     ModelBUParameterID = input.ModelBUParameterID,
                                                     ModelParameterID = input.ModelParameterID,
                                                     BusinessUnitID = input.BusinessUnitID,
                                                     High_Performance = input.High_Performance ?? 0,
                                                     Medium_Performance = input.Medium_Performance ?? 0,
                                                     Low_Performance = input.Low_Performance ?? 0,
                                                     Min_Partner_Investment = input.Min_Partner_Investment ?? 0,
                                                     New_Partner_RampUp_Scale = input.New_Partner_RampUp_Scale ?? 0,
                                                     Preferred_Partner_Cut_Off_Percentage = input.Preferred_Partner_Cut_Off_Percentage ?? 0,
                                                     Dist_cust_membership_weight_Silver_and_Below = input.Dist_cust_membership_weight_Silver_and_Below ?? 0,
                                                     Dist_cust_membership_weight_Platinum_and_Gold = input.Dist_cust_membership_weight_Platinum_and_Gold ?? 0,
                                                     //Sustain_Revenue = input.Sustain_Revenue ?? 0,
                                                     Growth_Revenue = input.Growth_Revenue ?? 0
                                                 }).ToList();
                            }                                
                        }

                    }


                    List<dynamic> result = new List<dynamic>();
                    result.Add(modelResult);
                    result.Add(modelBuresult);

                    if(frmStep1)
                    {
                        bool flag = DefaultCheckData(VersionID??0, UserID);
                        if (flag)
                        {
                            return result;
                        }
                        else
                        {
                            ModelParameterInputmodel model = new ModelParameterInputmodel();
                            model.Modelparameters = new ModelParametermodel();
                            model.Modelparameters.VersionID = VersionID??0;
                            model.Modelparameters.ModelParameterID = modelResult.FirstOrDefault().ModelParameterID;
                            //model.Modelparameters.FinancialYearID = modelResult.FirstOrDefault().FinancialYearID;
                            //model.Modelparameters.PartnerTypeID = modelResult.FirstOrDefault().PartnerTypeID;
                            model.Modelparameters.Max_Sellout_HighDecline_Max_MDF = modelResult.FirstOrDefault().Max_Sellout_HighDecline_Max_MDF;
                            model.Modelparameters.Max_Sellout_HighDecline_Min_MDF = modelResult.FirstOrDefault().Max_Sellout_HighDecline_Min_MDF;
                            model.Modelparameters.Max_Sellout_ModerateDecline_Max_MDF = modelResult.FirstOrDefault().Max_Sellout_ModerateDecline_Max_MDF;
                            model.Modelparameters.Max_Sellout_ModerateDecline_Min_MDF = modelResult.FirstOrDefault().Max_Sellout_ModerateDecline_Min_MDF;
                            model.Modelparameters.Max_Sellout_Steady_Max_MDF = modelResult.FirstOrDefault().Max_Sellout_Steady_Max_MDF;
                            model.Modelparameters.Max_Sellout_Steady_Min_MDF = modelResult.FirstOrDefault().Max_Sellout_Steady_Min_MDF;
                            model.Modelparameters.Max_Sellout_ModerateGrowth_Max_MDF = modelResult.FirstOrDefault().Max_Sellout_ModerateGrowth_Max_MDF;
                            model.Modelparameters.Max_Sellout_ModerateGrowth_Min_MDF = modelResult.FirstOrDefault().Max_Sellout_ModerateGrowth_Min_MDF;
                            model.Modelparameters.Max_Sellout_HighGrowth_Max = modelResult.FirstOrDefault().Max_Sellout_HighGrowth_Max;
                            model.Modelparameters.Max_Sellout_HighGrowth_Min = modelResult.FirstOrDefault().Max_Sellout_HighGrowth_Min;
                            model.Modelparameters.Target_accomplish_HighPrecision_Score = modelResult.FirstOrDefault().Target_accomplish_HighPrecision_Score;
                            model.Modelparameters.Target_accomplish_MediumPrecision_Score = modelResult.FirstOrDefault().Target_accomplish_MediumPrecision_Score;
                            model.Modelparameters.Max_Target_Accomplish_percentage = modelResult.FirstOrDefault().Max_Target_Accomplish_percentage;
                            model.Modelparameters.Min_Target_Accomplish_percentage = modelResult.FirstOrDefault().Min_Target_Accomplish_percentage;
                            model.Modelparameters.JPB_Max = modelResult.FirstOrDefault().JPB_Max;
                            model.Modelparameters.JPB_Min = modelResult.FirstOrDefault().JPB_Min;
                            model.Modelparameters.Prediction_High_Max = modelResult.FirstOrDefault().Prediction_High_Max;
                            model.Modelparameters.Prediction_High_Min = modelResult.FirstOrDefault().Prediction_High_Min;
                            model.Modelparameters.Prediction_Low_Max = modelResult.FirstOrDefault().Prediction_Low_Max;
                            model.Modelparameters.Prediction_Low_Min = modelResult.FirstOrDefault().Prediction_Low_Min;
                            model.Modelparameters.Weights_applied_t_1H = modelResult.FirstOrDefault().Weights_applied_t_1H;
                            model.Modelparameters.Weights_applied_t_2H = modelResult.FirstOrDefault().Weights_applied_t_2H;
                            model.Modelparameters.Weights_applied_t_3H = modelResult.FirstOrDefault().Weights_applied_t_3H;
                            model.Modelparameters.Min_Target_Productivity = modelResult.FirstOrDefault().Min_Target_Productivity;
                            model.Modelparameters.Last_Quarter_Sellout_Scale_Factor = modelResult.FirstOrDefault().Last_Quarter_Sellout_Scale_Factor;
                            model.Modelparameters.partner_size_threshold = modelResult.FirstOrDefault().partner_size_threshold;
                            model.Modelparameters.VersionID = modelResult.FirstOrDefault().VersionID;
                            model.Modelparameters.WMGO_Perf_Acc_VeryHigh = modelResult.FirstOrDefault().WMGO_Perf_Acc_VeryHigh;
                            model.Modelparameters.WMGO_Perf_Acc_High = modelResult.FirstOrDefault().WMGO_Perf_Acc_High;
                            model.Modelparameters.WMGO_Perf_Acc_VeryLow = modelResult.FirstOrDefault().WMGO_Perf_Acc_VeryLow;
                            model.Modelparameters.WMGO_Perf_Acc_Low = modelResult.FirstOrDefault().WMGO_Perf_Acc_Low;
                            model.Modelparameters.WMGO_Perf_Acc_Medium = modelResult.FirstOrDefault().WMGO_Perf_Acc_Medium;

                            List<ModelBUParametermodel> lstmodelBuresult = new List<ModelBUParametermodel>();
                            foreach (var item in modelBuresult)
                            {
                                ModelBUParametermodel modelBUResult = new ModelBUParametermodel();
                                modelBUResult.ModelBUParameterID = item.ModelBUParameterID;
                                modelBUResult.ModelParameterID = item.ModelParameterID;
                                modelBUResult.BusinessUnitID = item.BusinessUnitID;
                                modelBUResult.High_Performance = item.High_Performance;
                                modelBUResult.Medium_Performance = item.Medium_Performance;
                                modelBUResult.Low_Performance = item.Low_Performance;
                                modelBUResult.Min_Partner_Investment = item.Min_Partner_Investment;
                                modelBUResult.New_Partner_RampUp_Scale = item.New_Partner_RampUp_Scale;
                                modelBUResult.Preferred_Partner_Cut_Off_Percentage = item.Preferred_Partner_Cut_Off_Percentage;
                                modelBUResult.Dist_cust_membership_weight_Silver_and_Below = item.Dist_cust_membership_weight_Silver_and_Below;
                                modelBUResult.Dist_cust_membership_weight_Platinum_and_Gold = item.Dist_cust_membership_weight_Platinum_and_Gold;
                                //modelBUResult.Sustain_Revenue = item.Sustain_Revenue;
                                modelBUResult.Growth_Revenue = item.Growth_Revenue;
                                lstmodelBuresult.Add(modelBUResult);
                            }


                            model.ModelBUparameters = new List<ModelBUParametermodel>();
                            model.ModelBUparameters = lstmodelBuresult;
                            model.Modelparameters.UserID = UserID;
                            AddModelParameters(model);
                            return result;
                        }
                    }
                    else
                    {
                        return result;
                    }




                    //return result;
                }
            }

            catch (Exception ex)
            {

                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }


        /// <summary>
        /// To insert data from the view to ModelParameterTable.
        /// </summary>
        /// <returns></returns>
        [Route("api/ModelParameter/AddModelParameters")]
        [HttpPost]
        public IHttpActionResult AddModelParameters(ModelParameterInputmodel ModelInputParameter)
        {
            try
            {
                using (MPOWREntities db = new MPOWREntities())
                {
                    TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                    DateTime createdDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                    int ModelParameterID = 0;
                    //int? CountryID = ModelInputParameter.Modelparameters.CountryID;
                    //int? PartnerTypeID = ModelInputParameter.Modelparameters.PartnerTypeID;
                    //int? DistrictID = ModelInputParameter.Modelparameters.DistrictID;
                    //int? FinancialyearID = ModelInputParameter.Modelparameters.FinancialYearID;
                    decimal medium = 12.0m / 10.0m;
                    decimal lower = 15.0m / 10.0m;


                    //if (DistrictID == 0)
                    //{
                    //    ModelInputParameter.Modelparameters.DistrictID = 18;
                    //}

                    dynamic MP_Count = 0;
                    ModelParameterTable model = new ModelParameterTable();
                    //if (CountryID == 138)
                    //{
                    //    MP_Count = (from MP in db.ModelParameterTables where (MP.VersionID == ModelInputParameter.Modelparameters.VersionID) select MP).FirstOrDefault();
                    //}
                    //else
                    //{
                        MP_Count = (from MP in db.ModelParameterTables where (MP.VersionID == ModelInputParameter.Modelparameters.VersionID) select MP).FirstOrDefault();
                    //}


                    if (MP_Count == null)
                    {
                        ModelInputParameter.Modelparameters.ModelParameterID = 0;
                        model.CreatedBy = ModelInputParameter.Modelparameters.UserID;
                        model.CreatedDate = createdDate;
                        model.ModifiedBy = ModelInputParameter.Modelparameters.UserID;
                        model.ModifiedDate = createdDate;


                    }
                    else
                    {
                        ModelInputParameter.Modelparameters.ModelParameterID = MP_Count.ModelParameterID;
                        model.CreatedBy = MP_Count.CreatedBy;
                        model.CreatedDate = MP_Count.CreatedDate;
                        model.ModifiedBy = ModelInputParameter.Modelparameters.UserID;
                        model.ModifiedDate = createdDate;

                    }


                    //modelParamTable data   
                    model.Max_Sellout_HighDecline_Max_MDF = ModelInputParameter.Modelparameters.Max_Sellout_HighDecline_Max_MDF;
                    model.Max_Sellout_HighDecline_Min_MDF = ModelInputParameter.Modelparameters.Max_Sellout_HighDecline_Min_MDF;
                    model.Max_Sellout_ModerateDecline_Max_MDF = ModelInputParameter.Modelparameters.Max_Sellout_ModerateDecline_Max_MDF;
                    model.Max_Sellout_ModerateDecline_Min_MDF = ModelInputParameter.Modelparameters.Max_Sellout_ModerateDecline_Min_MDF;
                    model.Max_Sellout_Steady_Max_MDF = ModelInputParameter.Modelparameters.Max_Sellout_Steady_Max_MDF;
                    model.Max_Sellout_Steady_Min_MDF = ModelInputParameter.Modelparameters.Max_Sellout_Steady_Min_MDF;
                    model.Max_Sellout_ModerateGrowth_Max_MDF = ModelInputParameter.Modelparameters.Max_Sellout_ModerateGrowth_Max_MDF;
                    model.Max_Sellout_ModerateGrowth_Min_MDF = ModelInputParameter.Modelparameters.Max_Sellout_ModerateGrowth_Min_MDF;
                    model.Max_Sellout_HighGrowth_Max = ModelInputParameter.Modelparameters.Max_Sellout_HighGrowth_Max;
                    model.Max_Sellout_HighGrowth_Min = ModelInputParameter.Modelparameters.Max_Sellout_HighGrowth_Min;
                    model.Target_accomplish_HighPrecision_Score = ModelInputParameter.Modelparameters.Target_accomplish_HighPrecision_Score;
                    model.Target_accomplish_MediumPrecision_Score = ModelInputParameter.Modelparameters.Target_accomplish_MediumPrecision_Score;
                    model.Max_Target_Accomplish_percentage = ModelInputParameter.Modelparameters.Max_Target_Accomplish_percentage;
                    model.Min_Target_Accomplish_percentage = ModelInputParameter.Modelparameters.Min_Target_Accomplish_percentage;
                    model.JPB_Max = ModelInputParameter.Modelparameters.JPB_Max;
                    model.JPB_Min = ModelInputParameter.Modelparameters.JPB_Min;
                    model.Prediction_High_Max = ModelInputParameter.Modelparameters.Prediction_High_Max;
                    model.Prediction_High_Min = ModelInputParameter.Modelparameters.Prediction_High_Min;
                    model.Prediction_Low_Max = ModelInputParameter.Modelparameters.Prediction_Low_Max;
                    model.Prediction_Low_Min = ModelInputParameter.Modelparameters.Prediction_Low_Min;
                    model.Weights_applied_t_1H = ModelInputParameter.Modelparameters.Weights_applied_t_1H;
                    model.Weights_applied_t_2H = ModelInputParameter.Modelparameters.Weights_applied_t_2H;
                    model.Weights_applied_t_3H = ModelInputParameter.Modelparameters.Weights_applied_t_3H;
                    model.WMGO_Perf_Acc_High = ModelInputParameter.Modelparameters.WMGO_Perf_Acc_High;
                    model.WMGO_Perf_Acc_VeryHigh = ModelInputParameter.Modelparameters.WMGO_Perf_Acc_VeryHigh;
                    model.WMGO_Perf_Acc_Low = ModelInputParameter.Modelparameters.WMGO_Perf_Acc_Low;
                    model.WMGO_Perf_Acc_VeryLow = ModelInputParameter.Modelparameters.WMGO_Perf_Acc_VeryLow;
                    model.WMGO_Perf_Acc_Medium = ModelInputParameter.Modelparameters.WMGO_Perf_Acc_Medium;
                    model.Min_Target_Productivity = ModelInputParameter.Modelparameters.Min_Target_Productivity;
                    model.Last_Quarter_Sellout_Scale_Factor = ModelInputParameter.Modelparameters.Last_Quarter_Sellout_Scale_Factor;
                    model.partner_size_threshold = ModelInputParameter.Modelparameters.partner_size_threshold;
                    model.VersionID = ModelInputParameter.Modelparameters.VersionID;
                    var modelParamID = ModelInputParameter.Modelparameters.ModelParameterID;
                    var modelversionID = ModelInputParameter.Modelparameters.VersionID;
                    var modelparameter = db.ModelParameterTables.Where(x => x.ModelParameterID == modelParamID && x.VersionID == modelversionID);
                    //if (modelparameter == null)
                    //{
                    //    //int dd;
                    //    //var cl = (from dt in db.ModelParameterTables select dt).ToList();
                    //    //if (cl.Count == 0)
                    //    //{ dd = 0; }
                    //    //else
                    //    //{
                    //    //    dd = db.ModelParameterTables.Max(x => x.ModelParameterID);
                    //    //}
                    //    //ModelParameterID = dd + 1;
                    //    try
                    //    {
                    //        db.ModelParameterTables.Add(model);
                    //        db.SaveChanges();
                    //        ModelParameterID = model.ModelParameterID;
                    //    }

                    //    catch (Exception ex)
                    //    {

                    //        MPOWRLogManager.LogMessage(ex.Message.ToString());
                    //        MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                    //        return null;
                    //    }
                    //}
                    //else
                    //{

                        model.ModelParameterID = ModelInputParameter.Modelparameters.ModelParameterID;
                        if (model.ModelParameterID != 1)
                        {
                            db.Set<ModelParameterTable>().AddOrUpdate(model);
                            db.SaveChanges();
                         }
                        ModelParameterID = model.ModelParameterID;

                    //}

                    //insert and update in modelBU Table
                    var ModelParameter = (from MP in db.ModelParameterTables where (MP.ModelParameterID == ModelParameterID && MP.VersionID == ModelInputParameter.Modelparameters.VersionID) select MP).FirstOrDefault();
                    var BUparameter = (from MBP in db.ModelBUParameterTables where (MBP.ModelParameterID != 1 && MBP.ModelParameterID == ModelParameter.ModelParameterID) select MBP).ToList();
                    foreach (var buspecific in ModelInputParameter.ModelBUparameters)
                    {

                        ModelBUParameterTable modelBU = new ModelBUParameterTable();

                       // modelBU.ModelBUParameterID = buspecific.ModelBUParameterID;
                        modelBU.ModelParameterID = ModelParameterID;
                        modelBU.BusinessUnitID = buspecific.BusinessUnitID;
                        modelBU.High_Performance = buspecific.High_Performance;
                        modelBU.Min_Partner_Investment = buspecific.Min_Partner_Investment;
                        modelBU.New_Partner_RampUp_Scale = buspecific.New_Partner_RampUp_Scale;
                        modelBU.Preferred_Partner_Cut_Off_Percentage = buspecific.Preferred_Partner_Cut_Off_Percentage;
                        modelBU.Dist_cust_membership_weight_Silver_and_Below = buspecific.Dist_cust_membership_weight_Silver_and_Below;
                        modelBU.Dist_cust_membership_weight_Platinum_and_Gold = buspecific.Dist_cust_membership_weight_Platinum_and_Gold;
                        modelBU.Medium_Performance = medium * buspecific.High_Performance;
                        modelBU.Low_Performance = lower * buspecific.High_Performance;
                        //modelBU.Sustain_Revenue = buspecific.Sustain_Revenue;
                        modelBU.Growth_Revenue = buspecific.Growth_Revenue;
                        if (BUparameter == null || BUparameter.Count() == 0)
                        {
                            try
                            {
                                modelBU.CreatedBy = ModelInputParameter.Modelparameters.UserID;
                                modelBU.CreatedDate = createdDate;
                                modelBU.ModifiedBy = ModelInputParameter.Modelparameters.UserID;
                                modelBU.ModifiedDate = createdDate;
                                db.ModelBUParameterTables.Add(modelBU);
                                db.SaveChanges();
                                db.Database.Connection.Close();
                            }
                            catch (MPOWRException ex)
                            {
                                MPOWRLogManager.LogMessage(ex.Message.ToString());
                                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                                return null;
                            }
                            catch (Exception ex)
                            {

                                MPOWRLogManager.LogMessage(ex.Message.ToString());
                                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                                return null;
                            }

                        }
                        else
                        {
                            try
                            {
                                modelBU.CreatedBy = BUparameter.Where(x=>x.BusinessUnitID == buspecific.BusinessUnitID).Select(x=>x.CreatedBy).SingleOrDefault();
                                modelBU.CreatedDate = BUparameter.Where(x => x.BusinessUnitID == buspecific.BusinessUnitID).Select(x => x.CreatedDate).SingleOrDefault();
                                modelBU.ModifiedBy = ModelInputParameter.Modelparameters.UserID;
                                modelBU.ModifiedDate = createdDate;
                                modelBU.ModelBUParameterID = buspecific.ModelBUParameterID;
                                if (modelBU.ModelParameterID != 1)
                                {
                                    db.Set<ModelBUParameterTable>().AddOrUpdate(modelBU);
                                    db.SaveChanges();
                                }
                                db.Database.Connection.Close();
                            }
                            catch (MPOWRException ex)
                            {
                                MPOWRLogManager.LogMessage(ex.Message.ToString());
                                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                                return null;
                            }
                            catch (Exception ex)
                            {

                                MPOWRLogManager.LogMessage(ex.Message.ToString());
                                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                                return null;
                            }
                        }

                    }


                    DbCommand command = db.Database.Connection.CreateCommand();
                    command.CommandText = "HPE_MDF_RMODEL";
                    //command.Parameters.Add(new SqlParameter("@C_ID", Convert.ToInt32(ModelInputParameter.Modelparameters.CountryID)));
                    //command.Parameters.Add(new SqlParameter("@PT_ID", Convert.ToInt32(ModelInputParameter.Modelparameters.PartnerTypeID)));
                    //command.Parameters.Add(new SqlParameter("@D_ID", Convert.ToInt32(ModelInputParameter.Modelparameters.DistrictID)));
                    //command.Parameters.Add(new SqlParameter("@F_ID", Convert.ToInt32(ModelInputParameter.Modelparameters.FinancialYearID)));
                    command.Parameters.Add(new SqlParameter("@V_ID", Convert.ToInt32(ModelInputParameter.Modelparameters.VersionID)));
                    command.Parameters.Add(new SqlParameter("@USER_ID",ModelInputParameter.Modelparameters.UserID));
                    command.Parameters.Add(new SqlParameter("@ISDEFAULTMP", false));
                    command.CommandTimeout = 1000;
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    try
                    {
                        db.Database.Connection.Open();

                        var reader = command.ExecuteReader();


                        //var result = ((IObjectContextAdapter)db).ObjectContext.Translate<dynamic>
                        //  (reader);
                        //reader.NextResult();



                    }
                    catch (MPOWRException ex)
                    {
                        MPOWRLogManager.LogMessage(ex.Message.ToString());
                        MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                        return null;
                    }
                    catch (Exception ex)
                    {

                        MPOWRLogManager.LogMessage(ex.Message.ToString());
                        MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                        return null;
                    }
                    finally
                    {
                        db.Database.Connection.Close();
                    }

                    //to push warning if the budget is over allocated
                    decimal baselineMdf = 0;
                    decimal allocated_budget = 0;
                    decimal difference = 0;
                    decimal round_diff = 0;


                    List<string> overAllocatedBu = new List<string>();
                    List<BusinessUnit> BuList = db.BusinessUnits.ToList();
                    List<BUBudget> budgetList = new List<BUBudget>();
                    List<ModelOutputSummaryTable> modelOutputSummaryList = new List<ModelOutputSummaryTable>();
                    
                    budgetList = db.BUBudgets.Where(c => c.VersionID == ModelInputParameter.Modelparameters.VersionID).OrderBy(c => c.BusinessUnitID).OrderBy(c => c.BusinessUnitID).ToList();

                    modelOutputSummaryList = db.ModelOutputSummaryTables.Where(c => c.ModelParameterID != 1 && c.BusinessUnitID != 0 && c.VersionID == ModelInputParameter.Modelparameters.VersionID).OrderBy(c => c.BusinessUnitID).ToList();
                    
                    foreach (var bubudgetlist in budgetList)
                    {
                        foreach (var modelsummary in modelOutputSummaryList)
                        {
                            if (bubudgetlist.BusinessUnitID == modelsummary.BusinessUnitID)
                            {
                                baselineMdf = (decimal)bubudgetlist.BaselineMDF;
                                allocated_budget = (decimal)modelsummary.Allocated;
                                difference = allocated_budget - baselineMdf;
                                round_diff = Math.Round(difference);
                                if (round_diff > 1)
                                {
                                    overAllocatedBu.Add(modelsummary.Business_Unit + ':' + '$' + round_diff.ToString() + ',');
                                }

                            }
                        }
                    }

                    // Update Version Table
                    var MDFPlanning = db.MDFPlannings.Find(ModelInputParameter.Modelparameters.VersionID);
                    if (MDFPlanning != null)
                    {
                        MDFPlanning.ModifiedBy = ModelInputParameter.Modelparameters.UserID;
                        MDFPlanning.ModifiedDate = createdDate;

                        db.Set<MDFPlanning>().AddOrUpdate(MDFPlanning);
                    }
                    db.SaveChanges();

                     if (overAllocatedBu.Count != 0)

                    {
                        return Ok(overAllocatedBu);
                    }

                    else
                    {
                        return Ok("Save Successfully");
                    }
 
                }
            }

            catch (Exception ex)
            {

                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }


        /// <summary>
        /// To get graph data based on businessunit id
        /// </summary>
        /// <param name="BusinessUnitID"></param>
        /// <param name="VersionID"></param>
        /// <returns></returns>                       
        [Route("api/ModelParameter/GetGraphdetailsByBusinessUnit")]
        [HttpGet]
        public GraphModel GetGraphdetailsByBusinessUnit(int BusinessUnitID, int VersionID)
        {
            using (MPOWREntities db = new MPOWREntities())
            {

                try
                {
                    GraphModel graphInputs = new GraphModel();
                    ModelOutputSummaryTable modelsummaryoutputDefaultvaues = new ModelOutputSummaryTable();
                    ModelOutputSummaryTable modelsummaryoutput = new ModelOutputSummaryTable();
                    string allocationLevel = (from mdf in db.MDFPlannings
                                              where mdf.ID == VersionID && mdf.Flag == true
                                              select mdf.AllocationLevel).FirstOrDefault();
                    int partnerTypeId = (from mdf in db.MDFPlannings
                                           where mdf.ID == VersionID && mdf.Flag == true
                                         select mdf.PartnerTypeID).FirstOrDefault();
                    if (partnerTypeId != 0)
                    {
                        
                        modelsummaryoutputDefaultvaues = db.ModelOutputSummaryTables.Where(x => x.ModelParameterID == 1 && x.VersionID == VersionID && x.BusinessUnitID == BusinessUnitID).FirstOrDefault();
                        modelsummaryoutput = db.ModelOutputSummaryTables.Where(x => x.ModelParameterID != 1 && x.VersionID == VersionID && x.BusinessUnitID == BusinessUnitID ).FirstOrDefault();
                        

                        if (modelsummaryoutput != null && modelsummaryoutputDefaultvaues != null)
                        {
                            // baseline

                            graphInputs.Default_Growth_MDF_Percentage = modelsummaryoutputDefaultvaues.growth_percentage ?? 0;
                            graphInputs.Default_Sustain_MDF_Percentage = modelsummaryoutputDefaultvaues.sustain_percentage ?? 0;
                            graphInputs.Final_Growth_MDF_Percentage = modelsummaryoutput.growth_percentage ?? 0;
                            graphInputs.Final_Sustain_MDF_Percentage = modelsummaryoutput.sustain_percentage ?? 0;



                            //Sales Growth


                            graphInputs.Default_SalesGrowth_with_mdf = modelsummaryoutputDefaultvaues.Sales_Growth_With_MDF ?? 0;
                            graphInputs.Default_SalesGrowth_without_mdf = modelsummaryoutputDefaultvaues.Sales_Growth_Without_MDF ?? 0;
                            graphInputs.SalesGrowth_with_mdf = modelsummaryoutput.Sales_Growth_With_MDF ?? 0;
                            graphInputs.SalesGrowth_without_mdf = modelsummaryoutput.Sales_Growth_Without_MDF ?? 0;



                            // MDF /Sellout


                            graphInputs.Default_MDFBYSellout_Percentage = modelsummaryoutputDefaultvaues.MDF_over_Sellout_Current_Period ?? 0;
                            graphInputs.MDFBYSellout_Percentage = modelsummaryoutput.MDF_over_Sellout_Current_Period ?? 0;


                            //Membership Tier
                            // alignment  vs miss aligned.
                            graphInputs.Default_Aligned_Percentage = modelsummaryoutputDefaultvaues.Aligned ?? 0;
                            graphInputs.Default_Misaligned_Percentage = modelsummaryoutputDefaultvaues.MisAligned ?? 0;
                            graphInputs.Aligned_Percentage = modelsummaryoutput.Aligned ?? 0;
                            graphInputs.MisAligned_Percentage = modelsummaryoutput.MisAligned ?? 0;

                            // By Membership

                            graphInputs.Default_MDF_Platinum_Percentage = modelsummaryoutputDefaultvaues.MDF_Platinum ?? 0;
                            graphInputs.Default_MDF_Gold_Percentage = modelsummaryoutputDefaultvaues.MDF_Gold ?? 0;
                            graphInputs.Default_MDF_SB_Percentage = modelsummaryoutputDefaultvaues.MDF_SB ?? 0;
                            graphInputs.MDF_Platinum_Percentage = modelsummaryoutput.MDF_Platinum ?? 0;
                            graphInputs.MDF_Gold_Percentage = modelsummaryoutput.MDF_Gold ?? 0;
                            graphInputs.MDF_SB_Percentage = modelsummaryoutput.MDF_SB ?? 0;


                            //YOY
                        }
                        else if (modelsummaryoutputDefaultvaues != null)
                        {
                            //Default values

                            // baseline

                            graphInputs.Default_Growth_MDF_Percentage = modelsummaryoutputDefaultvaues.growth_percentage ?? 0;
                            graphInputs.Default_Sustain_MDF_Percentage = modelsummaryoutputDefaultvaues.sustain_percentage ?? 0;
                            graphInputs.Final_Growth_MDF_Percentage = 0;
                            graphInputs.Final_Sustain_MDF_Percentage = 0;



                            //Sales Growth


                            graphInputs.Default_SalesGrowth_with_mdf = modelsummaryoutputDefaultvaues.Sales_Growth_With_MDF ?? 0;
                            graphInputs.Default_SalesGrowth_without_mdf = modelsummaryoutputDefaultvaues.Sales_Growth_Without_MDF ?? 0;
                            graphInputs.SalesGrowth_with_mdf = 0;
                            graphInputs.SalesGrowth_without_mdf = 0;



                            // MDF /Sellout


                            graphInputs.Default_MDFBYSellout_Percentage = modelsummaryoutputDefaultvaues.MDF_over_Sellout_Current_Period ?? 0;
                            graphInputs.MDFBYSellout_Percentage = 0;


                            //Membership Tier
                            // alignment  vs miss aligned.
                            graphInputs.Default_Aligned_Percentage = modelsummaryoutputDefaultvaues.Aligned ?? 0;
                            graphInputs.Default_Misaligned_Percentage = modelsummaryoutputDefaultvaues.MisAligned ?? 0;
                            graphInputs.Aligned_Percentage = 0;
                            graphInputs.MisAligned_Percentage = 0;

                            // By Membership

                            graphInputs.Default_MDF_Platinum_Percentage = modelsummaryoutputDefaultvaues.MDF_Platinum ?? 0;
                            graphInputs.Default_MDF_Gold_Percentage = modelsummaryoutputDefaultvaues.MDF_Gold ?? 0;
                            graphInputs.Default_MDF_SB_Percentage = modelsummaryoutputDefaultvaues.MDF_SB ?? 0;
                            graphInputs.MDF_Platinum_Percentage = 0;
                            graphInputs.MDF_Gold_Percentage = 0;
                            graphInputs.MDF_SB_Percentage = 0;

                        }

                    }
                    //    if (countryid != 0 && PartnertypeId != 0)
                    //{
                    //    if ((countryid == 138) && (PartnertypeId == 2))
                    //    {

                    //        modelsummaryoutputDefaultvaues = db.ModelOutputSummaryTables.Where(x => x.ModelParameterID == 1 && x.BusinessUnitID == BusinessUnitID ).FirstOrDefault();
                    //        modelsummaryoutput = db.ModelOutputSummaryTables.Where(x => x.ModelParameterID != 1 && x.VersionID == VersionID && x.BusinessUnitID == BusinessUnitID).FirstOrDefault();
                    //    }
                    //    else
                    //    {
                    //        modelsummaryoutputDefaultvaues = db.ModelOutputSummaryTables.Where(x => x.ModelParameterID == 1 && x.BusinessUnitID == BusinessUnitID ).FirstOrDefault();
                    //        modelsummaryoutput = db.ModelOutputSummaryTables.Where(x => x.ModelParameterID != 1 && x.VersionID == VersionID && x.BusinessUnitID == BusinessUnitID).FirstOrDefault();
                    //    }


                    //    if (modelsummaryoutput != null && modelsummaryoutputDefaultvaues != null)
                    //    {
                    //        // baseline

                    //        graphInputs.Default_Growth_MDF_Percentage = modelsummaryoutputDefaultvaues.growth_percentage ?? 0;
                    //        graphInputs.Default_Sustain_MDF_Percentage = modelsummaryoutputDefaultvaues.sustain_percentage ?? 0;
                    //        graphInputs.Final_Growth_MDF_Percentage = modelsummaryoutput.growth_percentage ?? 0;
                    //        graphInputs.Final_Sustain_MDF_Percentage = modelsummaryoutput.sustain_percentage ?? 0;



                    //        //Sales Growth


                    //        graphInputs.Default_SalesGrowth_with_mdf = modelsummaryoutputDefaultvaues.Sales_Growth_With_MDF ?? 0;
                    //        graphInputs.Default_SalesGrowth_without_mdf = modelsummaryoutputDefaultvaues.Sales_Growth_Without_MDF ?? 0;
                    //        graphInputs.SalesGrowth_with_mdf = modelsummaryoutput.Sales_Growth_With_MDF ?? 0;
                    //        graphInputs.SalesGrowth_without_mdf = modelsummaryoutput.Sales_Growth_Without_MDF ?? 0;



                    //        // MDF /Sellout


                    //        graphInputs.Default_MDFBYSellout_Percentage = modelsummaryoutputDefaultvaues.MDF_over_Sellout_Current_Period ?? 0;
                    //        graphInputs.MDFBYSellout_Percentage = modelsummaryoutput.MDF_over_Sellout_Current_Period ?? 0;


                    //        //Membership Tier
                    //        // alignment  vs miss aligned.
                    //        graphInputs.Default_Aligned_Percentage = modelsummaryoutputDefaultvaues.Aligned ?? 0;
                    //        graphInputs.Default_Misaligned_Percentage = modelsummaryoutputDefaultvaues.MisAligned ?? 0;
                    //        graphInputs.Aligned_Percentage = modelsummaryoutput.Aligned ?? 0;
                    //        graphInputs.MisAligned_Percentage = modelsummaryoutput.MisAligned ?? 0;

                    //        // By Membership

                    //        graphInputs.Default_MDF_Platinum_Percentage = modelsummaryoutputDefaultvaues.MDF_Platinum ?? 0;
                    //        graphInputs.Default_MDF_Gold_Percentage = modelsummaryoutputDefaultvaues.MDF_Gold ?? 0;
                    //        graphInputs.Default_MDF_SB_Percentage = modelsummaryoutputDefaultvaues.MDF_SB ?? 0;
                    //        graphInputs.MDF_Platinum_Percentage = modelsummaryoutput.MDF_Platinum ?? 0;
                    //        graphInputs.MDF_Gold_Percentage = modelsummaryoutput.MDF_Gold ?? 0;
                    //        graphInputs.MDF_SB_Percentage = modelsummaryoutput.MDF_SB ?? 0;


                    //        //YOY
                    //    }
                    //    else if (modelsummaryoutputDefaultvaues != null)
                    //    {
                    //        //Default values

                    //        // baseline

                    //        graphInputs.Default_Growth_MDF_Percentage = modelsummaryoutputDefaultvaues.growth_percentage ?? 0;
                    //        graphInputs.Default_Sustain_MDF_Percentage = modelsummaryoutputDefaultvaues.sustain_percentage ?? 0;
                    //        graphInputs.Final_Growth_MDF_Percentage = 0;
                    //        graphInputs.Final_Sustain_MDF_Percentage = 0;



                    //        //Sales Growth


                    //        graphInputs.Default_SalesGrowth_with_mdf = modelsummaryoutputDefaultvaues.Sales_Growth_With_MDF ?? 0;
                    //        graphInputs.Default_SalesGrowth_without_mdf = modelsummaryoutputDefaultvaues.Sales_Growth_Without_MDF ?? 0;
                    //        graphInputs.SalesGrowth_with_mdf = 0;
                    //        graphInputs.SalesGrowth_without_mdf = 0;



                    //        // MDF /Sellout


                    //        graphInputs.Default_MDFBYSellout_Percentage = modelsummaryoutputDefaultvaues.MDF_over_Sellout_Current_Period ?? 0;
                    //        graphInputs.MDFBYSellout_Percentage = 0;


                    //        //Membership Tier
                    //        // alignment  vs miss aligned.
                    //        graphInputs.Default_Aligned_Percentage = modelsummaryoutputDefaultvaues.Aligned ?? 0;
                    //        graphInputs.Default_Misaligned_Percentage = modelsummaryoutputDefaultvaues.MisAligned ?? 0;
                    //        graphInputs.Aligned_Percentage = 0;
                    //        graphInputs.MisAligned_Percentage = 0;

                    //        // By Membership

                    //        graphInputs.Default_MDF_Platinum_Percentage = modelsummaryoutputDefaultvaues.MDF_Platinum ?? 0;
                    //        graphInputs.Default_MDF_Gold_Percentage = modelsummaryoutputDefaultvaues.MDF_Gold ?? 0;
                    //        graphInputs.Default_MDF_SB_Percentage = modelsummaryoutputDefaultvaues.MDF_SB ?? 0;
                    //        graphInputs.MDF_Platinum_Percentage = 0;
                    //        graphInputs.MDF_Gold_Percentage = 0;
                    //        graphInputs.MDF_SB_Percentage = 0;

                    //    }
                    //}
                    return graphInputs;
                }

                catch (Exception ex)
                {

                    MPOWRLogManager.LogMessage(ex.Message.ToString());
                    MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                    return null;
                }
            }
        }


        /// <summary>
        /// To get data for YOY Graph
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("api/ModelParameter/YOYGraphData")]
        public IHttpActionResult YOYGraphData(Graph request)
        {
            using (MPOWREntities db = new MPOWREntities())
            {
                try
                {
                    var Result = string.Empty;
                    request.Type = "MODELPARAMETER";
                    DbCommand command = db.Database.Connection.CreateCommand();
                    command.CommandText = "usp_YOYGraph";
                    //command.Parameters.Add(new SqlParameter("@CountryID", request.CountryID));
                    //command.Parameters.Add(new SqlParameter("@PartnerTypeID", request.PartnerTypeID));
                    //command.Parameters.Add(new SqlParameter("@DistrictID", request.DistrictID));
                    //command.Parameters.Add(new SqlParameter("@FinancialYearID", request.FinancialYearID));
                    command.Parameters.Add(new SqlParameter("@BusinessUnitID", request.BusinessUnitID));
                    command.Parameters.Add(new SqlParameter("@type", request.Type));
                    command.Parameters.Add(new SqlParameter("@VersionID", request.VersionID));


                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    db.Database.Connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (Result == string.Empty)
                            {
                                Result = reader.GetValue(0).ToString();
                            }
                            else
                            {
                                Result = Result + reader.GetValue(0).ToString();
                            }
                        }
                    }
                    reader.Close();
                    Object json;

                    if (Result == null || Result == "")
                    {
                        json = Result;
                    }
                    else { json = JToken.Parse(Result); }
                     db.Database.Connection.Close();

                    return Ok(json);
                }

                catch (Exception ex)
                {

                    MPOWRLogManager.LogMessage(ex.Message.ToString());
                    MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                    db.Database.Connection.Close();
                    return null;
                }

            }
        }


        /// <summary>
        /// To call R Program
        /// </summary>
        /// <param name="CountryID"></param>
        /// <param name="PartnerTypeID"></param>
        /// <param name="DistrictID"></param>
        /// <param name="FinancialYearID"></param>
        /// <returns></returns>
        public int Rprograming(int CountryID, int PartnerTypeID, int DistrictID, int FinancialYearID)
        {
            using (MPOWREntities db = new MPOWREntities())
            {
                try
                {


                    var MDF_RMODEL = db.Database.SqlQuery<ModelOutputTable>("HPE_MDF_RMODEL @C_ID,@PT_ID,@D_ID,@F_ID",
                        new SqlParameter("@C_ID", CountryID),
                         new SqlParameter("@PT_ID", PartnerTypeID),
                          new SqlParameter("@D_ID", DistrictID),
                           new SqlParameter("@F_ID", FinancialYearID)
                        );
                    return 1;
                }
                catch (MPOWRException ex)
                {
                    MPOWRLogManager.LogMessage(ex.Message.ToString());
                    MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

                    return 0;

                }
                catch (Exception ex)
                {

                    MPOWRLogManager.LogMessage(ex.Message.ToString());
                    MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                    db.Database.Connection.Close();
                    return 0;
                }

            }
        }


        /// <summary>
        /// To reset the modelparameters to default
        /// </summary>
        /// <param name="CountryID"></param>
        /// <param name="PartnerTypeID"></param>
        /// <param name="FinancialyearID"></param>
        /// <param name="DistrictID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ModelParameter/ResetModelparam")]
        public dynamic ResetModelparam(int VersionID,string UserID)
        {
            using (MPOWREntities db = new MPOWREntities())
            {
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                DateTime createdDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                decimal medium = 12.0m / 10.0m;
                decimal lower = 15.0m / 10.0m;
                try
                {
                    ModelParameterTable defaultModelParameter = new ModelParameterTable();

                    defaultModelParameter = db.ModelParameterTables.Where(x => x.ModelParameterID == 1).FirstOrDefault();

                    ModelParameterTable modelParameterTable = new ModelParameterTable();

                    List<ModelBUParameterTable> modelBUTable = new List<ModelBUParameterTable>();

                    List<ModelBUParameterTable> modelDefaultBUTable = new List<ModelBUParameterTable>();
                    //var modelBUEntity = db.ModelBUParameterTables.ToList();
                   
                    modelParameterTable = db.ModelParameterTables.Where(x => 
                                                            x.VersionID == VersionID).FirstOrDefault();

                    if (defaultModelParameter != null)
                    {
                        modelDefaultBUTable = db.ModelBUParameterTables.Where(x => x.ModelParameterID == 1).ToList();
                    }

                    if (modelParameterTable != null)
                    {
                        modelBUTable = db.ModelBUParameterTables.Where(x => x.ModelParameterID == modelParameterTable.ModelParameterID &&
                                                                   x.ModelParameterID != 1).ToList();

                       // defaultModelParameter.ModelParameterID = modelParameterTable.ModelParameterID;




                        //foreach (var model in modelBUTable)
                        //{
                        //    ModelBUParameterTable userModel = modelBUTable.Where(x => x.BusinessUnitID == model.BusinessUnitID).FirstOrDefault();
                        //    if (userModel != null)
                        //    {
                        //        model.ModelParameterID = userModel.ModelParameterID;
                        //        model.ModelBUParameterID = userModel.ModelBUParameterID;
                        //    }

                        //}

                    }



                    ModelParametermodel Modelparametersmodel = new ModelParametermodel();

                    IList<ModelBUParametermodel> ModelBUparametersmodels = new List<ModelBUParametermodel>();

                 //   ModelBUParametermodel singleBuModel = new ModelBUParametermodel();
                    ModelParameterInputmodel result = new ModelParameterInputmodel();

                    if (defaultModelParameter != null)
                    {
                        Modelparametersmodel.Max_Sellout_HighDecline_Max_MDF =  modelParameterTable.Max_Sellout_HighDecline_Max_MDF = defaultModelParameter.Max_Sellout_HighDecline_Max_MDF;
                        Modelparametersmodel.Max_Sellout_HighDecline_Min_MDF=  modelParameterTable.Max_Sellout_HighDecline_Min_MDF = defaultModelParameter.Max_Sellout_HighDecline_Min_MDF;
                        Modelparametersmodel.Max_Sellout_ModerateDecline_Max_MDF = modelParameterTable.Max_Sellout_ModerateDecline_Max_MDF = defaultModelParameter.Max_Sellout_ModerateDecline_Max_MDF;
                        Modelparametersmodel.Max_Sellout_ModerateDecline_Min_MDF = modelParameterTable.Max_Sellout_ModerateDecline_Min_MDF = defaultModelParameter.Max_Sellout_ModerateDecline_Min_MDF;
                        Modelparametersmodel.Max_Sellout_Steady_Max_MDF = modelParameterTable.Max_Sellout_Steady_Max_MDF = defaultModelParameter.Max_Sellout_Steady_Max_MDF;
                        Modelparametersmodel.Max_Sellout_Steady_Min_MDF = modelParameterTable.Max_Sellout_Steady_Min_MDF = defaultModelParameter.Max_Sellout_Steady_Min_MDF;
                        Modelparametersmodel.Max_Sellout_ModerateGrowth_Max_MDF = modelParameterTable.Max_Sellout_ModerateGrowth_Max_MDF = defaultModelParameter.Max_Sellout_ModerateGrowth_Max_MDF;
                        Modelparametersmodel.Max_Sellout_ModerateGrowth_Min_MDF = modelParameterTable.Max_Sellout_ModerateGrowth_Min_MDF = defaultModelParameter.Max_Sellout_ModerateGrowth_Min_MDF;
                        Modelparametersmodel.Max_Sellout_HighGrowth_Max = modelParameterTable.Max_Sellout_HighGrowth_Max = defaultModelParameter.Max_Sellout_HighGrowth_Max;
                        Modelparametersmodel.Max_Sellout_HighGrowth_Min = modelParameterTable.Max_Sellout_HighGrowth_Min = defaultModelParameter.Max_Sellout_HighGrowth_Min;
                        Modelparametersmodel.Target_accomplish_HighPrecision_Score = modelParameterTable.Target_accomplish_HighPrecision_Score = defaultModelParameter.Target_accomplish_HighPrecision_Score;
                        Modelparametersmodel.Target_accomplish_MediumPrecision_Score = modelParameterTable.Target_accomplish_MediumPrecision_Score = defaultModelParameter.Target_accomplish_MediumPrecision_Score;
                        Modelparametersmodel.Max_Target_Accomplish_percentage = modelParameterTable.Max_Target_Accomplish_percentage = defaultModelParameter.Max_Target_Accomplish_percentage;
                        Modelparametersmodel.Min_Target_Accomplish_percentage = modelParameterTable.Min_Target_Accomplish_percentage = defaultModelParameter.Min_Target_Accomplish_percentage;
                        Modelparametersmodel.JPB_Max = modelParameterTable.JPB_Max = defaultModelParameter.JPB_Max;
                        Modelparametersmodel.JPB_Min = modelParameterTable.JPB_Min = defaultModelParameter.JPB_Min;
                        Modelparametersmodel.Prediction_High_Max = modelParameterTable.Prediction_High_Max = defaultModelParameter.Prediction_High_Max;
                        Modelparametersmodel.Prediction_High_Min = modelParameterTable.Prediction_High_Min = defaultModelParameter.Prediction_High_Min;
                        Modelparametersmodel.Prediction_Low_Max = modelParameterTable.Prediction_Low_Max = defaultModelParameter.Prediction_Low_Max;
                        Modelparametersmodel.Prediction_Low_Min = modelParameterTable.Prediction_Low_Min = defaultModelParameter.Prediction_Low_Min;
                        Modelparametersmodel.Weights_applied_t_1H = modelParameterTable.Weights_applied_t_1H = defaultModelParameter.Weights_applied_t_1H;
                        Modelparametersmodel.Weights_applied_t_2H = modelParameterTable.Weights_applied_t_2H = defaultModelParameter.Weights_applied_t_2H;
                        Modelparametersmodel.Weights_applied_t_3H = modelParameterTable.Weights_applied_t_3H = defaultModelParameter.Weights_applied_t_3H;
                        Modelparametersmodel.Min_Target_Productivity = modelParameterTable.Min_Target_Productivity = defaultModelParameter.Min_Target_Productivity;
                        Modelparametersmodel.Last_Quarter_Sellout_Scale_Factor = modelParameterTable.Last_Quarter_Sellout_Scale_Factor = defaultModelParameter.Last_Quarter_Sellout_Scale_Factor;
                        Modelparametersmodel.partner_size_threshold = modelParameterTable.partner_size_threshold = defaultModelParameter.partner_size_threshold;
                        Modelparametersmodel.WMGO_Perf_Acc_VeryHigh = modelParameterTable.WMGO_Perf_Acc_VeryHigh = defaultModelParameter.WMGO_Perf_Acc_VeryHigh;
                        Modelparametersmodel.WMGO_Perf_Acc_High = modelParameterTable.WMGO_Perf_Acc_High = defaultModelParameter.WMGO_Perf_Acc_High;
                        Modelparametersmodel.WMGO_Perf_Acc_VeryLow = modelParameterTable.WMGO_Perf_Acc_VeryLow = defaultModelParameter.WMGO_Perf_Acc_VeryLow;
                        Modelparametersmodel.WMGO_Perf_Acc_Low = modelParameterTable.WMGO_Perf_Acc_Low = defaultModelParameter.WMGO_Perf_Acc_Low;
                        Modelparametersmodel.WMGO_Perf_Acc_Medium = modelParameterTable.WMGO_Perf_Acc_Medium = defaultModelParameter.WMGO_Perf_Acc_Medium;
                        modelParameterTable.VersionID = VersionID;
                        Modelparametersmodel.VersionID = VersionID;
                        Modelparametersmodel.ModifiedBy = UserID;
                        Modelparametersmodel.ModifiedDate = createdDate;

                        db.Set<ModelParameterTable>().AddOrUpdate(modelParameterTable);
                        db.SaveChanges();
                    }


                    foreach(var item in modelDefaultBUTable)
                    {
                         foreach (var item1 in modelBUTable)
                        {
                            if(item1.BusinessUnitID == item.BusinessUnitID)
                            {
                                ModelBUParameterTable modl = new ModelBUParameterTable();
                                ModelBUParametermodel modelBU = new ModelBUParametermodel();

                                modelBU.ModelBUParameterID = modl.ModelBUParameterID = item1.ModelBUParameterID;
                                modelBU.ModelParameterID = modl.ModelParameterID = item1.ModelParameterID;
                                modelBU.BusinessUnitID = modl.BusinessUnitID = item1.BusinessUnitID = item.BusinessUnitID;
                                modelBU.High_Performance = modl.High_Performance = item1.High_Performance = item.High_Performance;
                                modelBU.Min_Partner_Investment = modl.Min_Partner_Investment= item1.Min_Partner_Investment = item.Min_Partner_Investment;
                                modelBU.New_Partner_RampUp_Scale = modl.New_Partner_RampUp_Scale = item1.New_Partner_RampUp_Scale = item.New_Partner_RampUp_Scale;
                                modelBU.Preferred_Partner_Cut_Off_Percentage = modl.Preferred_Partner_Cut_Off_Percentage = item1.Preferred_Partner_Cut_Off_Percentage = item.Preferred_Partner_Cut_Off_Percentage;
                                modelBU.Dist_cust_membership_weight_Silver_and_Below = modl.Dist_cust_membership_weight_Silver_and_Below = item1.Dist_cust_membership_weight_Silver_and_Below = item.Dist_cust_membership_weight_Silver_and_Below;
                                modelBU.Dist_cust_membership_weight_Platinum_and_Gold = modl.Dist_cust_membership_weight_Platinum_and_Gold = item1.Dist_cust_membership_weight_Platinum_and_Gold = item.Dist_cust_membership_weight_Platinum_and_Gold;
                                modelBU.Medium_Performance = modl.Medium_Performance = item1.Medium_Performance = medium * item.High_Performance;
                                modelBU.Low_Performance = modl.Low_Performance = item1.Low_Performance = lower * item.High_Performance;
                                modelBU.Growth_Revenue = modl.Growth_Revenue = item1.Growth_Revenue = item.Growth_Revenue;
                                //modelBU.Sustain_Revenue = modl.Sustain_Revenue = item1.Sustain_Revenue = item.Sustain_Revenue;
                                modl.ModifiedDate = createdDate;
                                modl.ModifiedBy = UserID;
                                modelBU.ModifiedBy = UserID;
                                modelBU.ModifiedDate = createdDate;
                                if(modelBU.ModelParameterID != 1)
                                {
                                    ModelBUparametersmodels.Add(modelBU);
                                    db.Set<ModelBUParameterTable>().AddOrUpdate(modl);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    MPOWRLogManager.LogMessage("Cannot Reset to Default");
                                    MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + "Cannot Reset to Default");
                                    return null; 
                                }
                               
                             
                            }
                        }

                    }


                    //List<dynamic> resultt = new List<dynamic>();
                    //resultt.Add(modelParameterTable);
                    //resultt.Add(modelBUTable);


                    result.Modelparameters = Modelparametersmodel;
                    result.ModelBUparameters = ModelBUparametersmodels;

                    var MDFPlanning = db.MDFPlannings.Find(VersionID);
                    if (MDFPlanning != null)
                    {
                        MDFPlanning.ModifiedBy = UserID;
                        MDFPlanning.ModifiedDate = createdDate;
                        db.Set<MDFPlanning>().AddOrUpdate(MDFPlanning);
                    }
                    db.SaveChanges();

                    return result;


                }

                catch (Exception ex)
                {

                    MPOWRLogManager.LogMessage(ex.Message.ToString());
                    MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                    return null;
                }

            }

        }


        /// <summary>
        /// To check the default data 
        /// </summary>
        /// <param name="CountryID"></param>
        /// <param name="PartnertypeId"></param>
        /// <param name="DistrictID"></param>
        /// <param name="FinancialYearID"></param>
        /// <param name="VersionID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ModelParameter/DefaultCheckData")]
        public bool DefaultCheckData(int VersionID,string UserID)
        {
            using (MPOWREntities db = new MPOWREntities())
            {
                try
                {
                    var userBuList = (from bu in  db.UserBusinessUnits  
                                      join urut in db.UserRoleUserTypes on bu.UserRoleUserTypeID equals urut.UserRoleUserTypeID
                                      join u in db.Users on urut.UserID equals u.UserID
                                      where u.UserID == UserID
                                      select bu.BusinessUnitID).Distinct().ToList();
                    
                 

                    var Result = db.PartnerBudgets.Count();
                    
                    var PartnerBudgetIDs = (from pb in db.PartnerBudgets
                                            join pbub in db.PartnerBUBudgets on pb.PartnerBudgetID equals pbub.PartnerBudgetID
                                            where pb.ModelParameterID != 1 && pb.VersionID == VersionID
                                            && userBuList.Contains(pbub.BusinessUnitID)
                                            select pb.PartnerBudgetID
                                   ).ToList();

                    Result = db.PartnerBudgets.Count(e => e.ModelParameterID != 1 && e.VersionID == VersionID && PartnerBudgetIDs.Contains(e.PartnerBudgetID));
                    
                    if (Result > 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }

                catch (Exception ex)
                {

                    MPOWRLogManager.LogMessage(ex.Message.ToString());
                    MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                    return false;
                }


            }
        }


    }
}