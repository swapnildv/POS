using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using POS_Business;
using MegabiteEntityLayer;
using System.Collections;
using System.ComponentModel;
using Hotel_POS.Resource;
using log4net;
namespace Hotel_POS.Reports
{
    /// <summary>
    /// Interaction logic for TransactionReport.xaml
    /// </summary>
    /// 
    public class Reports : List<Report> { }
    public partial class TransactionReport : Window
    {
        #region Variables
        private static readonly ILog _logger =
    LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        BL_EmployeeMaster objEM = new BL_EmployeeMaster();
        BL_Transaction objTM = new BL_Transaction();

        public TransactionReport()
        {
            InitializeComponent();
        }
        #endregion

        private void TransactionReport_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                FromDate.SelectedDate = DateTime.Now;
                ToDate.SelectedDate = DateTime.Now;
            }
            catch (Exception ex) { _logger.Error(ex); }
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                this.Close();

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime Fromdt = Convert.ToDateTime(FromDate.SelectedDate);
                DateTime Todt = Convert.ToDateTime(ToDate.SelectedDate);
                var report = objTM.getTransactionReport(Fromdt, Todt);
                if (report.Count > 0)
                {
                    lv_TransactionReport.ItemsSource = report;
                    totalSalesTextBlock.Text = TerminalCommon.currency + " " + report.Sum(a => a.Transaction_Amount).ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageHelper.MessageBox.ShowError(this);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageHelper.MessageBox.ShowError(this);
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lv_TransactionReport.Items.Count > 0)
                {


                    ExportToExcel<Report, Reports> s = new ExportToExcel<Report, Reports>();
                    ICollectionView view = CollectionViewSource.GetDefaultView(lv_TransactionReport.ItemsSource);
                    s.dataToPrint = (List<Report>)view.SourceCollection;
                    s.GenerateReport();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageHelper.MessageBox.ShowError(this);
            }
        }

    }
}
