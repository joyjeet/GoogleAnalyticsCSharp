using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Apis.Analytics.v3;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using Google.Apis.Analytics.v3.Data;
using static Google.Apis.Analytics.v3.DataResource;

namespace GoogleAnalyticsDataPull
{
   

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string[] scopes = new string[] {
           // AnalyticsService.Scope.Analytics,               // view and manage your Google Analytics data
           // AnalyticsService.Scope.AnalyticsEdit,           // Edit and manage Google Analytics Account
           // AnalyticsService.Scope.AnalyticsManageUsers,    // Edit and manage Google Analytics Users
            AnalyticsService.Scope.AnalyticsReadonly};      // View Google Analytics Data

            var clientId = "xxxxx";      // From https://console.developers.google.com
            var clientSecret = "xxxxxx";          // From https://console.developers.google.com
                                               // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            },
            scopes,
            Environment.UserName,
            CancellationToken.None,
            new FileDataStore("Daimto.GoogleAnalytics.Auth.Store")).Result;

            
            AnalyticsService service = new AnalyticsService( new BaseClientService.Initializer {
               HttpClientInitializer = credential,
                ApplicationName = "www.worldwinger.com"
            });
            var data = GADataSample.Get(service, "ga:31116021", "2019-01-01", "2019-04-23", "ga:sessions,ga:pageviews", null);

            GoogleAnalyticsData gad = new GoogleAnalyticsData();
            //foreach (var d in data)
                
            {
                //TODO: Add code to object

                // gad.<NonSerialize to store>;

            }

            gad.ToString();
        }

       
    }

   

}
