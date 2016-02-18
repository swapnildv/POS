using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using MegabiteEntityLayer;

namespace POS_DAL
{
    public class DAL_CompanyMaster : Connection
    {
        public List<Company_Master> GetAllCompanyMaster()
        {
            var qry = (from u in dc.Company_Master
                       where u.Is_Active == true
                       select u
                         ).ToList<Company_Master>();
            return qry;
        }
        public List<Company_Master> get_CompanyMaster()
        {
            return dc.Company_Master.OrderBy(c => c.Company_Name).ToList();
        }

        public int Create_Company(Company_Master obj)
        {
            dc.Company_Master.AddObject(obj);
            return dc.SaveChanges();
        }

        public int Update_Company(Company_Master obj)
        {

            var UpdateObj = (from c in dc.Company_Master
                             where c.Company_ID == obj.Company_ID
                             select c).FirstOrDefault();
            UpdateObj.City = obj.City;
            UpdateObj.Company_Address = obj.Company_Address;
            // UpdateObj.Company_ID = obj.Company_ID;
            UpdateObj.Company_Name = obj.Company_Name;
            UpdateObj.Contact_Person = obj.Contact_Person;
            UpdateObj.Contact_Person_Email = obj.Contact_Person_Email;
            UpdateObj.Contact_Person_Phone = obj.Contact_Person_Phone;
            UpdateObj.Country = obj.Country;
            UpdateObj.Email = obj.Email;
            UpdateObj.Is_Active = obj.Is_Active;
            UpdateObj.Phone = obj.Phone;
            UpdateObj.Remark = obj.Remark;
            UpdateObj.State = obj.State;
            UpdateObj.Updated_DateTime = obj.Updated_DateTime;
            UpdateObj.Updated_By = obj.Updated_By;


            return dc.SaveChanges();


        }

        public int check_isDuplicateCompany(string companyName, int Company_ID)
        {
            var qry = (from c in dc.Company_Master
                       where c.Company_Name.ToLower() == companyName.Trim().ToLower()
                         && c.Company_ID != Company_ID
                       select c.Company_ID).Count();

            return qry;
        }
    }
}
