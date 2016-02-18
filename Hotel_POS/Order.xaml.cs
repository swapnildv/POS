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
using System.Text.RegularExpressions;
using System.Windows.Media.Animation;
using System.Collections;
using System.ComponentModel;

namespace Hotel_POS
{
    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class Order : Window
    {
        #region Variables
        public Order()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        RFID_HW objRFID = new RFID_HW();
        public string errorStatus;
        public Int32 User_ID;
        public Int32 Role_ID, Company_ID;
        BackgroundWorker bg;

        #endregion

        #region Events

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                User_ID = Convert.ToInt32(((Label)(this.Owner.FindName("lblUserID"))).Content.ToString());
                Role_ID = Convert.ToInt32(((Label)(this.Owner.FindName("lblRoleID"))).Content.ToString());
                Company_ID = Convert.ToInt32(((Label)(this.Owner.FindName("lblCompanyID"))).Content.ToString());
                Bind_MenuGroup();
                Bind_MenuList();
                if (lvMenuCard.Items.Count > 0)
                {
                    btnRemoveItem.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

            }


        }

        private void cmbMenuGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Bind_MenuList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

            }


        }

        private void ListMenu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (ListBoxMenu.Items.Count > 0)
                {
                    Add_Menu_To_Cart();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lvMenuCard.Items.Count > 0)
                {

                    if (lvMenuCard.SelectedItems.Count > 0)
                    {
                        Remove_Menu_From_Cart();
                    }
                    else
                    {

                        MessageBox.Show("Please Select Item(s) to Remove !", "Megabite", MessageBoxButton.OK, MessageBoxImage.Warning);

                    }
                }
                else
                {

                    MessageBox.Show("Please Select Item(s)", "Megabite", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

            }





        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            try
            {
                e.Text.DefaultIfEmpty('1');
                Regex regex = new Regex("[^0-9]+");
                MyValidation objValidation = new MyValidation();

                if (objValidation.IsValidNonZeroQty(e.Text))
                {
                    e.Handled = true;
                }
                else
                {
                    e.Handled = false;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }


        private void ListView_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // get the current selected item
                if (pnlSubmit.Visibility != System.Windows.Visibility.Visible)
                {
                    var item1 = ((FrameworkElement)e.OriginalSource);

                    ListViewItem item = lvMenuCard.ItemContainerGenerator.ContainerFromIndex(lvMenuCard.SelectedIndex) as ListViewItem;
                    TextBox txtQty = null;
                    if (item != null)
                    {

                        ContentPresenter templateParent = GetFrameworkElementByName<ContentPresenter>(item);

                        DataTemplate dataTemplate = templateParent.ContentTemplate;
                        if (dataTemplate != null && templateParent != null)
                        {
                            txtQty = dataTemplate.FindName("txtQty", templateParent) as TextBox;
                            if (txtQty != null)
                            {
                                if (pnlSubmit.Visibility != System.Windows.Visibility.Visible)
                                {
                                    txtQty.IsEnabled = true;
                                }
                                else
                                {
                                    txtQty.IsEnabled = false;
                                }
                            }
                        }
                    }

                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

            }


        }

        private void txtQty_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox txtqty = ((TextBox)sender);
                if (pnlSubmit.Visibility != System.Windows.Visibility.Visible)
                {

                    if ((!string.IsNullOrEmpty(txtqty.Text.Trim())) || txtqty.Text.Trim() == "0")
                    {
                        try
                        {
                            if (Int32.Parse(txtqty.Text.Trim()) == 0)
                            {
                                txtqty.Text = "1";
                            }

                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Enter Only Numeric Value", "Megabite", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                            txtqty.Text = "1";
                        }

                    }
                    else
                    {
                        txtqty.Text = "1";
                    }
                    txtqty.IsEnabled = false;
                    BL_Menu obj = new BL_Menu();
                    txtTotal.Content = obj.get_total_amount(lvMenuCard.ItemsSource);
                    lblBillAmount.Content = txtTotal.Content;
                }
                else
                {
                    txtqty.IsEnabled = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        private void btnProceed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                blockButton(false);
                progressGrid.Visibility = System.Windows.Visibility.Visible;

                bg = new BackgroundWorker();
                bg.DoWork += new DoWorkEventHandler(proceed_dispatcher);
                bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
                bg.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
                blockButton(true);
            }
        }
        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressGrid.Visibility = System.Windows.Visibility.Collapsed;
            blockButton(true);
        }

        private void btnSubmit_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                blockButton(false);
                progressGrid.Visibility = System.Windows.Visibility.Visible;

                bg = new BackgroundWorker();
                bg.DoWork += new DoWorkEventHandler(PayNow_dispatcher);
                bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
                bg.RunWorkerAsync();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
                blockButton(true);

            }

        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListBoxMenu.ItemsSource);
                view.Filter = UserFilter;

                CollectionViewSource.GetDefaultView(ListBoxMenu.ItemsSource).Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        private void btnbackToMenu_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                clear_Card_Details();
                pnlMenu.Width = 470;
                pnlPayment.Visibility = System.Windows.Visibility.Collapsed;
                btnShow.IsEnabled = false;
                btnHide.IsEnabled = false;
                ((Storyboard)this.Resources["StoryboardProductsShow"]).Begin(this);
                btnRemoveItem.Visibility = System.Windows.Visibility.Visible;
                pnlProceed.Visibility = System.Windows.Visibility.Visible;
                pnlSubmit.Visibility = System.Windows.Visibility.Collapsed;
                pnlrbPayBy.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }



        #endregion



        #region Methods

        private void Bind_MenuGroup()
        {
            BL_Menu obj = new BL_Menu();
            cmbMenuGroup.ItemsSource = obj.get_Item_Group_List().Where(c => c.Is_Active == true);
        }

        private void Bind_MenuList()
        {
            BL_Menu obj = new BL_Menu();
            Int32 ItemTypeID = Convert.ToInt32(cmbMenuGroup.SelectedValue);


            ListBoxMenu.ItemsSource = obj.get_Menu_List(ItemTypeID).Where(m => m.Is_Active == true);
        }

        private void proceed_dispatcher(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() =>
               {
                   if (lvMenuCard.Items.Count > 0)
                   {
                       string msg;
                       //
                       //if (CustomerScreenGrid.Visibility == System.Windows.Visibility.Visible)
                       //{

                       //}
                       //else
                       //{
                       if (rbCard.IsChecked == true)
                       {
                           lblPaymentMode.Content = "Card";
                           msg = "You Have Selected To Pay By Card. Please Place Card On Reader & Press OK !";
                           MessageBoxResult x = MessageBox.Show(msg, "Megabite", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel);
                           pnlrbPayBy.Visibility = System.Windows.Visibility.Collapsed;

                           if (x == MessageBoxResult.OK)
                           {

                               errorStatus = objRFID.get_RFID();
                               if (!errorStatus.Contains("Error"))
                               {

                                   ((Storyboard)this.Resources["StoryboardProductsHide"]).Begin(this);

                                   pnlMenu.Width = 395;
                                   pnlPayment.Width = 395;
                                   pnlPayment.Visibility = System.Windows.Visibility.Visible;
                                   btnShow.IsEnabled = false;
                                   btnHide.IsEnabled = false;
                                   pnlProceed.Visibility = System.Windows.Visibility.Collapsed;
                                   pnlSubmit.Visibility = System.Windows.Visibility.Visible;

                                   btnRemoveItem.Visibility = System.Windows.Visibility.Collapsed;

                                   Row_1.Height = GridLength.Auto;
                                   Row_2.Height = GridLength.Auto;
                                   Row_3.Height = GridLength.Auto;
                                   Row_4.Height = GridLength.Auto;
                                   Row_5.Height = GridLength.Auto;
                                   get_Card_Datails();
                               }
                               else
                               {
                                   MessageBox.Show(errorStatus, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
                                   pnlrbPayBy.Visibility = System.Windows.Visibility.Visible;

                               }
                           }


                       }
                       else
                       {
                           msg = "You Have Selected To Pay By Cash. Are You Sure To Proceed !";
                           lblPaymentMode.Content = "Cash";
                           MessageBoxResult x = MessageBox.Show(msg, "Megabite", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
                           if (x == MessageBoxResult.OK)
                           {
                               ((Storyboard)this.Resources["StoryboardProductsHide"]).Begin(this);
                               btnRemoveItem.Visibility = System.Windows.Visibility.Collapsed;
                               pnlMenu.Width = 395;
                               pnlPayment.Width = 395;
                               pnlPayment.Visibility = System.Windows.Visibility.Visible;
                               btnShow.IsEnabled = false;
                               btnHide.IsEnabled = false;
                               pnlProceed.Visibility = System.Windows.Visibility.Collapsed;
                               pnlSubmit.Visibility = System.Windows.Visibility.Visible;

                               lblDate.Content = DateTime.Now.ToString();
                               lblEmaployeeName.Content = "";
                               lblCompanyName.Content = "";
                               lblCardID.Content = "";
                               lblBalance.Content = "";
                               lblBillAmount.Content = txtTotal.Content;
                               lblEmployeeID.Content = "";
                               lbldbCardID.Content = "";


                               Row_1.Height = new GridLength(0);
                               Row_2.Height = new GridLength(0);
                               Row_3.Height = new GridLength(0);
                               Row_4.Height = new GridLength(0);

                           }
                       }
                       //}

                   }
                   else
                   {
                       MessageBox.Show("Please Select Item(s)", "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
                   }

               }));

        }

        private void Add_Menu_To_Cart()
        {
            BL_Menu obj = new BL_Menu();
            if (lvMenuCard.Items.Count > 0)
            {


                Int32 ItemID = Convert.ToInt32(ListBoxMenu.SelectedValue.ToString());
                System.Collections.IEnumerable oldObj = lvMenuCard.ItemsSource;


                lvMenuCard.ItemsSource = obj.get_Item_Details_By_Item_ID(ItemID, oldObj);

            }
            else
            {

                Int32 ItemID = Convert.ToInt32(ListBoxMenu.SelectedValue.ToString());
                lvMenuCard.ItemsSource = obj.get_Item_Details_By_Item_ID(ItemID);

            }
            if (lvMenuCard.Items.Count > 0)
            {
                borderOrder.Visibility = System.Windows.Visibility.Visible;
                pnlrbPayBy.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                borderOrder.Visibility = System.Windows.Visibility.Collapsed;
                pnlrbPayBy.Visibility = System.Windows.Visibility.Collapsed;
            }
            txtTotal.Content = obj.get_total_amount(lvMenuCard.ItemsSource);
            lblBillAmount.Content = txtTotal.Content;
        }

        private void Remove_Menu_From_Cart()
        {

            BL_Menu obj = new BL_Menu();
            if (lvMenuCard.Items.Count > 0)
            {
                System.Collections.IEnumerable objAllMenus = lvMenuCard.ItemsSource;
                System.Collections.IEnumerable objSelected = lvMenuCard.SelectedItems;
                lvMenuCard.ItemsSource = obj.Remove_Item_From_Cart(objAllMenus, objSelected);
            }

            if (lvMenuCard.Items.Count > 0)
            {
                borderOrder.Visibility = System.Windows.Visibility.Visible;
                pnlrbPayBy.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                borderOrder.Visibility = System.Windows.Visibility.Collapsed;
                pnlrbPayBy.Visibility = System.Windows.Visibility.Collapsed;
            }
            txtTotal.Content = obj.get_total_amount(lvMenuCard.ItemsSource);
            lblBillAmount.Content = txtTotal.Content;
        }

        private void get_Card_Datails()
        {


            BlockData objBlockData = new BlockData();
            BL_Card objBL_Card = new BL_Card();

            objBlockData = objBL_Card.Read_Card();
            if (objBlockData.isactive == "1")
            {
                Card objCard = new Card();
                objCard = objBL_Card.get_EmployeeWithCard_Details(objBlockData.empid, objBlockData.rfid);
                if (objCard.Is_Active == true)
                {

                    double dbBalance = objCard.Current_Balance;
                    double cardbalance = Convert.ToDouble(objBlockData.balance);
                    double bill = Convert.ToDouble(txtTotal.Content.ToString());
                    dbBalance = cardbalance;
                    if (dbBalance == cardbalance)
                    {
                        if (cardbalance >= bill)
                        {
                            lblDate.Content = DateTime.Now.ToString();
                            lblEmaployeeName.Content = objCard.Employee_Name;
                            lblCompanyName.Content = objCard.Company_Name;
                            lblCardID.Content = objBlockData.rfid;
                            lblBalance.Content = objBlockData.balance;
                            lblBillAmount.Content = txtTotal.Content;
                            lblEmployeeID.Content = objCard.Employee_ID;
                            lbldbCardID.Content = objCard.Card_ID;
                        }
                        else
                        {
                            clear_Card_Details();
                            MessageBox.Show("Your Card Balance  is Less Than Bill Amount, Kindly Make Cash Payment!", "Megabite", MessageBoxButton.OK, MessageBoxImage.Information);
                            btnbackToMenu_Click_1(this, null);
                        }
                    }
                    else
                    {
                        clear_Card_Details();
                        MessageBox.Show("Balance Miss Match, Please Contact to Admin.", "Megabite", MessageBoxButton.OK, MessageBoxImage.Information);
                        btnbackToMenu_Click_1(this, null);
                    }
                }
                else
                {
                    clear_Card_Details();
                    MessageBox.Show("This Card is Blocked, Kindly Make Cash Payment!");
                    btnbackToMenu_Click_1(this, null);
                }

            }
            else
            {

                MessageBox.Show("This Card is Blocked, Kindly Make Cash Payment!");
                btnbackToMenu_Click_1(this, null);
            }

        }

        private void clear_Card_Details()
        {
            lblCardID.Content = "";
            lblBalance.Content = "";
            lblDate.Content = DateTime.Now.ToString();
            lblEmaployeeName.Content = "";
            lblBillAmount.Content = txtTotal.Content;
            lblCompanyName.Content = "";

        }
        private void clear_Form()
        {
            lblCardID.Content = "";
            lblBalance.Content = "";
            lblDate.Content = DateTime.Now.ToString();
            lblEmaployeeName.Content = "";
            lblBillAmount.Content = txtTotal.Content;
            lblCompanyName.Content = "";
            txtTotal.Content = "0";
            lvMenuCard.ItemsSource = null;
            pnlMenu.Width = 470;
            pnlPayment.Visibility = System.Windows.Visibility.Collapsed;
            btnShow.IsEnabled = false;
            btnHide.IsEnabled = false;
            ((Storyboard)this.Resources["StoryboardProductsShow"]).Begin(this);
            pnlProceed.Visibility = System.Windows.Visibility.Visible;
            pnlSubmit.Visibility = System.Windows.Visibility.Collapsed;
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtSearch.Text))
                return true;
            else
                return ((item as Item_Master).Item_Name.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void PayNow_dispatcher(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            this.Dispatcher.Invoke((Action)(() =>
              {
                  if (rbCard.IsChecked == true)
                  {
                      errorStatus = objRFID.get_RFID();
                      if (!errorStatus.Contains("Error"))
                      {
                          Submit_Order();
                          clear_Form();
                      }
                      else
                      {
                          MessageBox.Show(errorStatus, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

                      }
                  }
                  else
                  {

                      Submit_Order();
                      clear_Form();
                  }


              }));
        }

        private void Submit_Order()
        {

            Transaction_Master objTM = new Transaction_Master();
            Transaction_Details objTD = new Transaction_Details();
            List<Transaction_Details> lstTD = new List<Transaction_Details>();

            objTM.Transaction_Date = DateTime.Now;
            objTM.Transaction_Amount = Convert.ToDouble(lblBillAmount.Content);
            objTM.Created_By = User_ID;
            objTM.Created_DateTime = DateTime.Now;
            objTM.Updated_By = User_ID;
            objTM.Updated_DateTime = DateTime.Now;
            objTM.Company_ID = Company_ID;

            if (rbCard.IsChecked == true)
            {
                objTM.PaidBy_Card = true;
                objTM.RFID_No = lblCardID.Content.ToString();
                objTM.Card_Closing_Balance = (Convert.ToDouble(lblBalance.Content) - Convert.ToDouble(lblBillAmount.Content));
                objTM.Employee_ID = Convert.ToInt32(lblEmployeeID.Content);
                objTM.Card_ID = Convert.ToInt32(lbldbCardID.Content);
            }
            else
            {
                objTM.PaidBy_Card = false;
            }

            foreach (var item in lvMenuCard.ItemsSource)
            {
                objTD = new Transaction_Details();
                MenuCart objmenu = new MenuCart();
                objmenu = (MenuCart)item;
                objTD = new Transaction_Details();
                objTD.Item_ID = Convert.ToInt32(objmenu.Item_ID);
                objTD.Item_Unit_Price = Convert.ToDouble(objmenu.Item_Unit_Price);
                objTD.Item_Quantity = Convert.ToInt32(objmenu.Quantity);
                objTD.Item_Total_Cost = (objTD.Item_Unit_Price * objTD.Item_Quantity);
                objTD.Created_By = User_ID;
                objTD.Updated_By = User_ID;
                objTD.Created_DateTime = DateTime.Now;
                objTD.Updated_DateTime = DateTime.Now;
                objTD.Company_ID = Company_ID;
                lstTD.Add(objTD);
            }

            BL_Transaction objBL_Transaction = new BL_Transaction();
            Int64 tmid = objBL_Transaction.Submit_Order(objTM, lstTD);
            if (rbCard.IsChecked == true)
            {

                RFID_HW objRFID = new RFID_HW();
                errorStatus = objRFID.WriteDataBlock("8", objTM.Card_Closing_Balance.ToString());
                if (errorStatus.Contains("Error"))
                {
                    MessageBox.Show(errorStatus, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            objTM.Transaction_Master_ID = tmid;

            List<MenuCart> lstMenucart = new List<MenuCart>();
            lstMenucart = objBL_Transaction.get_PrintingMenuCart(tmid);

            Printing.PrintPaymentSlip(lstMenucart, objTM, false, lblEmaployeeName.Content.ToString().Trim());
            Printing.PrintPaymentSlip(lstMenucart, objTM, true, lblEmaployeeName.Content.ToString().Trim());

            MessageBox.Show(objTM.Order_No + "- Payment Successfull, Thank You !", "Megabite", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private static T GetFrameworkElementByName<T>(FrameworkElement referenceElement) where T : FrameworkElement
        {
            FrameworkElement child = null;
            for (Int32 i = 0; i < VisualTreeHelper.GetChildrenCount(referenceElement); i++)
            {
                child = VisualTreeHelper.GetChild(referenceElement, i) as FrameworkElement;
                System.Diagnostics.Debug.WriteLine(child);
                if (child != null && child.GetType() == typeof(T))
                { break; }
                else if (child != null)
                {
                    child = GetFrameworkElementByName<T>(child);
                    if (child != null && child.GetType() == typeof(T))
                    {
                        break;
                    }
                }
            }
            return child as T;
        }

        public void blockButton(bool flag)
        {

            foreach (Button child in Common.FindVisualChildren<Button>(LayoutRoot))
            {
                child.IsEnabled = flag;
            }
        }

        #endregion

        //private void customerNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        this.Dispatcher.Invoke((Action)(() =>
        //        {
        //            String Customername = new BL_UserMaster().getCustomerName(customerMobileTextBox.Text);
        //            if (Customername != string.Empty)
        //            {
        //                customerNameTextBox.Text = Customername;
        //                customerNameTextBox.IsEnabled = false;
        //            }
        //            else
        //                customerNameTextBox.Focus();
        //        }));
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Custom Message", "Cafe Name");
        //    }
        //}






    }
}
