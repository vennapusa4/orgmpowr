namespace MPOWR.Core
{
    public class MPOWRConstants
    {
        public const string MessageSeparator = " : ";
        public const string ExceptionFrom = "Common Functionality : ";
        public static  string[] Week={"MONDAY","TUESDAY","WEDNESDAY","THUSDAY","FRIDAY","SATURDAY","SUNDAY"};
        public string HasAccess = "";
        public const string Success = "success";
        public const string Failed = "Failed";
        public const string IsCompute = "IsCompute";
        public const string Compute = "Compute";
        public const string ComputeValue = "Compute Value";
        public const string ComputeVolume = "Compute Volume";
        public const string Round1MDF = "Round1MDF";
        public const string Round2MDF = "Round2MDF";
        public const string UnitedStates = "United States";
        public const string District = "District";
        public const string Sellout_Silver_Below = " Sellout to Silver & Below";
        public const string Sellout_Gold_Platinum = " Sellout to Gold & Platinum";
        public const string Unweighted_Sellout = " Unweighted Sellout";
        public const string MPOWRProjectedSellout = " M-POWR Projected Sellout";
        public const string MPOWRWeightedProjectedSellout = " M-POWR Projected Weighted Sellout";
        public const string SellOut = " Sellout($)";
        public const string WeightedSellOut = " Weighted Sellout ($)";
        public const string CanadaCountryId = "26";
        public const string UnitedStatesCountryId = "138";
        public const string ExcelExportRecordLimit = "ExcelExportRecordLimit";
        public const string ApplicationName = "MPOWR";
        public const string ShowDataUploadAccess = "ShowDataUploadAccess";

    }
    public class ImportTemplate
    {
        //Brillio - #704 - Sep 6 2017 - Start
        public static string InvalidBudget = "Invalid Budget";
        public static string InvalidAllocatedMDF1 = "Invalid Allocated Round1 MDF";
        public static string InvalidAllocatedMDF2 = "Invalid Allocated Round2 MDF";
        public static string InvalidMSA = "Invalid MSA";
        public static string InvalidArubaMSA = "Invalid Aruba MSA";
        public static string Mandatory = "is mandatory";
        public static string USA_Country = "USA";
        public static string CANADA_Country = "CANADA";
        public static string PartnerTypeReseller = "RESELLER";
        public const string MSABudgetError = "Allocated budget and  MSA budget is exceeding the actual MSA budget;";
        public const string ARUbaMSABudgetError="Allocated  ARUBA MSA budget is exceeding the actual ARUBA MSA budget; ";
        public const string BUBudgetError = "Allocated budget is exceeding the actual budget for these Business units : ";
        public static string InvalidColumnError = "Required columns are missing";

        public static string partnerId = "partnerId";
        public static string country = "country";
        public static string business_unit = "business_unit";
        public static string budget = "budget";
        public static string MFD1 = "MFD1";
        public static string MFD2 = "MFD2";
        public static string msa = "msa";
        public static string arubamsa = "arubamsa";
        public static string Carveout = "Carveout";
        public const string ExcelInvaliddata = "An invalid data entered";
        public const string ExcelvalidateNo= "MDF should be a number";
        public const string ExcelEmptyEror= "This field can't be empty"; 
        public const string ExcelEnterNumber = "Enter Number here";
        public const string ExcelEnterString = "Enter Data here";
        public const string Round2MDF = " Proposed Budget (Round 2)";
 

    }
}
