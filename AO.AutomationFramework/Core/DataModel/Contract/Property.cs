using System.Collections.Generic;

namespace LogiLease.AutomationFramework.Core.DataModel.Contract
{
    public class Property
    {
        public List<Unit> Units = new List<Unit>();
        public string Name { get; set; }

        public Property(string name, Unit unit = null)

        {
            Name = name;
            Units.Add(unit);
        }
    }
}