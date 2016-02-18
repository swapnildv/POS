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
using System.Text.RegularExpressions;
using System.Windows.Media.Animation;

namespace Hotel_POS
{

    public partial class UsersWindow : Window
    {

        BL_UserMaster obj = new BL_UserMaster();
        public Int32 User_ID;
        public Int32 Role_ID;
        public MyValidation objValidation = new MyValidation();
        #region Events
        public UsersWindow()
        {
            this.Cursor = Cursors.Arrow;
            this.InitializeComponent();



            BindRoles();
            BindUsers();
            Bind_CompanyList();

        }
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {



            EnableDisable_controls(false);
            pbPassword.IsEnabled = false;
            pbPassword.IsEnabled = false;
            pbConfirmPassword.IsEnabled = false;

            btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
            btnSave.Visibility = System.Windows.Visibility.Collapsed;
            btnEdit.Visibility = System.Windows.Visibility.Collapsed;
            btnNewUser.Visibility = System.Windows.Visibility.Visible;

            row_ConfirmPassword.Height = new GridLength(0);
            row_password.Height = new GridLength(0);

            User_ID = Convert.ToInt32(((Label)(this.Owner.FindName("lblUserID"))).Content.ToString());
            Role_ID = Convert.ToInt32(((Label)(this.Owner.FindName("lblRoleID"))).Content.ToString());


        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(UsersGridView.ItemsSource);
                view.Filter = UserFilter;

                CollectionViewSource.GetDefaultView(UsersGridView.ItemsSource).Refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        private void txtUserNsme_PriviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }


        }

        public void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex("[^0-9a-zA-Z_@.]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string validation = CheckValidation();
                if (pbPassword.Password.ToString().Trim().Length < 1)
                {
                    validation += "Please enter password !" + "\n";
                }

                if (pbConfirmPassword.Password.ToString().Trim().Length < 1)
                {
                    validation += "Please enter confirm password !" + "\n";
                }

                if (pbConfirmPassword.Password != pbPassword.Password)
                {
                    validation += " Entered New Password & confirm password Must Be Same  !" + "\n";
                }

                if (validation == "1")
                {
                    if (check_UserName() > 0)
                    {
                        MessageBox.Show(txtUserName.Text.Trim() + " User Name Already Exists");

                        pbConfirmPassword.Password = "";
                        pbPassword.Password = "";
                        txtUserName.Focus();
                    }
                    else
                    {


                        if (txtFullName.Text != "" || txtFullName.Text != "" || cmbRole.SelectedIndex != -1 || cmbCompany.SelectedIndex != -1 || (pbPassword.Password.ToString().Trim().Length < 1) || (pbConfirmPassword.Password.ToString().Trim().Length < 1))
                        {
                            if (!CheckPassword(pbPassword.Password, pbConfirmPassword.Password))
                            {
                                MessageBox.Show("New password and confirmed password must be same", "Megabite", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                User_Master objuser = new User_Master();
                                objuser.User_Name = txtUserName.Text.Trim();
                                objuser.Real_Name = txtFullName.Text.Trim();
                                objuser.Role_ID = Convert.ToInt32(cmbRole.SelectedValue);                                
                                objuser.Password = obj.CreateSHAHash(pbPassword.Password);
                                objuser.Created_By = User_ID;
                                objuser.Created_DateTime = DateTime.Now;
                                objuser.Updated_DateTime = DateTime.Now;
                                objuser.Company_ID = Convert.ToInt32(cmbCompany.SelectedValue);  
                                var count = obj.CreateUser(objuser);
                                if (count > 0)
                                {
                                    BindUsers();
                                    Reset();
                                    EnableDisable_controls(false);
                                    btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
                                    btnSave.Visibility = System.Windows.Visibility.Collapsed;
                                    btnEdit.Visibility = System.Windows.Visibility.Collapsed;
                                    btnNewUser.Visibility = System.Windows.Visibility.Visible;
                                    MessageBox.Show("New User Created Succesfully.", "Megabite", MessageBoxButton.OK, MessageBoxImage.Information);
                                }

                            }
                        }

                        btnNewUser.IsEnabled = true;
                    }
                }
                else
                {

                    MessageBox.Show(validation.Replace('1', ' '), "Megabite", MessageBoxButton.OK, MessageBoxImage.Warning);

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string validation = CheckValidation();
                if (validation == "1")
                {

                    if (txtUserName.Text != "" || txtFullName.Text != "" || cmbRole.SelectedIndex != -1 || cmbCompany.SelectedIndex != -1)
                    {
                        User_Master item = (User_Master)UsersGridView.SelectedValue;
                        if (!CheckPassword(pbPassword.Password, pbConfirmPassword.Password))
                        {
                            MessageBox.Show("New password and confirmed password must be same");
                        }
                        else
                        {
                            User_Master objuser = new User_Master();
                            objuser.User_Name = txtUserName.Text.Trim();
                            objuser.Real_Name = txtFullName.Text.Trim();
                            objuser.Role_ID = Convert.ToInt32(cmbRole.SelectedValue);
                            objuser.Company_ID = Convert.ToInt32(cmbCompany.SelectedValue);
                            objuser.User_ID = item.User_ID;
                            objuser.Updated_By = User_ID;
                            objuser.Updated_DateTime = DateTime.Now;

                            var count = obj.UpdateUser(objuser);
                            if (count > 0)
                            {
                                MessageBox.Show("User Updated Succesfully.", "Megabite", MessageBoxButton.OK, MessageBoxImage.Information);

                                BindUsers();
                                Reset();
                                EnableDisable_controls(false);
                                btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
                                btnSave.Visibility = System.Windows.Visibility.Collapsed;
                                btnEdit.Visibility = System.Windows.Visibility.Collapsed;
                                btnNewUser.Visibility = System.Windows.Visibility.Visible;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Plaese select atleast one user for update");
                    }
                }
                else
                {

                    MessageBox.Show(validation.Replace('1', ' '), "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnNewUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Reset();
                EnableDisable_controls(true);

                btnNewUser.IsEnabled = false;

                btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
                btnSave.Visibility = System.Windows.Visibility.Visible;
                btnEdit.Visibility = System.Windows.Visibility.Collapsed;
                btnNewUser.Visibility = System.Windows.Visibility.Visible;

                pbPassword.IsEnabled = true;
                pbConfirmPassword.IsEnabled = true;
                txtFullName.Focus();

                row_ConfirmPassword.Height = GridLength.Auto;
                row_password.Height = GridLength.Auto;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                row_ConfirmPassword.Height = new GridLength(0);
                row_password.Height = new GridLength(0);
                User_Master item = (User_Master)UsersGridView.SelectedValue;
                if (item != null)
                {
                    EnableDisable_controls(true);

                    pbPassword.IsEnabled = false;
                    pbConfirmPassword.IsEnabled = false;

                    EditgrdUser();
                    btnUpdate.Visibility = System.Windows.Visibility.Visible;
                    btnSave.Visibility = System.Windows.Visibility.Collapsed;
                    btnEdit.Visibility = System.Windows.Visibility.Collapsed;
                    btnNewUser.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Please select atleast one user for edit");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
        private void lstUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditgrdUser();
            row_ConfirmPassword.Height = new GridLength(0);
            row_password.Height = new GridLength(0);
            if (UsersGridView.SelectedValue != null)
            {
                btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
                btnSave.Visibility = System.Windows.Visibility.Collapsed;
                btnEdit.Visibility = System.Windows.Visibility.Visible;
                btnNewUser.Visibility = System.Windows.Visibility.Visible;
                btnNewUser.IsEnabled = true;
            }
            else
            {
                btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
                btnSave.Visibility = System.Windows.Visibility.Collapsed;
                btnEdit.Visibility = System.Windows.Visibility.Collapsed;
                btnNewUser.Visibility = System.Windows.Visibility.Visible;
                btnNewUser.IsEnabled = true;
            }




        }

        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
            btnSave.Visibility = System.Windows.Visibility.Collapsed;
            btnEdit.Visibility = System.Windows.Visibility.Collapsed;
            btnNewUser.Visibility = System.Windows.Visibility.Visible;
            btnNewUser.IsEnabled = true;



        }
        /// <summary>
        ///This is for reseting the selected user password to 123
        /// </summary>

        #endregion


        #region Methods
        public int check_UserName()
        {

            var username = txtUserName.Text.Trim();
            return obj.CheckAvilUserName(username);
        }

        private void EnableDisable_controls(bool flag)
        {


            txtFullName.IsEnabled = flag;
            txtUserName.IsEnabled = flag;
            cmbRole.IsEnabled = flag;
            cmbCompany.IsEnabled = flag;

        }

        public void EditgrdUser()
        {
            if (UsersGridView.Items.Count > 0)
            {

                User_Master item = (User_Master)UsersGridView.SelectedValue;
                if (item != null)
                {
                    txtFullName.Text = item.Real_Name;
                    txtUserName.Text = item.User_Name;
                    pbPassword.Password = "12345";
                    pbConfirmPassword.Password = "12345";
                    cmbRole.SelectedValue = Convert.ToInt32(item.Role_ID);
                    cmbCompany.SelectedValue = Convert.ToInt32(item.Company_ID);
                    pbPassword.IsEnabled = false;
                    pbConfirmPassword.IsEnabled = false;
                }

            }
        }
        private void Bind_CompanyList()
        {
            BL_CompanyMaster obj = new BL_CompanyMaster();
            cmbCompany.ItemsSource = obj.GetAllCompanyMaster();
            cmbCompany.DisplayMemberPath = "Company_Name";
//            cmbCompany.SetValue = "Company_ID";
        }
        private void BindUsers()
        {
            UsersGridView.ItemsSource = obj.BindUsers();
            

        }

        private void BindRoles()
        {
            try
            {
                cmbRole.ItemsSource = obj.GetAllUserRole();
                cmbRole.DisplayMemberPath = "Role_Name";
            }
            catch (SqlException)
            {

            }

        }




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

            txtFullName.Text = "";
            txtUserName.Text = "";
            pbPassword.Password = "";
            pbConfirmPassword.Password = "";
            cmbRole.SelectedIndex = -1;
            cmbCompany.SelectedIndex = -1;
            UsersGridView.SelectedIndex = -1;
            row_ConfirmPassword.Height = new GridLength(0);
            row_password.Height = new GridLength(0);

        }

        private string CheckValidation()
        {
            string errorText = "1";

            if (txtFullName.Text.Trim().Length < 1)
            {
                errorText += "Please enter full name !" + "\n";

            }
            else
            {
                Regex regex = new Regex(@"^[a-zA-Z _-]{3,30}$");
                if (!(regex.IsMatch(txtFullName.Text.Trim())))
                {
                    errorText += "Invalid Full Name" + "\n";
                }

            }

            if (txtUserName.Text.Trim().Length < 1)
            {
                errorText += "Please enter username !" + "\n";
            }
            else
            {
                if (!objValidation.IsValidUserName(txtUserName.Text.Trim()))
                {
                    errorText += "Invalid User Name (Ex Valid : 'myusername_09',myname-09)" + "\n";
                }

            }


            if (cmbRole.SelectedIndex < 0)
            {
                errorText += "Please Select role of user !" + "\n";
            }
            if (cmbCompany.SelectedIndex < 0)
            {
                errorText += "Please Select Company of user !" + "\n";
            }


            return errorText;
        }
        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtSearch.Text))
                return true;
            else
                return ((item as User_Master).User_Name.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        #endregion

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //#region Validation

        //previewRegex["Required"] = @"^[a-zA-Z\s.,0-9@():'/?!_-]{1,30}$";
        //  completionRegex["Required"] = @"^[a-zA-Z\s.,0-9@():'/?!_-]{1,50}$";
        //  errorMessage["Required"] = "This Filed is Required.";




        //  previewRegex["Email"] = @"";
        //  completionRegex["Email"] = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
        //  errorMessage["Email"] = "Enter a valid email, eg. sven.svensson@gmail.com";

        //  previewRegex["Mobile"] = @"^(\d)$";
        //  completionRegex["Mobile"] = @"^([7896]\d{9})$"; // This regex needs some improvement!
        //  errorMessage["Mobile"] = "Enter Valid Mobile Number";

        //enum ValidationStates { OK, ERROR, WARNING };

        //// Tables for regex and messages
        //Hashtable previewRegex = new Hashtable();
        //Hashtable completionRegex = new Hashtable();
        //Hashtable errorMessage = new Hashtable();
        //Hashtable validationState = new Hashtable();

        //const string fieldRequired = "This field is required";
        //private void KeypressValidation(object sender, TextCompositionEventArgs e)
        //{
        //    // Handle to the textbox tjhat should be validated..
        //    TextBox tbox = (TextBox)sender;
        //    // Fetch regex..
        //    Regex regex = new Regex((string)previewRegex[(string)tbox.Tag]);
        //    // Check match and put error styles and messages..
        //    if (regex.IsMatch(e.Text))
        //    {
        //        if ((ValidationStates)validationState[tbox.Name] != ValidationStates.OK) tbox.Style = (Style)FindResource("textBoxNormalStyle");
        //        validationState[tbox.Name] = ValidationStates.OK;
        //    }
        //    else
        //    {
        //        if ((ValidationStates)validationState[tbox.Name] != ValidationStates.WARNING)
        //        {
        //            BrushConverter bc = new BrushConverter();
        //            Brush brush = (Brush)bc.ConvertFrom("#FF0000");
        //            brush.Freeze();
        //            tbox.BorderBrush = brush;
        //            validationState[tbox.Name] = ValidationStates.WARNING;
        //            tbox.UpdateLayout(); // Very important if want to use Template.FindName when changing style dynamically!
        //        }
        //        // Fetch the errorimage in the tbox:s control template.. 

        //        // And set its tooltip to the errormessage of the textboxs validation code..
        //        tbox.ToolTip = (string)errorMessage[(string)tbox.Tag];
        //        // Use this if you dont want the user to enter something in textbox that invalidates it.
        //        e.Handled = true;
        //    }
        //}

        //private void CompletionValidation(object sender, RoutedEventArgs e)
        //{
        //    TextBox tbox = (TextBox)sender;
        //    Regex regex = new Regex((string)completionRegex[(string)tbox.Tag]);
        //    if (regex.IsMatch(tbox.Text))
        //    {
        //        if ((ValidationStates)validationState[tbox.Name] != ValidationStates.OK) tbox.Style = (Style)FindResource("textBoxNormalStyle");
        //        validationState[tbox.Name] = ValidationStates.OK;
        //    }
        //    else
        //    {
        //        if ((ValidationStates)validationState[tbox.Name] != ValidationStates.ERROR)
        //        {
        //            BrushConverter bc = new BrushConverter();
        //            Brush brush = (Brush)bc.ConvertFrom("#FF0000");
        //            brush.Freeze();
        //            tbox.BorderBrush = brush;
        //            tbox.UpdateLayout();
        //        }

        //    }
        //}

        //private void InitValidation(object sender, RoutedEventArgs e)
        //{
        //    TextBox tbox = (TextBox)sender;
        //    if (validationState[tbox.Name] == null) validationState[tbox.Name] = ValidationStates.OK;
        //}

        //#endregion


    }




}