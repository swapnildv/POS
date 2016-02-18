using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MegabiteEntityLayer;
using POS_DAL;
using System.Collections;
namespace POS_Business
{
    public class BL_Card
    {
        public BlockHeader objHeader = new BlockHeader();
        private DAL_Card obj = new DAL_Card();

        public int AddCard(MegabiteEntityLayer.Card_Master objCardMaster)
        {

            return obj.AddCard(objCardMaster);
        }

        public IEnumerable get_Card_Master()
        {
            return obj.get_Card_Master();
        }

        public Card get_Card_Details_By_ID(long Card_ID)
        {
            return obj.get_Card_Details_By_ID(Card_ID);
        }

        public List<Card> get_EmployeeWithCard_Details()
        {
            DAL_Card obj = new DAL_Card();
            return obj.get_EmployeeWithCard_Details();

        }

        public Card get_EmployeeWithCard_Details(string empid, string rfid)
        {
            DAL_Card obj = new DAL_Card();
            Int32 emp_id = Convert.ToInt32(empid);
            return obj.get_EmployeeWithCard_Details(emp_id, rfid);
        }



        public int Insert_Card_Master(Card_Master objCard)
        {

            return obj.Insert_Card_Master(objCard);
        }

        public bool check_isAlreadyAssigned(string rfid)
        {

            return obj.check_isAlreadyAssigned(rfid);
        }

        public void Clear_Card()
        {
            RFID_HW objWrite = new RFID_HW();
            objWrite.Clear_Card();
        }

        public BlockData Read_Card()
        {
            RFID_HW objRFID = new RFID_HW();

            BlockData obj = new BlockData();
            obj.rfid = objRFID.get_RFID();
            obj.empid = objRFID.ReadDataBlock(objHeader.empid).Replace("\0", " ").Trim();
            obj.isactive = objRFID.ReadDataBlock(objHeader.isactive).Replace("\0", " ").Trim();
            obj.fname = objRFID.ReadDataBlock(objHeader.fname).Replace("\0", " ").Trim();
            obj.lname = objRFID.ReadDataBlock(objHeader.lname).Replace("\0", " ").Trim();
            obj.company = objRFID.ReadDataBlock(objHeader.company).Replace("\0", " ").Trim();
            obj.balance = objRFID.ReadDataBlock(objHeader.balance).Replace("\0", " ").Trim();

            return obj;
        }



        public string Load_Card_Amount(Card_Details objblock, double current_bal)
        {
            RFID_HW objRFID = new RFID_HW();
            string msg;
            Double amt = Convert.ToDouble(objblock.Amount_Loaded + current_bal);
            if (objblock.RFID_No == objRFID.get_RFID())
            {
                msg = objRFID.WriteDataBlock(objHeader.balance, amt.ToString("0.00"));
                if (msg == "1")
                {
                    msg = obj.Insert_Card_details(objblock);
                }

            }
            else
            {
                msg = "Failed ! Please Place Card Properly";

            }
            return msg;
        }

        public string AssignCard(BlockData obj)
        {
            RFID_HW objWrite = new RFID_HW();
            BlockHeader objHeader = new BlockHeader();
            string msg = "";
            msg = msg + objWrite.WriteDataBlock(objHeader.empid, obj.empid);
            msg = msg + objWrite.WriteDataBlock(objHeader.isactive, obj.isactive);
            msg = msg + objWrite.WriteDataBlock(objHeader.fname, obj.fname);
            msg = msg + objWrite.WriteDataBlock(objHeader.lname, obj.lname);
            msg = msg + objWrite.WriteDataBlock(objHeader.company, obj.company);
            msg = msg + objWrite.WriteDataBlock(objHeader.balance, obj.balance);
            return msg;
        }


        public Int32 Block_Card_from_DB(string rfid, string empid, Boolean status)
        {
            int emp_id = Convert.ToInt32(empid);
            return obj.Block_Card_from_DB(rfid, emp_id, status);
        }

        public List<Card_Ledger> getEmployeeLedger(int EmpID, DateTime Fromdt, DateTime Todt)
        {
            return obj.getEmployeeLedger(EmpID, Fromdt, Todt);
        }

    }
}
