using MPOWR.Dal.Models;
using MPOWR.Api.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MPOWR.Core;
using MPOWR.Api.App_Start;

namespace MPOWR.Api.Controllers
{
  [AuthorizeUser]
    public class SetModelParametersController : ApiController
    {
        private MPOWREntities db = new MPOWREntities();
        RegionDetails Rdetails = new RegionDetails();
        FinancialYearDetails Fdetails = new FinancialYearDetails();
        //to get regions and fyear
        public dynamic GetRegionsandYears()
        {
            try
            {
                var Result = string.Empty;
                DateTime date = DateTime.Now;
                int year = date.Year;
                int month = date.Month;
                string period;
                if (month < 6)
                {
                    period = "1H";
                }
                else
                    period = "2H";
                year = year % 1000;
                string FinancialYearPeriod = "FY" + year.ToString() + " " + period;


                DbCommand command = db.Database.Connection.CreateCommand();
                command.CommandText = "usp_GetRegionandyearsDetails";
                command.Parameters.Add(new SqlParameter("@FinacialYearPeriod", FinancialYearPeriod));
                command.CommandType = System.Data.CommandType.StoredProcedure;
                db.Database.Connection.Open();
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (Result == string.Empty)
                                Result = reader.GetValue(i).ToString();
                            else
                                Result += reader.GetValue(i).ToString();
                        }


                    }
                }
                db.Database.Connection.Close();
                object json = JToken.Parse(Result);
                return Ok(json);

            }
            catch (MPOWRException ex)
            {
                db.Database.Connection.Close();
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

                return null;

            }
            catch (Exception ex)
            {
                db.Database.Connection.Close();
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }
        //to get expected productivity
        [HttpPost]
        [Route("api/SetModelParameters/GetExpectedProductivity")]
        public dynamic GetExpectedProductivity(SetModelParametersViewModel request)
        {
            try
            {
                var Result = string.Empty;

                DbCommand command = db.Database.Connection.CreateCommand();
                command.CommandText = "usp_GetSetModelParameters";
                command.Parameters.Add(new SqlParameter("@Fy_Id", request.FinancialYearId));
                command.Parameters.Add(new SqlParameter("@Region_Id", request.RegionId));

                command.CommandType = System.Data.CommandType.StoredProcedure;

                db.Database.Connection.Open();
                var reader = command.ExecuteReader();
                
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (Result == string.Empty)
                            Result = reader.GetValue(0).ToString();
                        else
                            Result += reader.GetValue(0).ToString();
                    }
                    object json = JToken.Parse(Result);
                    return Ok(json);
                }
                else
                {
                    return null;
                }
                db.Database.Connection.Close();
                // return Ok(Result);
                
            }
            catch (MPOWRException ex)
            {
                db.Database.Connection.Close();
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

                return null;

            }
            catch (Exception ex)
            {
                db.Database.Connection.Close();
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }

        }
        //to copy prevous model
        [HttpPost]
        [Route("api/SetModelParameters/GetPriviousExpectedProductivity")]
        public dynamic GetPriviousExpectedProductivity(SetModelParametersViewModel request)
        {
            var FinancialYearID = (from dat in db.FinancialYears
                                   where dat.ShortName == request.FinancialYearDetails && dat.IsActive == true
                                   select new
                                   {

                                       FinancialYearId = dat.FinancialYearID
                                   }).ToList();

            foreach (var DATA in FinancialYearID)
            {
                if (DATA.FinancialYearId != 0)
                {
                    Fdetails.FinancialYearID = DATA.FinancialYearId;
                   
                }

            }      // Rdetails.RegionId
            //var RegionId = (from dat in db.Regions
            //                where dat.ShortName == request.RegionDetails
            //                select new
            //                {

            //                    RegionId = dat.RegionID
            //                }).ToList();
            //foreach (var DATA in RegionId)
            //{
            //    if (DATA.RegionId != 0)
            //    {
            //        Rdetails.RegionId = DATA.RegionId;
            //    }

            //}

            try
            {
                var Result = string.Empty;

                DbCommand command = db.Database.Connection.CreateCommand();
                command.CommandText = "usp_GetSetModelParameters";
                command.Parameters.Add(new SqlParameter("@Fy_Id", Fdetails.FinancialYearID));
                command.Parameters.Add(new SqlParameter("@Region_Id", Rdetails.RegionId));

                command.CommandType = System.Data.CommandType.StoredProcedure;

                db.Database.Connection.Open();
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (Result == string.Empty)
                            Result = reader.GetValue(0).ToString();
                        else
                            Result += reader.GetValue(0).ToString();
                    }
                }
                db.Database.Connection.Close();
                // return Ok(Result);
                object json = JToken.Parse(Result);
                return Ok(json);
            }
            catch (MPOWRException ex)
            {
                db.Database.Connection.Close();
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

                return null;

            }
            catch (Exception ex)
            {
                db.Database.Connection.Close();
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }
        }

        //to insert SetModelDetails
        [HttpPost]
        [Route("api/SetModelParameters/SetModelDetails")]
        public IHttpActionResult SetModelDetails(SetModelParametersViewModel inputdata)
        {
            SetModelParametersViewModel modelinsertdata = new SetModelParametersViewModel();
                                
                var ModelParameterFYConfigID = (from dat in db.ModelParameterFYConfigs
                                           where dat.FinancialYearID == inputdata.FinancialYearId
                                           && dat.RegionID == inputdata.RegionId
                                           select new
                                           {
                                               ModelParameterFYConfigID = dat.ModelParameterFYConfigID
                                           }).ToList();

                foreach (var DATA in ModelParameterFYConfigID)
                {
                    if (DATA.ModelParameterFYConfigID != 0)
                    {
                    modelinsertdata.ModelParameterFYConfigID = DATA.ModelParameterFYConfigID;
                    }

                }
            try
            {
                var Result = string.Empty;
                DbCommand command = db.Database.Connection.CreateCommand();
                command.CommandText = "usp_InsertSetmodelparameters";
                command.Parameters.Add(new SqlParameter("@FinancialYearId", Convert.ToInt32(inputdata.FinancialYearId)));
                command.Parameters.Add(new SqlParameter("@RegionId", Convert.ToInt32(inputdata.RegionId)));
                command.Parameters.Add(new SqlParameter("@ModelParameterFYConfigID", Convert.ToInt32(modelinsertdata.ModelParameterFYConfigID)));
                command.Parameters.Add(new SqlParameter("@ModelParameterConfigID", Convert.ToInt32(inputdata.ModelParameterConfigID)));
                command.Parameters.Add(new SqlParameter("@HighPerformerProductivity", Convert.ToDecimal(inputdata.HighPerformerProductivity)));
                command.Parameters.Add(new SqlParameter("@MediumPerformerProductivityRatio", Convert.ToDecimal(inputdata.MediumPerformerProductivityRatio)));
                command.Parameters.Add(new SqlParameter("@LowPerformerProductivityRatio", Convert.ToDecimal(inputdata.LowPerformerProductivityRatio)));
                command.Parameters.Add(new SqlParameter("@UserId", inputdata.UserId));
                command.CommandType = System.Data.CommandType.StoredProcedure;

                db.Database.Connection.Open();
                command.ExecuteNonQuery();

                db.Database.Connection.Close();
                return Ok("Record Saved");
            }
            catch (MPOWRException ex)
            {
                db.Database.Connection.Close();
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());

                return null;

            }
            catch (Exception ex)
            {
                db.Database.Connection.Close();
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }

        }


        }

    }

