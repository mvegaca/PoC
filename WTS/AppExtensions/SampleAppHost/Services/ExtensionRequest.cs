using System;
using System.Threading.Tasks;
using SampleAppHost.Helpers;
using Windows.Foundation.Collections;

namespace SampleAppHost.Services
{
    public class ExtensionRequest
    {
        private ValueSet _valueSet = new ValueSet();

        public ExtensionRequest()
        {
        }

        public ValueSet GetValueSet() => _valueSet;

        public async Task AddParameterAsync(string paramKey, object paramValue)
        {
            var stringValue = await Json.StringifyAsync(paramValue);
            _valueSet.Add(paramKey, stringValue);
        }
    }
}
