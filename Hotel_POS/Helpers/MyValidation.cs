using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections;
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;



namespace Hotel_POS
{
    public class MyValidation
    {

        #region Validation

        public enum ValidationStates { OK, ERROR, WARNING };

        // Tables for regex and messages
        public Hashtable previewRegex = new Hashtable();
        public Hashtable completionRegex = new Hashtable();
        public Hashtable errorMessage = new Hashtable();
        public Hashtable validationState = new Hashtable();

        public const string fieldRequired = "This field is required";

        public List<object> lstChildren;

        public List<object> GetChildren(Visual p_vParent, int p_nLevel)
        {
            if (p_vParent == null)
            {
                throw new ArgumentNullException("Element {0} is null!", p_vParent.ToString());
            }

            this.lstChildren = new List<object>();

            this.GetChildControls(p_vParent, p_nLevel);

            return this.lstChildren;

        }

        public void GetChildControls(Visual p_vParent, int p_nLevel)
        {
            int nChildCount = VisualTreeHelper.GetChildrenCount(p_vParent);

            for (int i = 0; i <= nChildCount - 1; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(p_vParent, i);

                lstChildren.Add((object)v);

                if (VisualTreeHelper.GetChildrenCount(v) > 0)
                {
                    GetChildControls(v, p_nLevel + 1);
                }
            }
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var mail = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsValidPhone(string phone)
        {
            try
            {
                Regex regex = new Regex(@"^\+?[0-9-]+$");

                return regex.IsMatch(phone);
            }
            catch
            {
                return false;
            }
        }

        public bool IsValidUserName(string UserName)
        {
            try
            {
                Regex regex = new Regex(@"^[a-zA-Z][a-zA-Z0-9-_]{3,32}$");

                return regex.IsMatch(UserName);
            }
            catch
            {
                return false;
            }
        }

        public bool IsValidAmount(string Amount)
        {
            try
            {
                Regex regex = new Regex(@"^([0-9]+)([\.]?([0-9]+))?$");

                return regex.IsMatch(Amount);
            }
            catch
            {
                return false;
            }
        }

        public bool IsValidNonZeroQty(string Amount)
        {
            try
            {
                Regex regex = new Regex(@"^([1-9]+)([0-9]+)$");

                return regex.IsMatch(Amount);
            }
            catch
            {
                return false;
            }
        }


        public bool IsValidAlphaNumeric(string strng)
        {
            try
            {
                Regex regex = new Regex(@"^[a-zA-Z0-9._][a-zA-Z0-9- ._]{2,40}$");

                return regex.IsMatch(strng);
            }
            catch
            {
                return false;
            }
        }


        #endregion
    }

    /// <summary>
    /// Class for generator of Excel file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class ExportToExcel<T, U>
        where T : class
        where U : List<T>
    {
        public List<T> dataToPrint;
        public object[] header;
        // Excel object references.
        private Excel.Application _excelApp = null;
        private Excel.Workbooks _books = null;
        private Excel._Workbook _book = null;
        private Excel.Sheets _sheets = null;
        private Excel._Worksheet _sheet = null;
        private Excel.Range _range = null;
        private Excel.Font _font = null;
        // Optional argument variable
        private object _optionalValue = Missing.Value;

        /// <summary>
        /// Generate report and sub functions
        /// </summary>
        public void GenerateReport()
        {
            try
            {
                if (dataToPrint != null)
                {
                    if (dataToPrint.Count != 0)
                    {
                        Mouse.SetCursor(Cursors.Wait);
                        CreateExcelRef();
                        FillSheet();
                        OpenReport();
                        Mouse.SetCursor(Cursors.Arrow);
                    }
                }
            }
            catch (Exception) {

                throw;
            }
            finally
            {
                ReleaseObject(_sheet);
                ReleaseObject(_sheets);
                ReleaseObject(_book);
                ReleaseObject(_books);
                ReleaseObject(_excelApp);
            }
        }
        /// <summary>
        /// Make MS Excel application visible
        /// </summary>
        private void OpenReport()
        {
            _excelApp.Visible = true;
        }
        /// <summary>
        /// Populate the Excel sheet
        /// </summary>
        private void FillSheet()
        {
            header = CreateHeader();
            WriteData(header);
        }
        /// <summary>
        /// Write data into the Excel sheet
        /// </summary>
        /// <param name="header"></param>
        private void WriteData(object[] header)
        {
            object[,] objData = new object[dataToPrint.Count, header.Length];

            for (int j = 0; j < dataToPrint.Count; j++)
            {
                var item = dataToPrint[j];
                for (int i = 0; i < header.Length; i++)
                {
                    var y = typeof(T).InvokeMember(header[i].ToString(), BindingFlags.GetProperty, null, item, null);
                    objData[j, i] = (y == null) ? "" : y.ToString();
                }
            }
            AddExcelRows("A2", dataToPrint.Count, header.Length, objData);
            AutoFitColumns("A1", dataToPrint.Count + 1, header.Length);
        }
        /// <summary>
        /// Method to make columns auto fit according to data
        /// </summary>
        /// <param name="startRange"></param>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        private void AutoFitColumns(string startRange, int rowCount, int colCount)
        {
            _range = _sheet.get_Range(startRange, _optionalValue);
            _range = _range.get_Resize(rowCount, colCount);
            _range.Columns.AutoFit();
        }
        /// <summary>
        /// Create header from the properties
        /// </summary>
        /// <returns></returns>
        private object[] CreateHeader()
        {

            if (header == null)
            {
                PropertyInfo[] headerInfo = typeof(T).GetProperties();

                header = new object[headerInfo.Count()];
                //List<object> objHeaders = new List<object>();
                for (int n = 0; n < headerInfo.Length; n++)
                {
                    header[n] = headerInfo[n].Name;
                }

               // var headerToAdd = objHeaders.ToArray();
            }
            AddExcelRows("A1", 1, header.Count(), header);
            SetHeaderStyle();

            return header;
        }
        /// <summary>
        /// Set Header style as bold
        /// </summary>
        private void SetHeaderStyle()
        {
            _font = _range.Font;
            _font.Bold = true;
        }
        /// <summary>
        /// Method to add an excel rows
        /// </summary>
        /// <param name="startRange"></param>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        /// <param name="values"></param>
        private void AddExcelRows(string startRange, int rowCount, int colCount, object values)
        {
            _range = _sheet.get_Range(startRange, _optionalValue);
            _range = _range.get_Resize(rowCount, colCount);
            _range.set_Value(_optionalValue, values);
        }
        /// <summary>
        /// Create Excel applicaiton parameters instances
        /// </summary>
        private void CreateExcelRef()
        {
            _excelApp = new Excel.Application();
            _books = (Excel.Workbooks)_excelApp.Workbooks;
            _book = (Excel._Workbook)(_books.Add(_optionalValue));
            _sheets = (Excel.Sheets)_book.Worksheets;
            _sheet = (Excel._Worksheet)(_sheets.get_Item(1));
        }
        /// <summary>
        /// Release unused COM objects
        /// </summary>
        /// <param name="obj"></param>
        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }

}
