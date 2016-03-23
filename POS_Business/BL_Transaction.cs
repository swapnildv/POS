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
       

       

        public List<Report> getTransactionReport(DateTime Fromdt, DateTime Todt)
        {
            DAL_Transaction objDAL_Transaction = new DAL_Transaction();
            return objDAL_Transaction.getTransactionReport(Fromdt, Todt);
        }

       


        #region RevisedCode
        public Transaction_Master SubmitOrder(double discount)
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
