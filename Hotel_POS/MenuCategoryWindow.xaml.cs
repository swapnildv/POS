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
using Hotel_POS.Resource;
using log4net;

namespace Hotel_POS
{

    public partial class MenuCategoryWindow : Window
    {
        #region Variables
        public MyValidation objValidation = new MyValidation();
        private static readonly ILog _logger =
         LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public MenuCategoryWindow()
        {

            InitializeComponent();

        }
        BL_Menu obj = new BL_Menu();
        #endregion

        #region Events
        public Int32 Company_ID;
        public Int32 User_ID;
        private void MenuCategoryWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                BindMenuCategory();
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
                _logger.Error(ex);
                MessageHelper.MessageBox.ShowError(this);
            }

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txt_MenuCategory.Text.Trim() != "")
                {

                    Item_Group_Master objcategory = new Item_Group_Master();
                    objcategory = (Item_Group_Master)MenuCategoryGridView.SelectedValue;



                    bool isAlreadyAvailable = check_isAlreadyAvailable(txt_MenuCategory.Text.Trim(), objcategory.Item_Group_ID);


                    if (isAlreadyAvailable == false)
                    {
                        objcategory.Is_Active = chkis_ActiveCheckBox.IsChecked;
                        objcategory.Item_Group_Name = txt_MenuCategory.Text.Trim();
                        objcategory.Company_ID = Company_ID;
                        objcategory.Updated_DateTime = DateTime.Now;
                        objcategory.Updated_By = User_ID;
                        if (obj.UpdateMenuCategory(objcategory) >= 0)
                        {

                            MessageBox.Show("Category Updated Successfully !", "Megabite", MessageBoxButton.OK, MessageBoxImage.Information);
                            BindMenuCategory();

                            Reset();
                            EnableDisable_controls(false);
                            btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
                            btnSave.Visibility = System.Windows.Visibility.Collapsed;
                            btnNew.IsEnabled = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Entered menu category already availble", "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

                    }

                }
                else
                {
                    MessageBox.Show("Please enter menu category", "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageHelper.MessageBox.ShowError(this);
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EnableDisable_controls(false);
                btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
                btnSave.Visibility = System.Windows.Visibility.Collapsed;



                if (MenuCategoryGridView.SelectedValue != null)
                {
                    Set_Edit_Controls();
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
                txt_MenuCategory.Focus();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageHelper.MessageBox.ShowError(this);

            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EnableDisable_controls(true);
                btnUpdate.Visibility = System.Windows.Visibility.Visible;
                btnSave.Visibility = System.Windows.Visibility.Collapsed;

                btnNew.IsEnabled = false;
                btnEdit.IsEnabled = false;

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageHelper.MessageBox.ShowError(this);
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
                _logger.Error(ex);
                MessageHelper.MessageBox.ShowError(this);
            }
        }

        private void dgMenuCatDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                EnableDisable_controls(false);
                btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
                btnSave.Visibility = System.Windows.Visibility.Collapsed;



                if (MenuCategoryGridView.SelectedValue != null)
                {
                    Set_Edit_Controls();
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
                _logger.Error(ex);
                MessageHelper.MessageBox.ShowError(this);
            }
        }

        private void Set_Edit_Controls()
        {
            if (MenuCategoryGridView.Items.Count > 0)
            {
                // List<Item_Group_Master>
                Item_Group_Master item = (Item_Group_Master)MenuCategoryGridView.SelectedValue;
                if (item != null)
                {
                    txt_MenuCategory.Text = item.Item_Group_Name;
                    chkis_ActiveCheckBox.IsChecked = item.Is_Active;

                }

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
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(MenuCategoryGridView.ItemsSource);
                view.Filter = UserFilter;

                CollectionViewSource.GetDefaultView(MenuCategoryGridView.ItemsSource).Refresh();


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
                if (txt_MenuCategory.Text.Trim() != "")
                {
                    Item_Group_Master objcategory = new Item_Group_Master();
                    objcategory.Is_Active = chkis_ActiveCheckBox.IsChecked;
                    objcategory.Item_Group_Name = txt_MenuCategory.Text.Trim();
                    objcategory.Company_ID = Company_ID;
                    objcategory.Created_By = User_ID;
                    objcategory.Updated_By = User_ID;
                    objcategory.Created_DateTime = DateTime.Now;
                    objcategory.Updated_DateTime = DateTime.Now;
                    bool isAlreadyAvailable = check_isAlreadyAvailable(txt_MenuCategory.Text.Trim(), objcategory.Item_Group_ID);

                    if (isAlreadyAvailable == false)
                    {
                        if (obj.InsertMenuCategory(objcategory) > 0)
                        {

                            MessageBox.Show("Category Added Successfully !", "Megabite", MessageBoxButton.OK, MessageBoxImage.Information);
                            BindMenuCategory();

                            Reset();
                            EnableDisable_controls(false);
                            btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
                            btnSave.Visibility = System.Windows.Visibility.Collapsed;
                            btnNew.IsEnabled = true;

                        }
                    }
                    else
                    {
                        MessageBox.Show("Entered menu category already availble", "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

                    }

                }
                else
                {
                    MessageBox.Show("Please enter menu category", "Megabite", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageHelper.MessageBox.ShowError(this);
            }



        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        #endregion

        #region Methods

        private bool check_isAlreadyAvailable(string MenuCategory, Int32 Item_Type_ID)
        {
            return obj.check_isAlreadyAvailable(MenuCategory, Item_Type_ID);
        }


        private void BindMenuCategory()
        {
            List<Item_Group_Master> lstMenuCat = obj.BindMenuCategory();
            MenuCategoryGridView.ItemsSource = lstMenuCat;
            MenuCategoryGridView.ItemsSource = lstMenuCat;
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtSearch.Text))
                return true;
            else
                return ((item as Item_Group_Master).Item_Group_Name.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void EnableDisable_controls(bool flag)
        {
            txt_MenuCategory.IsEnabled = flag;
            chkis_ActiveCheckBox.IsEnabled = flag;
        }

        private void Reset()
        {
            txt_MenuCategory.Text = "";
            chkis_ActiveCheckBox.IsChecked = false;
            btnNew.IsEnabled = true;

            btnUpdate.Visibility = System.Windows.Visibility.Collapsed;
            btnSave.Visibility = System.Windows.Visibility.Collapsed;
            btnEdit.Visibility = System.Windows.Visibility.Collapsed;
            MenuCategoryGridView.SelectedIndex = -1;

        }
        #endregion


        #region Validation

        enum ValidationStates { OK, ERROR, WARNING };

        // Tables for regex and messages
        Hashtable previewRegex = new Hashtable();
        Hashtable completionRegex = new Hashtable();
        Hashtable errorMessage = new Hashtable();
        Hashtable validationState = new Hashtable();

        const string fieldRequired = "This field is required";
        private void KeypressValidation(object sender, TextCompositionEventArgs e)
        {
            try
            {

                // Handle to the textbox tjhat should be validated..
                TextBox tbox = (TextBox)sender;
                // Fetch regex..
                Regex regex = new Regex((string)previewRegex[(string)tbox.Tag]);
                // Check match and put error styles and messages..
                if (regex.IsMatch(e.Text))
                {
                    if ((ValidationStates)validationState[tbox.Name] != ValidationStates.OK) tbox.Style = (Style)FindResource("textBoxNormalStyle");
                    validationState[tbox.Name] = ValidationStates.OK;
                }
                else
                {
                    if ((ValidationStates)validationState[tbox.Name] != ValidationStates.WARNING)
                    {
                        BrushConverter bc = new BrushConverter();
                        Brush brush = (Brush)bc.ConvertFrom("#FF0000");
                        brush.Freeze();
                        tbox.BorderBrush = brush;
                        validationState[tbox.Name] = ValidationStates.WARNING;
                        tbox.UpdateLayout(); // Very important if want to use Template.FindName when changing style dynamically!
                    }
                    // Fetch the errorimage in the tbox:s control template.. 

                    // And set its tooltip to the errormessage of the textboxs validation code..
                    tbox.ToolTip = (string)errorMessage[(string)tbox.Tag];
                    // Use this if you dont want the user to enter something in textbox that invalidates it.
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageHelper.MessageBox.ShowError(this);
            }
        }

        private void CompletionValidation(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox tbox = (TextBox)sender;
                Regex regex = new Regex((string)completionRegex[(string)tbox.Tag]);
                if (regex.IsMatch(tbox.Text))
                {
                    if ((ValidationStates)validationState[tbox.Name] != ValidationStates.OK) tbox.Style = (Style)FindResource("textBoxNormalStyle");
                    validationState[tbox.Name] = ValidationStates.OK;
                }
                else
                {
                    if ((ValidationStates)validationState[tbox.Name] != ValidationStates.ERROR)
                    {
                        BrushConverter bc = new BrushConverter();
                        Brush brush = (Brush)bc.ConvertFrom("#FF0000");
                        brush.Freeze();
                        tbox.BorderBrush = brush;
                        validationState[tbox.Name] = ValidationStates.ERROR;
                        tbox.UpdateLayout();
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageHelper.MessageBox.ShowError(this);
            }
        }

        private void InitValidation(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox tbox = (TextBox)sender;
                if (validationState[tbox.Name] == null) validationState[tbox.Name] = ValidationStates.OK;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageHelper.MessageBox.ShowError(this);
            }
        }

        #endregion

       



    }
}
