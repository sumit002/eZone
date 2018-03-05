using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Reflection;
using System.Threading;

namespace MyElectronicZoneWpf.Utils
{
    public class ResourceHandler
    {
        public ResourceManager m_resourceManger = null;
        public ResourceHandler() {
            // Init m_resourceManger
            m_resourceManger = new ResourceManager("MyElectronicZoneWpf.Utils.Resource", Assembly.GetExecutingAssembly());
            // Init UICulture to CurrentCulture
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }
    }
}
