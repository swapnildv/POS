using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MegabiteEntityLayer;
using System.Collections;

namespace POS_DAL
{
    public class DAL_UserMaster : Connection
    {
        public List<Role_Master> GetAllUserRoles()
        {
            var qry = (from u in dc.Role_Master
                       select u
                         ).ToList<Role_Master>();
            return qry;
        }

        public List<User_Master> BindUser()
        {
            List<User_Master> lstUsers = new List<User_Master>();
            User_Master objUM = new User_Master();
            var qry = (from u in dc.User_Master
                       join r in dc.Role_Master on u.Role_ID equals r.Role_ID
                       // where u.Is_Active == true || u.Is_Active == null
                       orderby u.User_Name
                       select new
                       {
                           u.Role_ID,
                           r.Role_Name,
                           u.Real_Name,
                           u.Password,
                           u.Mobile,
                           u.Is_Active,
                           u.Company_ID,
                           u.Created_By,
                           u.Created_DateTime,
                           u.Email,
                           u.User_ID,
                           u.User_Name,
                           u.Updated_DateTime,
                           u.IsDiscount
                       }).ToList();

            foreach (var item in qry)
            {
                objUM = new User_Master();
                objUM.Real_Name = item.Real_Name;
                objUM.Role_ID = item.Role_ID;
                objUM.Role_Name = item.Role_Name;
                objUM.Password = item.Password;
                objUM.Mobile = item.Mobile;
                objUM.Is_Active = item.Is_Active;
                objUM.Company_ID = item.Company_ID;
                objUM.Created_By = item.Created_By;
                objUM.Created_DateTime = item.Created_DateTime;
                objUM.Email = item.Email;
                objUM.User_ID = item.User_ID;
                objUM.User_Name = item.User_Name;
                objUM.IsDiscount = item.IsDiscount;
                objUM.Updated_DateTime = item.Updated_DateTime;

                lstUsers.Add(objUM);
            }
            return lstUsers;
        }

        public IEnumerable Get_UserList()
        {

            var qry = (from u in dc.User_Master
                       where u.Is_Active == true || u.Is_Active == null
                       select new
                       {
                           u.User_ID,
                           u.User_Name

                       }).ToList();

            return qry;
        }

        public int CreateUsers(User_Master obj)
        {
            dc.User_Master.AddObject(obj);
            return dc.SaveChanges();
        }

        public int UpdateUsers(User_Master objupdateuser)
        {

            var UpdateUsers = (from u in dc.User_Master
                               where u.User_ID == objupdateuser.User_ID
                               select u
                          ).FirstOrDefault();

            UpdateUsers.User_Name = objupdateuser.User_Name;
            UpdateUsers.Real_Name = objupdateuser.Real_Name;
            UpdateUsers.Role_ID = objupdateuser.Role_ID;
            UpdateUsers.IsDiscount = objupdateuser.IsDiscount;
            //UpdateUsers.Company_ID = objupdateuser.Company_ID;
            UpdateUsers.Updated_By = objupdateuser.Updated_By;
            UpdateUsers.Updated_DateTime = objupdateuser.Updated_DateTime;
            return dc.SaveChanges();
        }
        public int deleteUsers(User_Master objdelete)
        {
            var qry = (from u in dc.User_Master
                       where u.User_ID == objdelete.User_ID
                       select u).SingleOrDefault();
            qry.Is_Active = false;
            qry.User_ID = objdelete.User_ID;
            //dc.User_Master.Add(qry);
            return dc.SaveChanges();
            //dc.User_Master.Remove(objdelete);
            //return dc.SaveChanges();

        }

        public User_Master CheckLoginUsers(User_Master objuser)
        {
            User_Master qry = (from u in dc.User_Master
                               where u.User_Name == objuser.User_Name && u.Password == objuser.Password
                               select u).FirstOrDefault();
            if (qry != null)
            {
                return qry;
            }
            else
            {
                return null;
            }
        }

        public String checkUserEnterInfo(string UserName, string Password)
        {
            var qry = (from u in dc.User_Master
                       where u.User_Name == UserName && u.Password == Password
                       select u
                          ).FirstOrDefault();
            if (qry != null)
            {
                return "Valid";
            }
            else
            {
                return "Invalid";
            }
        }

        public int changeUserPassword(User_Master objUserinfo)
        {
            var passwordchange = (from u in dc.User_Master
                                  where (u.User_Name == objUserinfo.User_Name || u.User_ID == objUserinfo.User_ID)
                                  select u).SingleOrDefault();
            passwordchange.Password = objUserinfo.Password;
            passwordchange.Updated_DateTime = DateTime.Now;
            passwordchange.Created_By = objUserinfo.Created_By;
            return dc.SaveChanges();
        }

        public int CheckUserNameavil(string UserName)
        {
            var checkavilUserName = (from u in dc.User_Master
                                     where u.User_Name == UserName
                                     select u
                ).Count();
            return checkavilUserName;
        }

        public string getUserName(int User_ID)
        {
            return dc.User_Master.Where(u => u.User_ID == User_ID).Select(u => u.User_Name).FirstOrDefault().ToString();
            
        }

        public Customer_Master getCustomerName(string mobile)
        {
            return dc.Customer_Master.Where(a => a.cust_MobileNo.Trim() == mobile.Trim()).SingleOrDefault();
        }
    }
}
