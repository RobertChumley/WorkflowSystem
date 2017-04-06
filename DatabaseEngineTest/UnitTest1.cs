using System;
using DatabaseEngine.CouchbaseDriver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DatabaseEngineTest
{
    [TestClass]
    public class CouchbaseTest
    {
        [TestMethod]
        public void RunTest()
        {
            var connector = new CouchbaseConnector();
            connector.TestCouch();
        }
    }
}
