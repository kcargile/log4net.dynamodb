using System;
using NUnit.Framework;
using log4net.Extensions;

namespace log4net.Tests.Extensions
{
    [TestFixture]
    public class ObjectExtensionsFixture
    {
        [Test]
        public void CheckNull()
        {
            object o = new object();
            Assert.DoesNotThrow(() => o.CheckNull("o"));
            Assert.DoesNotThrow(() => o.CheckNull());

            o = null;
            Assert.Throws<ArgumentNullException>(() => o.CheckNull("o"));
            Assert.Throws<ArgumentNullException>(() => o.CheckNull());
        }
    }
}
