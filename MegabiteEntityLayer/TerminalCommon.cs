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
        public static Dictionary<string, List<String>> menuDictionary = new Dictionary<string, List<string>>() { 
            {"user",new List<String>() { "users","changePassword" }},
            {"menu",new List<String>() { "category","item" }},
            {"orders",new List<String>() { "NewOrder" }},
            {"reports",new List<String>() { "transactionReport" }},
            {"logout",new List<String>() { "Logout" }}
        };

        public static List<String> adminRoleMenu = new List<string>() { "Users", 
                                         "ChangePasswordMenuItem", 
                                         "MenuCategory", 
                                         "Menu", 
                                         "NewOrder", 
                                         "TransactionReport", 
                                         "CustomerReport",
                                         "Logout",
                                         "Home"};

        public static List<String> operatorRoleMenu = new List<string>() {  "NewOrder",  
                                              "ChangePasswordMenuItem","Logout" };

        public static String currency { get { return "Rs."; } }

        public enum user_roles { adminRole = 4, operatorRole };
    }


}
