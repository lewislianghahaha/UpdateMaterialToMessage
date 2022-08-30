namespace UpdateMaterialToMessage
{
    //SQL
    public class SqlList
    {
        private string _result;

        /// <summary>
        /// 根据FMATERIALID 获取适合条件的记录
        /// 必须‘物料辅助’是‘包装材料’(571f369c14afda) 或 ‘外购辅料’(571f36db14afe2) 并且审核状态为‘已审核’
        /// </summary>
        /// <param name="materiallist"></param>
        /// <returns></returns>
        public string SearchMaterial(string materiallist)
        {
            _result = $@"
                            SELECT A.FMATERIALID
							FROM dbo.T_BD_MATERIAL A
							WHERE A.FMATERIALID IN ({materiallist})
							AND A.F_YTC_ASSISTANT5 IN ('571f369c14afda','571f36db14afe2')
                            AND A.FDOCUMENTSTATUS='C'
                        ";

            return _result;
        }

        /// <summary>
        /// 查询在ytc_t_Cust100001表是否包含指定物料记录
        /// </summary>
        /// <param name="materiallist"></param>
        /// <returns></returns>
        public string Searchytc_t_Cust100001(int materiallist)
        {
            _result = $@"
                            SELECT COUNT(*) Icount
						    FROM dbo.ytc_t_Cust100001 A
						    WHERE A.F_YTC_BASE = ('{materiallist}')
                        ";
            return _result;
        }

        /// <summary>
        /// 获取主键ID值
        /// </summary>
        /// <param name="typeid">0:获取ytc_t_Cust100001主键值 获取 1:获取ytc_t_Cust100001_L主键值</param>
        /// <returns></returns>
        public string GetKeyId(int typeid)
        {
            if (typeid == 0)
            {
                _result = @"
                                DECLARE
	                             @id INT;
                                BEGIN
                                    --获取ytc_t_Cust100001.FID主键值
								    INSERT INTO dbo.Z_ytc_t_Cust100001( Column1 )
								    VALUES  (1)

								    SELECT @id=Id FROM dbo.Z_ytc_t_Cust100001

								    DELETE FROM dbo.Z_ytc_t_Cust100001
                                
                                    SELECT @id                                
                                END
                           ";
            }
            else
            {
                _result = @"
                                DECLARE
	                             @id INT;
                                BEGIN
                                    --获取ytc_t_Cust100001_L.FPKID主键值
								    INSERT INTO dbo.Z_ytc_t_Cust100001_L( Column1 )
								    VALUES  (1)

								    SELECT @id=Id FROM dbo.Z_ytc_t_Cust100001_L

								    DELETE FROM dbo.Z_ytc_t_Cust100001_L

                                    SELECT @id
                                END
                           ";
            }

            return _result;
        }

        /// <summary>
        /// 获取ytc_t_Cust100001表最新的F_YTC_TEXT‘供应商对应物料编码’值
        /// </summary>
        /// <returns></returns>
        public string SearchMaxCode()
        {
            #region A
            //SELECT 'WM' + CONVERT(NVARCHAR(500), CONVERT(INT, SUBSTRING(x.F_YTC_TEXT, 3, LEN(X.F_YTC_TEXT))) + 1)

            //                FROM dbo.ytc_t_Cust100001 x

            //                WHERE x.FID = (SELECT TOP 1 A.FID FROM dbo.ytc_t_Cust100001 a

            //                                        ORDER BY CONVERT(INT, SUBSTRING(A.F_YTC_TEXT, 3, LEN(A.F_YTC_TEXT))) DESC)
            #endregion
            _result = @"
                            SELECT /*'WM'+CONVERT(NVARCHAR(500),*/CONVERT(INT,SUBSTRING(x.F_YTC_TEXT,3,LEN(X.F_YTC_TEXT)))/*+1)*/
							FROM dbo.ytc_t_Cust100001 x
							WHERE x.FID=( SELECT TOP 1 A.FID FROM dbo.ytc_t_Cust100001 a
													ORDER BY CONVERT(INT,SUBSTRING(A.F_YTC_TEXT,3,LEN(A.F_YTC_TEXT))) DESC)
                        ";

            return _result;
        }

        /// <summary>
        /// 当有物料ID在ytc_t_Cust100001表存在时,将FID更新至F_ytc_Base1内
        /// </summary>
        /// <param name="materialid"></param>
        /// <returns></returns>
        public string UpdateytcRecord(string materialid)
        {
            _result = $@"
                            UPDATE t1 SET t1.F_ytc_Base1=t2.fid
							FROM t_bd_material t1
							INNER JOIN ytc_t_Cust100001 t2 ON t1.FMATERIALID = t2.F_ytc_Base
							WHERE t1.FMATERIALID IN ({materialid}) AND t2.FBILLSTATUS<>'B'
                        ";

            return _result;
        }

    }
}
