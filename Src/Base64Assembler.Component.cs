using System;
using System.Collections.Generic;
using System.Linq;
using BizTalkComponents.Utils;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace BizTalkComponents.PipelineComponents.Base64Assembler
{
    public partial class Base64Assembler
    {
        public string Name { get { return "Base64Assembler"; } }
        public string Version { get { return "1.0"; } }
        public string Description
        {
            get
            {
                return
                    "Writes the current body to a new xml schema in base64 encoding.";
            }
        }
        public void GetClassID(out Guid classID)
        {
            classID = new Guid("F1368625-0DA0-4BEB-ACD1-AB0D79B2D715");
        }

        public void InitNew()
        {

        }

        public IEnumerator Validate(object projectSystem)
        {
            return ValidationHelper.Validate(this, false).ToArray().GetEnumerator();
        }

        public bool Validate(out string errorMessage)
        {
            var errors = ValidationHelper.Validate(this, true).ToArray();

            if (errors.Any())
            {
                errorMessage = string.Join(",", errors);

                return false;
            }

            errorMessage = string.Empty;

            return true;
        }

        public IntPtr Icon { get { return IntPtr.Zero; } }
    }
}
