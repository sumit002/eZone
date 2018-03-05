using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyElectronicZoneWpf.Utils;

namespace MyElectronicZoneWpf
{
    public class Context
    {

        private ResourceHandler m_getCultureInfo = null;
        public ResourceHandler GetCultureInfo
        {
            get
            {
                if (m_getCultureInfo == null)
                {
                    m_getCultureInfo = new ResourceHandler();
                }
                return m_getCultureInfo;
            }
        }
    }
}
