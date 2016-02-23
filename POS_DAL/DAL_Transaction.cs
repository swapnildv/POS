using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel_POS.Resource;
using MegabiteEntityLayer;

namespace POS_DAL
{
    public class DAL_Transaction : Connection
    {


        public TimeSpan ts = new TimeSpan(23, 59, 59);

        public Card_Ledger objCard_Ledger = new Card_Ledger();

        public Int64 Submit_Order(Transaction_Master objTM, List<Transaction_Details> lstTD)
        {

            dc.Transaction_Master.AddObject(objTM);
            dc.SaveChanges();
            Int64 TMID = objTM.Transaction_Master_ID;
            //Commented by Mohammed Naved Shaikh on 21/04/2014 Start
            //objTM.Order_No = "O" + objTM.Transaction_Master_ID.ToString();
            objTM.Order_No = "OC " + objTM.Company_ID + "-" + objTM.Transaction_Master_ID.ToString();
            //Commented by Mohammed Naved Shaikh on 21/04/2014 End
            foreach (var item in lstTD)
            {
                item.Transaction_Master_ID = TMID;
                dc.Transaction_Details.AddObject(item);

            }



            if (objTM.PaidBy_Card == true)
            {
                Card_Master objCM = new Card_Master();
                objCM = dc.Card_Master.Where(c => c.RFID_No == objTM.RFID_No && c.Employee_ID == objTM.Employee_ID).First();
                if (objCM != null)
                {
                    objCM.Current_Balance = objTM.Card_Closing_Balance;
                }

                objCard_Ledger = new Card_Ledger();
                objCard_Ledger.Debit = objTM.Transaction_Amount;
                objCard_Ledger.Employee_ID = Convert.ToInt32(objTM.Employee_ID);
                objCard_Ledger.RFID_NO = objTM.RFID_No;
                objCard_Ledger.TR_Date = DateTime.Now;
                objCard_Ledger.TR_Remark = "Food Purchase With ref " + objTM.Order_No;
                objCard_Ledger.TR_Type = "DR";
                objCard_Ledger.User_ID = objTM.Created_By;
                objCard_Ledger.Ref = "Order Number" + objTM.Order_No; ;
                objCard_Ledger.Closing_Balance = objTM.Card_Closing_Balance;
                objCard_Ledger.Company_ID = objTM.Company_ID;
                dc.Card_Ledger.AddObject(objCard_Ledger);

            }
            dc.SaveChanges();
            return TMID;
        }

        public List<MenuCart> get_PrintingMenuCart(long tmid)
        {
            var qry = from td in dc.Transaction_Details
                      join im in dc.Item_Master on td.Item_ID equals im.Item_ID
                      where td.Transaction_Master_ID == tmid
                      select new
                      {

                          td.Item_Quantity,
                          td.Item_Unit_Price,
                          td.Item_Total_Cost,
                          im.Item_Name


                      };

            MenuCart objMenuCart = new MenuCart();
            List<MenuCart> lstMenuCart = new List<MenuCart>();
            foreach (var item in qry)
            {
                objMenuCart = new MenuCart();
                objMenuCart.Item_Name = item.Item_Name;
                objMenuCart.Quantity = Convert.ToInt32(item.Item_Quantity);
                objMenuCart.Item_Unit_Price = Convert.ToDouble(item.Item_Unit_Price);
                objMenuCart.Item_Total = (objMenuCart.Item_Unit_Price * objMenuCart.Quantity);
                lstMenuCart.Add(objMenuCart);
            }

            return lstMenuCart;
        }

        public List<Report> getTransactionReport(int EmployeeID, DateTime Fromdt, DateTime Todt)
        {
            Fromdt = Convert.ToDateTime(Fromdt.ToShortDateString());
            Todt = Convert.ToDateTime(Todt.ToShortDateString()).Add(ts);


            List<Report> lstReport = new List<Report>();
            Report objReport = new Report();
            var qry = (from tm in dc.Transaction_Master
                       join em in dc.Employee_Master on tm.Employee_ID equals em.Employee_ID
                       where tm.Employee_ID == (EmployeeID == 0 ? tm.Employee_ID : EmployeeID)
                       && tm.Transaction_Date >= Fromdt
                       && tm.Transaction_Date <= Todt
                       select new
                       {
                           tm.Transaction_Date,
                           tm.Transaction_Master_ID,
                           tm.Order_No,
                           tm.RFID_No,
                           tm.Transaction_Amount,
                           tm.Employee_ID,
                           tm.Created_By,
                           em.Employee_Name
                       }).ToList();

            foreach (var item in qry)
            {
                objReport = new Report();

                objReport.Transaction_Date = Convert.ToDateTime(item.Transaction_Date);
                objReport.Transaction_Master_ID = item.Transaction_Master_ID;
                objReport.Order_No = item.Order_No;
                objReport.RFID_No = item.RFID_No;
                objReport.Transaction_Amount = Convert.ToDouble(item.Transaction_Amount);
                objReport.Employee_ID = Convert.ToInt32(item.Employee_ID);
                objReport.Created_By = Convert.ToInt32(item.Created_By);
                objReport.Employee_Name = item.Employee_Name;
                lstReport.Add(objReport);

            }

            return lstReport;



        }

        public List<Load_Report> getLoadReport(int EmployeeID, DateTime Fromdt, DateTime Todt)
        {
            Fromdt = Convert.ToDateTime(Fromdt.ToShortDateString());
            Todt = Convert.ToDateTime(Todt.ToShortDateString()).Add(ts);

            List<Load_Report> lstReport = new List<Load_Report>();
            Load_Report objReport = new Load_Report();
            var qry = (from cd in dc.Card_Details
                       join em in dc.Employee_Master on cd.Employee_ID equals em.Employee_ID
                       where cd.Employee_ID == (EmployeeID == 0 ? cd.Employee_ID : EmployeeID)
                       && cd.Loaded_Datetime >= Fromdt
                       && cd.Loaded_Datetime <= Todt
                       select new
                       {
                           cd.Loaded_Datetime,
                           cd.Card_Detail_ID,
                           cd.RFID_No,
                           cd.Amount_Loaded,
                           cd.Closing_Balance,
                           cd.Loaded_By,
                           em.Employee_Name

                       }).ToList();


            foreach (var item in qry)
            {
                objReport = new Load_Report();

                objReport.Loaded_Datetime = Convert.ToDateTime(item.Loaded_Datetime);
                objReport.Card_Detail_ID = item.Card_Detail_ID;

                objReport.RFID_No = item.RFID_No;
                objReport.Amount_Loaded = Convert.ToDouble(item.Amount_Loaded);
                objReport.Amount_Loaded = Convert.ToDouble(item.Amount_Loaded);
                objReport.Closing_Balance = Convert.ToInt32(item.Closing_Balance);
                objReport.Loaded_By = Convert.ToInt32(item.Loaded_By);
                objReport.Employee_Name = item.Employee_Name;

                lstReport.Add(objReport);

            }

            return lstReport;
        }

        #region RevisedCode
        public String SubmitOrder()
        {
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
                throw new Exception("Invalid Order.");

            objTM.Transaction_Date = DateTime.Now;
            objTM.Transaction_Amount = DAL_Item_Master.MenuCart.Sum(a => a.Item_Total);
            objTM.Created_By = TerminalCommon.LoggedInUser.User_ID;
            objTM.Created_DateTime = DateTime.Now;
            objTM.Updated_By = TerminalCommon.LoggedInUser.User_ID;
            objTM.Updated_DateTime = DateTime.Now;
            objTM.Company_ID = Convert.ToInt32(TerminalCommon.LoggedInUser.Company_ID);

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
            return objTM.Order_No;
        }
        /// <summary>
        /// Get today's sale.
        /// </summary>
        /// <returns></returns>
        public double _getTodaysSale()
        {
            return (from d in dc.Transaction_Master
                    where EntityFunctions.TruncateTime(d.Transaction_Date) == DateTime.Today
                    select d.Transaction_Amount.Value).Sum();
        }

        public List<sp_favorite_items_Result> _getFavoriteItems()
        {
             return dc.sp_favorite_items_function().ToList();
        }

        #endregion
    }
}
