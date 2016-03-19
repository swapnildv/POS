using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hotel_POS.Resource;

namespace MegabiteEntityLayer.Helpers
{
    public class FieldException : ApplicationException
    {
        private string message;

        public FieldException(string message)
        {
            if (string.IsNullOrEmpty(message))
                this.message = TerminalCommon.errorMessage;
            else
                this.message = message;
        }

        public override string Message
        {
            get
            {
                return this.message;
            }
        }
    }
}
