using System;
using System.Data;
using System.Data.SqlClient;

namespace UpdateMaterialToMessage
{
    //运算
    public class Generate
    {
        ConnDb conDb=new ConnDb();
        SeachDt searDt=new SeachDt();
        SqlList sqlList=new SqlList();
        Tempdt tempdt=new Tempdt();

        /// <summary>
        /// 获取物料ID并进行相关运算
        /// 中心:1)若物料ID存在ytc_t_Cust100001只需更新 2)若不存在,先插入新记录,后更新
        /// </summary>
        /// <param name="materiallist"></param>
        /// <returns></returns>
        public string GenerateYtcRecord(string materiallist)
        {
            var result = "Finish";
            var FID = 0;
            var FPKID = 0;
            var codeid = 0;
            var fmaterialidlist = string.Empty;

            try
            {
                //获取ytc_t_Cust100001临时表(插入至数据库时使用)
                var ytctempdt = tempdt.Insertdtytc();

                //获取ytc_t_Cust100001_L临时表(插入至数据库时使用)
                var ytcltempdt = tempdt.Insertdtytc_L();

                //获取最大‘供应商物料编码’值
                codeid = searDt.SearchMaxCode();
                //根据获取的materialid查询符合条件的相关值
                var matdt = searDt.SearchMaterial(materiallist).Copy();

                if (matdt.Rows.Count == 0)
                {
                    result = "所选择的物料对应的'物料分组(辅助)'不为'包装材料' 或 '外购辅料',故不能继续,请重新选择再执行更新操作";
                }
                else
                {
                    //循环执行
                    foreach (DataRow rows in matdt.Rows)
                    {
                        //循环读取matdt查询ytc_t_Cust100001是否存在--若不存在,先插入;注:无论有没有检测到有记录,跳出循环后都要将fmaterialid进行更新操作
                        var existdt = searDt.Searchytc_t_Cust100001(Convert.ToInt32(rows[0]));
                        //若不存在,收集
                        if (existdt.Rows.Count == 0)
                        {
                            //获取主键值
                            FID = searDt.GetKeyId(0);
                            FPKID = searDt.GetKeyId(1);

                            var code = "WM" + codeid;
                            //将相关值插入分别插入至ytctempdt 及 ytcltempdt临时表内
                            ytctempdt.Merge(InsertytcIntoTempdt(FID, Convert.ToInt32(rows[0]), code, ytctempdt));
                            ytcltempdt.Merge(InsertytcLIntoTempdt(FPKID,FID,ytcltempdt));
                            //每次插入完临时表后都要对codeid自增;为下一个循环行准备
                            codeid += codeid + 1;
                        }
                        //循环获取fmaterialid值(更新使用)
                        if (string.IsNullOrEmpty(fmaterialidlist))
                        {
                            fmaterialidlist = "'" + Convert.ToInt32(rows[0]) + "'";
                        }
                        else
                        {
                            fmaterialidlist += ","+ "'" + Convert.ToInt32(rows[0]) + "'";
                        }
                    }
                    //执行插入操作
                    if(ytctempdt.Rows.Count>0)
                        ImportDtToDb("ytc_t_Cust100001", ytctempdt);
                    if(ytcltempdt.Rows.Count>0)
                        ImportDtToDb("ytc_t_Cust100001_L", ytcltempdt);
                    //执行更新操作
                    UpdateytcRecord(fmaterialidlist);
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 插入值至ytc_t_Cust100001临时表
        /// </summary>
        /// <param name="code"></param>
        /// <param name="dt"></param>
        /// <param name="fid"></param>
        /// <param name="materialid"></param>
        /// <returns></returns>
        private DataTable InsertytcIntoTempdt(int fid,int materialid,string code,DataTable dt)
        {
            var newrow = dt.NewRow();
            newrow[0] = fid;        //FID
            newrow[1] = fid;        //FMasterID
            newrow[2] = code;       //F_YTC_TEXT
            newrow[3] = ' ';        //F_YTC_TEXT1
            newrow[4] = 1;          //F_YTC_ORGID
            newrow[5] = 0;          //F_YTC_ORGID1
            newrow[6] = materialid; //F_YTC_BASE
            newrow[7] = 0;          //F_YTC_BASE1
            newrow[8] = 0;          //F_YTC_GROUP
            newrow[9] = 'A';        //FBILLSTATUS
            dt.Rows.Add(newrow);

            return dt;
        }

        /// <summary>
        /// 插入值至ytc_t_Cust100001_L临时表
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="dt"></param>
        /// <param name="fpkid"></param>
        /// <returns></returns>
        private DataTable InsertytcLIntoTempdt(int fpkid,int fid,DataTable dt)
        {
            var newrow = dt.NewRow();
            newrow[0] = fpkid;     //FPKID
            newrow[1] = fid;       //FID
            newrow[2] = 2052;      //FLocaleID
            newrow[3] = ' ';       //F_YTC_MULLANGTEXT
            dt.Rows.Add(newrow);
            return dt;
        }

        /// <summary>
        /// 针对指定表进行数据插入
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dt">包含数据的临时表</param>
        private void ImportDtToDb(string tableName, DataTable dt)
        {
            var sqlcon = conDb.GetK3CloudConn();
            sqlcon.Open(); //若返回一个SqlConnection的话,必须要显式打开 
            //注:1)要插入的DataTable内的字段数据类型必须要与数据库内的一致;并且要按数据表内的字段顺序 2)SqlBulkCopy类只提供将数据写入到数据库内
            using (var sqlBulkCopy = new SqlBulkCopy(sqlcon))
            {
                sqlBulkCopy.BatchSize = 1000;                    //表示以1000行 为一个批次进行插入
                sqlBulkCopy.DestinationTableName = tableName;   //数据库中对应的表名
                sqlBulkCopy.NotifyAfter = dt.Rows.Count;       //赋值DataTable的行数
                sqlBulkCopy.WriteToServer(dt);                //数据导入数据库
                sqlBulkCopy.Close();                         //关闭连接 
            }
        }

        /// <summary>
        /// 根据fmaterialid更新指定值
        /// </summary>
        /// <param name="fmaterialid"></param>
        public void UpdateytcRecord(string fmaterialid)
        {
            var sqllist = sqlList.UpdateytcRecord(fmaterialid);
            Generdt(sqllist);
        }

        /// <summary>
        /// 按照指定的SQL语句执行记录(更新时使用)
        /// </summary>
        private void Generdt(string sqlscript)
        {
            using (var sql = conDb.GetK3CloudConn())
            {
                sql.Open();
                var sqlCommand = new SqlCommand(sqlscript, sql);
                sqlCommand.ExecuteNonQuery();
                sql.Close();
            }
        }
    }
}
