using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.XLANGs.BaseTypes;

namespace BizTalkComponents.Base64Assembler.Tests.UnitTests
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://test.SchemaMock", @"Root")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] { @"Root" })]
    public sealed class SchemaMock : Microsoft.XLANGs.BaseTypes.SchemaBase
    {

        [System.NonSerializedAttribute()]
        private static object _rawSchema;

        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://test.SchemaMock"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""http://test.SchemaMock"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""Root"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""Element"">
          <xs:simpleType>
            <xs:restriction base=""xs:base64Binary"" />
          </xs:simpleType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";

        public SchemaMock()
        {
        }

        public override string XmlContent
        {
            get
            {
                return _strSchema;
            }
        }

        public override string[] RootNodes
        {
            get
            {
                string[] _RootElements = new string[1];
                _RootElements[0] = "Root";
                return _RootElements;
            }
        }

        protected override object RawSchema
        {
            get
            {
                return _rawSchema;
            }
            set
            {
                _rawSchema = value;
            }
        }
    }
}
