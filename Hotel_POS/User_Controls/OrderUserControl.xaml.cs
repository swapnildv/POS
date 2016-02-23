using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Hotel_POS.Resource;
using MegabiteEntityLayer;
using POS_Business;

namespace Hotel_POS.User_Controls
{
    /// <summary>
    /// Interaction logic for OrderUserControl.xaml
    /// </summary>
    public partial class OrderUserControl : UserControl
    {
        public OrderUserControl()
        {
            InitializeComponent();
            bwPlaceOrder.WorkerReportsProgress = true;
            bwPlaceOrder.WorkerSupportsCancellation = true;
            bwPlaceOrder.DoWork += bwPlaceOrder_DoWork;
            bwPlaceOrder.ProgressChanged += bwPlaceOrder_ProgressChanged;
            bwPlaceOrder.RunWorkerCompleted += bwPlaceOrder_RunWorkerCompleted;
        }

        /// <summary>
        /// Background worker for placing order.
        /// </summary>
        private BackgroundWorker bwPlaceOrder = new BackgroundWorker();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //BL_Menu obj = new BL_Menu();
                MainMenuListBox.ItemsSource = new BL_Menu().get_Item_Group_List().Where(c => c.Is_Active == true);
                ItemListBox.ItemsSource = new BL_Menu().GetMenuCart();
                customerMobileTextBox.Focus();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show(TerminalCommon.errorMessage, TerminalCommon.cafeName);
            }
        }

        private void customerNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //this.Dispatcher.Invoke((Action)(() =>
                //{
                //    TerminalCommon.currentCustomer = new BL_UserMaster().getCustomerName(customerMobileTextBox.Text);
                //    if (TerminalCommon.currentCustomer != null)
                //    {
                //        customerNameTextBox.Text = TerminalCommon.currentCustomer.cust_Name;
                //        customerNameTextBox.IsEnabled = false;
                //    }
                //    else
                //        customerNameTextBox.Focus();
                //}));
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show(TerminalCommon.errorMessage, TerminalCommon.cafeName);
            }
        }

        private void MainMenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                MegabiteEntityLayer.Item_Group_Master raceEvent = (MegabiteEntityLayer.Item_Group_Master)MainMenuListBox.SelectedItem; // Get selected menu group.
                if (raceEvent != null)
                    SubMainMenuListBox.ItemsSource = new BL_Menu().get_Menu_List(raceEvent.Item_Group_ID).Where(m => m.Is_Active == true); //Bind Sub menu list.

            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show(TerminalCommon.errorMessage, TerminalCommon.cafeName);
            }
        }

        private void SubMainMenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                itemQuantity.Text = "1";
                AddItemButton.Focus();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show(TerminalCommon.errorMessage, TerminalCommon.cafeName);
            }

        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MegabiteEntityLayer.Item_Group_Master masterMenu = (MegabiteEntityLayer.Item_Group_Master)MainMenuListBox.SelectedItem; // Get selected menu group.
                MegabiteEntityLayer.Item_Master subMenu = (MegabiteEntityLayer.Item_Master)SubMainMenuListBox.SelectedItem; // Get selected sub menu.

                if (masterMenu != null &&
                    subMenu != null)
                {
                    new BL_Menu().AddMenuCartItem(subMenu.Item_ID, int.Parse(itemQuantity.Text.ToString()));
                }
                totalAmountTextBlock.Text = TerminalCommon.currency + " " + new BL_Menu().getTotatlCartValue().ToString();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show(TerminalCommon.errorMessage, TerminalCommon.cafeName);
            }
        }

        #region Place Order
        private void proceedOrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Set customer 
                if (TerminalCommon.currentCustomer == null)
                    TerminalCommon.currentCustomer = new Customer_Master()
                    {
                        cust_MobileNo = customerMobileTextBox.Text.Trim(),
                        cust_Name = customerNameTextBox.Text.Trim()
                    };
                if (bwPlaceOrder.IsBusy != true)
                {
                    MainWindow mainWindow = (MainWindow)Window.GetWindow(this);

                    if (mainWindow != null) mainWindow.progressControl.Visibility = System.Windows.Visibility.Visible;
                    bwPlaceOrder.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show(TerminalCommon.errorMessage, TerminalCommon.cafeName);
            }
        }

        private void cancelOrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bwPlaceOrder.IsBusy != true)
                {
                    bwPlaceOrder.CancelAsync();

                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show(TerminalCommon.errorMessage, TerminalCommon.cafeName);
            }
        }


        void bwPlaceOrder_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                }
                else
                {
                    e.Result = new BL_Transaction().SubmitOrder();

                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show(TerminalCommon.errorMessage, TerminalCommon.cafeName);
            }

        }
        void bwPlaceOrder_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }
        void bwPlaceOrder_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                //Hide Progress Window
                MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
                if (mainWindow != null) mainWindow.progressControl.Visibility = System.Windows.Visibility.Collapsed;
                if ((e.Cancelled == true))
                {
                    // this.tbProgress.Text = "Canceled!";
                }

                else if (!(e.Error == null))
                {
                    //this.tbProgress.Text = ("Error: " + e.Error.Message);
                }

                else
                {
                    Printing.PrintPaymentSlip(e.Result.ToString());
                    new BL_Menu().ClearMenuCart();
                    customerMobileTextBox.Text = string.Empty;
                    customerNameTextBox.Text = string.Empty;
                    TerminalCommon.currentCustomer = null;
                    MainMenuListBox.SelectedIndex = 0;
                    customerMobileTextBox.Focus();
                    totalAmountTextBlock.Text = "Rs. 0";
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show(TerminalCommon.errorMessage, TerminalCommon.cafeName);
            }

        }


        #endregion
    }
}
