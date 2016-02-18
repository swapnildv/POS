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
    public class BL_UserMaster
    {
        DAL_UserMaster obj = new DAL_UserMaster();

        public List<Role_Master> GetAllUserRole()
        {
            return obj.GetAllUserRoles();
        }

        public IEnumerable Get_UserList()
        {

            return obj.Get_UserList();
        }

        public List<User_Master> BindUsers()
        {

            return obj.BindUser();
        }


        public int CreateUser(User_Master objusers)
        {
            return obj.CreateUsers(objusers);

        }

        public string CreateSHAHash(string Password)
        {
            string Salt = "meg@b1te";
            System.Security.Cryptography.SHA512Managed HashTool = new System.Security.Cryptography.SHA512Managed();
            
            

            Byte[] PasswordAsByte = System.Text.Encoding.UTF8.GetBytes(string.Concat(Password, Salt));
            Byte[] EncryptedBytes = HashTool.ComputeHash(PasswordAsByte);
            HashTool.Clear();
            return Convert.ToBase64String(EncryptedBytes);

        }

        public int UpdateUser(User_Master objupdateuser)
        {

            return obj.UpdateUsers(objupdateuser);
        }


        public int deleteUser(User_Master objdel)
        {
            return obj.deleteUsers(objdel);
            //User_Master objdel = new User_Master();
            //objdel = (User_Master)p;
            //return obj.deleteUsers(objdel);
        }

        public User_Master CheckLoginUser(User_Master objuser)
        {
            return obj.CheckLoginUsers(objuser);
        }



        public String CheckUserInfo(string UserName, string Password)
        {
            return obj.checkUserEnterInfo(UserName, Password);
        }

        public int changePassword(User_Master objUserinfo)
        {
            return obj.changeUserPassword(objUserinfo);
        }

        public int CheckAvilUserName(string UserName)
        {
            return obj.CheckUserNameavil(UserName);
        }

        public string getUserName(int User_ID)
        {
            return obj.getUserName(User_ID);
        }

        public IEnumerable Bind_User()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get customer name by using mobile number.
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public Customer_Master getCustomerName(string Mobile)
        {
            return obj.getCustomerName(Mobile);
        }
    }

}
