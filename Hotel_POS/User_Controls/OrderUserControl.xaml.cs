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
using log4net;
using MegabiteEntityLayer;
using MegabiteEntityLayer.Helpers;
using POS_Business;

namespace Hotel_POS.User_Controls
{
    /// <summary>
    /// Interaction logic for OrderUserControl.xaml
    /// </summary>
    public partial class OrderUserControl : UserControl
    {
        MainWindow mainWindow;
        private static readonly ILog _logger =
       LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static RoutedCommand commonCommand =
            new RoutedCommand("btnBetCommand", typeof(MainWindow));

        private double totalAmount = 0;
        public OrderUserControl()
        {
            try
            {
                InitializeComponent();
                bwPlaceOrder.WorkerReportsProgress = true;
                bwPlaceOrder.WorkerSupportsCancellation = true;
                bwPlaceOrder.DoWork += bwPlaceOrder_DoWork;
                bwPlaceOrder.ProgressChanged += bwPlaceOrder_ProgressChanged;
                bwPlaceOrder.RunWorkerCompleted += bwPlaceOrder_RunWorkerCompleted;
            }
            catch (Exception ex) { _logger.Error(ex); }
        }

        /// <summary>
        /// Background worker for placing order.
        /// </summary>
        private BackgroundWorker bwPlaceOrder = new BackgroundWorker();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TerminalCommon.LoggedInUser != null)
                {
                    DiscountGrid.Visibility = TerminalCommon.LoggedInUser.IsDiscount == true ?
                        System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                }
                mainWindow = (MainWindow)Window.GetWindow(this);
                //BL_Menu obj = new BL_Menu();
                MainMenuListBox.ItemsSource = new BL_Menu().get_Item_Group_List().Where(c => c.Is_Active == true);
                MenuCartlistBox.ItemsSource = new BL_Menu().GetMenuCart();
                customerMobileTextBox.Focus();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (mainWindow != null)
                    MessageHelper.MessageBox.ShowError(mainWindow);
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
                _logger.Error(ex);
                if (mainWindow != null)
                    MessageHelper.MessageBox.ShowError(mainWindow);
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
                _logger.Error(ex);
                if (mainWindow != null)
                    MessageHelper.MessageBox.ShowError(mainWindow);
            }
        }

        private void SubMainMenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                itemQuantity.Text = "1";
                itemQuantity.Focus();
                itemQuantity.SelectAll();
                if (SubMainMenuListBox.SelectedItem != null)
                    _selectedItemId = ((MegabiteEntityLayer.Item_Master)SubMainMenuListBox.SelectedItem).Item_ID; // Get selected sub menu.
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (mainWindow != null)
                    MessageHelper.MessageBox.ShowError(mainWindow);
            }

        }

        private long _selectedItemId;
        private void MenuCartlistBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (MenuCartlistBox.SelectedItem != null)
                {
                    var item = (MenuCart)MenuCartlistBox.SelectedItem;
                    _selectedItemId = item.Item_ID;
                    itemQuantity.Text = item.Quantity.ToString();
                }

                //itemQuantity.Focus();
                //itemQuantity.SelectAll();

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (mainWindow != null)
                    MessageHelper.MessageBox.ShowError(mainWindow);
            }
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MegabiteEntityLayer.Item_Group_Master masterMenu = (MegabiteEntityLayer.Item_Group_Master)MainMenuListBox.SelectedItem; // Get selected menu group.
                MegabiteEntityLayer.Item_Master subMenu = (MegabiteEntityLayer.Item_Master)SubMainMenuListBox.SelectedItem; // Get selected sub menu.

                if (_selectedItemId != 0 && int.Parse(itemQuantity.Text.ToString()) > 00)
                {
                    new BL_Menu().AddMenuCartItem(_selectedItemId, int.Parse(itemQuantity.Text.ToString()));
                }
                totalAmountTextBlock.Text = TerminalCommon.currency + " " + new BL_Menu().getTotatlCartValue().ToString();


                MenuCartlistBox.Focus();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (mainWindow != null)
                    MessageHelper.MessageBox.ShowError(mainWindow);
            }
        }

        private void subMenuSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(SubMainMenuListBox.ItemsSource);
                view.Filter = UserFilter;
                CollectionViewSource.GetDefaultView(SubMainMenuListBox.ItemsSource).Refresh();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (mainWindow != null)
                    MessageHelper.MessageBox.ShowError(mainWindow);
            }
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(subMenuSearchTextBox.Text))
                return true;
            else
                return ((item as Item_Master).Item_Name.IndexOf(subMenuSearchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

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
                _logger.Error(ex);
                if (mainWindow != null)
                    MessageHelper.MessageBox.ShowError(mainWindow);
            }
        }
        private void cancelOrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bwPlaceOrder.IsBusy != true)
                    bwPlaceOrder.CancelAsync();

                new BL_Menu().ClearMenuCart();
                totalAmountTextBlock.Text = TerminalCommon.currency + " " + new BL_Menu().getTotatlCartValue().ToString();
                itemQuantity.Text = "1";
                customerMobileTextBox.Text = String.Empty;
                customerNameTextBox.Text = String.Empty;
                discountTextBox.Text = string.Empty;
                customerMobileTextBox.Focus();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                if (mainWindow != null)
                    MessageHelper.MessageBox.ShowError(mainWindow);
            }
        }
        void bwPlaceOrder_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if ((worker.CancellationPending == true))
                e.Cancel = true;
            else
            {
                double discount = 0;
                this.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    try { discount = double.Parse(discountTextBox.Text.Trim()); }
                    catch (Exception) { discount = 0; }
                });
                e.Result = new BL_Transaction().SubmitOrder(discount);
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
                    _logger.Error(e.Error.Message);
                    MessageHelper.MessageBox.ShowError(mainWindow, e.Error.Message);
                }
                else
                {
                    Printing.PrintPaymentSlip(e.Result.ToString());
                    new BL_Menu().ClearMenuCart();
                    customerMobileTextBox.Text = string.Empty;
                    discountTextBox.Text = string.Empty;
                    customerNameTextBox.Text = string.Empty;
                    TerminalCommon.currentCustomer = null;
                    MainMenuListBox.SelectedIndex = 0;
                    customerMobileTextBox.Focus();
                    totalAmountTextBlock.Text = "Rs. 0";
                }
            }
            catch (Exception ex)
            {
                this.Dispatcher.BeginInvoke((Action)delegate ()
                 {
                     _logger.Error(ex);
                     if (mainWindow != null)
                         MessageHelper.MessageBox.ShowError(mainWindow);
                 });
            }

        }

        private void discountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal disc = 0;
                try { disc = decimal.Parse(discountTextBox.Text); }
                catch (Exception) { disc = 0; }

                if (disc > 100 || disc < 0)
                    return;

                decimal totalValue = Convert.ToDecimal(new BL_Menu().getTotatlCartValue());

                totalValue = totalValue * (1 - (disc / 100));

                if (totalValue > 0)
                    totalAmountTextBlock.Text = TerminalCommon.currency + " " + totalValue.ToString("00.00");

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                //if (mainWindow != null)
                //    MessageHelper.MessageBox.ShowError(mainWindow);
            }

        }




        //This method is used to give shortcuts to controls on Main window page.
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var menucartItem = (MenuCart)MenuCartlistBox.SelectedItem;
                new BL_Menu().RemoveCartItem(menucartItem.Item_ID);
                totalAmountTextBlock.Text = TerminalCommon.currency + " " + new BL_Menu().getTotatlCartValue().ToString();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                if (MenuCartlistBox.SelectedItem != null)
                    e.CanExecute = true;
                else
                    e.CanExecute = false;

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
