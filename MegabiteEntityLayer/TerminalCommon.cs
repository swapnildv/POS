using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MegabiteEntityLayer;

namespace Hotel_POS.Resource
{
    public static class TerminalCommon
    {
        public static string errorMessage { get { return "An Error occured"; } }
        public static string cafeName { get { return "Bunkerz"; } }
        public static User_Master LoggedInUser { get; set; }
        public static Customer_Master currentCustomer { get; set; }
    }
}
