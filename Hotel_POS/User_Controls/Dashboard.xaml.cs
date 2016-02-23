﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hotel_POS.Resource;
using POS_Business;

namespace Hotel_POS.User_Controls
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                totalSalesTextBlock.Text = TerminalCommon.currency + new BL_Transaction().getTodaysSale();
                var favItems = new BL_Transaction().getFavoriteItems();
                if (favItems.Count > 0)
                {
                    dgFavoriteItems.Visibility = System.Windows.Visibility.Visible;
                    dgFavoriteItems.ItemsSource = favItems;
                }
                else
                    dgFavoriteItems.Visibility = System.Windows.Visibility.Collapsed;


                //dgOrders.ItemsSource = new BL_Transaction().getMasterOrders();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }
    }
}
