using ExcelDataReader;
using MPOWR.Bal;
using MPOWR.Core;
using MPOWR.Dal.Models;
using MPOWR.Model;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using MPOWR.Bal;

namespace MPOWR.Api.Controllers
{
    //[AuthorizeUser]
    public class ExportController : ApiController
    {
        MPOWREntities objDBEntity = new MPOWREntities();
        ExportBL exportBl = new ExportBL();
        dynamic ExcelExportRecord;
        private MPOWREntities db = new MPOWREntities();
        [HttpGet]
        [Route("api/Export/ExporttoExcelPartnerBudget")]
        public HttpResponseMessage ExporttoExcelPartnerBudget(int VersionID  , string bulist)
        {
            CommonBL CommonBL = new CommonBL();
            ExcelExportRecord = CommonBL.GetAppConfigValue(MPOWRConstants.ExcelExportRecordLimit);
            int RecordLimit = Convert.ToInt32(ExcelExportRecord);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var MDFData = (from mdfplanning in objDBEntity.MDFPlannings.AsNoTracking()
                           where mdfplanning.ID == VersionID
                           select new
                           {
                               FinancialYearID = mdfplanning.FinancialYearID,
                               parnerTypeId = mdfplanning.PartnerTypeID,
                               allocationLevel = mdfplanning.AllocationLevel,
                               CountryOrGeoOrDistrict = mdfplanning.CountryOrGeoOrDistrict
                           }).FirstOrDefault();


            string fileName = exportBl.GetFileName(VersionID, "PB_");

            var financialYearData = objDBEntity.FinancialYears.AsNoTracking().AsQueryable();
                var q_year = (from financial in financialYearData
                              where financial.FinancialYearID == (MDFData.FinancialYearID - 1)
                        select financial.ShortName.Substring(5, 2) + financial.ShortName.Substring(2, 2)).FirstOrDefault();
            var q_prev_year = (from financial in financialYearData
                               where financial.FinancialYearID == (MDFData.FinancialYearID - 2)
                             select  financial.ShortName.Substring(5, 2) + financial.ShortName.Substring(2, 2)).FirstOrDefault();
            var q_current_year = (from financial in financialYearData
                                  where financial.FinancialYearID == (MDFData.FinancialYearID)
                             select financial.ShortName.Substring(5, 2) + financial.ShortName.Substring(2, 2)).FirstOrDefault();
            var q_prev_year_first_half = (from financial in financialYearData
                                          where financial.FinancialYearID == (MDFData.FinancialYearID - 3)
                               select financial.ShortName.Substring(5, 2) + financial.ShortName.Substring(2, 2)).FirstOrDefault();
            var q_prev_toPevyear_sec_half = (from financial in financialYearData
                                           where financial.FinancialYearID == (MDFData.FinancialYearID - 4)
                                           select financial.ShortName.Substring(5, 2) + financial.ShortName.Substring(2, 2)).FirstOrDefault();
            var sow = (q_current_year.Substring(0, 2) == "1H") ? ("4Q" + (int.Parse(q_current_year.Substring(2, 2)) - 1).ToString()) : ("2Q" + (q_current_year.Substring(2, 2)));
             try
            {
                var IscanaraOrUSA = ((MDFData.allocationLevel == "C" && MDFData.CountryOrGeoOrDistrict.IndexOf(MPOWRConstants.CanadaCountryId) != -1) ||
                     (MDFData.allocationLevel == "C" && MDFData.CountryOrGeoOrDistrict.IndexOf(MPOWRConstants.UnitedStatesCountryId) != -1) || MDFData.allocationLevel == "D");
                var dtResult = exportBl.GetExportData(VersionID, MDFData.parnerTypeId , q_prev_year , q_current_year ,MDFData.allocationLevel, MDFData.CountryOrGeoOrDistrict , bulist) ;

                if (dtResult.Rows.Count > RecordLimit)
                {
                    StringBuilder sb = new StringBuilder();
                    StringBuilder colList = new StringBuilder();
                    for (int i = 0; i < dtResult.Columns.Count; i++)
                    {
                        //sb.Append(dtResult.Columns[i]);
                        //if (i < dtResult.Columns.Count - 1)
                        //    sb.Append(',');
                        colList.Append( dtResult.Columns[i] + ",");
                    }
                    sb.AppendLine(colList.ToString());
                    foreach (DataRow row in dtResult.Rows)
                    {
                        IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                        sb.AppendLine(string.Join(",", fields));
                    }
                    // File.WriteAllText("test.csv", sb.ToString());

                    var myByteArray = Encoding.UTF8.GetBytes(sb.ToString());
                    result.Content = new ByteArrayContent(myByteArray);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = fileName.Replace(".xlsx" , ".csv");// Path.GetFileName(path);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    result.Content.Headers.ContentLength = myByteArray.Length;
                    return result;
                }

                using (ExcelPackage app = new ExcelPackage())
                {
                    ExcelWorkbook workBook = app.Workbook;
                    ExcelWorksheet worksheet = workBook.Worksheets.Add("Data");
                    ExcelWorksheet allocatedBudgetWS = workBook.Worksheets.Add("Summary");
                   
                    if (MDFData.parnerTypeId == 2) // reseller
                    {
                        worksheet.Cells[1, 1, 1, 46].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[1, 1, 1, 46].Style.Font.Color.SetColor(Color.White);
                        worksheet.Cells[1, 1, 1, 46].Style.Font.Bold = true;

                        worksheet.Cells[1, 5, 1, 11].Merge = true;
                        worksheet.Cells[1, 5, 1, 11].Value = "Sellout";

                        worksheet.Cells[1, 12, 1, 15].Merge = true;
                        worksheet.Cells[1, 12, 1, 15].Value = "MDF";
                        worksheet.Cells[1, 12, 1, 15].Style.Font.Color.SetColor(Color.Black);
 
                        worksheet.Cells[1, 16, 1, 17].Merge = true;
                        worksheet.Cells[1, 16, 1, 17].Value = "Other";

                        worksheet.Cells[1, 18, 1, 26].Merge = true;
                        worksheet.Cells[1, 18, 1, 26].Value = "MDF";
                        worksheet.Cells[1, 18, 1, 26].Style.Font.Color.SetColor(Color.Black);

                        worksheet.Cells[1, 27, 1, 29].Merge = true;
                        worksheet.Cells[1, 27, 1, 29].Value = "MDF/Sellout";
                        worksheet.Cells[1, 27, 1, 29].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, 27, 1, 29].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(169, 208, 142));

                        worksheet.Cells[1, 30, 1, 46].Merge = true;
                        worksheet.Cells[1, 30, 1, 46].Value = "Other";

                        //color coding for header
                        worksheet.Cells[1, 1, 1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, 5, 1, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, 12, 1, 15].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, 16, 1, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, 18, 1, 26].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, 27, 1, 29].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, 30, 1, 46].Style.Fill.PatternType = ExcelFillStyle.Solid;

                        worksheet.Cells[1, 1, 1, 4].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(56, 181, 138));
                        worksheet.Cells[1, 5, 1, 11].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(169, 208, 142));
                        worksheet.Cells[1, 30, 1, 46].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(24, 180, 136));
                        worksheet.Cells[1, 27, 1, 29].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(24, 180, 136));
                        worksheet.Cells[1, 12, 1, 15].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(24, 180, 136));
                        worksheet.Cells[1, 16, 1, 17].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(24, 180, 136));
                        worksheet.Cells[1, 18, 1, 26].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(24, 180, 136));
                        //if(dt.Rows.Count>0)
                        worksheet.Cells[2, 1].LoadFromDataTable(dtResult,true);
                        //if (Result.Count > 0)
                        //    worksheet.Cells[3, 1].LoadFromCollection(Result).Skip(1);
                        worksheet.Cells.AutoFitColumns();
                        worksheet.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;


                        //worksheet.Cells["A2:AG2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //worksheet.Cells["A2:AG2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(135, 229, 224));

                        worksheet.Cells["A2:AT2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["A2:AT2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(135, 229, 224));

                        worksheet.Column(16).Style.Fill.PatternType = ExcelFillStyle.Solid; // highlighting the columns which are editanle by user
                        worksheet.Column(17).Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Column(20).Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Column(16).Style.Font.Color.SetColor(Color.Black);
                        worksheet.Column(17).Style.Font.Color.SetColor(Color.Black);
                        worksheet.Column(20).Style.Font.Color.SetColor(Color.Black);
                      
                        worksheet.Column(16).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                        worksheet.Column(17).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                        worksheet.Column(20).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));

                        if (!IscanaraOrUSA)
                        {
                            worksheet.Column(22).Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Column(22).Style.Font.Color.SetColor(Color.Black);
                            worksheet.Column(22).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));

                            worksheet.Column(23).Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Column(23).Style.Font.Color.SetColor(Color.Black);
                            worksheet.Column(23).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));

                           
                        }
                        else
                        {
                            worksheet.Column(13).Hidden = true;
                            worksheet.DeleteColumn(13);
                        }
                        
                        AddDataValidation(ref worksheet, "P", "Q", "T", "V","W" ,"AK" , "AM" , "AO");
                       
                    }
                    else
                    {

                        worksheet.Cells[1, 1, 1, 48].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[1, 1, 1, 48].Style.Font.Color.SetColor(Color.White);
                        worksheet.Cells[1, 1, 1, 48].Style.Font.Bold = true;

                        worksheet.Cells[1, 5, 1, 14].Merge = true;
                        worksheet.Cells[1, 5, 1, 14].Value = "Sellout";

                        worksheet.Cells[1, 15, 1, 18].Merge = true;
                        worksheet.Cells[1, 15, 1, 18].Value = "MDF";
                        worksheet.Cells[1, 15, 1, 18].Style.Font.Color.SetColor(Color.Black);

                        worksheet.Cells[1, 19, 1, 20].Merge = true;
                        worksheet.Cells[1, 19, 1, 20].Value = "Other";

                        worksheet.Cells[1, 21, 1, 26].Merge = true;
                        worksheet.Cells[1, 21, 1, 26].Value = "MDF";
                        worksheet.Cells[1, 21, 1, 26].Style.Font.Color.SetColor(Color.Black);



                        worksheet.Cells[1, 27, 1, 28].Merge = true;
                        worksheet.Cells[1, 27, 1, 28].Value = "MDF/Sellout";
                        worksheet.Cells[1, 27, 1, 28].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, 27, 1, 28].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(169, 208, 142));

                        worksheet.Cells[1, 29, 1, 48].Merge = true;
                        worksheet.Cells[1, 29, 1, 48].Value = "Other";

                        //color coding for header
                        worksheet.Cells[1, 1, 1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, 5, 1, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, 15, 1, 27].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, 28, 1, 48].Style.Fill.PatternType = ExcelFillStyle.Solid;

                        worksheet.Cells[1, 1, 1, 4].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(56, 181, 138));
                        worksheet.Cells[1, 5, 1, 14].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(169, 208, 142));
                        worksheet.Cells[1, 15, 1, 27].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(24, 180, 136));
                        worksheet.Cells[1, 28, 1, 48].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(24, 180, 136));
                        worksheet.Cells[2, 1].LoadFromDataTable(dtResult, true);

                        worksheet.Cells.AutoFitColumns();
                        worksheet.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        //worksheet.Cells["A2:AG2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //worksheet.Cells["A2:AG2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(135, 229, 224));
                        
                        worksheet.Cells["A2:AV2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells["A2:AV2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(135, 229, 224));

                        worksheet.Column(23).Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Column(20).Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Column(19).Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Column(23).Style.Font.Color.SetColor(Color.Black);
                        worksheet.Column(20).Style.Font.Color.SetColor(Color.Black);
                        worksheet.Column(19).Style.Font.Color.SetColor(Color.Black);
                        worksheet.Column(19).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                        worksheet.Column(20).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                        worksheet.Column(23).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));

                        if (!IscanaraOrUSA)
                        {
                            worksheet.Column(25).Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Column(25).Style.Font.Color.SetColor(Color.Black);
                            worksheet.Column(25).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));

                            worksheet.Column(26).Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Column(26).Style.Font.Color.SetColor(Color.Black);
                            worksheet.Column(26).Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 0));
                        }
                        else
                        {
                            worksheet.Column(16).Hidden = true;
                            worksheet.DeleteColumn(16);
                        }
                        AddDataValidation(ref worksheet, "T", "S","Y", "W", "Z", "AN", "AP", "AQ");
                     }
                    var summaryData = exportBl.GetSummaryData( 0, VersionID);
                    objDBEntity.Database.CommandTimeout = 1000000;
                    allocatedBudgetWS.Cells[1, 1, 1,4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    allocatedBudgetWS.Cells[1, 1, 1, 4].Style.Font.Color.SetColor(Color.White);
                    allocatedBudgetWS.Cells[1, 1, 1, 4].Style.Font.Bold = true;
                    allocatedBudgetWS.Cells[2, 1].Value = "Business Unit";
                    allocatedBudgetWS.Cells[2, 2].Value = "Allocated MDF";
                    allocatedBudgetWS.Cells[2, 3].Value = "Remaining MDF";
                    allocatedBudgetWS.Cells[2, 4].Value = "Total MDF";
                    if (summaryData.Count > 0)
                        allocatedBudgetWS.Cells[3, 1].LoadFromCollection(summaryData).Skip(1);
                    allocatedBudgetWS.Cells.AutoFitColumns();
                    allocatedBudgetWS.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    allocatedBudgetWS.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    allocatedBudgetWS.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    allocatedBudgetWS.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    allocatedBudgetWS.Cells["A2:D2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    allocatedBudgetWS.Cells[1, 1, 1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    allocatedBudgetWS.Cells[1, 1, 1, 4].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(56, 181, 138));
                    allocatedBudgetWS.Cells["A2:D2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    allocatedBudgetWS.Cells["A2:D2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(135, 229, 224));

                   
                    var memoryStream = app.GetAsByteArray();
                    app.Dispose();
                    result.Content = new ByteArrayContent(memoryStream);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = fileName;// Path.GetFileName(path);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    result.Content.Headers.ContentLength = memoryStream.Length;
                    return result;
                }

            }

            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
            }

            return result;
        }

        [HttpGet]
        [Route("api/Export/ExporttoExcelSDFC")]
        public HttpResponseMessage ExporttoExcelSFDC(int VersionID)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            ExportBL bl = new ExportBL();
            List<Export.Export_SDFC> listItems = new List<Export.Export_SDFC>();

            try
            {
                string fileName = bl.GetFileName(VersionID , "SFDC_");
                listItems = bl.GetExportExcelSFDCData( VersionID);
                objDBEntity.Database.CommandTimeout = 1000000;
                using (ExcelPackage app = new ExcelPackage())
                {
                    ExcelWorkbook workBook = app.Workbook;
                    ExcelWorksheet worksheet = workBook.Worksheets.Add("SDFC");
                    worksheet.Cells[1, 1].Value = "Fund ID";
                    worksheet.Cells[1, 2].Value = "Budget ID";
                    worksheet.Cells[1, 3].Value = "Budget Name";
                    worksheet.Cells[1, 4].Value = "Description";
                    worksheet.Cells[1, 5].Value = "IsPanHPE Fund";
                    worksheet.Cells[1, 6].Value = "Partner Name";
                    worksheet.Cells[1, 7].Value = "Locator ID";
                    worksheet.Cells[1, 8].Value = "Business Relationship type";
                    worksheet.Cells[1, 9].Value = "Allocation Approver";
                    worksheet.Cells[1, 10].Value = "Upload Currency";
                    worksheet.Cells[1, 11].Value = "Allocation Amount";
                    worksheet.Cells[2, 11].Style.Numberformat.Format = "#,##0";
                    worksheet.Cells[1, 12].Value = "Is MSA?(Default No)";
                    worksheet.Cells[1, 13].Value = "Internal comments";
                    worksheet.Cells[1, 14].Value = "Business Relationship";
                    worksheet.Cells[1, 15].Value = "Is ARUBA MSA?(Default No)";
                    worksheet.Cells[1, 16].Value = "WMGO Ratio";


                    if (listItems.Count > 0)
                        worksheet.Cells[2, 1].LoadFromCollection(listItems).Skip(1);

                    worksheet.Cells.AutoFitColumns();
                    worksheet.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells["A1:P1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A1:P1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(135, 229, 224));

                    var memoryStream = app.GetAsByteArray();
                    app.Dispose();
                    var path = HttpContext.Current.Server.MapPath("~/Export_SFDC.xlsx");
                    result.Content = new ByteArrayContent(memoryStream);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = fileName;
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    result.Content.Headers.ContentLength = memoryStream.Length;
                    return result;
                }

            }

            catch (MPOWRException ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
            }
            catch (Exception ex)
            {
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
            }

            return result;
        }

        [HttpPost]
        [Route("api/Export/importMDF")]

        public IHttpActionResult importMDF(int versionId , string bulist)//int financialyearId, int versionId, int countryId, int partnerTypeId, int districtId)
        {

            var postedFile = HttpContext.Current.Request.Files[0];
            ImportBL bl = new ImportBL();
            if (postedFile.FileName.EndsWith(".csv"))
                return Ok(bl.parseCSV(HttpContext.Current.Request.Files[0].InputStream, versionId, bulist));
           
            var excelReader = ExcelReaderFactory.CreateOpenXmlReader(HttpContext.Current.Request.Files[0].InputStream);
            var ds2 = excelReader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true,
                    ReadHeaderRow = (rowReader) => {
                        rowReader.Read();
                    }
                }
            });
            excelReader.Close();
            return Ok(bl.Import(versionId, ds2.Tables[0] , bulist)); 
        }

        [HttpPost]
        [Route("api/Export/SaveImportData")]
        public IHttpActionResult SaveImportData( int  versionId , DataTable dt, string UserID)
        {
            ImportBL bl = new ImportBL();
            return Ok(bl.SaveImportData(dt, versionId, UserID));
        }


        public void AddDataValidation( ref ExcelWorksheet sheet , params string[] columns)
        {
            if (sheet.Dimension.Rows <= 2)
                return;
            for (var i = 0; i<4; i++)
            {
                var vlExcel = sheet.DataValidations.AddDecimalValidation(columns [i]+ "3:"+ columns[i] + sheet.Dimension.Rows);
                vlExcel.ShowErrorMessage = true;
                vlExcel.ErrorTitle = ImportTemplate.ExcelInvaliddata;
                vlExcel.Error = ImportTemplate.ExcelvalidateNo;
                vlExcel.Prompt = ImportTemplate.ExcelEnterNumber;
                vlExcel.AllowBlank = false;
                vlExcel.Formula.Value = 0;
                vlExcel.Formula2.Value = long.MaxValue;
                vlExcel.Operator = ExcelDataValidationOperator.greaterThanOrEqual;
            }
            for(var i = 4; i < columns.Length; i++)
            {

                var vlExcel = sheet.DataValidations.AddTextLengthValidation(columns[i] + "3:" + columns[i] + sheet.Dimension.Rows);
                vlExcel.ShowErrorMessage = true;
                vlExcel.ErrorTitle = ImportTemplate.ExcelInvaliddata;
                vlExcel.Error = ImportTemplate.ExcelEmptyEror;
                vlExcel.Prompt = ImportTemplate.ExcelEnterString;
                vlExcel.AllowBlank = false;
                vlExcel.Formula.Value = 1;
                vlExcel.Formula2.Value = 50;
            }

        }

        [HttpGet]
        [Route("api/Export/ExporttoExcelBudgetReport")]
        public HttpResponseMessage ExporttoExcelBudgetReport()
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
           
            int financialyearID = db.Database.SqlQuery<int>("select dbo.GetPlanningFinancialPeriodId()").FirstOrDefault();
            string FinacialYear = db.FinancialYears.Where(x => x.FinancialYearID == financialyearID).Select(x => x.ShortName).FirstOrDefault();

            string fileName = "BudgetReport_"+ FinacialYear+".xlsx";
            var dtResult = exportBl.GetBudgetReportData();
            var dtAllocationData = exportBl.GetBudgetAllocationDateReportData();

            int columnCount = dtResult.Columns.Count;
            int dtColumns = dtAllocationData.Columns.Count;

            using (ExcelPackage app = new ExcelPackage())
            {
                ExcelWorkbook workBook = app.Workbook;
                ExcelWorksheet worksheet = workBook.Worksheets.Add("BudgetReport");
                ExcelWorksheet AllocationSheet = workBook.Worksheets.Add("BudgetAllocationReport");

                //worksheet.Cells[1, 1, 1, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //worksheet.Cells[1, 1, 1, 43].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[1, 1, 1, columnCount].Style.Font.Bold = true;
                worksheet.Cells[1, 1, 1, columnCount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1, 1, columnCount].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(135, 229, 224));


                worksheet.Cells[1, 1].LoadFromDataTable(dtResult, true);
                worksheet.Cells.AutoFitColumns();
                worksheet.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;

                AllocationSheet.Cells[1, 1, 1, dtColumns].Style.Font.Bold = true;
                AllocationSheet.Cells[1, 1, 1, dtColumns].Style.Fill.PatternType = ExcelFillStyle.Solid;
                AllocationSheet.Cells[1, 1, 1, dtColumns].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(135, 229, 224));

                AllocationSheet.Cells[1, 1].LoadFromDataTable(dtAllocationData, true);
                AllocationSheet.Cells.AutoFitColumns();
                AllocationSheet.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                AllocationSheet.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                AllocationSheet.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                AllocationSheet.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;

                var memoryStream = app.GetAsByteArray();
                app.Dispose();
                result.Content = new ByteArrayContent(memoryStream);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = fileName;// Path.GetFileName(path);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                result.Content.Headers.ContentLength = memoryStream.Length;
                return result;

            }
        }

        [HttpGet]
        [Route("api/Export/ExporttoExcelCarveoutReport")]
        public HttpResponseMessage ExporttoExcelCarveoutReport()
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            int financialyearID = db.Database.SqlQuery<int>("select dbo.GetPlanningFinancialPeriodId()").FirstOrDefault();
            string FinacialYear = db.FinancialYears.Where(x => x.FinancialYearID == financialyearID).Select(x => x.ShortName).FirstOrDefault();

            string fileName = "CarveOutReport_" + FinacialYear + ".xlsx";
            var dtResult = exportBl.GetCarveOutReportData();
            int columnCount = dtResult.Columns.Count;

            using (ExcelPackage app = new ExcelPackage())
            {
                ExcelWorkbook workBook = app.Workbook;
                ExcelWorksheet worksheet = workBook.Worksheets.Add("CarveOutReport");

                //worksheet.Cells[1, 1, 1, 43].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //worksheet.Cells[1, 1, 1, 43].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[1, 1, 1, columnCount].Style.Font.Bold = true;
                worksheet.Cells[1, 1, 1, columnCount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, 1, 1, columnCount].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(135, 229, 224));

                worksheet.Cells[1, 1].LoadFromDataTable(dtResult, true);
                worksheet.Cells.AutoFitColumns();
                worksheet.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;

                var memoryStream = app.GetAsByteArray();
                app.Dispose();
                result.Content = new ByteArrayContent(memoryStream);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = fileName;// Path.GetFileName(path);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                result.Content.Headers.ContentLength = memoryStream.Length;
                return result;

            }
        }
    }
}
