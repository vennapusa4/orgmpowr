using MPOWR.Core;
using MPOWR.Dal.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace MPOWR.Api.App_Start
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Constructor)]
    public class AuthorizeGlossaryUserAttribute : AuthorizeAttribute
    {

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (HttpContext.Current.Request.Headers["authenticationToken"] != null)
            {
                TokenID = Convert.ToString(actionContext.Request.Headers.GetValues("authenticationToken").FirstOrDefault());
                var array = TokenID.Split('/');
                TokenID = array[0].ToString();
                RoleID = Convert.ToInt32(array[1]);
                Method = actionContext.Request.Method.ToString();
                string query = "";
                AbsolutePath = actionContext.Request.RequestUri.AbsolutePath.ToString().ToLower();
                if (AbsolutePath == "/api/glossary/approveglossary")
                    ApproveGlossary = true;
                else
                    ApproveGlossary = false;
                Flag = false;
                Original = actionContext.Request.RequestUri.OriginalString.ToString().ToLower();
                if (actionContext.Request.RequestUri.AbsoluteUri.IndexOf('?') == -1 && Method == "GET")
                {
                    return true;
                }


                if (Method == "GET" && (TokenID != null || TokenID != ""))
                {
                    query = actionContext.Request.RequestUri.ToString();

                    UserId = HttpContext.Current.Request.QueryString["userid"];
                    PartnerTypeID = Convert.ToInt32(HttpContext.Current.Request.QueryString["PartnerTypeID"]);
                    //CountryID = Convert.ToInt32(HttpContext.Current.Request.QueryString["CountryId"]);
                    CountryID = 57;
                    DistrictID = Convert.ToInt32(HttpContext.Current.Request.QueryString["DistrictID"]);

                }

                else if (Method == "POST" && (TokenID != null || TokenID != ""))
                {
                    query = actionContext.Request.Content.ReadAsStringAsync().Result.ToLower().ToString();

                    if (query.Contains("countryid") && query.Contains("partnertypeid"))
                    {
                        CountryID = Finder(query, "countryid");
                        PartnerTypeID = Finder(query, "partnertypeid");
                    }

                }
                return AuthenticatedUser(TokenID);
            }
            else
            {
                Message = "Header is not Present ";
                return false;
            }

        }

        private bool AuthenticatedUser(string TokenID)
        {
            using (MPOWREntities db = new MPOWREntities())
            {
                LoginAuthentication Record = new LoginAuthentication();
                bool userinfo = false;
                try
                {
                    Record = db.LoginAuthentications.Find(TokenID);
                    userinfo = ValidateUserInfo(Record.UserID);

                }
                catch (MPOWRException ex)
                {
                    Message = ex.Message.ToString();
                    MPOWRLogManager.LogMessage(ex.Message.ToString());
                    MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                }
                catch (Exception ex)
                {
                    Message = ex.Message.ToString();
                    MPOWRLogManager.LogMessage(ex.Message.ToString());
                    MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
                }

                if (Record == null || Record.Status == false || userinfo == false || (Record.UserID.ToUpper() != (UserId == null ? UserId : UserId.ToUpper()) && AbsolutePath.Contains("api/countries/getcountries")))
                {
                    return false;
                }

                else
                    return true;
            }
        }

        private bool ValidateUserInfo(string UserID)
        {
            using (MPOWREntities db = new MPOWREntities())
            {

                var Data = (from dat in db.UserRoleUserTypes
                            where dat.UserID == UserID
                            select new
                            {
                                RoleID = dat.RoleID,
                                IsAdmin = dat.IsAdmin,
                                GlossaryApprover = dat.GlossaryApprover,
                                UserTypeID = dat.UserTypeID,
                                Countries = (from usercontry in db.UserCountries
                                             join ugeo in db.UserGeos on dat.UserRoleUserTypeID equals ugeo.UserRoleUserTypeID
                                             join contry in db.Countries on usercontry.CountryID equals contry.CountryID
                                             where usercontry.UserRoleUserTypeID == dat.UserRoleUserTypeID
                                             select new
                                             {
                                                 contry.CountryID
                                             }).ToList(),
                                Partners = (from rolepartner in db.UserRTMs
                                            where rolepartner.UserRoleUserTypeID == dat.UserRoleUserTypeID
                                            select new
                                            {
                                                rolepartner.PartnerTypeID
                                            }).ToList()
                            }).ToList();


                if (Data == null || Data.Count == 0)
                {
                    Message = "Records are null";
                    return false;
                }
                if(ApproveGlossary == true)
                {
                    if (Flag == true && Data[0].IsAdmin == true && Data[0].RoleID == RoleID && Data[0].GlossaryApprover == true)
                    {
                        return true;
                    }
                    if ((Data[0].IsAdmin == true && Data[0].RoleID == RoleID && Data[0].GlossaryApprover == true) || (Data[0].UserTypeID == (short?)EnumUserType.WorldWide && Data[0].RoleID == RoleID && Data[0].GlossaryApprover == true) || (Data[0].UserTypeID == (short?)EnumUserType.Geo && Data[0].RoleID == RoleID && Data[0].GlossaryApprover == true) || (CountryID != 0 && CountryID != null && PartnerTypeID != 0 && PartnerTypeID != null && Data[0].Countries.Where(x => x.CountryID == CountryID).ToList().Count > 0 && (Data[0].Partners.Where(x => x.PartnerTypeID == PartnerTypeID).ToList().Count > 0) && Data[0].RoleID == RoleID && Data[0].GlossaryApprover == true))
                    {
                        return true;
                    }
                    if ((CountryID == null || CountryID == 0 || PartnerTypeID == null || PartnerTypeID == 0) && Data[0].GlossaryApprover == true)
                    {
                        return true;
                    }
                }
                else
                {
                    if (Flag == true && Data[0].IsAdmin == true && Data[0].RoleID == RoleID)
                    {
                        return true;
                    }
                    if ((Data[0].IsAdmin == true && Data[0].RoleID == RoleID) || (Data[0].UserTypeID == (short?)EnumUserType.WorldWide && Data[0].RoleID == RoleID) || (Data[0].UserTypeID == (short?)EnumUserType.Geo && Data[0].RoleID == RoleID) || (CountryID != 0 && CountryID != null && PartnerTypeID != 0 && PartnerTypeID != null && Data[0].Countries.Where(x => x.CountryID == CountryID).ToList().Count > 0 && (Data[0].Partners.Where(x => x.PartnerTypeID == PartnerTypeID).ToList().Count > 0) && Data[0].RoleID == RoleID))
                    {
                        return true;
                    }
                    if (CountryID == null || CountryID == 0 || PartnerTypeID == null || PartnerTypeID == 0)
                    {
                        return true;
                    }
                }
                
                Message = "User Information does not match";
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = new System.Net.Http.HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.Forbidden,
                Content = new StringContent(Message)
            };
        }
        public int Finder(string source, string param)
        {
            int Result = 0;
            try
            {
                int first = source.IndexOf(param) + param.Length + 2;
                int last = source.IndexOf(',', first);
                Result = Convert.ToInt32(source.Substring(first, last - first));
            }
            catch (Exception ex)
            {
                Message = ex.Message.ToString();
                MPOWRLogManager.LogMessage(ex.Message.ToString());
                MPOWRLogManager.LogException(MPOWRConstants.MessageSeparator + ex.ToString());
            }
            return Result;

        }

        #region Properties
        private string[] mediaExtensions = new string[]  {
                                                            ".png", ".jpg", ".jpeg",".gif",".xlsx",
                                                            ".mp3",".avi",".mp4",".mkv",".pdf",".xls",".doc",".txt",".docx",".pptx"
                                                         };

        private string[] IllegalSqlFragmentTokens = new string[] {"truncate","+","from","union","group by","sp_",
                                                                "char","update","delete",
                                                                "alter", "begin", "cast", "create", "cursor", "declare",
                                                                "drop",  "exec", "execute", "fetch", "insert", "kill",
                                                                "open", "select", "sys", "sysobjects", "syscolumns", "table"
                                                            };
        public string[] Ignore = new string[] { "delete", "create", "update" };

        public string Message = "Sorry, You do not have the required permission to perform this action. ";
        public string UserId { get; set; }
        public string AbsolutePath { get; set; }
        public bool Flag { get; set; }
        public bool ApproveGlossary { get; set; }
        public string URL { get; set; }
        public string Original { get; set; }
        public int? DistrictID { get; set; }
        public int? PartnerTypeID { get; set; }
        public int? CountryID { get; set; }
        public int? RegionID { get; set; }
        public int? SubRegionID { get; set; }
        public string Method { get; set; }
        public string TokenID { get; set; }
        public int? RoleID { get; set; }
        public int? UserTypeID { get; set; }
        public int[] Countries { get; set; }
        public string[] PartnerType { get; set; }

        #endregion
    }
}