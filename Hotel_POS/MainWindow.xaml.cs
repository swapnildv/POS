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
using System.ComponentModel;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Threading;
using System.Timers;


using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Hotel_POS;
using Hotel_POS.Reports;
using Hotel_POS.User_Controls;
using Hotel_POS.Resource;
using POS_Business;
namespace Hotel_POS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables
        public Int32 User_ID;
        public Int32 Role_ID;



        #endregion
        public MainWindow()
        {
            InitializeComponent();

        }
        #region Events


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                User_ID = Convert.ToInt32(lblUserID.Content.ToString());
                Role_ID = Convert.ToInt32(lblRoleID.Content.ToString());
                switch (TerminalCommon.LoggedInUser.Role_ID)
                {
                    case (int)TerminalCommon.user_roles.operatorRole:
                        shuffleMenus(TerminalCommon.operatorRoleMenu);
                        MainGrid.Children.Clear();
                        MainGrid.Children.Add(new OrderUserControl());
                        break;
                    case (int)TerminalCommon.user_roles.adminRole:
                        shuffleMenus(TerminalCommon.adminRoleMenu);
                        MainGrid.Children.Clear();
                        MainGrid.Children.Add(new Dashboard());
                        break;
                }

                this.WindowState = System.Windows.WindowState.Maximized;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


            }

        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;
                UsersWindow objUsersWindow = new UsersWindow();
                objUsersWindow.Owner = this;
                objUsersWindow.ShowDialog();
                objUsersWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


            }
        }

        private void Company_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;
                CompanyWindow objCompanyWindow = new CompanyWindow();
                objCompanyWindow.Owner = this;
                objCompanyWindow.ShowDialog();
                objCompanyWindow = null;
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


            }
        }
        private void ManageEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;

                Employee_Window objemployee_Window = new Employee_Window();
                objemployee_Window.Owner = this;
                objemployee_Window.ShowDialog();
                objemployee_Window = null;

                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


            }
        }
        private void Order_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainGrid.Children.Clear();
                MainGrid.Children.Add(new OrderUserControl());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CardAssign_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;

                CardAssignWindow objCardAssignWindow = new CardAssignWindow();
                objCardAssignWindow.Owner = this;

                objCardAssignWindow.ShowDialog();

                objCardAssignWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                this.Cursor = Cursors.Arrow;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;

                MenuWindow objMenuWindow = new MenuWindow();
                objMenuWindow.Owner = this;
                objMenuWindow.ShowDialog();

                objMenuWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                this.Cursor = Cursors.Arrow;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


            }
        }

        private void LoadCard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;

                CardLoadWindow objCardLoadWindow = new CardLoadWindow();
                objCardLoadWindow.Owner = this;
                objCardLoadWindow.ShowDialog();

                objCardLoadWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                this.Cursor = Cursors.Arrow;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


            }
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;

                ChangePasswordWindow objChangePwdWindow = new ChangePasswordWindow();
                objChangePwdWindow.Owner = this;
                objChangePwdWindow.ShowDialog();

                objChangePwdWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                this.Cursor = Cursors.Arrow;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


            }
        }

        private void MenuCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;

                MenuCategoryWindow objMenucatWindow = new MenuCategoryWindow();
                objMenucatWindow.Owner = this;
                objMenucatWindow.ShowDialog();

                objMenucatWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                this.Cursor = Cursors.Arrow;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


            }
        }

        private void employeeLedger_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;

                EmployeeReport objEmployeeReport = new EmployeeReport();
                objEmployeeReport.Owner = this;
                objEmployeeReport.ShowDialog();

                objEmployeeReport.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                this.Cursor = Cursors.Arrow;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


            }
        }

        private void TransactionReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;

                TransactionReport objTransactionReport = new TransactionReport();
                objTransactionReport.Owner = this;
                objTransactionReport.ShowDialog();

                objTransactionReport.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                this.Cursor = Cursors.Arrow;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


            }
        }

        private void LoadReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;

                LoadReport objLoadReport = new LoadReport();
                objLoadReport.Owner = this;
                objLoadReport.ShowDialog();

                objLoadReport.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                this.Cursor = Cursors.Arrow;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


            }

        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new BL_Menu().ClearMenuCart();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


            }
        }

        private void Change_Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;
                lblRoleID.Content = 0;
                lblUserID.Content = 0;
                LoginWindow objLoginWindow = new LoginWindow();

                objLoginWindow.ShowDialog();

                objLoginWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


            }
        }

        private void Get_Database_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    this.Cursor = Cursors.Wait;

            //    DatabaseWindow objDBWindow = new DatabaseWindow();
            //    objDBWindow.Owner = this;
            //    objDBWindow.ShowDialog();

            //    objDBWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            //    this.Cursor = Cursors.Arrow;

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


            //}
        }

        private void GEt_Data_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;

            ////BackUp_RestoreData objDBWindow = new BackUp_RestoreData();
            //objDBWindow.Owner = this;
            //objDBWindow.ShowDialog();

            //objDBWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            //this.Cursor = Cursors.Arrow;
        }

        private void FormatCard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;

                ClearCard objClearCard = new ClearCard();
                objClearCard.Owner = this;
                objClearCard.ShowDialog();

                objClearCard.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                this.Cursor = Cursors.Arrow;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


            }
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainGrid.Children.Clear();
                MainGrid.Children.Add(new Dashboard());
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        #endregion
        #region Methods

        private void Load_RoleWiseMenus(String[] RoleMenus)
        {
            var items = MenuBar.Items;

            foreach (MenuItem item in items)
            {

                var submenus = item.Items;
                foreach (MenuItem subitem in submenus)
                {
                    if (RoleMenus.Contains(subitem.Name))
                    {
                        subitem.Visibility = System.Windows.Visibility.Visible;

                    }
                    else
                    {
                        subitem.Visibility = System.Windows.Visibility.Collapsed;
                    }
                }

            }
        }

        private void shuffleMenus(List<string> menu)
        {
            int submenuCount = 0;
            foreach (MenuItem menuItem in MenuBar.Items)
            {
                var submenus = menuItem.Items;

                foreach (MenuItem subitem in submenus)
                {
                    if (menu.Contains(subitem.Name))
                    {
                        submenuCount++;
                        subitem.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                        subitem.Visibility = System.Windows.Visibility.Collapsed;

                }
                if (submenuCount > 0 || menuItem.Name == "Logout"  || menuItem.Name == "Home" )
                    menuItem.Visibility = System.Windows.Visibility.Visible;
                else
                    menuItem.Visibility = System.Windows.Visibility.Collapsed;
                submenuCount = 0;
            }
        }
        #endregion

        
    }
}
