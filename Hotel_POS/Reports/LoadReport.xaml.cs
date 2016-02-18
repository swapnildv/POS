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
    /// Interaction logic for LoadReport.xaml
    /// </summary>
    /// 


    public partial class LoadReport : Window
    {
        #region Variables
        BL_EmployeeMaster objEM = new BL_EmployeeMaster();
        BL_Transaction objTM = new BL_Transaction();
        public class LoadReports : List<Load_Report> { }
        #endregion

        public LoadReport()
        {
            InitializeComponent();
        }



        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbcompany.SelectedValue == null || cmbEmployee.SelectedValue == null)
                {
                    MessageBox.Show("Please Select Company & Employee Name ", "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    Bind_TransactionReport();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);




            }
        }

        private void Load_Report_Loaded(object sender, RoutedEventArgs e)
        {
            FromDate.SelectedDate = DateTime.Now;
            ToDate.SelectedDate = DateTime.Now;
            BindCompany();
        }

        private void Bind_TransactionReport()
        {


            DateTime Fromdt = Convert.ToDateTime(FromDate.SelectedDate);
            DateTime Todt = Convert.ToDateTime(ToDate.SelectedDate);
            Int32 EmployeeID = Convert.ToInt32(((Employee_Master)(cmbEmployee.SelectedValue)).Employee_ID);
            lv_LoadReport.ItemsSource = objTM.getLoadReport(EmployeeID, Fromdt, Todt);
        }


        private void BindCompany()
        {

            cmbcompany.ItemsSource = objEM.Bindcompany();

        }

        private void cmbcompany_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                Bind_Employees();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);




            }
        }


        private void Bind_Employees()
        {
            Int32 Company_ID = Convert.ToInt32(((Company_Master)(cmbcompany.SelectedValue)).Company_ID);

            Employee_Master obj = new Employee_Master();
            obj.Employee_ID = 0;
            obj.Employee_Name = "-- Select All --";
            List<Employee_Master> lst = (objEM.getEmployeeList_By_CompanyID(Company_ID));
            lst.Add(obj);
            cmbEmployee.ItemsSource = lst.OrderBy(e => e.Employee_ID);
            cmbEmployee.SelectedIndex = 1;

        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lv_LoadReport.Items.Count > 0)
                {

                    ExportToExcel<Load_Report, LoadReports> s = new ExportToExcel<Load_Report, LoadReports>();
                    ICollectionView view = CollectionViewSource.GetDefaultView(lv_LoadReport.ItemsSource);
                    s.dataToPrint = (List<Load_Report>)view.SourceCollection;
                    s.GenerateReport();
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
    }
}
