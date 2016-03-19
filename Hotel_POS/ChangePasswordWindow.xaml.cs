using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using POS_Business;
using System.Collections;
using MegabiteEntityLayer;
using log4net;

namespace Hotel_POS
{
    /// <summary>
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {

        private static readonly ILog _logger =
   LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        BL_UserMaster obj = new BL_UserMaster();
        public Int32 User_ID;
        public Int32 Role_ID;

        public ChangePasswordWindow()
        {
            InitializeComponent();
            txtresult.Content = "";
        }
        #region Events

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                User_ID = Convert.ToInt32(((Label)(this.Owner.FindName("lblUserID"))).Content.ToString());
                Role_ID = Convert.ToInt32(((Label)(this.Owner.FindName("lblRoleID"))).Content.ToString());
                txt_UserName.Text = obj.getUserName(User_ID);
                txt_UserName.IsEnabled = false;

                if (Role_ID == 3)
                {
                    BorderReset.Visibility = System.Windows.Visibility.Visible;
                    Bind_User();
                }
                else
                {
                    BorderReset.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageHelper.MessageBox.ShowError(this);
            }

        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string validation = CheckValidation();
                if (validation == "1")
                {
                    if (txt_UserName.Text.Trim() != "" || txt_OldPassword.Password.Trim() != "" || txt_NewPassword.Password.Trim() != "" || txt_ConfirmPassword.Password.Trim() != "")
                    {
                        if (!CheckPassword(txt_NewPassword.Password.Trim(), txt_ConfirmPassword.Password.Trim()))
                        {

                            MessageBox.Show("New password and confirmed password must be same", "Megabite", MessageBoxButton.OK, MessageBoxImage.Warning);

                            this.Cursor = Cursors.Arrow;
                        }
                        else
                        {
                            String CheckUserValid = obj.CheckUserInfo(txt_UserName.Text.Trim(), obj.CreateSHAHash(txt_OldPassword.Password.Trim()));
                            if (CheckUserValid == "Valid")
                            {
                                User_Master objUserinfo = new User_Master();
                                objUserinfo.User_Name = txt_UserName.Text.Trim();
                                objUserinfo.Password = obj.CreateSHAHash(txt_NewPassword.Password.Trim());
                                objUserinfo.User_ID = User_ID;
                                objUserinfo.Created_By = User_ID;
                                var count = obj.changePassword(objUserinfo);

                                MessageBox.Show("Password updated sucessfully", "Megabite", MessageBoxButton.OK, MessageBoxImage.Information);

                                Reset();
                            }
                            else
                            {
                                MessageBox.Show("Entered Wrong Old Password ", "Megabite", MessageBoxButton.OK, MessageBoxImage.Information);

                                Reset();
                                this.Cursor = Cursors.Arrow;
                            }
                        }

                    }

                }
                else
                {
                    MessageBox.Show(validation.Replace('1', ' '), "Megabite", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageHelper.MessageBox.ShowError(this);
            }

        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbUser.SelectedIndex > -1)
                {
                    string resetpassword = "qwerty";
                    User_Master objUserinfo = new User_Master();
                    objUserinfo.User_Name = cmbUser.Text.Trim();
                    objUserinfo.Password = obj.CreateSHAHash(resetpassword.Trim());
                    objUserinfo.User_ID = Convert.ToInt32(cmbUser.SelectedValue);
                    objUserinfo.Created_By = User_ID;
                    var count = obj.changePassword(objUserinfo);
                    if (count > 0)
                    {
                        MessageBox.Show("Password Reset sucessfully, New Password is 'qwerty' ", "Megabite", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtresult.Content = "New Password is 'qwerty'";
                        Reset();
                        cmbUser.SelectedIndex = -1;
                    }
                    else
                    {
                        MessageBox.Show("Failed To Reset", "Megabite", MessageBoxButton.OK, MessageBoxImage.Warning);

                    }
                }
                else
                {
                    txtresult.Content = "";

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageHelper.MessageBox.ShowError(this);
            }
        }

        private void CloseCommand_Executed(object sender, RoutedEventArgs e)
        {
            txtresult.Content = "";
            this.Close();
        }
        #endregion




        #region Methods

        private bool CheckPassword(string pass1, string pass2)
        {
            if (pass1 == pass2)
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }
        private void Reset()
        {

            txt_OldPassword.Password = "";
            txt_NewPassword.Password = "";
            txt_ConfirmPassword.Password = "";

        }
        private string CheckValidation()
        {
            string errorText = "1";

            if (txt_UserName.Text.Length < 1)
            {

                errorText += "Please enter user name !" + "\n";

            }
            if (txt_OldPassword.Password.Length < 1)
            {
                errorText += "Please enter old password !" + "\n";


            }
            if (txt_NewPassword.Password.Length < 1)
            {
                errorText += "Please enter new password !" + "\n";


            }
            if (txt_ConfirmPassword.Password.Length < 1)
            {
                errorText += "Please enter confirm password !" + "\n";


            }
            if (errorText != "1")
            {
                errorText.Replace("1", " ");
            }
            return errorText;
        }

        private void Bind_User()
        {
            cmbUser.ItemsSource = obj.Get_UserList();
        }
        #endregion
    }
}
