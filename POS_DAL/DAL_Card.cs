using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MegabiteEntityLayer;
using System.Collections;

namespace POS_DAL
{
    public class DAL_Card : Connection
    {
        public TimeSpan ts = new TimeSpan(23, 59, 59);

        public int AddCard(MegabiteEntityLayer.Card_Master objCardMaster)
        {
            dc.Card_Master.AddObject(objCardMaster);
            return dc.SaveChanges();
        }

        public List<Card> get_Card_Master()
        {
            var qry = (from c in dc.Card_Master
                       join cs in dc.Card_Status_Master
                            on c.Card_Status_ID equals cs.Card_Status_ID
                       select new
                       {
                           c.Card_ID,
                           c.Card_Status_ID,
                           c.Card_Assigned_Date,
                           c.Created_DateTime,
                           c.Current_Balance,
                           c.Employee_ID,
                           c.RFID_No,
                           cs.Card_Status,
                           c.Updated_By,
                           c.Updated_DateTime,
                           c.Created_By
                       });
            Card objCard = new Card();
            List<Card> lst = new List<Card>();

            foreach (var c in qry)
            {
                objCard = new Card();


                objCard.Card_Status = c.Card_Status;
                objCard.Card_Assigned_Date = Convert.ToDateTime(c.Card_Assigned_Date);
                objCard.Card_ID = c.Card_ID;
                objCard.Card_Status_ID = Convert.ToInt32(c.Card_Status_ID);
                objCard.Created_DateTime = Convert.ToDateTime(c.Created_DateTime);
                objCard.Current_Balance = Convert.ToDouble(c.Current_Balance);
                objCard.Employee_ID = Convert.ToInt32(c.Employee_ID);
                objCard.RFID_No = c.RFID_No;
                objCard.Updated_By = Convert.ToInt32(c.Updated_By);
                objCard.Updated_DateTime = Convert.ToDateTime(c.Updated_DateTime);
                objCard.Created_By = Convert.ToInt32(c.Created_By);

                lst.Add(objCard);


            }
            return lst;

        }

        public Card get_Card_Details_By_ID(long Card_ID)
        {
            Card objCard = new Card();


            var qry = (from c in dc.Card_Master
                       join cs in dc.Card_Status_Master
                           on c.Card_Status_ID equals cs.Card_Status_ID
                       join e in dc.Employee_Master
                           on c.Employee_ID equals e.Employee_ID
                       where c.Card_ID == Card_ID
                       select new
                       {
                           cs.Card_Status,
                           e.Employee_Name,
                           c.Card_Assigned_Date,
                           c.Card_ID,
                           c.Card_Status_ID,
                           c.Created_DateTime,
                           c.Current_Balance,
                           c.Employee_ID,
                           c.RFID_No,
                           c.Updated_By,
                           c.Updated_DateTime,
                           c.Created_By
                       }).ToList();

            foreach (var item in qry)
            {
                objCard = new Card();

                objCard.Card_Status = item.Card_Status;
                objCard.Employee_Name = item.Employee_Name;
                objCard.Card_Assigned_Date = Convert.ToDateTime(item.Card_Assigned_Date);
                objCard.Card_ID = Convert.ToInt32(item.Card_ID);
                objCard.Card_Status_ID = Convert.ToInt32(item.Card_Status_ID);
                objCard.Created_DateTime = Convert.ToDateTime(item.Created_DateTime);
                objCard.Current_Balance = Convert.ToDouble(item.Current_Balance);
                objCard.Employee_ID = Convert.ToInt64(item.Employee_ID);
                objCard.RFID_No = item.RFID_No;
                objCard.Updated_DateTime = Convert.ToDateTime(item.Updated_DateTime);
                objCard.Created_By = Convert.ToInt32(item.Created_By);
            }

            return objCard;

        }


        public List<Card> get_EmployeeWithCard_Details()
        {
            Card objCard = new Card();
            List<Card> lstEmp = new List<Card>();

            var qry = from e in dc.Employee_Master
                      join cm in dc.Company_Master on new { Company_ID = (Int32)e.Company_ID } equals new { Company_ID = cm.Company_ID }
                      join c in dc.Card_Master on e.Employee_ID equals c.Employee_ID into c_join
                      from c in c_join.DefaultIfEmpty()
                      //join csm in dc.Card_Status_Master on new { Card_Status_ID = (Byte)c.Card_Status_ID } equals new { Card_Status_ID = csm.Card_Status_ID } into csm_join
                      //from csm in csm_join.DefaultIfEmpty()
                      where e.Is_Active == true
                      select new
                      {
                          e.Employee_ID,
                          e.Company_ID,
                          e.Email,
                          e.Phone,
                          e.Employee_Name,
                          cm.Company_Name,
                          e.Department,
                          RFID_No = c.RFID_No,
                          Card_ID = (Int64?)c.Card_ID,
                          Is_Active = (Boolean?)c.Is_Active,
                          Card_Assigned_Date = (DateTime?)c.Card_Assigned_Date,
                          Card_Status_ID = (Int32?)c.Card_Status_ID,
                          //Card_Status = "",
                          Created_DateTime = (DateTime?)c.Created_DateTime,
                          Current_Balance = (Double?)c.Current_Balance,
                          Created_By = (Int32?)c.Created_By,
                          Updated_DateTime = (DateTime?)c.Updated_DateTime

                      };

            foreach (var item in qry)
            {
                objCard = new Card();

                objCard.Employee_ID = Convert.ToInt64(item.Employee_ID);
                objCard.Employee_Name = item.Employee_Name;
                objCard.Department = item.Department;
                objCard.Company_ID = Convert.ToInt64(item.Company_ID);
                objCard.Company_Name = item.Company_Name;
                objCard.Email = item.Email;
                objCard.Phone = item.Phone;
                objCard.Card_ID = Convert.ToInt64(item.Card_ID);
                objCard.Is_Active = Convert.ToBoolean(item.Is_Active);
                objCard.RFID_No = item.RFID_No;
                objCard.Card_Assigned_Date = Convert.ToDateTime(item.Card_Assigned_Date);
                objCard.Card_Status_ID = Convert.ToInt32(item.Card_Status_ID);
                objCard.Created_DateTime = Convert.ToDateTime(item.Created_DateTime);
                objCard.Current_Balance = Convert.ToDouble(item.Current_Balance);
                objCard.Updated_DateTime = Convert.ToDateTime(item.Updated_DateTime);
                objCard.Created_By = Convert.ToInt32(item.Created_By);

                lstEmp.Add(objCard);
            }

            return lstEmp;

        }

        public Card get_EmployeeWithCard_Details(Int32 emp_id, string rfid)
        {
            Card objCard = new Card();
            List<Card> lstEmp = new List<Card>();

            var qry = (from e in dc.Employee_Master
                       join c in dc.Card_Master
                             on e.Employee_ID
                         equals (Int32)c.Employee_ID
                       // join csm in dc.Card_Status_Master on new { Card_Status_ID = (Byte)c.Card_Status_ID } equals new { Card_Status_ID = csm.Card_Status_ID }
                       join cm in dc.Company_Master on new { Company_ID = (Int32)e.Company_ID } equals new { Company_ID = cm.Company_ID }

                       where
                         c.RFID_No == rfid &&
                         c.Employee_ID == emp_id &&
                         c.Is_Active == true
                       select new
                        {
                            e.Employee_ID,
                            e.Company_ID,
                            e.Email,
                            e.Phone,
                            e.Employee_Name,
                            cm.Company_Name,
                            e.Department,
                            RFID_No = c.RFID_No,
                            c.Card_ID,
                            c.Is_Active,
                            c.Card_Assigned_Date,
                            c.Card_Status_ID,
                            Card_Status = "",//csm.Card_Status,
                            c.Created_DateTime,
                            c.Current_Balance,
                            c.Created_By,
                            c.Updated_DateTime

                        });

            foreach (var item in qry)
            {
                objCard = new Card();

                objCard.Employee_ID = Convert.ToInt64(item.Employee_ID);
                objCard.Employee_Name = item.Employee_Name;
                objCard.Department = item.Department;
                objCard.Company_ID = Convert.ToInt64(item.Company_ID);
                objCard.Company_Name = item.Company_Name;
                objCard.Card_ID = Convert.ToInt64(item.Card_ID);
                objCard.Is_Active = Convert.ToBoolean(item.Is_Active);
                objCard.RFID_No = item.RFID_No;
                objCard.Card_Assigned_Date = Convert.ToDateTime(item.Card_Assigned_Date);
                objCard.Card_Status_ID = Convert.ToInt32(item.Card_Status_ID);
                objCard.Email = item.Email;
                objCard.Phone = item.Phone;
                objCard.Created_DateTime = Convert.ToDateTime(item.Created_DateTime);
                objCard.Current_Balance = Convert.ToDouble(item.Current_Balance);
                objCard.Updated_DateTime = Convert.ToDateTime(item.Updated_DateTime);
                objCard.Created_By = Convert.ToInt32(item.Created_By);


            }

            return objCard;
        }

        public int Insert_Card_Master(Card_Master objCard)
        {
            dc.Card_Master.AddObject(objCard);
            return dc.SaveChanges();
            //if (dc.SaveChanges() > 0)
            //{
            //    Employee_Master objemp = new Employee_Master();
            //    objemp = dc.Employee_Master.Where(e => e.Employee_ID == objCard.Employee_ID).First();
            //    objemp.Card_ID = objCard.Card_ID;
            //    objemp.RFID_NO = objCard.RFID_No;
            //    return dc.SaveChanges();
            //}
            //else
            //{
            //    return 0;
            //}

        }

        public bool check_isAlreadyAssigned(string rfid)
        {
            bool flg = false;

            Int32 qryCm = dc.Card_Master.Where(c => c.RFID_No == rfid).Count();
            //var qryEm = dc.Employee_Master.Where(e => e.RFID_NO == rfid);
            if (qryCm > 0)
            {
                flg = true;

            }
            else
            {
                flg = false;
            }

            return flg;
        }

        public string Insert_Card_details(Card_Details objblock)
        {
            dc.Card_Details.AddObject(objblock);
            Card_Master obj = new Card_Master();
            obj = dc.Card_Master.Where(c => c.RFID_No == objblock.RFID_No && c.Card_ID == objblock.Card_ID).First();
            if (obj != null)
            {
                obj.Current_Balance = (obj.Current_Balance + objblock.Amount_Loaded);
            }
            dc.SaveChanges();

            Card_Ledger objCard_Ledger = new Card_Ledger();
            objCard_Ledger.Credit = objblock.Amount_Loaded;
            objCard_Ledger.Employee_ID = objblock.Employee_ID;
            objCard_Ledger.RFID_NO = objblock.RFID_No;
            objCard_Ledger.TR_Date = DateTime.Now;
            objCard_Ledger.TR_Remark = "Card Loaded ";
            objCard_Ledger.TR_Type = "CR";
            objCard_Ledger.User_ID = objblock.Loaded_By;
            objCard_Ledger.Ref = "Card Details ID " + objblock.Card_Detail_ID.ToString();
            objCard_Ledger.Closing_Balance = objblock.Closing_Balance;
            objCard_Ledger.Created_By = objblock.Created_By;
            objCard_Ledger.Updated_By = objblock.Updated_By;
            objCard_Ledger.Created_DateTime = objblock.Created_DateTime;
            objCard_Ledger.Updated_DateTime = objblock.Updated_DateTime;
            objCard_Ledger.Company_ID = objblock.Company_ID;
            dc.Card_Ledger.AddObject(objCard_Ledger);
            return dc.SaveChanges().ToString();
        }

        public Int32 Block_Card_from_DB(string rfid, int emp_id, Boolean status)
        {
            Card_Master objCardMaster = new Card_Master();
            objCardMaster = dc.Card_Master.Where(c => c.RFID_No == rfid && c.Employee_ID == emp_id && c.Is_Active == (!status)).First();
            objCardMaster.Is_Active = status;
            if (status)
            {
                objCardMaster.Card_Status_ID = 2;
            }
            else
            {
                objCardMaster.Card_Status_ID = 4;
            }
            return dc.SaveChanges();
        }

        public List<Card_Ledger> getEmployeeLedger(int EmpID, DateTime Fromdt, DateTime Todt)
        {
            Fromdt = Convert.ToDateTime(Fromdt.ToShortDateString());
            Todt = Convert.ToDateTime(Todt.ToShortDateString()).Add(ts);

            var qry = (from cl in dc.Card_Ledger
                       where cl.Employee_ID == EmpID
                       && cl.TR_Date >= Fromdt
                       && cl.TR_Date <= Todt
                       orderby cl.ID ascending
                       select cl
                           ).ToList<Card_Ledger>();

            return qry;
        }
    }
}


#region Commented


//from e in dc.Employee_Master
//join cm in dc.Company_Master on new { Company_ID = (Int32)e.Company_ID } equals new { Company_ID = cm.Company_ID }
//join c in dc.Card_Master on new { RFID_NO = e.RFID_NO } equals new { RFID_NO = c.RFID_No } into c_join
//from c in c_join.DefaultIfEmpty()
//join csm in dc.Card_Status_Master on new { Card_Status_ID = (Int32)c.Card_Status_ID } equals new { Card_Status_ID = csm.Card_Status_ID } into csm_join
//from csm in csm_join.DefaultIfEmpty()
//select new {
//  Is_Active = (Boolean?)c.Is_Active,
//  e.Employee_ID,
//  e.Employee_Name,
//  e.Company_ID,
//  e.Department,
//  e.Designation,
//  e.Phone,
//  e.Email,
//  Column1 = e.Is_Active,
//  e.Created_DateTime,
//  e.Updated_DateTime,
//  e.Remark,
//  e.RFID_NO,
//  e.Card_ID,
//  e.Created_By,
//  Column2 = cm.Company_ID,
//  cm.Company_Name,
//  cm.Company_Address,
//  cm.Company_Logo,
//  cm.City,
//  cm.State,
//  cm.Country,
//  Column3 = cm.Phone,
//  Column4 = cm.Email,
//  cm.Contact_Person,
//  cm.Contact_Person_Phone,
//  cm.Contact_Person_Email,
//  Column5 = cm.Is_Active,
//  Column6 = cm.Created_DateTime,
//  Column7 = cm.Updated_DateTime,
//  Column8 = cm.Remark,
//  Column9 = (Int64?)c.Card_ID,
//  RFID_No = c.RFID_No,
//  Column10 = (Int32?)c.Employee_ID,
//  Current_Balance = (Double?)c.Current_Balance,
//  Card_Assigned_Date = (DateTime?)c.Card_Assigned_Date,
//  Column11 = (Boolean?)c.Is_Active,
//  Card_Status_ID = (Int32?)c.Card_Status_ID,
//  Column12 = (DateTime?)c.Created_DateTime,
//  Column13 = (DateTime?)c.Updated_DateTime,
//  Column14 = (Int32?)c.Created_By,
//  Updated_By = (Int32?)c.Updated_By,
//  Column15 = (Int32?)csm.Card_Status_ID,
//  Card_Status = csm.Card_Status
//}

//1	New
//2	Assigned
//3	Lost
//4	Blocked
#endregion
