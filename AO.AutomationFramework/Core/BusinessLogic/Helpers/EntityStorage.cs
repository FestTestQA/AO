using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AO.AutomationFramework.Core.BusinessLogic.Helpers
{
    internal static class EntityStorage
    {
        private static readonly Dictionary<IWebDriver, IDictionary<Type, object>> InstancesForDriver;

        private static readonly object lockList = new object();

        static EntityStorage()
        {
            InstancesForDriver = new Dictionary<IWebDriver, IDictionary<Type, object>>();
        }

        private static IDictionary<Type, object> GetCurrentDriver(IWebDriver webDriver)
        {
            if (!InstancesForDriver.ContainsKey(webDriver))
            {
                InstancesForDriver.Add(webDriver, new Dictionary<Type, object>());
            }
            InstancesForDriver.TryGetValue(webDriver, out var Instances);
            return Instances;
        }

        internal static bool IsEntityExist<T>(IWebDriver webDriver)
        {
            var Instances = GetCurrentDriver(webDriver);
            //lock (lockList)
            {
                return Instances.ContainsKey(typeof(T));
            }
        }

        public static T GetEntity<T>(IWebDriver webDriver)
        {
            var Instances = GetCurrentDriver(webDriver);
            lock (lockList)
            {
                try
                {
                    return (T)Instances[typeof(T)];
                }
                catch (KeyNotFoundException)
                {
                    foreach (var instance in Instances.Where(instance => instance.Key.BaseType == typeof(T)))
                    {
                        return (T)instance.Value;
                    }
                    return (T)Instances[typeof(T)];
                }
            }
        }

        public static void UnregisterAllEntities()
        {
            foreach (var webDriver in InstancesForDriver)
            {
                var Instances = GetCurrentDriver(webDriver.Key);
                lock (lockList)
                {
                    Instances.Clear();
                }
            }
        }

        public static void UnregisterEntityByInstance(Type instance, IWebDriver webDriver)
        {
            var Instances = GetCurrentDriver(webDriver);
            lock (lockList)
            {
                if (Instances.Any(kvp => kvp.Key == instance))
                {
                    Instances.Remove(instance);
                }
            }
        }
    }
}