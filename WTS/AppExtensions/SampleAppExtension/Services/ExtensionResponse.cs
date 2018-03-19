using System;
using System.Threading.Tasks;
using SampleAppExtension.Helpers;
using Windows.Foundation.Collections;

namespace SampleAppExtension.Services
{
    public class ExtensionResponse
    {
        private ValueSet _response = new ValueSet();

        public ExtensionResponse()
        {
        }

        public ValueSet GetValueSet() => _response;

        public async Task AddValueAsync(string paramKey, object paramValue)
        {
            var stringValue = await Json.StringifyAsync(paramValue);
            _response.Add(paramKey, stringValue);
        }
    }
}
