using System;
using System.Data;

namespace UpdateMaterialToMessage
{
    //临时表
    public class Tempdt
    {
        /// <summary>
        /// 插入ytc_t_Cust100001表时使用
        /// </summary>
        /// <returns></returns>
        public DataTable Insertdtytc()
        {
            var dt = new DataTable();
            for (var i = 0; i < 10; i++)
            {
                var dc = new DataColumn();
                switch (i)
                {
                    case 0:
                        dc.ColumnName = "FID";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    case 1:
                        dc.ColumnName = "FMasterID";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    case 2:
                        dc.ColumnName = "F_YTC_TEXT";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 3:
                        dc.ColumnName = "F_YTC_TEXT1";
                        dc.DataType = Type.GetType("System.String");
                        break;
                    case 4:
                        dc.ColumnName = "F_YTC_ORGID";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    case 5:
                        dc.ColumnName = "F_YTC_ORGID1";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    case 6:
                        dc.ColumnName = "F_YTC_BASE";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    case 7:
                        dc.ColumnName = "F_YTC_BASE1";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    case 8:
                        dc.ColumnName = "F_YTC_GROUP";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    case 9:
                        dc.ColumnName = "FBILLSTATUS";
                        dc.DataType = Type.GetType("System.String"); 
                        break;
                }
                dt.Columns.Add(dc);
            }
            return dt;
        }

        /// <summary>
        /// 插入ytc_t_Cust100001_L时使用
        /// </summary>
        /// <returns></returns>
        public DataTable Insertdtytc_L()
        {
            var dt = new DataTable();
            for (var i = 0; i < 4; i++)
            {
                var dc = new DataColumn();
                switch (i)
                {
                    case 0:
                        dc.ColumnName = "FPKID";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    case 1:
                        dc.ColumnName = "FID";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    case 2:
                        dc.ColumnName = "FLocaleID";
                        dc.DataType = Type.GetType("System.Int32");
                        break;
                    case 3:
                        dc.ColumnName = "F_YTC_MULLANGTEXT";
                        dc.DataType = Type.GetType("System.String");
                        break;
                }
                dt.Columns.Add(dc);
            }
            return dt;
        }

    }
}
