﻿using System;
using System.Threading.Tasks;
using AppExtension.Helpers;
using Windows.Foundation.Collections;

namespace AppExtension.Services
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
                _response.Add(paramKey, stringValue);
            }
        }
    }
}
