namespace MPOWR.Core
{
    public class MPOWRMessages
    {
        public const string USER_NOT_FOUND = "User not found for personal Number: {0}";
        public const string RECORD_ALREADY_EXIST = "Record already exist";
        public const string RECORD_NOT_FOUND = "Record not Found";
        public const string GENERIC_API_EXCEPTION = "Something went wrong, please contact admin or try later";
        public const string GENERIC_INSERT_ENTITY_FAILURE = "Insert operation failed for method: {0}";
        public const string NO_CONFIGURATION_FOUND = "No configuration setting found";
        public const string NO_MASTER_ACTIVITY_FOUND = "No master activity found";
        public const string BAD_REQUEST_FOR_CONFIG = "Either config key or config value is missing or null";
        public const string BAD_REQUEST_FOR_SUBMIT_LEAVE = "Either UserId or some field value is missing or null";
        public const string BAD_REQUEST_FOR_GETTING_TERRITORY = "UserId value is missing or null";
        public const string BAD_REQUEST_FOR_UPLOADINGIMAGE = "Either UserId or Profile image is missing or null";
        public const string BAD_REQUEST_FOR_CHECKINSTATUS = "UserId value is missing or null";
        public const string BAD_REQUEST_FOR_DEPUTEE_VALIDATION = "UserId value is missing or null";
        public const string BAD_REQUEST_FOR_GET_TIMELINE = "UserId value is missing or null";
        public const string BAD_REQUEST_FOR_LOGIN = "Either Username or Password is missing or null";
        public const string UnAuthenticated = "User is unauthenticated to access the API";
        public const string NO_TARGET_FOUND = "Target has not been uploaded for this activity type for this particualr user";
        public const string NO_WEEK_ID_FOUND = "Week has not been uploaded yet. No week id found in the database for the selected date";
        public const string ONE_PRIMARY_CROP = "There should be only 1 primary crop product";
        public const string ONE_ACTIVITY_IMAGE = "There should be only 1 primary activity image";
        public const string NO_RETAILER_EXIST = "Invalid retailer code";
        public const string NO_PLANNINGTERRITORY_EXIST = "Planning territory does not exist";
        public const string INVALID_DATAENTRY_PERIOD = "You cannot enter the data in this period";
    }
}
