using ExcelDataReader;
using MPOWR.Api.App_Start;
using MPOWR.Bal;
using MPOWR.Core;
using MPOWR.Dal.Models;
using MPOWR.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace MPOWR.Api.Controllers
{
    [AuthorizeGlossaryUser]
    public class GlossaryController : ApiController
    {
        [HttpGet]
        [Route("api/Glossary/GetGlossaryDetails")]
        public IHttpActionResult GetGlossaryDetails()
        {
            GlossaryBL bl = new GlossaryBL();

            try
            {
                var Result = bl.GetGlossaryDetails();
                return Ok(Result);
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

        [HttpGet]
        [Route("api/Glossary/GetGlossaryEditDetails")]
        public IHttpActionResult GetGlossaryEditDetails()
        {
            GlossaryBL bl = new GlossaryBL();

            try
            {
                var Result = bl.GetGlossaryEditDetails();
                return Ok(Result);
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

        [HttpPost]
        [Route("api/Glossary/SaveGlossaryParameter")]
        public IHttpActionResult SaveGlossaryParameter(ParameterEditModel data, string user)
        {
            GlossaryBL bl = new GlossaryBL();

            try
            {
                var result = bl.SaveGlossaryParameter(data, user);
                return Ok(result);
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

        [HttpPost]
        [Route("api/Glossary/SaveGlossaryScreen")]
        public IHttpActionResult SaveGlossaryScreen(GlossaryEditModel data, string user)
        {
            GlossaryBL bl = new GlossaryBL();

            try
            {
                var result = bl.SaveGlossaryScreen(data, user);
                return Ok(result);
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

        [HttpPost]
        [Route("api/Glossary/ApproveGlossary")]
        public IHttpActionResult ApproveGlossary(List<GlossaryEditModel> data, string user)
        {
            GlossaryBL bl = new GlossaryBL();

            try
            {
                var result = bl.ApproveGlossary(data, user);
                return Ok(result);
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
        [HttpGet]
        [Route("api/Glossary/InsertGlossaryData")]
        public IHttpActionResult InsertGlossaryData()
        {
            try
            {
                GlossaryBL bl = new GlossaryBL();
                MPOWREntities db = new MPOWREntities();
                DataSet DtExcel = new DataSet();
                string fileName = ConfigurationManager.AppSettings["Glossary"];
                FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
                var excelReader = ExcelDataReader.ExcelReaderFactory.CreateOpenXmlReader(stream);
                //DtExcel = excelReader.AsDataSet();
                DtExcel = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });
                excelReader.Close();  DateTime currentTime = CommonFunction.GetCurrentTime;
                List<GlossaryScreenParameterDetail> List = new List<GlossaryScreenParameterDetail>();
                var prescreenid = 2;
                if (DtExcel != null && DtExcel.Tables.Count > 0)
                {
                    var count = 0;
                    foreach (DataRow row in DtExcel.Tables[0].Rows)
                    {
                        
                        var PageName = row[0].ToString();
                        var parameterData = new Dal.Models.GlossaryScreenParameterDetail();
                        parameterData.ScreenID = db.GlossaryScreenDetails.Where(x => x.ScreenName == PageName).Select(x => x.ID).FirstOrDefault();
                        parameterData.ParameterName = row[1].ToString();
                        parameterData.CreatedBy = "System Admin";
                        parameterData.CreatedDate = currentTime;
                        parameterData.RefinedDescription= Regex.Replace(row[2].ToString(), @"Formula", "</br><span style='color: #00b388;'>Formula</span></br>"); 
                        parameterData.Description = Regex.Replace(row[2].ToString(), @"Formula", "</br><span style='color: #00b388;'>Formula</span></br>"); 
                        parameterData.ModifiedBy = "System Admin";
                        parameterData.ModifiedDate = currentTime;
                        parameterData.RefinedParameter= row[1].ToString(); 
                        count = prescreenid == parameterData.ScreenID ? count+1 : 1;
                        parameterData.DisplayOrder = count;
                        parameterData.IsActive = true;
                        parameterData.IsDeleted = false;
                        prescreenid = parameterData.ScreenID;
                        List.Add(parameterData);
                    }

                }
                bl.InsertGlossaryScreenParams(List);
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                return null;
            }


            return Ok();
        }



        [HttpGet]
        [Route("api/Glossary/GetGlossaryConfiguration")]
        public IHttpActionResult GetGlossaryConfiguration()
        {
            GlossaryBL bl = new GlossaryBL();

            try
            {
                var Result = bl.GetGlossaryConfiguration();
                return Ok(Result);
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
}