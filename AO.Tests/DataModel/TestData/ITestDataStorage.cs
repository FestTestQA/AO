using System.Collections.Generic;

namespace AO.Tests.DataModel.TestData
{
    internal interface ITestDataStorage
    {
        List<PortalUser> PortalUsers { get; set; }
    }
}