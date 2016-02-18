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

namespace Hotel_POS
{
    /// <summary>
    /// Interaction logic for ClearCard.xaml
    /// </summary>
    public partial class ClearCard : Window
    {
        #region Variables

        public ClearCard()
        {
            InitializeComponent();
        }
        public BL_Card objBL_Card = new BL_Card();
        public Int32 User_ID;
        public Int32 Role_ID, Company_Id;

        #endregion

        #region Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Place Card on Reader & Press Read", "Load Card");
            User_ID = Convert.ToInt32(((Label)(this.Owner.FindName("lblUserID"))).Content.ToString());
            Role_ID = Convert.ToInt32(((Label)(this.Owner.FindName("lblRoleID"))).Content.ToString());
            Company_Id = Convert.ToInt32(((Label)(this.Owner.FindName("lblCompanyID"))).Content.ToString());
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
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Clear_Controls();
        }


        private void btnRead_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                RFID_HW objRFID = new RFID_HW();
                string errorStatus = objRFID.get_RFID();
                if (!errorStatus.Contains("Error"))
                {
                    ReadCard();
                }
                else
                {
                    MessageBox.Show(errorStatus, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RFID_HW objRFID = new RFID_HW();
                string errorStatus = objRFID.get_RFID();
                if (!errorStatus.Contains("Error"))
                {
                    MessageBoxResult x = MessageBox.Show("Are You Sure To Format Card ?", "Megabite", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    if (x == MessageBoxResult.OK)
                    {
                        objRFID.Clear_Card();
                    }
                }
                else
                {
                    MessageBox.Show(errorStatus, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        #endregion

        #region Methods

        private void ReadCard()
        {
            BlockData objBlockData = new BlockData();
            objBlockData = objBL_Card.Read_Card();
            Card objCard = new Card();
            objCard = objBL_Card.get_EmployeeWithCard_Details(objBlockData.empid, objBlockData.rfid);

            Employee_NameTextBox.Text = objCard.Employee_Name;
            company_NameTextBox.Text = objCard.Company_Name;
            departmentTextBox.Text = objCard.Department;
            emailTextBox.Text = objCard.Email;
            mobileTextBox.Text = objCard.Phone;
            // String bal = objBlockData.balance;
            CardUIDTextBox.Text = objBlockData.rfid;
            assigned_DateTextBox.Text = objCard.Card_Assigned_Date.ToString();
            current_BalanceTextBox.Text = objBlockData.balance;
            employee_IDTextBox.Text = objCard.Employee_ID.ToString();
            isActiveTextBox.Text = objCard.Is_Active.ToString();
            lblCard_ID.Content = objCard.Card_ID.ToString();

        }

        private void Clear_Controls()
        {


            Employee_NameTextBox.Text = "";
            company_NameTextBox.Text = "";
            departmentTextBox.Text = "";
            emailTextBox.Text = "";
            mobileTextBox.Text = "";
            // String bal = objBlockData.balance;
            CardUIDTextBox.Text = "";
            assigned_DateTextBox.Text = "";
            current_BalanceTextBox.Text = "";
            employee_IDTextBox.Text = "";
            isActiveTextBox.Text = "";
            lblCard_ID.Content = "";
        }
        #endregion



    }
}
