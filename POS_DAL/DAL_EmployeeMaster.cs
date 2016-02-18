using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MegabiteEntityLayer;

namespace POS_DAL
{
    public class DAL_EmployeeMaster : Connection
    {

        public int SaveEmp(Employee_Master objemp)
        {
            objemp.Created_DateTime = DateTime.Now;
            dc.Employee_Master.AddObject(objemp);
            return dc.SaveChanges();
        }

        public List<Employee_Master> BindEmp()
        {
            List<Employee_Master> lstEmp = new List<Employee_Master>();
            Employee_Master objEmp = new Employee_Master();

            var emp = (from e in dc.Employee_Master
                       join c in dc.Company_Master on e.Company_ID equals c.Company_ID
                       join g in dc.Grade_Master on e.Grade_ID equals g.Grade_ID
                       //where e.Is_Active == true
                       orderby e.Employee_Name
                       select new
                       {

                           e.Card_ID,
                           e.Company_ID,
                           e.Created_By,
                           e.Created_DateTime,
                           e.Department,
                           e.Designation,
                           e.Email,
                           e.Employee_ID,
                           e.Employee_Name,
                           e.Grade_ID,
                           e.Is_Active,
                           e.Phone,
                           e.Remark,
                           e.RFID_NO,
                           e.Updated_DateTime,
                           c.Company_Name,
                           g.Grade_Name



                       }).ToList();

            foreach (var item in emp)
            {
                objEmp = new Employee_Master();

                objEmp.Card_ID = item.Card_ID;
                objEmp.Company_ID = item.Company_ID;
                objEmp.Created_By = item.Created_By;
                objEmp.Created_DateTime = item.Created_DateTime;
                objEmp.Department = item.Department;
                objEmp.Designation = item.Designation;
                objEmp.Email = item.Email;
                objEmp.Employee_ID = item.Employee_ID;
                objEmp.Employee_Name = item.Employee_Name;
                objEmp.Grade_ID = item.Grade_ID;
                objEmp.Is_Active = item.Is_Active;
                objEmp.Phone = item.Phone;
                objEmp.Remark = item.Remark;
                objEmp.RFID_NO = item.RFID_NO;
                objEmp.Updated_DateTime = item.Updated_DateTime;
                objEmp.Company_Name = item.Company_Name;
                objEmp.Grade_Name = item.Grade_Name;
                lstEmp.Add(objEmp);
            }

            return lstEmp;
        }

        public List<Grade_Master> get_GradeList()
        {
            return dc.Grade_Master.ToList<Grade_Master>();
        }
        public List<Employee_Master> getEmployeeList_By_CompanyID(Int32 Company_ID)
        {
            var emp = (from e in dc.Employee_Master
                       where e.Is_Active == true
                       && e.Company_ID == Company_ID

                       orderby e.Employee_Name
                       select e).ToList();

            return emp;
        }

        public List<Company_Master> BindALLCompanies()
        {
            var comp = (from c in dc.Company_Master
                        where c.Is_Active == true
                        orderby c.Company_Name
                        select c).ToList<Company_Master>();
            return comp;
        }

        public int Update_emp(Employee_Master objemp)
        {
            var Update_emp = (from e in dc.Employee_Master
                              where e.Employee_ID == objemp.Employee_ID
                              select e
                          ).FirstOrDefault();
            Update_emp.Employee_Name = objemp.Employee_Name;
            Update_emp.Department = objemp.Department;
            Update_emp.Designation = objemp.Designation;
            Update_emp.Email = objemp.Email;
            // Update_emp.Company_ID = objemp.Company_ID;
            Update_emp.Phone = objemp.Phone;
            Update_emp.Remark = objemp.Remark;
            Update_emp.Is_Active = objemp.Is_Active;
            Update_emp.Grade_ID = objemp.Grade_ID;
            Update_emp.Updated_DateTime = DateTime.Now;
            Update_emp.Updated_By = objemp.Updated_By;
            return dc.SaveChanges();
        }

        public int DeleteEmployee(Employee_Master objemp)
        {
            var Delete_emp = (from e in dc.Employee_Master
                              where e.Employee_ID == objemp.Employee_ID
                              select e
                          ).FirstOrDefault();
            Delete_emp.Is_Active = objemp.Is_Active;
            Delete_emp.Updated_DateTime = DateTime.Now;
            return dc.SaveChanges();
        }

        public int checkIsCompanyMapped(int Company_ID)
        {
            return dc.Employee_Master.Where(e => e.Company_ID == Company_ID).Count();
        }

        public string check_isCardAssigned(int emp_id)
        {
            return dc.Employee_Master.Where(e => e.Employee_ID == emp_id).Select(e => e.RFID_NO).SingleOrDefault();
        }

        public int check_isDuplicateEmpaloyee(string EmployeeName, int Employee_ID)
        {
            var qry = (from e in dc.Employee_Master
                       where e.Employee_Name.ToLower() == EmployeeName.Trim().ToLower()
                         && e.Employee_ID != Employee_ID
                       select e.Employee_ID).Count();

            return qry;
        }
    }
}
