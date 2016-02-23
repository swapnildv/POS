using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MegabiteEntityLayer;

namespace POS_DAL
{
    public class DAL_Item_Master : Connection
    {
        private static ObservableCollection<MenuCart> menuCart = new ObservableCollection<MenuCart>();

        public static ObservableCollection<MenuCart> MenuCart
        {
            get
            {
                return menuCart;
            }
        }
        public List<Item_Group_Master> get_Item_Group_Master()
        {
            return dc.Item_Group_Master.OrderBy(c => c.Item_Group_Name).ToList<Item_Group_Master>();

        }

        public List<Item_Master> get_Item_Master(int ItemTypeID)
        {
            List<Item_Master> lst = new List<Item_Master>();
            Item_Master obj = new Item_Master();
            var qry = (from m in dc.Item_Master
                       where m.Item_Type_ID == (ItemTypeID == 0 ? m.Item_Type_ID : ItemTypeID)
                       orderby m.Item_Name ascending
                       select new
                       {
                           m.Item_ID,
                           m.Item_Group_GUID,
                           m.Is_Active,
                           m.Item_Name,
                           m.Item_Unit_Price,
                           m.Item_Type_ID
                       }).ToList();

            foreach (var item in qry)
            {
                obj = new Item_Master();
                obj.Item_ID = item.Item_ID;
                obj.Item_Group_GUID = item.Item_Group_GUID;
                obj.Is_Active = item.Is_Active;
                obj.Item_Name = (item.Item_Name + " (Rs- " + String.Format("{0:n2}", item.Item_Unit_Price) + " ) ");
                obj.Item_Unit_Price = item.Item_Unit_Price;
                obj.Item_Type_ID = item.Item_Type_ID;

                lst.Add(obj);
            }



            return lst;
        }

        public System.Collections.IEnumerable get_Item_Master_By_ItemID(int ItemID)
        {
            var qry = (from m in dc.Item_Master
                       where m.Item_ID == ItemID
                       orderby m.Item_Name ascending
                       select new
                       {
                           m.Item_Name,
                           m.Item_ID,
                           m.Item_Unit_Price,


                       }).ToList();

            MenuCart obj = new MenuCart();
            List<MenuCart> lstMenuCart = new List<MenuCart>();
            foreach (var item in qry)
            {

                obj = new MenuCart();
                obj.Item_ID = item.Item_ID;
                obj.Item_Name = item.Item_Name;
                obj.Item_Unit_Price = Convert.ToDouble(item.Item_Unit_Price);
                obj.Quantity = 1;

                lstMenuCart.Add(obj);
            }

            return lstMenuCart;
        }

        public int InsertMenuItems(Item_Master itemMaster)
        {
            dc.Item_Master.AddObject(itemMaster);
            return dc.SaveChanges();
        }

        public List<ItemMasterMenu> BindGvMenus()
        {
            List<ItemMasterMenu> lst = new List<ItemMasterMenu>();
            ItemMasterMenu obj = new ItemMasterMenu();
            var qry = (from i in dc.Item_Master
                       join c in dc.Item_Group_Master on i.Item_Type_ID equals c.Item_Group_ID
                       orderby i.Item_Name
                       select new
                       {
                           i.Item_ID,
                           i.Is_Active,
                           i.Item_Name,
                           i.Item_Type_ID,
                           i.Item_Unit_Price,
                           i.Updated_By,
                           i.Updated_DateTime,
                           i.Created_By,
                           i.Created_DateTime,
                           c.Item_Group_Name


                       }).ToList();
            foreach (var item in qry)
            {
                obj = new ItemMasterMenu();
                if (item.Is_Active == true)
                {
                    obj.IS_Active = "Available";
                }
                else
                {
                    obj.IS_Active = "Not available";
                }
                obj.Item_Name = item.Item_Name;
                obj.Item_Unit_Price = Convert.ToDouble(item.Item_Unit_Price);
                obj.Item_ID = Convert.ToInt32(item.Item_ID);
                obj.Item_Type_Name = item.Item_Group_Name;
                obj.Item_Type_Id = Convert.ToInt32(item.Item_Type_ID);
                lst.Add(obj);
            }
            return lst;
        }

        public int UpdateMenuItems(Item_Master itemMaster)
        {
            var MenuItems = (from i in dc.Item_Master
                             where i.Item_ID == itemMaster.Item_ID
                             select i).FirstOrDefault();
            MenuItems.Item_ID = itemMaster.Item_ID;
            MenuItems.Item_Name = itemMaster.Item_Name;
            MenuItems.Item_Type_ID = itemMaster.Item_Type_ID;
            MenuItems.Item_Unit_Price = itemMaster.Item_Unit_Price;
            MenuItems.Is_Active = itemMaster.Is_Active;
            MenuItems.Updated_DateTime = DateTime.Now;
           // MenuItems.Company_ID = itemMaster.Company_ID;
            return dc.SaveChanges();
        }

        public int InsertMenuCat(Item_Group_Master objcategory)
        {

            objcategory.Item_Group_GUID = SequentialGuidGenerator.NewSequentialGuid(SequentialGuidType.SequentialAtEnd);
            dc.Item_Group_Master.AddObject(objcategory);
            return dc.SaveChanges();
        }

        public List<Item_Group_Master> BindMenuCat()
        {
            var qry = (from i in dc.Item_Group_Master
                       orderby i.Item_Group_Name
                       select i).ToList<Item_Group_Master>();
            return qry;
        }

        public Int32 check_isAlreadyAvailable(string MenuCategory, Int32 Item_Type_ID)
        {
            var qry = (from i in dc.Item_Group_Master
                       where i.Item_Group_Name.ToLower() == MenuCategory
                       && i.Item_Group_ID != Item_Type_ID
                       select i.Item_Group_Name.ToLower()).Count();

            return qry;

        }

        public Int32 check_isDuplicateMenu(string Menu, long Item_ID)
        {
            var qry = (from i in dc.Item_Master
                       where i.Item_Name.ToLower() == Menu
                         && i.Item_ID != Item_ID
                       select i.Item_Name.ToLower()).Count();

            return qry;

        }
        public int UpdateMenuCat(Item_Group_Master objcategory)
        {
            var MenuCategory = dc.Item_Group_Master.Where(c => c.Item_Group_ID == objcategory.Item_Group_ID).First();
            MenuCategory.Item_Group_Name = objcategory.Item_Group_Name;
            MenuCategory.Is_Active = objcategory.Is_Active;
            MenuCategory.Company_ID = objcategory.Company_ID;
            MenuCategory.Updated_DateTime = objcategory.Updated_DateTime;
            MenuCategory.Updated_By = objcategory.Updated_By;
            return dc.SaveChanges();
        }

        #region Revised Code
        public ICollection<MenuCart> GetMenuCart()
        {
            return MenuCart;
        }

        public void ClearMenuCart()
        {
            MenuCart.Clear();
        }

        public void AddMenuCart(long submenuId, int qty)
        {
            var item = dc.Item_Master.Where(a=>a.Item_ID == submenuId).SingleOrDefault();
            MenuCart obj = new MegabiteEntityLayer.MenuCart()
            {
                Item_ID = item.Item_ID,
                Item_Unit_Price = item.Item_Unit_Price.Value,
                Item_Name = item.Item_Name,
                Quantity = qty
            };
            menuCart.Add(obj);
        }

        public double _getTotatlCartValue()
        {
            return menuCart.ToList().Select(a => a.Item_Total).Sum();
        }


        #endregion
    }



}
