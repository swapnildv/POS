using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using POS_DAL;
using MegabiteEntityLayer;
using System.Collections;

namespace POS_Business
{
    public class BL_EmployeeMaster
    {
        DAL_EmployeeMaster obj = new DAL_EmployeeMaster();
        public int Save_Employee(Employee_Master objemp)
        {

            return obj.SaveEmp(objemp);
        }

        public List<Employee_Master> Bind_Employee()
        {

            return obj.BindEmp();
        }

        public List<Grade_Master> get_GradeList()
        {
            return obj.get_GradeList();//.get_GradeList();

        }
        public IEnumerable Bindcompany()
        {
            return obj.BindALLCompanies();
        }

        public int Update_Employee(Employee_Master objemp)
        {
            return obj.Update_emp(objemp);
        }

        public int DeleteEmp(Employee_Master objemp)
        {
            return obj.DeleteEmployee(objemp);
        }

        public List<Employee_Master> getEmployeeList_By_CompanyID(Int32 Company_ID)
        {
            return obj.getEmployeeList_By_CompanyID(Company_ID);

        }



        public int checkIsCompanyMapped(int Company_ID)
        {
            return obj.checkIsCompanyMapped(Company_ID);
        }

        public bool check_isCardAssigned(int emp_id)
        {
            bool flag = false;
            string rfid = obj.check_isCardAssigned(emp_id);
            if (rfid == null || String.IsNullOrEmpty(rfid))
            {
                flag = true;
            }

            return flag;
        }

        public bool check_isDuplicateEmpaloyee(string EmployeeName, int Employee_ID)
        {
            bool flag = false;
            Int32 cnt = obj.check_isDuplicateEmpaloyee(EmployeeName, Employee_ID);
            if (cnt > 0)
            {
                flag = true;
            }
            return flag;
        }
    }
}
