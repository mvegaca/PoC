using System;
using System.Collections.Generic;
using System.Web;

namespace ActivityFeed.Activation
{
    public class SchemeActivationData
    {
        public const string ProtocolName = "wtsapp";
        public const string ViewsNamespace = "ActivityFeed.Views";

        public Type PageType { get; private set; }

        public bool IsValid => PageType != null;

        public Dictionary<string, string> Parameters { get; private set; }

        private SchemeActivationData()
        {
        }

        public SchemeActivationData(Type pageType, Dictionary<string, string> parameters = null)
        {
            PageType = pageType;
            Parameters = parameters;
        }

        public Uri BuildUri()
        {
            var uriBuilder = new UriBuilder($"{ProtocolName}:{PageType.Name}");
            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var parameter in Parameters)
            {
                query.Set(parameter.Key, parameter.Value);
            }
            uriBuilder.Query = query.ToString();
            return new Uri(uriBuilder.ToString());
        }

        public static SchemeActivationData CreateFromUri(Uri activationUri)
        {
            var data = new SchemeActivationData();
            // By default, this handler expects URIs of the format:
            // 'wtsapp:MainPage?paramName1=paramValue1&paramName2=paramValue2'
            // i.e. 'tvshowsapp:VideoPlayerPage?showId=115&season=3&episode=1'
            var pageName = activationUri.AbsolutePath;
            data.PageType = typeof(App).Assembly.GetType($"{ViewsNamespace}.{pageName}");

            if (data.IsValid && !string.IsNullOrEmpty(activationUri.Query))
            {
                var uriQuery = HttpUtility.ParseQueryString(activationUri.Query);
                data.Parameters = new Dictionary<string, string>();
                foreach (var paramKey in uriQuery.AllKeys)
                {
                    data.Parameters.Add(paramKey, uriQuery.Get(paramKey));
                }
            }
            return data;
        }
    }
}
