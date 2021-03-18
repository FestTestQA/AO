using System.Linq;
using System.Reflection;

namespace AO.AutomationFramework.Core.BusinessLogic.Extensions
{
    public static class SystemExtension
    {
        public static Target CopyProperties<Source, Target>(this Source source, Target target)
        {
            foreach (var sProp in source.GetType().GetRuntimeProperties())
            {
                bool isMatched = target.GetType()
                                       .GetRuntimeProperties()
                                       .Any(tProp => tProp.Name == sProp.Name
                                                     && tProp.GetType() == sProp.GetType()
                                                     && tProp.CanWrite);
                if (isMatched)
                {
                    var value = sProp.GetValue(source);
                    try
                    {
                        PropertyInfo propertyInfo = target.GetType().GetRuntimeProperty(sProp.Name);
                        propertyInfo.SetValue(target, value);
                    }
                    //used to handle static properties which we dont want to copy
                    catch { }
                }
            }
            return target;
        }
    }
}