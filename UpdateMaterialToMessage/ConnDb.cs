using System.Data.SqlClient;

namespace UpdateMaterialToMessage
{
    public class ConnDb
    {
        /// <summary>
        /// 获取连接返回信息
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetK3CloudConn()
        {
            var sqlcon = new SqlConnection(GetConnectionString());
            return sqlcon;
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString()
        {
            //if (conid == 0)
            //{
               var strcon = @"Data Source='192.168.1.228';Initial Catalog='AIS20220817082811';Persist Security Info=True;User ID='sa'; Password='kingdee';
                       Pooling=true;Max Pool Size=40000;Min Pool Size=0";
            //}
            //else
            //{
            //    strcon = @"Data Source='172.16.4.249';Initial Catalog='RTIM_YATU';Persist Security Info=True;User ID='sa'; Password='Yatu8888';
            //           Pooling=true;Max Pool Size=40000;Min Pool Size=0";
            //}

            return strcon;
        }
    }
}
