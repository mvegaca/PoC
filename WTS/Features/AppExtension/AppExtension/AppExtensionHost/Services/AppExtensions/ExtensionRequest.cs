using System;
using System.Threading.Tasks;
using AppExtensionHost.Helpers;
using Windows.Foundation.Collections;

namespace AppExtensionHost.Services
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
            if (string.IsNullOrEmpty(paramKey))
            {
                throw new ArgumentNullException(nameof(paramKey));
            }
            if (paramValue == null)
            {
                throw new ArgumentNullException(nameof(paramValue));
            }
            if (paramValue.GetType().IsSerializable)
            {
                var stringValue = await Json.StringifyAsync(paramValue);
                _valueSet.Add(paramKey, stringValue);
            }
        }
    }
}
