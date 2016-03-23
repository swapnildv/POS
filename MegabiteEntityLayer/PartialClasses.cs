using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegabiteEntityLayer
{
    public class PartialClasses
    {

    }

    public partial class MenuCart : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String PropertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
        private Int32 _Quantity;
        private double _Item_Total;

        public Int64 Item_ID { get; set; }
        public String Item_Name { get; set; }//1
        public double Item_Unit_Price { get; set; }//2
        public Int32 Quantity
        {
            get { return _Quantity; }
            set
            {
                _Quantity = value;
                Item_Total = Item_Unit_Price * _Quantity;
                NotifyPropertyChanged("Quantity");
            }
        }
        public double Item_Total
        {
            get { return _Item_Total; }
            set
            {
                _Item_Total = value;
                NotifyPropertyChanged("Item_Total");
            }

        }
    }



    public partial class Card
    {
        public Int64 Card_ID { get; set; }
        public String RFID_No { get; set; }
        public double Current_Balance { get; set; }
        public DateTime Card_Assigned_Date { get; set; }
        public Boolean Is_Active { get; set; }
        public Int64 Card_Status_ID { get; set; }
        public String Card_Status { get; set; }
        public DateTime Created_DateTime { get; set; }
        public DateTime Updated_DateTime { get; set; }
        public Int32 Created_By { get; set; }
        public Int32 Updated_By { get; set; }
        public Int64 Employee_ID { get; set; }
        public String Employee_Name { get; set; }
        public Int64 Company_ID { get; set; }
        public String Department { get; set; }
        public String Company_Name { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }




        //    [Card_ID] [bigint] IDENTITY(1,1) NOT NULL,
        //[RFID_No] [varchar](50) NULL,
        //[Employee_ID] [int] NULL,
        //[Current_Balance] [float] NULL,
        //[Card_Assigned_Date] [date] NULL,
        //[Is_Active] [bit] NULL,
        //[Card_Status_ID] [tinyint] NULL,
        //[Created_DateTime] [datetime] NULL,
        //[Updated_DateTime] [datetime] NULL,
        //  [Card_Status_ID]
        //,[Card_Status]
        //  ,[Employee_Name]
        //,[Company_ID]
        //,[Department]
        //,[Designation]
        //,[Phone]
        //,[Email]
        //,[Is_Active]
        //,[Created_DateTime]
        //,[Updated_DateTime]
        //,[Remark]
        //,[Card_ID]
        //,[Created_By]
    }
    // Used For Transaction Report
    public partial class Report
    {

        public DateTime Transaction_Date { get; set; }

        public String Order_No { get; set; }

        public double Transaction_Amount { get; set; }

        public double Discount_Perc { get; set; }

        public double Discount_Value { get; set; }
        


            

    }

    // Used For Load Report
    public partial class Load_Report
    {
        public DateTime Loaded_Datetime { get; set; }
        public Int64 Card_Detail_ID { get; set; }
        public String RFID_No { get; set; }
        public Double Amount_Loaded { get; set; }
        public Double Closing_Balance { get; set; }
        public Int32 Loaded_By { get; set; }
        public String Employee_Name { get; set; }

    }


    public class ItemMasterMenu
    {
        public string IS_Active { get; set; }
        public string Item_Name { get; set; }
        public Double Item_Unit_Price { get; set; }
        public Int32 Item_Type_Id { get; set; }
        public string Item_Type_Name { get; set; }
        public Int32 Item_ID { get; set; }

    }

    public partial class BlockHeader
    {
        public String rfid { get; set; }
        public String empid { get; set; }
        public String isactive { get; set; }
        public String fname { get; set; }
        public String lname { get; set; }
        public String company { get; set; }
        public String balance { get; set; }

        public BlockHeader()
        {
            rfid = "0";
            empid = "1";
            isactive = "2";
            fname = "4";
            lname = "5";
            company = "6";
            balance = "8";
        }
    }


    public class BlockData
    {
        public String rfid { get; set; }
        public String empid { get; set; }
        public String isactive { get; set; }
        public String fname { get; set; }
        public String lname { get; set; }
        public String company { get; set; }
        public String balance { get; set; }

    }

    public partial class Employee_Master
    {
        public string Company_Name { get; set; }
        public string Grade_Name { get; set; }
    }
    public partial class User_Master
    {
        public string Role_Name { get; set; }
    }


}
