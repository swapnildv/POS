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
using System.Reflection;
using MegabiteEntityLayer;
using POS_Business;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace Hotel_POS
{
    /// <summary>
    /// Interaction logic for CardLoadWindow.xaml
    /// </summary>
    public partial class CardLoadWindow : Window
    {
        public BL_Card objBL_Card = new BL_Card();
        public Int32 User_ID;
        public Int32 Role_ID, Company_Id;
        BackgroundWorker bg;

        #region Validations
        public void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex("[^0-9^.]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion

        #region Events

        public CardLoadWindow()
        {
            InitializeComponent();


        }

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

        private void amounttextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (amounttextBox.Text.Length > 0)
            {
                btnLoad.IsEnabled = true;

            }
            else
            {
                btnLoad.IsEnabled = false;

            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearTextBox();
                btnLoad.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (amounttextBox.Text.Trim() != "" && CardUIDTextBox.Text.Trim() != "")
                {
                    MessageBoxResult x = MessageBox.Show("Are You Sure To Load Card ?", "Load Card", MessageBoxButton.YesNo);
                    if (x == MessageBoxResult.Yes)
                    {
                        blockButton(false);
                        progressGrid.Visibility = System.Windows.Visibility.Visible;

                        bg = new BackgroundWorker();
                        bg.DoWork += new DoWorkEventHandler(Load_Amount);
                        bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);



                        bg.RunWorkerAsync();
                        btnLoad.IsEnabled = false;
                    }
                }
                else
                {

                    MessageBox.Show("Invalid Data", "Load Card");
                    ClearTextBox();
                    blockButton(true);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ClearTextBox();
                blockButton(true);

            }
        }

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                blockButton(false);
                progressGrid.Visibility = System.Windows.Visibility.Visible;

                bg = new BackgroundWorker();
                bg.DoWork += new DoWorkEventHandler(ReadCard);
                bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);



                bg.RunWorkerAsync();


                btnLoad.IsEnabled = true;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ClearTextBox();
                blockButton(true);
            }
            blockButton(true);
        }

        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressGrid.Visibility = System.Windows.Visibility.Collapsed;
            blockButton(true);
        }

        #endregion

        #region Methods

        private void Load_Amount(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
             {
                 string rfid = CardUIDTextBox.Text.Trim();
                 int card_id = Convert.ToInt32(lblCard_ID.Content);
                 String msg = "";
                 double current_bal = Convert.ToDouble(current_BalanceTextBox.Text.Trim());
                 double recharge_amount = Convert.ToDouble(amounttextBox.Text.Trim());
                 if (Convert.ToInt64(recharge_amount) > 0)
                 {
                     Card_Details objCard_Details = new Card_Details();
                     objCard_Details.Card_ID = Convert.ToInt64(lblCard_ID.Content);
                     objCard_Details.Loaded_By = User_ID;
                     objCard_Details.Loaded_Datetime = DateTime.Now;
                     objCard_Details.RFID_No = rfid;
                     objCard_Details.Amount_Loaded = (recharge_amount);
                     objCard_Details.Employee_ID = Convert.ToInt32(employee_IDTextBox.Text);
                     objCard_Details.Closing_Balance = (recharge_amount + current_bal);
                     objCard_Details.Created_By = User_ID;
                     objCard_Details.Updated_By = User_ID;
                     objCard_Details.Created_DateTime = DateTime.Now;
                     objCard_Details.Updated_DateTime = DateTime.Now;
                     objCard_Details.Company_ID = Company_Id;
                     msg = objBL_Card.Load_Card_Amount(objCard_Details, current_bal);
                     try
                     {
                         if (Convert.ToInt32(msg) > 0)
                         {
                             amounttextBox.Text = "0";
                             msg = "Amount Loaded Successfully";
                         }

                     }
                     catch { }
                 }
                 else
                 {
                     msg = "Amount Must Be Greater Than Zero(0)";

                 }

                 MessageBox.Show(msg);
                 //  btnRead_Click(btnRead, null);


                 bg = new BackgroundWorker();
                 bg.DoWork += new DoWorkEventHandler(ReadCard);
                 bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);

                 progressGrid.Visibility = System.Windows.Visibility.Visible;


                 bg.RunWorkerAsync();

             }));
        }






        private void ReadCard(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            RFID_HW objRFID = new RFID_HW();
            string errorStatus = objRFID.get_RFID();
            if (!errorStatus.Contains("Error"))
            {

                BlockData objBlockData = new BlockData();
                objBlockData = objBL_Card.Read_Card();
                if (!String.IsNullOrEmpty(objBlockData.empid))
                {
                    if (objBlockData.isactive == "1")
                    {
                        Card objCard = new Card();
                        objCard = objBL_Card.get_EmployeeWithCard_Details(objBlockData.empid, objBlockData.rfid);

                        this.Dispatcher.Invoke((Action)(() =>
                        {


                            Employee_NameTextBox.Text = objCard.Employee_Name;
                            company_NameTextBox.Text = objCard.Company_Name;
                            departmentTextBox.Text = objCard.Department;
                            emailTextBox.Text = objCard.Email;
                            mobileTextBox.Text = objCard.Phone;
                            // String bal = objBlockData.balance;
                            CardUIDTextBox.Text = objBlockData.rfid;
                            assigned_DateTextBox.Text = objCard.Card_Assigned_Date.ToShortDateString().ToString();
                            current_BalanceTextBox.Text = objBlockData.balance;
                            employee_IDTextBox.Text = objCard.Employee_ID.ToString();
                            isActiveTextBox.Text = objCard.Is_Active.ToString();
                            lblCard_ID.Content = objCard.Card_ID.ToString();
                        }));
                    }
                    else
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            MessageBox.Show("This Card is Blocked");
                        }));
                    }
                }
                else
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        MessageBox.Show("This Card is Not Assigned");
                    }));

                }

            }
            else
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    MessageBox.Show(errorStatus, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
                    ClearTextBox();
                }));
            }

        }

        //private void ReadCard(object sender, System.ComponentModel.DoWorkEventArgs e)
        //{
        //    BlockData objBlockData = new BlockData();
        //    objBlockData = objBL_Card.Read_Card();
        //    if (!String.IsNullOrEmpty(objBlockData.empid))
        //    {
        //        if (objBlockData.isactive == "1")
        //        {
        //            Card objCard = new Card();
        //            objCard = objBL_Card.get_EmployeeWithCard_Details(objBlockData.empid, objBlockData.rfid);

        //            this.Dispatcher.Invoke((Action)(() =>
        //            {


        //                Employee_NameTextBox.Text = objCard.Employee_Name;
        //                company_NameTextBox.Text = objCard.Company_Name;
        //                departmentTextBox.Text = objCard.Department;
        //                emailTextBox.Text = objCard.Email;
        //                mobileTextBox.Text = objCard.Phone;
        //                // String bal = objBlockData.balance;
        //                CardUIDTextBox.Text = objBlockData.rfid;
        //                assigned_DateTextBox.Text = objCard.Card_Assigned_Date.ToString();
        //                current_BalanceTextBox.Text = objBlockData.balance;
        //                employee_IDTextBox.Text = objCard.Employee_ID.ToString();
        //                isActiveTextBox.Text = objCard.Is_Active.ToString();
        //                lblCard_ID.Content = objCard.Card_ID.ToString();
        //            }));
        //        }
        //        else
        //        {
        //            this.Dispatcher.Invoke((Action)(() =>
        //            {
        //                MessageBox.Show("This Card is Blocked");
        //            }));
        //        }
        //    }
        //    else
        //    {
        //        this.Dispatcher.Invoke((Action)(() =>
        //        {
        //            MessageBox.Show("This Card is Not Assigned");
        //        }));

        //    }



        //}

        public void ClearTextBox()
        {
            foreach (TextBox child in Common.FindVisualChildren<TextBox>(root))
            {
                child.Text = string.Empty;
            }
        }

        public void blockButton(bool flag)
        {

            foreach (Button child in Common.FindVisualChildren<Button>(root))
            {
                child.IsEnabled = flag;
            }
        }

        #endregion








    }
}
