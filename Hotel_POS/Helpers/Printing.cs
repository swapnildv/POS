//Name:                       Printing class
//Description:                This class used for printing the data.
//Date of creation:           19th Jan 2014
//Created by:                 Nitin Kadam
//                            8976657787
//                            nitinkadam.567@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Printing;
using MegabiteEntityLayer;
using System.Management;
using POS_Business;
using Hotel_POS.Resource;

namespace Hotel_POS
{
    public static class Printing
    {
        private static double slipWidth = 180;
        private static void PrintSlip(Panel printElement)
        {
            PrintDialog printDialog = new PrintDialog();
            printElement.Measure(new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight));
            printElement.Arrange(new Rect(new Point(0, 0), printElement.DesiredSize));
            for (int i = 0; i < 2; i++)
            {
                printDialog.PrintVisual(printElement, "Printing....");
            }
        }
        public static void CheckPrinterStaus()
        {
            // Set management scope
            ManagementScope scope = new ManagementScope(@"\root\cimv2");
            scope.Connect();

            // Select Printers from WMI Object Collections
            ManagementObjectSearcher searcher = new
             ManagementObjectSearcher("SELECT * FROM Win32_Printer");

            string printerName = "";
            foreach (ManagementObject printer in searcher.Get())
            {
                printerName = printer["Name"].ToString().ToLower();
                if (printerName.Equals(@"hp deskjet 930c"))
                {
                    Console.WriteLine("Printer = " + printer["Name"]);
                    if (printer["WorkOffline"].ToString().ToLower().Equals("true"))
                    {
                        // printer is offline by user
                        Console.WriteLine("Your Plug-N-Play printer is not connected.");
                    }
                    else
                    {
                        // printer is not offline
                        Console.WriteLine("Your Plug-N-Play printer is connected.");
                    }
                }
            }
        }
        public static void PrintPaymentSlip(Transaction_Master order)
        {
            StackPanel printPanel = new StackPanel() { Width = slipWidth, Margin = new Thickness(12, 0, 10, 10) };
            printPanel.Children.Add(new TextBlock() { Height = 5 });
            printPanel.Children.Add(new TextBlock() { Text = TerminalCommon.cafeName, HorizontalAlignment = HorizontalAlignment.Center, FontSize = 5, FontWeight = FontWeights.Bold, Margin = new Thickness(5, 0, 0, 0) });
            printPanel.Children.Add(new TextBlock() { Text = TerminalCommon.cafeAddress1, HorizontalAlignment = HorizontalAlignment.Center, FontSize = 5, FontWeight = FontWeights.Bold, TextWrapping = TextWrapping.Wrap });
            printPanel.Children.Add(new TextBlock() { Text = TerminalCommon.cafeAddress2, HorizontalAlignment = HorizontalAlignment.Center, FontSize = 5, FontWeight = FontWeights.Bold, TextWrapping = TextWrapping.Wrap });
            printPanel.Children.Add(new TextBlock() { Height = 5 });
            TextBlock Order_No = new TextBlock() { Text = "Order: " + order.Order_No, HorizontalAlignment = HorizontalAlignment.Left, FontSize = 9, FontWeight = FontWeights.Bold };
            TextBlock txtDateTime = new TextBlock() { Text = "Date : " + DateTime.Now.ToString(), FontSize = 9 };
            printPanel.Children.Add(Order_No);
            printPanel.Children.Add(txtDateTime);

            printPanel.Children.Add(new TextBlock() { Text = "----------------------------------------------------------------", FontSize = 10 });
            var menuCart = new BL_Menu().GetMenuCart();
            foreach (MenuCart item in menuCart)
            {
                TextBlock MenuName = new TextBlock() { Text = item.Item_Name, Width = slipWidth * 0.60, FontSize = 9, TextWrapping = TextWrapping.WrapWithOverflow };
                TextBlock MenuPrice = new TextBlock() { Text = item.Quantity.ToString() + " x " + item.Item_Unit_Price.ToString("0.00"), Width = slipWidth * 0.25, FontSize = 8 };
                TextBlock MenuTotal = new TextBlock() { Text = item.Item_Total.ToString("0.00"), Width = slipWidth * 0.15, FontSize = 8 };

                StackPanel displayMenus = new StackPanel() { Orientation = Orientation.Horizontal };
                displayMenus.Children.Add(MenuName);
                displayMenus.Children.Add(MenuPrice);
                displayMenus.Children.Add(MenuTotal);
                printPanel.Children.Add(displayMenus);
            }

            TextBlock totalStake = new TextBlock() { Text = "Total :", Width = slipWidth * 0.60, FontSize = 8, FontWeight = FontWeights.Bold };
            StackPanel totalStakeWithValue = new StackPanel() { Orientation = Orientation.Horizontal };
            totalStakeWithValue.Children.Add(totalStake);
            StackPanel discountStack = new StackPanel() { Width = slipWidth * 0.40 };
            discountStack.Children.Add(new TextBlock() { Text = " " + order.Transaction_Amount.Value.ToString("0.00"), Width = slipWidth * 0.40, FontSize = 9, FontWeight = FontWeights.Bold });
            if (order.Discount_Perc > 0)
            {
                discountStack.Children.Add(new TextBlock() { Text = "- " + order.Discount_Perc + " % ", FontSize = 8, FontWeight = FontWeights.Bold });
                discountStack.Children.Add(new TextBlock() { Text = order.Discount_Value.ToString("0.00"), FontSize = 8, FontWeight = FontWeights.Bold });
            }
            totalStakeWithValue.Children.Add(discountStack);
            printPanel.Children.Add(totalStakeWithValue);

            TextBlock promoText = new TextBlock() { Margin = new Thickness(0, 2, 0, 0), Text = "---Thank You, Visit Again---", HorizontalAlignment = HorizontalAlignment.Center, FontSize = 7 };
            printPanel.Children.Add(promoText);



            Printing.PrintSlip(printPanel);
        }
    }
}
