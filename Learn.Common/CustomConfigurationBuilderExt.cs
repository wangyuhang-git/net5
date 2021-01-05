using Learn.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Configuration//Learn.Common
{
    public static class CustomConfigurationBuilderExt
    {
        public static IConfigurationBuilder AddCustomConfiguration(this IConfigurationBuilder builer)
        {
            builer.Add(new CustomConfigurationSource());
            return builer;
        }
    }
}
