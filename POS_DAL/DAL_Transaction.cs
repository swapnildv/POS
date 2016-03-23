using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel_POS.Resource;
using MegabiteEntityLayer;
using MegabiteEntityLayer.Helpers;

namespace POS_DAL
{
    public class DAL_Transaction : Connection
    {

        public List<Report> getTransactionReport(DateTime Fromdt, DateTime Todt)
        {
            Fromdt = Convert.ToDateTime(Fromdt.ToShortDateString());
            Todt = Convert.ToDateTime(Todt.ToShortDateString()).Add(new TimeSpan(23, 59, 59));


            List<Report> lstReport = new List<Report>();
            Report objReport = new Report();
            var qry = (from tm in dc.Transaction_Master
                       where tm.Transaction_Date >= Fromdt
                       && tm.Transaction_Date <= Todt
                       select new
                       {
                           tm.Transaction_Date,
                           tm.Order_No,
                           tm.Transaction_Amount,
                           tm.Discount_Perc,
                           tm.Discount_Value
                       }).ToList();

            foreach (var item in qry)
            {
                objReport = new Report();

                objReport.Transaction_Date = Convert.ToDateTime(item.Transaction_Date);
                objReport.Order_No = item.Order_No;
                objReport.Transaction_Amount = Convert.ToDouble(item.Transaction_Amount);
                objReport.Discount_Perc = item.Discount_Perc;
                objReport.Discount_Value = item.Discount_Value;
                lstReport.Add(objReport);
            }
            return lstReport;
        }

        public Transaction_Master SubmitOrder(double discount)
        {
            //Calculte discounted value.
            double discounted_val = DAL_Item_Master.MenuCart.Sum(a => a.Item_Total);
            discounted_val = discounted_val * (discount / 100);

            //Add customer Entry.
            var cust = dc.Customer_Master.Where(a => a.cust_MobileNo == TerminalCommon.currentCustomer.cust_MobileNo).SingleOrDefault();
            if (cust == null && TerminalCommon.currentCustomer.cust_MobileNo != "")
            {
                dc.Customer_Master.AddObject(TerminalCommon.currentCustomer);
                dc.SaveChanges();
            }
            Transaction_Master objTM = new Transaction_Master();
            Transaction_Details objTD = new Transaction_Details();
            List<Transaction_Details> lstTD = new List<Transaction_Details>();
            if (DAL_Item_Master.MenuCart.Count <= 0)
                throw new FieldException("Invalid Order.");

            objTM.Transaction_Date = DateTime.Now;
            objTM.Transaction_Amount = DAL_Item_Master.MenuCart.Sum(a => a.Item_Total);
            objTM.Created_By = TerminalCommon.LoggedInUser.User_ID;
            objTM.Created_DateTime = DateTime.Now;
            objTM.Updated_By = TerminalCommon.LoggedInUser.User_ID;
            objTM.Updated_DateTime = DateTime.Now;
            objTM.Company_ID = Convert.ToInt32(TerminalCommon.LoggedInUser.Company_ID);
            objTM.Discount_Perc = discount;
            objTM.Discount_Value = DAL_Item_Master.MenuCart.Sum(a => a.Item_Total) - discounted_val;

            foreach (var item in DAL_Item_Master.MenuCart)
            {
                objTD = new Transaction_Details();
                MenuCart objmenu = new MenuCart();
                objmenu = (MenuCart)item;
                objTD = new Transaction_Details();
                objTD.Item_ID = Convert.ToInt32(objmenu.Item_ID);
                objTD.Item_Unit_Price = Convert.ToDouble(objmenu.Item_Unit_Price);
                objTD.Item_Quantity = Convert.ToInt32(objmenu.Quantity);
                objTD.Item_Total_Cost = (objTD.Item_Unit_Price * objTD.Item_Quantity);
                objTD.Created_By = TerminalCommon.LoggedInUser.User_ID;
                objTD.Updated_By = TerminalCommon.LoggedInUser.User_ID;
                objTD.Created_DateTime = DateTime.Now;
                objTD.Updated_DateTime = DateTime.Now;
                objTD.Company_ID = Convert.ToInt32(TerminalCommon.LoggedInUser.Company_ID);
                lstTD.Add(objTD);
            }
            dc.Transaction_Master.AddObject(objTM);
            dc.SaveChanges();
            objTM.Order_No = "OC " + objTM.Company_ID + "-" + objTM.Transaction_Master_ID.ToString();
            foreach (var item in lstTD)
            {
                item.Transaction_Master_ID = objTM.Transaction_Master_ID;
                dc.Transaction_Details.AddObject(item);
            }
            dc.SaveChanges();
            return objTM;
        }
        /// <summary>
        /// Get today's sale.
        /// </summary>
        /// <returns></returns>
        public double _getTodaysSale()
        {
            return (from d in dc.Transaction_Master
                    where EntityFunctions.TruncateTime(d.Transaction_Date) == DateTime.Today
                    select d.Discount_Value).Sum();
        }

        public List<sp_favorite_items_Result> _getFavoriteItems()
        {
            return dc.sp_favorite_items_function().ToList();
        }

    }
}
