using System;
using System.Collections.Generic;
using System.Text;

namespace FTCollectorApp
{

    public class Constants
    {
        public const string BaseUrl = "https://collector.fibertrak.com/FTService/";
        public const string InsertTimeSheetUrl = "xPostTimesCheck.php";
        public const string GetJobTableUrl = "xamarinJob.php";
        public const string GetEndUserTableUrl = "xamarinLogin.php";
        public const string InsertJobEvents = "xSaveJobEvents.php";
        public const string GetSiteTableUrl= "xGetSite.php";
        public const string InsertSiteTableUrl = "xInsertSite.php";
        public const string GetCrewdefaultTableUrl = "getCrewdefault.php";

        //////////////////// AWS S3 params ////////////////////////////////
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
    }
}
