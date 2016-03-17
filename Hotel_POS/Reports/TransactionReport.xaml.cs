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
        BL_EmployeeMaster objEM = new BL_EmployeeMaster();
        BL_Transaction objTM = new BL_Transaction();



        public TransactionReport()
        {
            InitializeComponent();
        }
        #endregion



        private void TransactionReport_Loaded(object sender, RoutedEventArgs e)
        {
            FromDate.SelectedDate = DateTime.Now;
            ToDate.SelectedDate = DateTime.Now;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

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
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
