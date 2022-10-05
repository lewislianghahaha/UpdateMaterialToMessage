using System;
using System.Data;
using System.Data.SqlClient;

namespace UpdateMaterialToMessage
{
    //查询
    public class SeachDt
    {
        ConnDb conDb=new ConnDb();
        SqlList sqlList=new SqlList();

        /// <summary>
        /// 根据SQL语句查询得出对应的DT
        /// </summary>
        /// <param name="sqlscript"></param>
        /// <returns></returns>
        private DataTable UseSqlSearchIntoDt(string sqlscript)
        {
            var resultdt = new DataTable();

            try
            {
                var sqlDataAdapter = new SqlDataAdapter(sqlscript, conDb.GetK3CloudConn());
                sqlDataAdapter.Fill(resultdt);
            }
            catch (Exception)
            {
                resultdt.Rows.Clear();
                resultdt.Columns.Clear();
            }

            return resultdt;
        }

        /// <summary>
        /// 按照指定的SQL语句执行记录并返回执行结果（true 或 false）
        /// 作用:更新及删除
        /// </summary>
        /// <param name="sqlscript"></param>
        /// <returns></returns>
        public bool Generdt(string sqlscript)
        {
            var result = true;
            try
            {
                var sqlcon = conDb.GetK3CloudConn();
                using (sqlcon)
                {
                    sqlcon.Open();
                    var sqlCommand = new SqlCommand(sqlscript, sqlcon);
                    sqlCommand.ExecuteNonQuery();
                    sqlcon.Close();
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 根据FMATERIALID 获取适合条件的记录
        /// 必须‘物料辅助’是‘包装材料’(571f369c14afda) 或 ‘外购辅料’(571f36db14afe2)
        /// </summary>
        /// <param name="materiallist"></param>
        /// <returns></returns>
        public DataTable SearchMaterial(string materiallist)
        {
            var dt = UseSqlSearchIntoDt(sqlList.SearchMaterial(materiallist));
            return dt;
        }

        /// <summary>
        /// 查询在ytc_t_Cust100001表是否包含指定物料记录
        /// </summary>
        /// <param name="materiallist"></param>
        /// <returns></returns>
        public DataTable Searchytc_t_Cust100001(int materiallist)
        {
            var dt = UseSqlSearchIntoDt(sqlList.Searchytc_t_Cust100001(materiallist));
            return dt;
        }

        /// <summary>
        /// 获取主键ID值
        /// </summary>
        /// <param name="typeid">0:获取ytc_t_Cust100001主键值 获取 1:获取ytc_t_Cust100001_L主键值</param>
        /// <returns></returns>
        public int GetKeyId(int typeid)
        {
            var value = Convert.ToInt32(UseSqlSearchIntoDt(sqlList.GetKeyId(typeid)).Rows[0][0]);
            return value;
        }

        /// <summary>
        /// 获取ytc_t_Cust100001表最新的F_YTC_TEXT‘供应商对应物料编码’值
        /// </summary>
        /// <returns></returns>
        //public int SearchMaxCode()
        //{
        //    var value = Convert.ToInt32(UseSqlSearchIntoDt(sqlList.SearchMaxCode()).Rows[0][0]);
        //    return value;
        //}

        /// <summary>
        /// 创建(更新)T_BAS_BILLCODES(编码规则最大编码表)中的相关记录
        /// </summary>
        /// <returns></returns>
        public bool Get_MakeUnitKey(string code)
        {
            return Generdt(sqlList.Get_MakeUnitKey(code));
        }

        /// <summary>
        /// 获取最新的FNUMMAX值,作为‘供应商对应物料编码’使用
        /// </summary>
        /// <returns></returns>
        public string SearchUnitMaxKey()
        {
            var value = Convert.ToString(UseSqlSearchIntoDt(sqlList.SearchUnitMaxKey()).Rows[0][0]);
            return value;
        }

    }
}
