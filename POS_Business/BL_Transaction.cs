using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MegabiteEntityLayer;
using POS_DAL;

namespace POS_Business
{
    public class BL_Transaction
    {
        public Int64 Submit_Order(Transaction_Master objTM, List<Transaction_Details> lstTD)
        {
            DAL_Transaction objDAL_Transaction = new DAL_Transaction();
            return objDAL_Transaction.Submit_Order(objTM, lstTD);
        }


        public List<MenuCart> get_PrintingMenuCart(long tmid)
        {
            DAL_Transaction objDAL_Transaction = new DAL_Transaction();
            return objDAL_Transaction.get_PrintingMenuCart(tmid);
        }

        public List<Report> getTransactionReport(DateTime Fromdt, DateTime Todt)
        {
            DAL_Transaction objDAL_Transaction = new DAL_Transaction();
            return objDAL_Transaction.getTransactionReport(Fromdt, Todt);
        }

        public List<Load_Report> getLoadReport(int EmployeeID, DateTime Fromdt, DateTime Todt)
        {
            DAL_Transaction objDAL_Transaction = new DAL_Transaction();
            return objDAL_Transaction.getLoadReport(EmployeeID, Fromdt, Todt);
        }


        #region RevisedCode
        public String SubmitOrder(double discount)
        {
            return new DAL_Transaction().SubmitOrder(discount);
        }

        public double getTodaysSale()
        {
            return new DAL_Transaction()._getTodaysSale();
        }

        public List<sp_favorite_items_Result> getFavoriteItems()
        {
            return new DAL_Transaction()._getFavoriteItems();
        }

        #endregion
    }
}
