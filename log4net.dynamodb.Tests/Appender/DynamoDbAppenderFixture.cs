using System;
using System.Reflection;
using NUnit.Framework;

// NOTE: This is an integration test that WILL access your AWS account. Be sure you understand the ramifications of this.

namespace log4net.Tests.Appender
{
    [TestFixture]
    public class DynamoDbAppenderFixture
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [Test]
        public void WriteLogMessage()
        {
            Config.XmlConfigurator.Configure();

            // an example of how you can append custom properties and pick the up 
            // in the parameter list see the App.config file for pattern.
            ThreadContext.Properties["log4net:CorrelationId"] = Guid.NewGuid();

            // using a numeric field
            ThreadContext.Properties["log4net:ImportantNumber"] = "42";

            // using a binary field
            ThreadContext.Properties["log4net:ImportantObject"] = new Tuple<string, int>("Number", 42);

            // log it
            Assert.DoesNotThrow(() => Logger.Error("An error occured.", new ApplicationException("You did something stupid!")));
        }
    }
}