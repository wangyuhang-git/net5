using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Common
{
    class CustomConfigurationProvider : ConfigurationProvider
    {
        public CustomConfigurationProvider() : base()
        {

        }

        public override void Load()
        {
            Load(false);
        }

        void Load(bool reload)
        {
            this.Data["lastTime"] = DateTime.Now.ToString();
            if(reload)
            {
                base.OnReload();
            }
        }
    }
}
