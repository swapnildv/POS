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

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem _sender = (MenuItem)sender;
            switch (_sender.Name)
            {
                case "Home":
                    MainGrid.Children.Clear();
                    MainGrid.Children.Add(new Dashboard());
                    break;

                case "Users":
                    this.Cursor = Cursors.Wait;
                    UsersWindow usersWindow = new UsersWindow();
                    usersWindow.Owner = this;
                    usersWindow.ShowDialog();
                    usersWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                    this.Cursor = Cursors.Arrow;
                    break;

                case "ChangePasswordMenuItem":

                    this.Cursor = Cursors.Wait;

                    ChangePasswordWindow objChangePwdWindow = new ChangePasswordWindow();
                    objChangePwdWindow.Owner = this;
                    objChangePwdWindow.ShowDialog();

                    objChangePwdWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                    this.Cursor = Cursors.Arrow;
                    break;

                case "MenuCategory":
                    this.Cursor = Cursors.Wait;

                    MenuCategoryWindow objMenucatWindow = new MenuCategoryWindow();
                    objMenucatWindow.Owner = this;
                    objMenucatWindow.ShowDialog();

                    objMenucatWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                    this.Cursor = Cursors.Arrow;
                    break;

                case "Menu":
                    this.Cursor = Cursors.Wait;

                    MenuWindow objMenuWindow = new MenuWindow();
                    objMenuWindow.Owner = this;
                    objMenuWindow.ShowDialog();

                    objMenuWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                    this.Cursor = Cursors.Arrow;
                    break;

                case "NewOrder":
                    MainGrid.Children.Clear();
                    MainGrid.Children.Add(new OrderUserControl());
                    break;

                case "TransactionReport":
                    this.Cursor = Cursors.Wait;

                    TransactionReport objTransactionReport = new TransactionReport();
                    objTransactionReport.Owner = this;
                    objTransactionReport.ShowDialog();

                    objTransactionReport.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                    this.Cursor = Cursors.Arrow;
                    break;

                case "Logout":
                    new BL_Menu().ClearMenuCart();
                    this.Close();
                    break;

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
                if (submenuCount > 0 || menuItem.Name == "Logout" || menuItem.Name == "Home" || menuItem.Name == "NewOrder")
                    menuItem.Visibility = System.Windows.Visibility.Visible;
                else
                    menuItem.Visibility = System.Windows.Visibility.Collapsed;
                submenuCount = 0;
            }
        }
        #endregion


    }
}
