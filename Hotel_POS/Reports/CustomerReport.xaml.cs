using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using MegabiteEntityLayer;
using POS_Business;

namespace Hotel_POS.Reports
{
    public class customerExport : List<Customer_Master> {
        
    }
    /// <summary>
    /// Interaction logic for CustomerReport.xaml
    /// </summary>
    public partial class CustomerReport : Window
    {
        public CustomerReport()
        {
            InitializeComponent();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(customerDataGrid.ItemsSource);
                view.Filter = UserFilter;
                CollectionViewSource.GetDefaultView(customerDataGrid.ItemsSource).Refresh();
            }
            catch (Exception)
            {
               
            }
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtSearch.Text))
                return true;
            else
                return ((item as Customer_Master).cust_Name.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void btnGetCustomers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                customerDataGrid.ItemsSource = new BL_Customer().getCustomers();
            }
            catch (Exception) { }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (customerDataGrid.Items.Count > 0)
                {
                    object[] header = { "cust_id", "cust_Name", "cust_MobileNo" };
                    ExportToExcel<Customer_Master, customerExport> s = new ExportToExcel<Customer_Master, customerExport>();
                    ICollectionView view = CollectionViewSource.GetDefaultView(customerDataGrid.ItemsSource);
                    s.header = header;
                    s.dataToPrint = (List<Customer_Master>)view.SourceCollection;
                    s.GenerateReport();
                }
            }
            catch (Exception) { }
        }
    }
}
