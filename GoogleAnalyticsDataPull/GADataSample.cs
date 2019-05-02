using Google.Apis.Analytics.v3;
using Google.Apis.Analytics.v3.Data;
using System;
using System.Collections.Generic;
using System.Text;
using static Google.Apis.Analytics.v3.DataResource;

namespace GoogleAnalyticsDataPull
{
    public static class GADataSample
    {
        public class GaGetOptionalParms
        {
            /// A comma-separated list of Analytics dimensions. E.g., 'ga:browser,ga:city'.
            public string Dimensions { get; set; }
            /// A comma-separated list of dimension or metric filters to be applied to Analytics data.
            public string Filters { get; set; }
            /// The response will include empty rows if this parameter is set to true, the default is true
            public bool? Include_empty_rows { get; set; }

            /// The maximum number of entries to include in this feed.
            public int? Max_results { get; set; }
            /// The selected format for the response. Default format is JSON.
            public string Output { get; set; }
            /// The desired sampling level.
            public string SamplingLevel { get; set; }
            /// An Analytics segment to be applied to data.
            public string Segment { get; set; }
            /// A comma_separated list of dimensions or metrics that determine the sort order for Analytics data.
            public string Sort { get; set; }
            /// An index of the first entity to retrieve. Use this parameter as a pagination mechanism along with the max_results parameter.
            public int? Start_index { get; set; }

        }

        /// <summary>
        /// Returns Analytics data for a view (profile). 
        /// Documentation https://developers.google.com/analytics/v3/reference/ga/get
        /// Generation Note: This does not always build corectly.  Google needs to standardise things I need to figuer out which ones are wrong.
        /// </summary>
        /// <param name="service">Authenticated Analytics service.</param>  
        /// <param name="ids">Unique table ID for retrieving Analytics data. Table ID is of the form ga:XXXX, where XXXX is the Analytics view (profile) ID.</param>
        /// <param name="start-date">Start date for fetching Analytics data. Requests can specify a start date formatted as YYYY-MM-DD, or as a relative date (e.g., today, yesterday, or 7daysAgo). The default value is 7daysAgo.</param>
        /// <param name="end-date">End date for fetching Analytics data. Request can should specify an end date formatted as YYYY-MM-DD, or as a relative date (e.g., today, yesterday, or 7daysAgo). The default value is yesterday.</param>
        /// <param name="metrics">A comma-separated list of Analytics metrics. E.g., 'ga:sessions,ga:pageviews'. At least one metric must be specified.</param>
        /// <param name="optional">Optional paramaters.</param>
        /// <returns>GaDataResponse</returns>
        public static GaData Get(AnalyticsService service, string ids, string start_date, string end_date, string metrics, GaGetOptionalParms optional = null)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (ids == null)
                    throw new ArgumentNullException(ids);
                if (start_date == null)
                    throw new ArgumentNullException(start_date);
                if (end_date == null)
                    throw new ArgumentNullException(end_date);
                if (metrics == null)
                    throw new ArgumentNullException(metrics);

                // Building the initial request.
                var request = service.Data.Ga.Get(ids, start_date, end_date, metrics);

                // Applying optional parameters to the request.                
                request = (GaResource.GetRequest)GADataHelpers.ApplyOptionalParms(request, optional);

                // Requesting data.
                return request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Ga.Get failed.", ex);
            }
        }
    }

    public static class GADataHelpers
    {
        /// <summary>
        /// Using reflection to apply optional parameters to the request.  
        /// 
        /// If the optonal parameters are null then we will just return the request as is.
        /// </summary>
        /// <param name="request">The request. </param>
        /// <param name="optional">The optional parameters. </param>
        /// <returns></returns>
        public static object ApplyOptionalParms(object request, object optional)
        {
            if (optional == null)
                return request;

            System.Reflection.PropertyInfo[] optionalProperties = (optional.GetType()).GetProperties();

            foreach (System.Reflection.PropertyInfo property in optionalProperties)
            {
                // Copy value from optional parms to the request.  They should have the same names and datatypes.
                System.Reflection.PropertyInfo piShared = (request.GetType()).GetProperty(property.Name);
                if (property.GetValue(optional, null) != null) // TODO Test that we do not add values for items that are null
                    piShared.SetValue(request, property.GetValue(optional, null), null);
            }

            return request;
        }
    }
}
