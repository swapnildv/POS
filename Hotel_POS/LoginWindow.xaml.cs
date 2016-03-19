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
using System.Collections;
using System.Text.RegularExpressions;
using Hotel_POS.Resource;
using log4net;
namespace Hotel_POS
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private static readonly ILog _logger =
         LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Variables
        BL_UserMaster obj = new BL_UserMaster();
        public LoginWindow()
        {
            InitializeComponent();
            lbl_Msg.Text = "";
        }
        #endregion

        #region Events

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            txtUserName.Focus();
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string errorText = "";
                bool state = false;
                if (txtUserName.Text.Length < 1)
                {
                    errorText += "Please enter User Name !" + "\n";
                    state = true;
                }
                if (pbPassword.Password.Length < 1)
                {
                    errorText += "Please enter Password !" + "\n";
                    state = true;
                }
                if (state == true)
                {
                    lbl_Msg.Text = errorText;
                    MessageBox.Show(errorText, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (txtUserName.Text != "" || pbPassword.Password != "")
                    {
                        User_Master objuser = new User_Master();
                        objuser.User_Name = txtUserName.Text.Trim();
                        objuser.Password = obj.CreateSHAHash(pbPassword.Password);
                        TerminalCommon.LoggedInUser = obj.CheckLoginUser(objuser);

                        if (TerminalCommon.LoggedInUser != null)
                        {
                            TerminalCommon.LoggedInUser = TerminalCommon.LoggedInUser;
                            MainWindow NavigateObj = new MainWindow() { Owner = this };
                            NavigateObj.lblUserID.Content = TerminalCommon.LoggedInUser.User_ID.ToString();
                            NavigateObj.lblRoleID.Content = TerminalCommon.LoggedInUser.Role_ID.ToString();
                            //NavigateObj.lblRoleID.Content = objResponse.Company_Name.ToString();
                            NavigateObj.lblCompanyID.Content = TerminalCommon.LoggedInUser.Company_ID.ToString();
                            NavigateObj.lblUserName.Content = "Welcome, " + TerminalCommon.LoggedInUser.Real_Name.ToString();
                            this.Hide();
                            NavigateObj.ShowDialog();
                            this.Show();
                            btnCancel_Click(null, null);
                            txtUserName.Focus();
                            //this.Close();
                        }
                        else
                        {
                            lbl_Msg.Text = "The email or password you entered is incorrect.";
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageHelper.MessageBox.ShowError(this,"Error in login.");
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lbl_Msg.Text = "";
                txtUserName.Text = String.Empty;
                pbPassword.Password = String.Empty;
                txtUserName.Focus();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageHelper.MessageBox.ShowError(this, "Error in login.");
            }

        }
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                this.Close();

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageHelper.MessageBox.ShowError(this, "Error in login.");
            }

        }

        #endregion

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageHelper.MessageBox.ShowError(this, "Error in login.");

            }
        }

    }
}
