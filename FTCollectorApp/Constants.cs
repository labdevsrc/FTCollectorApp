using System;
using System.Collections.Generic;
using System.Text;

namespace FTCollectorApp
{

    public class Constants
    {
        public const bool AutoSyncAWSTables = false;

        public const string BaseUrl = "https://collector.fibertrak.com/FTService/";
        public const string InsertTimeSheetUrl = "xPostTimesCheck.php";
        public const string GetJobTableUrl = "xamarinJob.php";
        public const string GetEndUserTableUrl = "xamarinLogin.php";
        public const string InsertJobEvents = "xSaveJobEvents.php";
        public const string GetSiteTableUrl= "xGetSite.php";
        public const string GetCrewdefaultTableUrl = "getCrewdefault.php";
        public const string GetCodeSiteTypeTableUrl = "xGetCodeSiteType.php";
        public const string SaveCrewUrl = "saveCrew.php";
        public const string CreateSiteTableUrl = "Createsite.php";
        public const string UpdateSiteTableUrl = "Savebuilding.php";
        public const string UpdateAfiberCableTableUrl = "xFiberOpticCableSave.php";
        public const string PostDuctTrace = "ajaxSaveduct.php";
        public const string GetBuildingsParamUrl = "getBuildingEntries.php";
        public const string PostSplice = "ajaxSavesplice.php";
        public const string SaveRacks = "ajaxSaverack.php";
        public const string SaveActiveDevice = "ajaxSaveactivedevice.php";
        public const string SaveSlack = "ajaxSaveslack.php";
        public const string SaveSheathMark = "ajaxSaveSheathmark.php";

        //////////////////// AWS S3 params - start ////////////////////////////////
        public const string COGNITO_POOL_ID = "us-east-2:5ad27ed4-59be-49f6-b103-3edb3e4d20c5";

        /*
        * Region of your Cognito identity pool ID.
        */
        public const string COGNITO_POOL_REGION = "us-east-2";

        /*
            * Note, you must first create a bucket using the S3 console before running
            * the sample (https://console.aws.amazon.com/s3/). After creating a bucket,
            * put it's name in the field below.
            */
        public const string BUCKET_NAME = "fibertrakmedia";

        /*
            * Region of your bucket.
            */
        public const string BUCKET_REGION = "us-east-2";
        public const string ACCES_KEY_ID = "AKIAJTM6EJOVYMZEVPPQ";
        public const string SECRET_ACCESS_KEY = "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";
        //////////////////// AWS S3 params - end ////////////////////////////////
    }
}
