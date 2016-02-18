using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using POS_DAL;
using MegabiteEntityLayer;
namespace POS_Business
{
    public class BL_CompanyMaster
    {
        DAL_CompanyMaster obj_dal = new DAL_CompanyMaster();
        public List<Company_Master> GetAllCompanyMaster()
        {
            return obj_dal.GetAllCompanyMaster();
        }
        public List<Company_Master> get_CompanyMaster()
        {
            //DAL_CompanyMaster obj = new DAL_CompanyMaster();
            return obj_dal.get_CompanyMaster();
        }

        public int Create_Company(MegabiteEntityLayer.Company_Master obj)
        {
          //  DAL_CompanyMaster obj_dal = new DAL_CompanyMaster();
            return obj_dal.Create_Company(obj);
        }

        public int Update_Company(MegabiteEntityLayer.Company_Master obj)
        {
            //DAL_CompanyMaster obj_dal = new DAL_CompanyMaster();
            return obj_dal.Update_Company(obj);
        }

        public bool check_isDuplicateCompany(string companyName, int Company_ID)
        {
            bool flag = false;
            Int32 cnt = obj_dal.check_isDuplicateCompany(companyName, Company_ID);
            if (cnt > 0)
            {
                flag = true;
            }
            return flag;
        }
    }
}
