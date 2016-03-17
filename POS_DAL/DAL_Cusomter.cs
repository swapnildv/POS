using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MegabiteEntityLayer;

namespace POS_DAL
{
    public class DAL_Cusomter : Connection
    {
        public List<Customer_Master> getCustomers()
        {
            return dc.Customer_Master.ToList();
        }
    }
}
