using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
namespace Hotel_POS
{
    /// <summary>
    /// Interaction logic for CardAssignWindow.xaml
    /// </summary>
    public partial class CardAssignWindow : Window
    {

        #region Variables

        public Int32 User_ID;
        public Int32 Role_ID, Company_Id;
        BackgroundWorker bg;
        #endregion

        #region Events


        public CardAssignWindow()
        {
            InitializeComponent();
            Bind_Employee();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(EmployeeGridView.ItemsSource);
            view.Filter = UserFilter;
        }

        private void lstEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Card objCard = new Card();

                if (EmployeeGridView.SelectedValue == null)
                {
                    foreach (Card item in EmployeeGridView.Items)
                    {
                        if (item.Employee_ID.ToString().Trim() == employee_IDTextBox.Text.Trim())
                        {
                            EmployeeGridView.SelectedValue = item;
                        }

                    }
                }
                objCard = (Card)EmployeeGridView.SelectedValue;
                if (objCard != null)
                {
                    Employee_NameTextBox.Text = objCard.Employee_Name;
                    company_NameTextBox.Text = objCard.Company_Name;
                    departmentTextBox.Text = objCard.Department;
                    emailTextBox.Text = objCard.Email;
                    mobileTextBox.Text = objCard.Phone;
                    employee_IDTextBox.Text = objCard.Employee_ID.ToString();
                    if (objCard.RFID_No != null)
                    {

                        if (objCard.RFID_No.Trim() != "" || objCard.Card_ID != null)
                        {
                            CardUIDTextBox.Text = objCard.RFID_No;
                            assigned_DateTextBox.Text = objCard.Card_Assigned_Date.ToShortDateString();
                            current_BalanceTextBox.Text = objCard.Current_Balance.ToString().Replace(" ", "0");
                            employee_IDTextBox.Text = objCard.Employee_ID.ToString();
                            isActiveTextBox.Text = objCard.Is_Active.ToString();

                            BorderCardDetails.Visibility = System.Windows.Visibility.Visible;
                            btnAssign.Visibility = System.Windows.Visibility.Collapsed;
                            if (objCard.Is_Active.ToString().ToLower() == "false")
                            {
                                btnBlock.Visibility = System.Windows.Visibility.Collapsed;
                                btnActivate.Visibility = System.Windows.Visibility.Visible;
                            }
                            else
                            {
                                btnBlock.Visibility = System.Windows.Visibility.Visible;
                                btnActivate.Visibility = System.Windows.Visibility.Collapsed;
                            }
                        }
                    }
                    else
                    {
                        CardUIDTextBox.Text = "";
                        assigned_DateTextBox.Text = "";
                        current_BalanceTextBox.Text = "";


                        isActiveTextBox.Text = "";
                        BorderCardDetails.Visibility = System.Windows.Visibility.Collapsed;

                        btnAssign.Visibility = System.Windows.Visibility.Visible;
                        btnBlock.Visibility = System.Windows.Visibility.Collapsed;
                        btnActivate.Visibility = System.Windows.Visibility.Collapsed;

                    }
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                User_ID = Convert.ToInt32(((Label)(this.Owner.FindName("lblUserID"))).Content.ToString());
                Role_ID = Convert.ToInt32(((Label)(this.Owner.FindName("lblRoleID"))).Content.ToString());
                Company_Id = Convert.ToInt32(((Label)(this.Owner.FindName("lblRoleID"))).Content.ToString());
                BorderCardDetails.Visibility = System.Windows.Visibility.Collapsed;
                btnAssign.Visibility = System.Windows.Visibility.Collapsed;
                btnBlock.Visibility = System.Windows.Visibility.Collapsed;
                btnActivate.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressGrid.Visibility = System.Windows.Visibility.Collapsed;
            blockButton(true);
        }
        private void btnAssign_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                blockButton(false);
                progressGrid.Visibility = System.Windows.Visibility.Visible;
                bg = new BackgroundWorker();
                bg.DoWork += new DoWorkEventHandler(Assign_Card_Disptcher);
                bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);


                bg.RunWorkerAsync();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
                blockButton(true);
            }


        }

        private void btnBlock_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                progressGrid.Visibility = System.Windows.Visibility.Visible;
                bg = new BackgroundWorker();
                bg.DoWork += new DoWorkEventHandler(Block_Card_Disptcher);
                bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);

                bg.RunWorkerAsync();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
                blockButton(true);
            }

        }


        private void btnActivate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                progressGrid.Visibility = System.Windows.Visibility.Visible;
                bg = new BackgroundWorker();
                bg.DoWork += new DoWorkEventHandler(ReActivate_Card);
                bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);

                bg.RunWorkerAsync();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }


        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(EmployeeGridView.ItemsSource).Refresh();
        }
        #endregion

        #region Methods

        private void Assign_Card_Disptcher(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                RFID_HW objRFID = new RFID_HW();
                string rfid = objRFID.get_RFID();
                string msg;

                if (objRFID.isError.Contains(rfid))
                {
                    msg = rfid;

                    MessageBox.Show(msg, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    btnAssign.Visibility = System.Windows.Visibility.Collapsed;
                    AssignCard(rfid);
                    Bind_Employee();

                }
            }));


        }


        private void AssignCard(string rfid)
        {
            string msg;
            BL_Card objBLCard = new BL_Card();
            if (objBLCard.check_isAlreadyAssigned(rfid))
            {

                msg = rfid + " is Already Used !";

            }
            else
            {
                msg = "Are You Sure to Assign Card : " + rfid + " To " + Employee_NameTextBox.Text;

                MessageBoxResult x = MessageBox.Show(msg, "Assign Card", MessageBoxButton.OKCancel);

                if (x == MessageBoxResult.OK)
                {
                    BlockData obj = new BlockData();
                    obj.balance = "0";
                    obj.company = Company_Id.ToString();
                    obj.empid = employee_IDTextBox.Text;
                    string[] name = Employee_NameTextBox.Text.Split(' ');

                    if (name.Length > 0)
                    {
                        obj.fname = name[0];
                    }
                    else
                    {
                        obj.fname = " ";
                    }
                    if (name.Length > 1)
                    {
                        obj.lname = name[1];
                    }
                    else
                    {
                        obj.lname = " ";
                    }


                    obj.isactive = "1";

                    msg = objBLCard.AssignCard(obj);
                    if (msg == "111111")
                    {
                        Card_Master objCard = new Card_Master();
                        MainWindow objMainWindow = new MainWindow();
                        objCard.Card_Status_ID = 2;
                        objCard.RFID_No = rfid;
                        objCard.Employee_ID = Convert.ToInt32(employee_IDTextBox.Text);
                        objCard.Current_Balance = 0.0;
                        objCard.Card_Assigned_Date = DateTime.Now;
                        objCard.Is_Active = true;
                        objCard.Created_DateTime = DateTime.Now;
                        objCard.Created_By = User_ID;
                        objCard.Updated_By = User_ID;
                        objCard.Updated_DateTime = DateTime.Now;
                        objCard.Company_ID = Company_Id;
                        if (objBLCard.Insert_Card_Master(objCard) > 0)
                        {
                            msg = "Card : " + rfid + " Assigned To " + Employee_NameTextBox.Text; ;

                        }
                        else
                        {
                            msg = "Error In Data Submission In DataBase! ";
                            objBLCard.Clear_Card();
                            btnAssign.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                    else
                    {
                        objBLCard.Clear_Card();
                        btnAssign.Visibility = System.Windows.Visibility.Visible;
                    }

                }

            }

            MessageBox.Show(msg);
        }


        private void Bind_Employee()
        {
            BL_Card obj = new BL_Card();
            EmployeeGridView.ItemsSource = obj.get_EmployeeWithCard_Details();
        }

        private void Block_Card_Disptcher(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
           {
               RFID_HW objRFID = new RFID_HW();
               string rfid = objRFID.get_RFID();
               string msg;

               if (!objRFID.isError.Contains(rfid))
               {
                   if (rfid == CardUIDTextBox.Text.Trim())
                   {
                       MessageBoxResult x = MessageBox.Show("Are You Sure To Block Card : " + rfid + "?", "Megabite", MessageBoxButton.YesNo, MessageBoxImage.Question);
                       if (x == MessageBoxResult.Yes)
                       {
                           msg = Block_Card(rfid.Trim(), "0");
                           if ((Block_Card_from_DB(CardUIDTextBox.Text.Trim(), false) > 0) && msg == "1")
                           {

                               MessageBox.Show("Card Blocked Successfully !", "Megabite", MessageBoxButton.OK, MessageBoxImage.Information);
                               Bind_Employee();
                           }
                       }


                   }
                   else
                   {
                       MessageBox.Show("Card Placed On Reader Dose Not Belong To Selected Employee !", "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
                   }
               }
               else
               {
                   MessageBoxResult x = MessageBox.Show(rfid + ", Do You Want To Block From Database Only ?", "Megabite", MessageBoxButton.YesNo, MessageBoxImage.Error);
                   if (x == MessageBoxResult.Yes)
                   {
                       if ((Block_Card_from_DB(CardUIDTextBox.Text.Trim(), false) > 0))
                       {

                           MessageBox.Show("Card Blocked Successfully (Only From DataBase) !", "Megabite", MessageBoxButton.OK, MessageBoxImage.Information);
                           Bind_Employee();
                       }
                   }
               }
           }));

        }

        private string Block_Card(string rfid, string status)
        {
            RFID_HW objRfid = new RFID_HW();
            return objRfid.WriteDataBlock("2", status);
        }

        private int Block_Card_from_DB(string rfid, Boolean status)
        {
            BL_Card obj = new BL_Card();
            return obj.Block_Card_from_DB(rfid, employee_IDTextBox.Text.Trim(), status);
        }

        private void ReActivate_Card(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            this.Dispatcher.Invoke((Action)(() =>
            {
                RFID_HW objRFID = new RFID_HW();
                string rfid = objRFID.get_RFID();
                string msg;

                if (!objRFID.isError.Contains(rfid))
                {
                    if (rfid == CardUIDTextBox.Text.Trim())
                    {
                        MessageBoxResult x = MessageBox.Show("Are You Sure To Activate Card : " + rfid + "?", "Megabite", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (x == MessageBoxResult.Yes)
                        {
                            msg = Block_Card(rfid.Trim(), "1");
                            if ((Block_Card_from_DB(CardUIDTextBox.Text.Trim(), true) > 0) && msg == "1")
                            {

                                MessageBox.Show("Card Activated Successfully !", "Megabite", MessageBoxButton.OK, MessageBoxImage.Information);
                                Bind_Employee();
                            }
                        }


                    }
                    else
                    {
                        MessageBox.Show("Card Placed On Reader Dose Not Belong To Selected Employee !", "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBoxResult x = MessageBox.Show(rfid + ", Do You Want To Activate From Database Only ?", "Megabite", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (x == MessageBoxResult.Yes)
                    {
                        if ((Block_Card_from_DB(CardUIDTextBox.Text.Trim(), true) > 0))
                        {

                            MessageBox.Show("Card Activated Successfully (Only From DataBase) !", "Megabite", MessageBoxButton.OK, MessageBoxImage.Information);
                            Bind_Employee();
                        }
                    }
                }

            }));


        }



        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtSearch.Text))
                return true;
            else
                return ((item as Card).Employee_Name.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }


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
