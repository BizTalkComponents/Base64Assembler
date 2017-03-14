using BizTalkComponents.Utils;
using Microsoft.BizTalk.Component.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Streaming;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using System.Collections;

namespace BizTalkComponents.PipelineComponents.Base64Assembler
{
    [System.Runtime.InteropServices.Guid("a496e8a7-f8bb-4e55-b7ff-33f6bea70b56")]
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_AssemblingSerializer)]
    public partial class Base64Assembler :  IBaseComponent,
                                        IPersistPropertyBag, IComponentUI, IAssemblerComponent
    {
        private const string DestinationXpathPropertyName = "DestinationXpath";
        private const string DocumentSpecNamePropertyName = "DocumentSpecName";
        private readonly Queue _outputQueue = new Queue();

        [RequiredRuntime]
        [DisplayName("Destination xpath")]
        [Description("Path to the node that should contain the base64 string.")]
        public string DestinationXpath { get; set; }

        [RequiredRuntime]
        [DisplayName("DocumentSpecName")]
        [Description("DocumentSpecName of schema that which the base64 string should be enclosed in.")]
        public string DocumentSpecName { get; set; }

        public void AddDocument(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            string errorMessage;

            if (!Validate(out errorMessage))
            {
                var ex = new ArgumentException(errorMessage);
                throw ex;
            }

            DocumentSpec documentSpec;

            try
            {
                documentSpec = (DocumentSpec)pContext.GetDocumentSpecByName(DocumentSpecName);
            }
            catch (COMException)
            {
                throw new ArgumentException(string.Format("Could not find document with documentspecname = {0}", DocumentSpecName));
            }

            var doc = new XmlDocument();

            using (var sw = new StringWriter(new StringBuilder()))
            {

                doc.Load(documentSpec.CreateXmlInstance(sw));
            }

            var data = pInMsg.BodyPart.GetOriginalDataStream();

            const int bufferSize = 0x280;
            const int thresholdSize = 0x100000;

            if (!data.CanSeek || !data.CanRead)
            {
                data = new ReadOnlySeekableStream(data, new VirtualStream(bufferSize, thresholdSize), bufferSize);
                pContext.ResourceTracker.AddResource(data);
            }

            var ms = new MemoryStream();
            data.CopyTo(ms);

            ms.Seek(0, SeekOrigin.Begin);
            ms.Position = 0;

            var node = doc.SelectSingleNode(DestinationXpath);

            if (node == null)
            {
                throw new ArgumentException("Could not find element at {0}", DestinationXpath);
            }


            node.InnerText = Convert.ToBase64String(ms.ToArray());

            var outMs = new MemoryStream();
            doc.Save(outMs);
            outMs.Seek(0, SeekOrigin.Begin);

            pInMsg.BodyPart.Data = outMs;

            _outputQueue.Enqueue(pInMsg);

            
        }

        public IBaseMessage Assemble(IPipelineContext pContext)
        {
            if (_outputQueue.Count > 0)
            {
                return (IBaseMessage)_outputQueue.Dequeue();
            }

            return null;
        }
             

        public void Load(IPropertyBag propertyBag, int errorLog)
        {
            DocumentSpecName = PropertyBagHelper.ReadPropertyBag(propertyBag, DocumentSpecNamePropertyName, DocumentSpecName);
            DestinationXpath = PropertyBagHelper.ReadPropertyBag(propertyBag, DestinationXpathPropertyName, DestinationXpath);
        }

        public void Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {
            PropertyBagHelper.WritePropertyBag(propertyBag, DocumentSpecNamePropertyName, DocumentSpecName);
            PropertyBagHelper.WritePropertyBag(propertyBag, DestinationXpathPropertyName, DestinationXpath);
        }
    }
}
