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

namespace Hotel_POS.MessageHelper
{
    /// <summary>
    /// Interaction logic for ErrorBox.xaml
    /// </summary>
    public partial class ErrorBox : Window
    {
        public static RoutedCommand enterCommand = new RoutedCommand("enterCommand", typeof(ErrorBox));
        private bool isCanExecute = true;

        public ErrorBox(string errorMessage)
        {
            InitializeComponent();
            errorMessageTextBlock.Text = errorMessage;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            isCanExecute = true;
            this.Close(); 
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Button target = e.Source as Button;
            if (target != null)
            {
                if (isCanExecute)
                {
                    okButton_Click(target, e);
                }
                isCanExecute = true;
            }
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
