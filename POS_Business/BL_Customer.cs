using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MegabiteEntityLayer;

namespace POS_Business
{
    public class BL_Customer
    {
        public List<Customer_Master> getCustomers()
        {
            return new POS_DAL.DAL_Cusomter().getCustomers();
        }
    }
}
