using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Winterdom.BizTalk.PipelineTesting;
using BizTalkComponents.PipelineComponents.Base64Assembler;
using System.Xml;
using System.Text;

namespace BizTalkComponents.Base64Assembler.Tests.UnitTests
{
    [TestClass]
    public class Base64AssemblerTest
    {
        [TestMethod]
        public void TestHappyPath()
        {
            var pipeline = PipelineFactory.CreateEmptySendPipeline();

            pipeline.AddDocSpec(typeof(SchemaMock));

            var assembler = new PipelineComponents.Base64Assembler.Base64Assembler
            {
                DocumentSpecName = "BizTalkComponents.Base64Assembler.Tests.UnitTests.SchemaMock",
                DestinationXpath = "/*[local-name()='Root' and namespace-uri()='http://test.SchemaMock']/*[local-name()='Element' and namespace-uri()='']"
            };

            var message = MessageHelper.CreateFromString("<message></message>");

            pipeline.AddComponent(assembler, PipelineStage.Assemble);

            var result = pipeline.Execute(message);

            var doc = new XmlDocument();

            doc.Load(result.BodyPart.GetOriginalDataStream());

            var node = doc.SelectSingleNode("/*[local-name() = 'Root']/*[local-name() = 'Element']");
            byte[] data = Convert.FromBase64String(node.InnerText);
            string decodedString = Encoding.Default.GetString(data);

            Assert.AreEqual("<message></message>", decodedString.Replace("\0",""));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidDocSpecName()
        {
            var pipeline = PipelineFactory.CreateEmptySendPipeline();

            pipeline.AddDocSpec(typeof(SchemaMock));

            var assembler = new PipelineComponents.Base64Assembler.Base64Assembler
            {
                DocumentSpecName = "Invalid",
                DestinationXpath = "/*[local-name()='Root' and namespace-uri()='http://test.SchemaMock']/*[local-name()='Element' and namespace-uri()='']"
            };

            var message = MessageHelper.CreateFromString("<message></message>");

            pipeline.AddComponent(assembler, PipelineStage.Assemble);

            var result = pipeline.Execute(message);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidXpath()
        {
            var pipeline = PipelineFactory.CreateEmptySendPipeline();

            pipeline.AddDocSpec(typeof(SchemaMock));

            var assembler = new PipelineComponents.Base64Assembler.Base64Assembler
            {
                DocumentSpecName = "BizTalkComponents.Base64Assembler.Tests.UnitTests.SchemaMock",
                DestinationXpath = "/*[local-name()='RootInvalid' and namespace-uri()='http://test.SchemaMock']/*[local-name()='Element' and namespace-uri()='']"
            };

            var message = MessageHelper.CreateFromString("<message></message>");

            pipeline.AddComponent(assembler, PipelineStage.Assemble);

            var result = pipeline.Execute(message);
        }
    }
}
