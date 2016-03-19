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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hotel_POS.Resource;
using log4net;
using POS_Business;

namespace Hotel_POS.User_Controls
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {
        private static readonly ILog _logger =
         LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Dashboard()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex) { _logger.Error(ex); }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                try
                {
                    totalSalesTextBlock.Text = TerminalCommon.currency + new BL_Transaction().getTodaysSale();
                }
                catch (Exception ine) { _logger.Error(ine); }
                var favItems = new BL_Transaction().getFavoriteItems();
                if (favItems.Count > 0)
                {
                    favMenuStackPanel.Visibility = System.Windows.Visibility.Visible;
                    dgFavoriteItems.ItemsSource = favItems;
                }
                else
                {
                    favMenuStackPanel.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
