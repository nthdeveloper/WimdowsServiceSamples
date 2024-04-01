using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceWithWorkers
{
    internal static class ServiceLocator
    {
        static Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static void Register<T>(T service)
        {
            _services.Add(typeof(T), service);
        }

        public static T Get<T>()
        {
            return (T)_services[typeof(T)];
        }
    }
}
