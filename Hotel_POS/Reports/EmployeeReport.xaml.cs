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
using System.ComponentModel;

namespace Hotel_POS.Reports
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class EmployeeReport : Window
    {
        #region Variables

        BL_EmployeeMaster objEM = new BL_EmployeeMaster();

        public class Card_Ledgers : List<Card_Ledger> { }

        #endregion

        public EmployeeReport()
        {
            InitializeComponent();
        }

        private void CloseCommand_Executed(object sender, RoutedEventArgs e)
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
                if (cmbcompany_ComboBox.SelectedValue == null || cmbEmployee.SelectedValue == null)
                {
                    MessageBox.Show("Please Select Company & Employee Name", "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    Bind_EmployeeLedger();
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);




            }
        }

        private void Bind_EmployeeLedger()
        {
            BL_Card objCL = new BL_Card();
            DateTime Fromdt = Convert.ToDateTime(FromDate.SelectedDate);
            DateTime Todt = Convert.ToDateTime(ToDate.SelectedDate);
            Int32 EmployeeID = Convert.ToInt32(((Employee_Master)(cmbEmployee.SelectedValue)).Employee_ID);
            lv_EmployeeLedger.ItemsSource = objCL.getEmployeeLedger(EmployeeID, Fromdt, Todt);
        }

        private void EmployeeLedger_Loaded(object sender, RoutedEventArgs e)
        {
            FromDate.SelectedDate = DateTime.Now;
            ToDate.SelectedDate = DateTime.Now;
            BindCompany();
        }
        private void BindCompany()
        {

            cmbcompany_ComboBox.ItemsSource = objEM.Bindcompany();

        }


        private void cmbcompany_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                BindEmployeeList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);




            }

        }

        private void BindEmployeeList()
        {
            try
            {
                Int32 Company_ID = Convert.ToInt32(((Company_Master)(cmbcompany_ComboBox.SelectedValue)).Company_ID);

                cmbEmployee.ItemsSource = objEM.getEmployeeList_By_CompanyID(Company_ID);

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
                if (lv_EmployeeLedger.Items.Count > 0)
                {
                    //Card_Ledgers : List<Card_Ledger> { }
                    ExportToExcel<Card_Ledger, Card_Ledgers> s = new ExportToExcel<Card_Ledger, Card_Ledgers>();
                    ICollectionView view = CollectionViewSource.GetDefaultView(lv_EmployeeLedger.ItemsSource);
                    s.dataToPrint = (List<Card_Ledger>)view.SourceCollection;
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
