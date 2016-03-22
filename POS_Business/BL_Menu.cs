using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using POS_DAL;
using MegabiteEntityLayer;
using System.Collections;


namespace POS_Business
{
    public class BL_Menu
    {
        DAL_Item_Master objitem = new DAL_Item_Master();
        public List<Item_Group_Master> get_Item_Group_List()
        {
            DAL_Item_Master obj = new DAL_Item_Master();
            return obj.get_Item_Group_Master();

        }

        public List<Item_Master> get_Menu_List(int ItemTypeID)
        {
            DAL_Item_Master obj = new DAL_Item_Master();
            return obj.get_Item_Master(ItemTypeID);
        }

        public IEnumerable get_Item_Details_By_Item_ID(int ItemID)
        {
            DAL_Item_Master obj = new DAL_Item_Master();
            return obj.get_Item_Master_By_ItemID(ItemID);
        }

        public IEnumerable get_Item_Details_By_Item_ID(int ItemID, IEnumerable oldObj)
        {
            List<MenuCart> lstMenuCart = new List<MenuCart>();
            MenuCart obj = new MenuCart();
            bool isExist = false;
            foreach (var item in oldObj)
            {
                obj = new MenuCart();
                MenuCart NewObj = new MenuCart();
                NewObj = (MenuCart)item;
                if (NewObj.Item_ID == ItemID)
                {
                    isExist = true;
                    obj.Item_ID = NewObj.Item_ID;
                    obj.Item_Name = NewObj.Item_Name;
                    obj.Item_Unit_Price = Convert.ToDouble(NewObj.Item_Unit_Price);
                    obj.Quantity = NewObj.Quantity + 1;

                }
                else
                {
                    obj = NewObj;

                }

                lstMenuCart.Add(obj);

            }

            if (isExist == false)
            {
                DAL_Item_Master objDal = new DAL_Item_Master();

                IEnumerable lst = objDal.get_Item_Master_By_ItemID(ItemID);
                foreach (var i in lst)
                {
                    obj = new MenuCart();
                    obj = (MenuCart)i;
                    lstMenuCart.Add(obj);
                }


            }


            return lstMenuCart;
        }

        public string get_total_amount(IEnumerable enumerable)
        {
            MenuCart obj = new MenuCart();
            string tot = "Total : 0.0";
            Double total = 0.0;
            foreach (var item in enumerable)
            {
                obj = new MenuCart();
                obj = (MenuCart)item;

                if (obj.Quantity == null)
                {
                    obj.Quantity = 1;
                }
                if (obj.Quantity == 0)
                {
                    obj.Quantity = 1;
                }

                total += (obj.Quantity * obj.Item_Unit_Price);
            }
            tot = total.ToString("0.00");
            return tot;
        }

        public IEnumerable Remove_Item_From_Cart(IEnumerable objAllMenus, IEnumerable objSelected)
        {
            List<MenuCart> lstMenuCart = new List<MenuCart>();
            MenuCart obj = new MenuCart();

            foreach (var item in objAllMenus)
            {
                obj = new MenuCart();

                obj = (MenuCart)item;
                bool isSelected = false;
                MenuCart objSel = new MenuCart();
                foreach (var i in objSelected)
                {
                    objSel = new MenuCart();
                    objSel = (MenuCart)i;

                    if (obj.Item_ID == objSel.Item_ID)
                    {
                        isSelected = true;

                    }
                }

                if (isSelected == false)
                {
                    lstMenuCart.Add(obj);
                }

            }



            return lstMenuCart;
        }
        public int InsertItems(Item_Master itemMaster)
        {
            return objitem.InsertMenuItems(itemMaster);
        }
        public List<Item_Group_Master> BindItemTypeID()
        {
            return objitem.get_Item_Group_Master();
        }

        public List<ItemMasterMenu> BindGvMenu()
        {
            return objitem.BindGvMenus();
        }

        public int UpdateItems(Item_Master itemMaster)
        {
            return objitem.UpdateMenuItems(itemMaster);
        }

        public int InsertMenuCategory(Item_Group_Master objcategory)
        {
            return objitem.InsertMenuCat(objcategory);
        }

        public List<Item_Group_Master> BindMenuCategory()
        {
            return objitem.BindMenuCat();
        }

        public bool check_isAlreadyAvailable(string MenuCategory, Int32 Item_Type_ID)
        {
            bool flag = false;
            Int32 cnt = objitem.check_isAlreadyAvailable(MenuCategory.ToLower(), Item_Type_ID);
            if (cnt > 0)
            {
                flag = true;
            }
            return flag;
        }

        public bool check_isDuplicateMenu(string Menu, long Item_ID)
        {
            bool flag = false;
            Int32 cnt = objitem.check_isDuplicateMenu(Menu, Item_ID);
            if (cnt > 0)
            {
                flag = true;
            }
            return flag;
        }

        public int UpdateMenuCategory(Item_Group_Master objcategory)
        {
            return objitem.UpdateMenuCat(objcategory);
        }

        #region RevisedCode
        public void AddMenuCartItem(long submenuid,int qty)
        {
            new DAL_Item_Master().AddMenuCart(submenuid, qty);
        }

        public ICollection<MenuCart> GetMenuCart()
        {
            return new DAL_Item_Master().GetMenuCart();
        }

        public void RemoveCartItem(long itemid)
        {
            new DAL_Item_Master().RemoveCartItem(itemid);
        }

        public void ClearMenuCart()
        {
           new DAL_Item_Master().ClearMenuCart();
        }

        public double getTotatlCartValue()
        {
           return new DAL_Item_Master()._getTotatlCartValue();
        }
        #endregion

    }
}
