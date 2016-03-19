using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Hotel_POS.Resource;
using log4net;

namespace Hotel_POS.MessageHelper
{
    public static class MessageBox
    {
        private static readonly ILog _logger =
      LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void ShowError(Window ownerWindow, string message = "")
        {
            try
            {
                message = String.IsNullOrEmpty(message) == true ? TerminalCommon.errorMessage : message;
                ErrorBox messageWindow = new ErrorBox(message) { Owner = ownerWindow };
                messageWindow.ShowDialog();
                ownerWindow.Effect = null;
            }
            catch (Exception ex) {
                _logger.Error(ex);
            }
        }


    }
}
