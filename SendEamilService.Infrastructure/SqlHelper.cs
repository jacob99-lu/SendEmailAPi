using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendEamilService.Infrastructure
{
    public class SqlHelper
    {
        //satic静态，一旦赋值，会一直保留这个值。不会被回收。
        public static string? Constr { get; set; }
        public static DataTable ExecuteTable(string cmdText)
        {
            using (SqlConnection con = new SqlConnection(Constr))
            {
                DataSet ds = null;
                try
                {
                    con.Open();
                    //　SqlCommand对象：Ado.Net中执行数据库命令的对象。
                    SqlCommand cmd = new SqlCommand(cmdText, con);
                    //1、表示用于填充 DataSet 和更新 SQL Server 数据库的一组数据命令和一个数据库连接。
                    //2、在SqlDataAdapter和DataSet之间没有直接连接。当完成SqlDataAdpater.Fill(DataSet)调用后，两个对象之间就没有连接了。
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    sda.Fill(ds);
                }
                catch (Exception ex)
                {

                }
                return ds.Tables[0];
                
            }
        }
    }
}
