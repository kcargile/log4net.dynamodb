using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Amazon.DynamoDBv2.Model;
using NUnit.Framework;
using log4net.Appender;

namespace log4net.Tests.Appender
{
    [TestFixture]
    public class DynamoDbAttributeBuilderFixture
    {
        private readonly DynamoDbAttributeBuilder _builder = new DynamoDbAttributeBuilder();

        [Test]
        public void BuildStringAttributeSuceeds()
        {
            const string item = "TESTING1234";

            AttributeValue value = _builder.BuildAttributeForTypeString(item);
            Assert.IsNotNull(value);
            Assert.AreEqual(item, value.S);
        }

        [Test]
        public void BuildStringAttributeWithNullItemFails()
        {
            Assert.Throws<ArgumentNullException>(() => _builder.BuildAttributeForTypeString(null));
        }

        [Test]
        public void BuildNumericAttributeSuceeds()
        {
            const int item = 42;

            AttributeValue value = _builder.BuildAttributeForTypeNumeric(item);
            Assert.IsNotNull(value);
            Assert.AreEqual(item, int.Parse(value.N));
        }

        [Test]
        public void BuildNumericAttributeWithNullItemFails()
        {
            Assert.Throws<ArgumentNullException>(() => _builder.BuildAttributeForTypeNumeric(null));
        }

        [Test]
        public void BuildBinaryAttributeSuceeds()
        {
            Tuple<string, int> item = new Tuple<string, int>("Number", 42);
            AttributeValue value = _builder.BuildAttributeForTypeBinary(item);
            Assert.IsNotNull(value);

            using (MemoryStream serialized = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(serialized, item);

                Assert.AreEqual(serialized.ToArray(), value.B.ToArray());
            }
        }

        [Test]
        public void BuildBinaryAttributeWithNullItemFails()
        {
            Assert.Throws<ArgumentNullException>(() => _builder.BuildAttributeForTypeBinary(null));
        }
    }
}
