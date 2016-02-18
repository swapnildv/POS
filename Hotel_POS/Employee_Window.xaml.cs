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
using System.Data.SqlClient;
using POS_Business;
using System.Collections;
using MegabiteEntityLayer;
using System.Text.RegularExpressions;

namespace Hotel_POS
{
    /// <summary>
    /// Interaction logic for Employee_Window.xaml
    /// </summary>
    public partial class Employee_Window : Window
    {


        #region Variables

        BL_EmployeeMaster obj = new BL_EmployeeMaster();
        public Employee_Window()
        {
            InitializeComponent();


            EnableDisable_controls(false);
            Bindemp();
        }
        public MyValidation objValidation = new MyValidation();


        private bool firstLoad = true;
        public Int32 User_ID;
        public Int32 Role_ID;

        #endregion
        #region Propertis
        #endregion

        #region Events

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(EmployeeGridView.ItemsSource);
                view.Filter = UserFilter;

                CollectionViewSource.GetDefaultView(EmployeeGridView.ItemsSource).Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                User_ID = Convert.ToInt32(((Label)(this.Owner.FindName("lblUserID"))).Content.ToString());
                Role_ID = Convert.ToInt32(((Label)(this.Owner.FindName("lblRoleID"))).Content.ToString());
                EnableDisable_controls(false);

                btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
                btnSave.Visibility = System.Windows.Visibility.Collapsed;
                btnEdit.Visibility = System.Windows.Visibility.Collapsed;

                Bind_Grade();
                BindCompany();

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
                string validate = CheckValidation();
                if (validate == "1")
                {
                    if (EmployeeGridView.SelectedItems.Count > 0)
                    {

                        // CheckValidation();
                        Employee_Master item = (Employee_Master)EmployeeGridView.SelectedValue;
                        if (item != null)
                        {

                            Employee_Master objemp = new Employee_Master();
                            objemp.Employee_ID = item.Employee_ID;
                            objemp.Department = txtdepartment.Text.Trim();
                            objemp.Designation = txtdesignation.Text.Trim();
                            objemp.Email = txtemail.Text.Trim();
                            objemp.Employee_Name = txtemployee_Name.Text.Trim();
                            objemp.Phone = txtphone.Text.Trim();
                            objemp.Remark = txtremark.Text.Trim();
                            objemp.Company_ID = Convert.ToInt32(cmbcompany_IDComboBox.SelectedValue);
                            objemp.Updated_DateTime = DateTime.Now;
                            objemp.Is_Active = chkis_ActiveCheckBox.IsChecked;
                            objemp.Grade_ID = Convert.ToInt32(cmbGrade.SelectedValue.ToString());
                            //objemp.Created_By = User_ID;
                            objemp.Updated_By = User_ID;
                            bool isAlreadyAvailable = obj.check_isDuplicateEmpaloyee(txtemployee_Name.Text.Trim(), objemp.Employee_ID);

                            if (isAlreadyAvailable == false)
                            {
                                if (obj.Update_Employee(objemp) > 0)
                                {
                                    Bindemp();

                                    MessageBox.Show("Employee Updated sussufully", "Megabite", MessageBoxButton.OK, MessageBoxImage.Information);
                                    Reset();
                                    EnableDisable_controls(false);

                                }
                                else
                                {
                                    MessageBox.Show("Employee Updated failed", "Megabite", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Entered employee name already availble", "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


                            }
                        }
                    }

                }
                else
                {
                    MessageBox.Show(validate, "Megabite", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string validate = CheckValidation();
                if (validate == "1")
                {
                    Employee_Master objemp = new Employee_Master();
                    objemp.Department = txtdepartment.Text.Trim();
                    objemp.Designation = txtdesignation.Text.Trim();
                    objemp.Email = txtemail.Text.Trim();
                    objemp.Employee_Name = txtemployee_Name.Text.Trim();
                    objemp.Phone = txtphone.Text.Trim();
                    objemp.Remark = txtremark.Text.Trim();
                    objemp.Company_ID = Convert.ToInt32(cmbcompany_IDComboBox.SelectedValue);
                    objemp.Grade_ID = Convert.ToInt32(cmbGrade.SelectedValue.ToString());
                    objemp.Is_Active = chkis_ActiveCheckBox.IsChecked;
                    objemp.Created_By = User_ID;
                    objemp.Updated_By = User_ID;
                    objemp.Updated_DateTime = DateTime.Now;
                    objemp.Created_DateTime = DateTime.Now;

                    bool isAlreadyAvailable = obj.check_isDuplicateEmpaloyee(txtemployee_Name.Text.Trim(), objemp.Employee_ID);

                    if (isAlreadyAvailable == false)
                    {
                        var count = obj.Save_Employee(objemp);
                        if (count > 0)
                        {
                            MessageBox.Show("Employee Added sussufully", "Megabite", MessageBoxButton.OK, MessageBoxImage.Information);
                            Bindemp();
                            Reset();
                            EnableDisable_controls(false);
                        }
                        else
                        {
                            MessageBox.Show("failed to add employee", "Megabite", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Entered employee name already availble", "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


                    }
                }
                else
                {
                    MessageBox.Show(validate, "Megabite", MessageBoxButton.OK, MessageBoxImage.Information);

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            EnableDisable_controls(false);
            btnNew.IsEnabled = true;
            btnEdit.IsEnabled = true;


        }
        private void lstEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                EnableDisable_controls(false);
                btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
                btnSave.Visibility = System.Windows.Visibility.Collapsed;

                if (EmployeeGridView.SelectedValue != null)
                {
                    EditgrdEmp(); ;
                    btnEdit.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    btnEdit.Visibility = System.Windows.Visibility.Collapsed;
                }
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;

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
                btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
                btnSave.Visibility = System.Windows.Visibility.Visible;

                btnNew.IsEnabled = false;
                btnEdit.IsEnabled = false;



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
                Employee_Master item = (Employee_Master)EmployeeGridView.SelectedValue;
                if (item != null)
                {
                    EnableDisable_controls(true);


                    if (!checkIsCardAssigned(item.Employee_ID))
                    {

                        chkis_ActiveCheckBox.IsEnabled = false;
                    }
                    else
                    {
                        chkis_ActiveCheckBox.IsEnabled = true;
                    }
                    btnNew.IsEnabled = false;
                    btnEdit.IsEnabled = false;

                    btnUpdate.Visibility = System.Windows.Visibility.Visible;
                    btnSave.Visibility = System.Windows.Visibility.Collapsed;

                }
                else
                {
                    MessageBox.Show("Please select atleast one employee for edit");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool checkIsCardAssigned(int emp_id)
        {
            return obj.check_isCardAssigned(emp_id);
        }


        private void CloseCommand_Executed(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Methods

        private void Bind_Grade()
        {
            cmbGrade.ItemsSource = obj.get_GradeList();
        }

        private void EnableDisable_controls(bool flag)
        {

            txtemployee_Name.IsEnabled = flag;
            txtdepartment.IsEnabled = flag;
            txtdesignation.IsEnabled = flag;
            txtemail.IsEnabled = flag;
            txtphone.IsEnabled = flag;
            txtremark.IsEnabled = flag;
            cmbcompany_IDComboBox.IsEnabled = flag;
            cmbGrade.IsEnabled = flag;
            chkis_ActiveCheckBox.IsEnabled = flag;


        }

        public void EditgrdEmp()
        {
            if (EmployeeGridView.Items.Count > 0)
            {
                Employee_Master item = (Employee_Master)EmployeeGridView.SelectedValue;
                if (item != null)
                {
                    txtemployee_Name.Text = item.Employee_Name;
                    txtdepartment.Text = item.Department;
                    txtdesignation.Text = item.Designation;
                    txtemail.Text = item.Email;
                    txtphone.Text = item.Phone;
                    txtremark.Text = item.Remark;
                    chkis_ActiveCheckBox.IsChecked = item.Is_Active;
                    cmbcompany_IDComboBox.SelectedValue = item.Company_ID.ToString();//Convert.ToInt32(item.Company_ID);
                    cmbGrade.SelectedValue = item.Grade_ID.ToString();
                }
            }
        }

        private void Bindemp()
        {
            EmployeeGridView.ItemsSource = obj.Bind_Employee();

        }

        private void Reset()
        {


            txtemployee_Name.Text = "";
            txtdepartment.Text = "";
            txtdesignation.Text = "";
            txtemail.Text = "";
            txtphone.Text = "";
            txtremark.Text = "";
            cmbcompany_IDComboBox.SelectedIndex = -1;
            EmployeeGridView.SelectedIndex = -1;
            cmbGrade.SelectedIndex = -1;

            btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
            btnSave.Visibility = System.Windows.Visibility.Collapsed;
            btnEdit.Visibility = System.Windows.Visibility.Collapsed;
            btnNew.IsEnabled = true;
        }

        private void BindCompany()
        {

            cmbcompany_IDComboBox.ItemsSource = obj.Bindcompany();

        }

        private string CheckValidation()
        {
            string errorText = "1";

            if (txtemployee_Name.Text.Trim().Length < 1)
            {
                errorText += " Please enter employee name, " + "\n";

            }

            if (cmbcompany_IDComboBox.SelectedIndex < 0)
            {
                errorText += " Please select company, " + "\n";

            }

            if (txtdepartment.Text.Trim().Length < 1)
            {
                errorText += " Please enter employee department " + "\n";

            }
            if (txtdesignation.Text.Trim().Length < 1)
            {
                errorText += " Please enter employee designation, " + "\n";


            }
            if (txtemail.Text.Trim().Length < 1)
            {
                errorText += " Please enter employee email, " + "\n";

            }
            else
            {
                if (!(objValidation.IsValidEmail(txtemail.Text.Trim())))
                {
                    errorText += "Invalid email !" + "\n";
                }

            }

            if (txtphone.Text.Trim().Length < 1)
            {
                errorText += " Please enter employee phone no., " + "\n";


            }
            else
            {
                if (!(objValidation.IsValidPhone(txtphone.Text.Trim())))
                {
                    errorText += "Invalid Mobile !" + "\n";
                }

            }
            if (cmbGrade.SelectedIndex < 0)
            {
                errorText += " Please select Grade, " + "\n";
            }

            if (errorText != "1")
            {

                errorText = errorText.Replace('1', ' ');
            }
            return errorText;
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtSearch.Text))
                return true;
            else
                return ((item as Employee_Master).Employee_Name.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        #endregion



    }
}
