using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;


namespace calendar_excel_desktop
{
    class Excel_reader
    {
        private Excel_reader()
        {
             Excel.Workbook MyBook = null;
             Excel.Worksheet MySheet = null;
             Excel.Application MyApp = null;

            string cal_path = Environment.SpecialFolder.MyDocuments.ToString();

            MyApp = new Excel.Application();
            MyApp.Visible = false;
            MyBook = MyApp.Workbooks.Open(cal_path + "2018 Calendar.xlsx");
            MySheet = MyBook.Sheets[1];
        }
        
    }
    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}
