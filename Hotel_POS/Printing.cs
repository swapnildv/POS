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

namespace Hotel_POS
{
    public static class Printing
    {
        private static double slipWidth = 250;

        private static void PrintSlip(Panel printElement)
        {
            PrintDialog printDialog = new PrintDialog();
            printElement.Measure(new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight));
            printElement.Arrange(new Rect(new Point(0, 0), printElement.DesiredSize));
            printDialog.PrintVisual(printElement, "Printing....");


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
        private static void AddLogo(Panel printElement)
        {

            BitmapImage logo = new BitmapImage();
            try
            {
                logo.BeginInit();
                //C:\Program Files (x86)\Microsoft\MegabiteSetup\Images
                //C:\Program Files (x86)\DIIPL\Megabite\
                logo.StreamSource = new MemoryStream(File.ReadAllBytes("C:\\Program Files (x86)\\DIIPL\\Megabite\\Images\\mega-bite.bmp"));
                //mega-bite.png
                logo.EndInit();
            }
            catch (Exception ex)
            {

            }

            Image logoImage = new Image() { Source = logo, Stretch = Stretch.Uniform, Width = 140, Height = 70, HorizontalAlignment = HorizontalAlignment.Center };
            printElement.Children.Add(logoImage);

            //TextBlock hotelName = new TextBlock() { Text = "Megabite", HorizontalAlignment = HorizontalAlignment.Center, FontSize = 12, FontWeight = FontWeights.Bold };
            //printElement.Children.Add(hotelName);

        }

        private static void AddSlipEnd(Panel printElement)
        {
            printElement.Children.Add(new TextBlock() { Height = 15 });
            printElement.Children.Add(new TextBlock() { Height = 15 });

            TextBlock text = new TextBlock() { Text = "-x-", HorizontalAlignment = HorizontalAlignment.Center, FontSize = 10 };
            printElement.Children.Add(text);
        }

        private static void AddCardDetails(Panel printElement, Transaction_Master tm)
        {
            TextBlock txtCardHeader = new TextBlock() { Text = "Card Details ", FontWeight = FontWeights.Bold, HorizontalAlignment = HorizontalAlignment.Left, FontSize = 10 };
            printElement.Children.Add(txtCardHeader);

            TextBlock txtCardNumber = new TextBlock() { Text = "Card : " + tm.RFID_No, HorizontalAlignment = HorizontalAlignment.Left, FontSize = 10 };
            printElement.Children.Add(txtCardNumber);


            TextBlock txtRemaining = new TextBlock() { Text = "Remaining Bal : " + tm.Card_Closing_Balance, HorizontalAlignment = HorizontalAlignment.Left, FontSize = 10 };
            printElement.Children.Add(txtRemaining);

            TextBlock line2 = new TextBlock() { Text = "----------------------------------------------------------------", FontSize = 10 };

            printElement.Children.Add(line2);
        }

        public static void PrintPaymentSlip(List<MenuCart> lstTD, Transaction_Master tm, Boolean isCustomer, String EmpName)
        {
            StackPanel printPanel = new StackPanel() { Width = slipWidth, Margin = new Thickness(15, 0, 10, 10) };

            Printing.AddLogo(printPanel);



            TextBlock Order_No = new TextBlock() { Text = "Order: " + tm.Order_No, HorizontalAlignment = HorizontalAlignment.Left, Width = slipWidth * 0.30, FontSize = 10, FontWeight = FontWeights.Bold };
            TextBlock txtDateTime = new TextBlock() { Text = "Date : " + DateTime.Now.ToString(), HorizontalAlignment = HorizontalAlignment.Right, Width = slipWidth * 0.70, FontSize = 10 };

            StackPanel pnlOrderDate = new StackPanel() { Orientation = Orientation.Horizontal, Width = slipWidth };
            pnlOrderDate.Children.Add(Order_No);
            pnlOrderDate.Children.Add(txtDateTime);
            printPanel.Children.Add(pnlOrderDate);

            TextBlock horizontalLine = new TextBlock() { FontSize = 10, Text = "----------------------------------------------------------------" };
            printPanel.Children.Add(horizontalLine);

            foreach (MenuCart item in lstTD)
            {
                TextBlock MenuName = new TextBlock() { Text = item.Item_Name, Width = slipWidth * 0.40, FontSize = 8, FontWeight = FontWeights.Bold };
                TextBlock MenuQuanity = new TextBlock() { Text = item.Quantity.ToString(), Width = slipWidth * 0.20, FontSize = 8, FontWeight = FontWeights.Bold };
                TextBlock MenuPrice = new TextBlock() { Text = item.Item_Unit_Price.ToString("0.00"), Width = slipWidth * 0.20, FontSize = 8, FontWeight = FontWeights.Bold };
                TextBlock MenuTotal = new TextBlock() { Text = item.Item_Total.ToString("0.00"), Width = slipWidth * 0.20, FontSize = 8, FontWeight = FontWeights.Bold };

                StackPanel displayMenus = new StackPanel() { Orientation = Orientation.Horizontal };
                displayMenus.Children.Add(MenuName);
                displayMenus.Children.Add(MenuQuanity);
                displayMenus.Children.Add(MenuPrice);
                displayMenus.Children.Add(MenuTotal);

                printPanel.Children.Add(displayMenus);


            }
            TextBlock line = new TextBlock() { Text = "----------------------------------------------------------------", FontSize = 10 };
            printPanel.Children.Add(line);

            TextBlock totalStake = new TextBlock() { Text = "Total :", Width = slipWidth * 0.80, FontSize = 10, FontWeight = FontWeights.Bold };
            TextBlock totalStakeValue = new TextBlock() { Text = " " + Convert.ToDouble(tm.Transaction_Amount).ToString("0.00"), Width = slipWidth * 0.20, FontSize = 10, FontWeight = FontWeights.Bold };
            StackPanel totalStakeWithValue = new StackPanel() { Orientation = Orientation.Horizontal };
            totalStakeWithValue.Children.Add(totalStake);
            totalStakeWithValue.Children.Add(totalStakeValue);
            printPanel.Children.Add(totalStakeWithValue);

            TextBlock line1 = new TextBlock() { Text = "----------------------------------------------------------------", FontSize = 10 };

            printPanel.Children.Add(line1);

            if (EmpName != "")
            {
                TextBlock txtEmpName = new TextBlock() { Text = "Name : " + EmpName, HorizontalAlignment = HorizontalAlignment.Left, FontSize = 10 };
                printPanel.Children.Add(txtEmpName);
            }

            if (isCustomer)
            {
                if (tm.PaidBy_Card == true)
                {
                    Printing.AddCardDetails(printPanel, tm);
                }

            }
            else
            {
                TextBlock txtSign = new TextBlock() { Text = "Sign :________________ ", HorizontalAlignment = HorizontalAlignment.Left, FontSize = 10 };
                printPanel.Children.Add(txtSign);
            }






            TextBlock promoText = new TextBlock() { Margin = new Thickness(0, 10, 0, 0), Text = "---Thank You, Visit Again---", HorizontalAlignment = HorizontalAlignment.Center, FontSize = 10 };
            printPanel.Children.Add(promoText);

            Printing.PrintSlip(printPanel);
        }

        #region RevisedCode
        public static void PrintPaymentSlip(String OrderNum)
        {
            StackPanel printPanel = new StackPanel() { Width = slipWidth, Margin = new Thickness(15, 0, 10, 10) };

            Printing.AddLogo(printPanel);



            TextBlock Order_No = new TextBlock() { Text = "Order: " + OrderNum, HorizontalAlignment = HorizontalAlignment.Left, Width = slipWidth * 0.30, FontSize = 10, FontWeight = FontWeights.Bold };
            TextBlock txtDateTime = new TextBlock() { Text = "Date : " + DateTime.Now.ToString(), HorizontalAlignment = HorizontalAlignment.Right, Width = slipWidth * 0.70, FontSize = 10 };

            StackPanel pnlOrderDate = new StackPanel() { Orientation = Orientation.Horizontal, Width = slipWidth };
            pnlOrderDate.Children.Add(Order_No);
            pnlOrderDate.Children.Add(txtDateTime);
            printPanel.Children.Add(pnlOrderDate);

            TextBlock horizontalLine = new TextBlock() { FontSize = 10, Text = "----------------------------------------------------------------" };
            printPanel.Children.Add(horizontalLine);

            var menuCart = new BL_Menu().GetMenuCart();

            foreach (MenuCart item in menuCart)
            {
                TextBlock MenuName = new TextBlock() { Text = item.Item_Name, Width = slipWidth * 0.40, FontSize = 8, FontWeight = FontWeights.Bold };
                TextBlock MenuQuanity = new TextBlock() { Text = item.Quantity.ToString(), Width = slipWidth * 0.20, FontSize = 8, FontWeight = FontWeights.Bold };
                TextBlock MenuPrice = new TextBlock() { Text = item.Item_Unit_Price.ToString("0.00"), Width = slipWidth * 0.20, FontSize = 8, FontWeight = FontWeights.Bold };
                TextBlock MenuTotal = new TextBlock() { Text = item.Item_Total.ToString("0.00"), Width = slipWidth * 0.20, FontSize = 8, FontWeight = FontWeights.Bold };

                StackPanel displayMenus = new StackPanel() { Orientation = Orientation.Horizontal };
                displayMenus.Children.Add(MenuName);
                displayMenus.Children.Add(MenuQuanity);
                displayMenus.Children.Add(MenuPrice);
                displayMenus.Children.Add(MenuTotal);

                printPanel.Children.Add(displayMenus);


            }
            TextBlock line = new TextBlock() { Text = "----------------------------------------------------------------", FontSize = 10 };
            printPanel.Children.Add(line);

            TextBlock totalStake = new TextBlock() { Text = "Total :", Width = slipWidth * 0.80, FontSize = 10, FontWeight = FontWeights.Bold };
            TextBlock totalStakeValue = new TextBlock() { Text = " " + Convert.ToDouble(menuCart.Sum(a=>a.Item_Total)).ToString("0.00"), Width = slipWidth * 0.20, FontSize = 10, FontWeight = FontWeights.Bold };
            StackPanel totalStakeWithValue = new StackPanel() { Orientation = Orientation.Horizontal };
            totalStakeWithValue.Children.Add(totalStake);
            totalStakeWithValue.Children.Add(totalStakeValue);
            printPanel.Children.Add(totalStakeWithValue);

            TextBlock line1 = new TextBlock() { Text = "----------------------------------------------------------------", FontSize = 10 };

            printPanel.Children.Add(line1);
            TextBlock promoText = new TextBlock() { Margin = new Thickness(0, 10, 0, 0), Text = "---Thank You, Visit Again---", HorizontalAlignment = HorizontalAlignment.Center, FontSize = 10 };
            printPanel.Children.Add(promoText);

            Printing.PrintSlip(printPanel);
        }
        #endregion
    }
}
