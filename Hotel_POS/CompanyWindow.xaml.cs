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


namespace Hotel_POS
{
    /// <summary>
    /// Interaction logic for CompanyWindow.xaml
    /// </summary>
    public partial class CompanyWindow : Window
    {


        #region Variables
        public MyValidation objValidation = new MyValidation();



        #endregion

        #region Events

        public CompanyWindow()
        {
            InitializeComponent();
        }
        public int User_ID;
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

            try
            {
                this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                EnableDisable_controls(false);
                Bind_CompanyList();
                btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
                btnSave.Visibility = System.Windows.Visibility.Collapsed;
                btnEdit.Visibility = System.Windows.Visibility.Collapsed;
                User_ID = Convert.ToInt32(((Label)(this.Owner.FindName("lblUserID"))).Content.ToString());
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
                string validation = CheckValidation();
                if (validation == "1")
                {
                    Create_Company();
                    clear_controls();
                    EnableDisable_controls(false);
                    btnNew.IsEnabled = true;
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                clear_controls();
                EnableDisable_controls(false);
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
                clear_controls();
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
                EnableDisable_controls(true);

                MegabiteEntityLayer.Company_Master obj = new MegabiteEntityLayer.Company_Master();
                obj = (MegabiteEntityLayer.Company_Master)company_MasterListView.SelectedValue;

                if (checkIsCompanyMapped(obj.Company_ID) > 0)
                {
                    is_ActiveCheckBox.IsEnabled = false;
                }
                else
                {
                    is_ActiveCheckBox.IsEnabled = true;
                }
                btnUpdate.Visibility = System.Windows.Visibility.Visible;
                btnSave.Visibility = System.Windows.Visibility.Collapsed;

                btnNew.IsEnabled = false;
                btnEdit.IsEnabled = false;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void CloseCommand_Executed(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void company_MasterListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                EnableDisable_controls(false);
                btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
                btnSave.Visibility = System.Windows.Visibility.Collapsed;


                if (company_MasterListView.SelectedValue != null)
                {
                    Set_Controls(company_MasterListView.SelectedValue);
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

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string validation = CheckValidation();
                if (validation == "1")
                {

                    Update_Company();
                    clear_controls();
                    EnableDisable_controls(false);
                    btnNew.IsEnabled = true;
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

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(company_MasterListView.ItemsSource);
                view.Filter = UserFilter;
                CollectionViewSource.GetDefaultView(company_MasterListView.ItemsSource).Refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        #endregion

        #region Methods




        private int checkIsCompanyMapped(int Company_ID)
        {
            BL_EmployeeMaster objEmp = new BL_EmployeeMaster();
            return objEmp.checkIsCompanyMapped(Company_ID);
        }



        private void clear_controls()
        {



            company_NameTextBox.Text = "";
            company_AddressTextBox.Text = "";
            cityTextBox.Text = "";
            stateTextBox.Text = "";
            phoneTextBox.Text = "";
            emailTextBox.Text = "";
            contact_PersonTextBox.Text = "";
            contact_Person_EmailTextBox.Text = "";
            contact_Person_PhoneTextBox.Text = "";
            remarkTextBox.Text = "";
            is_ActiveCheckBox.IsChecked = true;
            company_MasterListView.SelectedIndex = -1;
            btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
            btnSave.Visibility = System.Windows.Visibility.Collapsed;
            btnEdit.Visibility = System.Windows.Visibility.Collapsed;
            btnNew.IsEnabled = true;
        }

        private void EnableDisable_controls(bool flag)
        {

            company_NameTextBox.IsEnabled = flag;
            company_AddressTextBox.IsEnabled = flag;
            cityTextBox.IsEnabled = flag;
            stateTextBox.IsEnabled = flag;
            phoneTextBox.IsEnabled = flag;
            emailTextBox.IsEnabled = flag;
            contact_PersonTextBox.IsEnabled = flag;
            contact_Person_EmailTextBox.IsEnabled = flag;
            contact_Person_PhoneTextBox.IsEnabled = flag;
            remarkTextBox.IsEnabled = flag;
            is_ActiveCheckBox.IsEnabled = flag;

        }

        private void Set_Controls(object p)
        {
            MegabiteEntityLayer.Company_Master obj = new MegabiteEntityLayer.Company_Master();
            obj = (MegabiteEntityLayer.Company_Master)p;
            if (obj != null)
            {
                company_NameTextBox.Text = obj.Company_Name;
                company_AddressTextBox.Text = obj.Company_Address;
                cityTextBox.Text = obj.City;
                stateTextBox.Text = obj.State;
                phoneTextBox.Text = obj.Phone;
                emailTextBox.Text = obj.Email;
                contact_PersonTextBox.Text = obj.Contact_Person;
                contact_Person_EmailTextBox.Text = obj.Contact_Person_Email;
                contact_Person_PhoneTextBox.Text = obj.Contact_Person_Phone;
                remarkTextBox.Text = obj.Remark;
                is_ActiveCheckBox.IsChecked = obj.Is_Active;

            }


        }


        private void Bind_CompanyList()
        {
            BL_CompanyMaster obj = new BL_CompanyMaster();
            company_MasterListView.ItemsSource = obj.get_CompanyMaster();


        }



        private void Create_Company()
        {
            Company_Master obj = new Company_Master();

            obj.Company_Name = company_NameTextBox.Text;
            obj.Company_Address = company_AddressTextBox.Text;
            obj.City = cityTextBox.Text;
            obj.State = stateTextBox.Text;
            obj.Phone = phoneTextBox.Text;
            obj.Email = emailTextBox.Text;
            obj.Contact_Person = contact_PersonTextBox.Text;
            obj.Contact_Person_Email = contact_Person_EmailTextBox.Text;
            obj.Contact_Person_Phone = contact_Person_PhoneTextBox.Text;
            obj.Remark = remarkTextBox.Text;
            obj.Is_Active = is_ActiveCheckBox.IsChecked;
            obj.Created_DateTime = DateTime.Now;
            obj.Updated_DateTime = DateTime.Now;
            obj.Created_By = User_ID;
            obj.Updated_By = User_ID;
            BL_CompanyMaster objbl = new BL_CompanyMaster();

            bool isAlreadyAvailable = objbl.check_isDuplicateCompany(company_NameTextBox.Text.Trim(), obj.Company_ID);

            if (isAlreadyAvailable == false)
            {
                if (objbl.Create_Company(obj) > 0)
                {
                    MessageBox.Show("Company Created Successfully !", "Megabite", MessageBoxButton.OK, MessageBoxImage.Information);

                    Bind_CompanyList();
                }

            }
            else
            {
                MessageBox.Show("Entered comapany name already availble", "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


            }




        }

        private void Update_Company()
        {
            Company_Master obj = new Company_Master();
            obj = (Company_Master)company_MasterListView.SelectedValue;


            obj.Company_Name = company_NameTextBox.Text;
            obj.Company_Address = company_AddressTextBox.Text;
            obj.City = cityTextBox.Text;
            obj.State = stateTextBox.Text;
            obj.Phone = phoneTextBox.Text;
            obj.Email = emailTextBox.Text;
            obj.Contact_Person = contact_PersonTextBox.Text;
            obj.Contact_Person_Email = contact_Person_EmailTextBox.Text;
            obj.Contact_Person_Phone = contact_Person_PhoneTextBox.Text;
            obj.Remark = remarkTextBox.Text;
            obj.Is_Active = is_ActiveCheckBox.IsChecked;
            obj.Updated_DateTime = DateTime.Now;
            obj.Updated_By = User_ID;
            BL_CompanyMaster objbl = new BL_CompanyMaster();

            bool isAlreadyAvailable = objbl.check_isDuplicateCompany(company_NameTextBox.Text.Trim(), obj.Company_ID);

            if (isAlreadyAvailable == false)
            {

                if (objbl.Update_Company(obj) > 0)
                {
                    Bind_CompanyList();
                    MessageBox.Show("Company Updated Successfully !", "Megabite", MessageBoxButton.OK, MessageBoxImage.Information);


                }
                else
                {
                    MessageBox.Show("Failed To Update ", "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Entered comapany name already availble", "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


            }

        }

        private string CheckValidation()
        {
            string errorText = "1";

            if (company_NameTextBox.Text.Trim().Length < 1)
            {
                errorText += "Please enter company name !" + "\n";

            }
            if (company_AddressTextBox.Text.Trim().Length < 1)
            {
                errorText += "Please enter address !" + "\n";
            }
            if (emailTextBox.Text.Trim().Length < 1)
            {
                errorText += "Please enter email !" + "\n";
            }
            else
            {
                if (!(objValidation.IsValidEmail(emailTextBox.Text.Trim())))
                {
                    errorText += "Invalid email !" + "\n";
                }

            }
            if (contact_PersonTextBox.Text.Trim().Length < 1)
            {
                errorText += "Please enter contact person !" + "\n";
            }
            if (contact_Person_EmailTextBox.Text.Trim().Length < 1)
            {
                errorText += "Please enter contact person email !" + "\n";
            }
            else
            {
                if (!(objValidation.IsValidEmail(contact_Person_EmailTextBox.Text.Trim())))
                {
                    errorText += "Invalid Contact Person Email !" + "\n";
                }

            }
            if (cityTextBox.Text.Trim().Length < 1)
            {
                errorText += "Please enter city !" + "\n";
            }
            if (phoneTextBox.Text.Trim().Length < 1)
            {
                errorText += "Please enter phone no. !" + "\n";
            }
            else
            {
                if (!(objValidation.IsValidPhone(phoneTextBox.Text.Trim())))
                {
                    errorText += "Invalid Phone !" + "\n";
                }

            }
            if (contact_Person_PhoneTextBox.Text.Trim().Length < 1)
            {
                errorText += "Please enter contact person mobile no. !" + "\n";
            }
            else
            {
                if (!(objValidation.IsValidPhone(contact_Person_PhoneTextBox.Text.Trim())))
                {
                    errorText += "Invalid Contact Person Mobile !" + "\n";
                }

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
                return ((item as Company_Master).Company_Name.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        #endregion











    }
}
