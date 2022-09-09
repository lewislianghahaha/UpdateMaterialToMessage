using System;
using Kingdee.BOS.Core.DynamicForm.PlugIn.Args;
using Kingdee.BOS.Core.List.PlugIn;

namespace UpdateMaterialToMessage
{
    public class ButtonEvents : AbstractListPlugIn
    {
        Generate generate=new Generate();

        public override void BarItemClick(BarItemClickEventArgs e)
        {
            //定义主键变量(用与收集所选中的行主键值)
            var flistid = string.Empty;
            //中转判断值
            var tempstring = string.Empty;
            //返回信息记录
            var mesage = string.Empty;

            base.BarItemClick(e);

            if (e.BarItemKey == "tbUpMaterial")
            {
                //获取当前登录用户名称
                var username = this.Context.UserName;
                //检测需指定用户才能执行此功能
                if (username == "易嘉涛" || username== "黄伟豪" || username== "信息管理科" /*|| username== "冯嘉伟" 
                    || username== "陈容爱" || username== "冯兆华" || username=="梁嘉杰"*/)
                {
                    //获取列表上通过复选框勾选的记录
                    var selectedrows = this.ListView.SelectedRowsInfo;
                    //判断需要有选择记录时才继续
                    if (selectedrows.Count > 0)
                    {
                        //通过循环将选中行的主键进行收集(注:去除重复的选项,只保留不重复的主键记录)
                        foreach (var row in selectedrows)
                        {
                            if (string.IsNullOrEmpty(flistid))
                            {
                                flistid = "'" + Convert.ToString(row.PrimaryKeyValue) + "'";
                                tempstring = Convert.ToString(row.PrimaryKeyValue);
                            }
                            else
                            {
                                if (tempstring != Convert.ToString(row.PrimaryKeyValue))
                                {
                                    flistid += "," + "'" + Convert.ToString(row.PrimaryKeyValue) + "'";
                                    tempstring = Convert.ToString(row.PrimaryKeyValue);
                                }
                            }
                        }
                        //执行运算并返回相关结果
                        mesage = generate.GenerateYtcRecord(flistid);
                        View.ShowMessage(mesage != "Finish" ? "更新异常,请联系管理员" : "更新成功,请进入'供货信息维护'进行查阅");
                    }
                    else
                    {
                        View.ShowErrMessage("请选择‘物料’后继续.");
                    }
                }
                else
                {
                    View.ShowErrMessage($"登录用户:'{username}',没权限使用此功能,请联系管理员处理");
                }
            }
        }
    }
}
