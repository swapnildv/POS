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
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        #region Variables

        public BL_Menu objitemMaster = new BL_Menu();
        public MyValidation objValidation = new MyValidation();
        public Int32 Company_ID, User_ID;
        public MenuWindow()
        {
            try
            {

                InitializeComponent();
                BindMenuGrd();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);




            }
        }

        #endregion

        #region Events

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {

                this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                BindItemTypeID();
                EnableDisable_controls(false);
                btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
                btnSave.Visibility = System.Windows.Visibility.Collapsed;
                btnEdit.Visibility = System.Windows.Visibility.Collapsed;
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                Company_ID = Convert.ToInt32(((Label)(this.Owner.FindName("lblCompanyID"))).Content.ToString());
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

                string validate = CheckValidation();
                if (validate == "1")
                {
                    if (txtitem_NameTextBox.Text != "" || txtitem_Unit_PriceTextBlock.Text != "" || cmbitem_Type_IDComboBox.SelectedIndex != -1)
                    {

                        Item_Master itemMaster = new Item_Master();
                        itemMaster.Item_Name = txtitem_NameTextBox.Text.Trim();
                        itemMaster.Item_Unit_Price = Convert.ToDouble(txtitem_Unit_PriceTextBlock.Text.Trim());
                        itemMaster.Item_Type_ID = Convert.ToInt32(cmbitem_Type_IDComboBox.SelectedValue);
                        itemMaster.Is_Active = chkis_ActiveCheckBox.IsChecked;
                        itemMaster.Company_ID = Company_ID;
                        itemMaster.Created_DateTime = DateTime.Now;
                        itemMaster.Updated_DateTime = DateTime.Now;
                        itemMaster.Created_By = User_ID;
                        itemMaster.Updated_By = User_ID;

                        bool isAlreadyAvailable = check_isAlreadyAvailable(txtitem_NameTextBox.Text.Trim(), itemMaster.Item_ID);

                        if (isAlreadyAvailable == false)
                        {
                            var count = objitemMaster.InsertItems(itemMaster);
                            if (count > 0)
                            {
                                MessageBox.Show("Item Added Successfully !", "Megabite", MessageBoxButton.OK, MessageBoxImage.Information);
                                BindMenuGrd();
                            }
                            Reset();
                            EnableDisable_controls(false);
                            btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
                            btnSave.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else
                        {
                            MessageBox.Show("Entered menu name already availble", "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


                        }


                    }
                    else
                    {
                        MessageBox.Show("Please enter all mandetory fields", "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }
                else
                {
                    MessageBox.Show(validate.Replace('1', ' '), "Megabite", MessageBoxButton.OK, MessageBoxImage.Warning);



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
                btnSave.Visibility = System.Windows.Visibility.Collapsed;
                btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
                EnableDisable_controls(false);

                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                Reset();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);




            }

        }

        private void lstMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                EnableDisable_controls(false);
                btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
                btnSave.Visibility = System.Windows.Visibility.Collapsed;


                BindItemTypeID();
                if (MenuItemGridView.SelectedValue != null)
                {
                    EditgrdMenu();
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
                EnableDisable_controls(false);
                btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
                btnSave.Visibility = System.Windows.Visibility.Collapsed;



                if (MenuItemGridView.SelectedValue != null)
                {
                    EditgrdMenu();
                    btnEdit.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    btnEdit.Visibility = System.Windows.Visibility.Collapsed;
                }
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                Reset();
                EnableDisable_controls(true);
                btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
                btnSave.Visibility = System.Windows.Visibility.Visible;

                btnNew.IsEnabled = false;
                btnEdit.IsEnabled = false;

                BindItemTypeID();
                cmbitem_Type_IDComboBox.IsEnabled = true;
                cmbitem_Type_IDComboBox.ItemsSource = ((List<Item_Group_Master>)cmbitem_Type_IDComboBox.ItemsSource).Where(c => c.Is_Active == true);

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
                btnUpdate.Visibility = System.Windows.Visibility.Visible;
                btnSave.Visibility = System.Windows.Visibility.Collapsed;
                cmbitem_Type_IDComboBox.IsEnabled = false;
                btnNew.IsEnabled = false;
                btnEdit.IsEnabled = false;
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
                    if (txtitem_NameTextBox.Text != "" || txtitem_Unit_PriceTextBlock.Text != "" || cmbitem_Type_IDComboBox.SelectedIndex != -1)
                    {

                        ItemMasterMenu item = (ItemMasterMenu)MenuItemGridView.SelectedValue;
                        Item_Master itemMaster = new Item_Master();
                        itemMaster.Item_ID = item.Item_ID;
                        itemMaster.Item_Name = txtitem_NameTextBox.Text.Trim();
                        itemMaster.Item_Unit_Price = Convert.ToDouble(txtitem_Unit_PriceTextBlock.Text.Trim());
                        itemMaster.Item_Type_ID = Convert.ToInt32(cmbitem_Type_IDComboBox.SelectedValue);
                        itemMaster.Is_Active = chkis_ActiveCheckBox.IsChecked;
                        itemMaster.Company_ID = Company_ID;
                        itemMaster.Updated_By = User_ID;
                        itemMaster.Updated_DateTime = DateTime.Now;
                        bool isAlreadyAvailable = check_isAlreadyAvailable(txtitem_NameTextBox.Text.Trim(), itemMaster.Item_ID);

                        if (isAlreadyAvailable == false)
                        {
                            var count = objitemMaster.UpdateItems(itemMaster);
                            if (count > 0)
                            {
                                BindMenuGrd();
                                MessageBox.Show("Menu Item Updated sucessfully !", "Megabite", MessageBoxButton.OK, MessageBoxImage.Information);

                            }
                            Reset();
                            EnableDisable_controls(false);
                            btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
                            btnSave.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else
                        {
                            MessageBox.Show("Entered menu name already availble", "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);


                        }

                    }
                    else
                    {
                        MessageBox.Show("Please enter all mandetory fields", "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }
                else
                {
                    MessageBox.Show(validate.Replace('1', ' '), "Megabite", MessageBoxButton.OK, MessageBoxImage.Warning);



                }


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

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(MenuItemGridView.ItemsSource);
                view.Filter = UserFilter;
                CollectionViewSource.GetDefaultView(MenuItemGridView.ItemsSource).Refresh();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);




            }

        }
        #endregion

        #region Methods

        private void EnableDisable_controls(bool flag)
        {

            txtitem_NameTextBox.IsEnabled = flag;
            txtitem_Unit_PriceTextBlock.IsEnabled = flag;
            cmbitem_Type_IDComboBox.IsEnabled = flag;
            chkis_ActiveCheckBox.IsEnabled = flag;
        }

        private void Reset()
        {
            txtitem_NameTextBox.Text = "";
            txtitem_Unit_PriceTextBlock.Text = "";
            cmbitem_Type_IDComboBox.SelectedIndex = -1;
            chkis_ActiveCheckBox.IsChecked = false;
            btnNew.IsEnabled = true;
        }

        private void BindItemTypeID()
        {
            cmbitem_Type_IDComboBox.ItemsSource = objitemMaster.BindItemTypeID();
        }

        private void BindMenuGrd()
        {
            MenuItemGridView.ItemsSource = null;

            MenuItemGridView.ItemsSource = objitemMaster.BindGvMenu();
        }

        private string CheckValidation()
        {
            string errorText = "1";

            if (txtitem_NameTextBox.Text.Length < 1)
            {
                errorText += "Please enter Item Name !" + "\n";

            }
            else
            {
                if (!(objValidation.IsValidAlphaNumeric(txtitem_NameTextBox.Text.Trim())))
                {
                    errorText += "Invalid Item Name (Must Be AlphaNumeric Allowed Only _ - & chars & Numbers) !" + "\n";
                }

            }
            if (txtitem_Unit_PriceTextBlock.Text.Length < 1)
            {
                errorText += "Please enter Unit Price !" + "\n";
            }
            else
            {
                if (!(objValidation.IsValidAmount(txtitem_Unit_PriceTextBlock.Text.Trim())))
                {
                    errorText += "Invalid Price !" + "\n";
                }
                else
                {
                    if (Double.Parse(txtitem_Unit_PriceTextBlock.Text.Trim()) == 0.0)
                    {
                        errorText += "Price Must Be Greater Than Zero(0) " + "\n";
                    }
                }

            }

            if (cmbitem_Type_IDComboBox.SelectedIndex < 0)
            {
                errorText += "Please select Item Type !" + "\n";

            }

            if (errorText != "1")
            {
                errorText.Replace("1", " ");
            }

            return errorText;

        }

        private bool check_isAlreadyAvailable(string Item_Name, long Item_ID)
        {
            return objitemMaster.check_isDuplicateMenu(Item_Name, Item_ID);
        }


        public void EditgrdMenu()
        {
            if (MenuItemGridView.Items.Count > 0)
            {

                ItemMasterMenu item = (ItemMasterMenu)MenuItemGridView.SelectedValue;
                if (item != null)
                {
                    txtitem_NameTextBox.Text = item.Item_Name;
                    txtitem_Unit_PriceTextBlock.Text = item.Item_Unit_Price.ToString("0.00");
                    cmbitem_Type_IDComboBox.SelectedValue = Convert.ToInt32(item.Item_Type_Id);
                    if (item.IS_Active == "Available")
                    {
                        chkis_ActiveCheckBox.IsChecked = true;
                    }
                    else
                    {
                        chkis_ActiveCheckBox.IsChecked = false;
                    }
                }

            }
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtSearch.Text))
                return true;
            else
                return ((item as ItemMasterMenu).Item_Name.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        #endregion




    }
}
