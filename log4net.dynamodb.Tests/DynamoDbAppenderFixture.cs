using System;
using System.Reflection;
using NUnit.Framework;

// NOTE: This is an integration test that WILL access your AWS account. Be sure you understand the ramifications of this.

namespace log4net.Appender.Tests
{
    [TestFixture]
    public class DynamoDbAppenderFixture
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [Test]
        public void WriteLogMessage()
        {
            // NOTE: yeah, this unit test sucks, I know...

            Config.XmlConfigurator.Configure();

            string fakeMessage = string.Concat("Error message ", Guid.NewGuid());
            Assert.DoesNotThrow(() => Logger.Error(fakeMessage, new ApplicationException("Important Error!")));
        }
    }
}
