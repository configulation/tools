using System.Data;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Microsoft.Data.Sqlite;

/// <summary>
/// 开源软件
/// </summary>
namespace Chess.OpenSource
{
    /// <summary>
    /// Sqlite 助手
    /// </summary>
    public class SqliteHelper
    {
        private static string DbFile = "";
        private static string DbSourcePath = "";

        /// <summary>
        /// 检查数据库文件是否存在
        /// </summary>
        /// <returns>true=文件存在；false=文件不存在</returns>
        private static bool DbFileExist()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
#if (DEBUG) //  调试期间数据库文件使用代码路径。
            path = Directory.GetParent(path).FullName;
            path = Directory.GetParent(path).FullName;
            path = Directory.GetParent(path).FullName;
            path = Directory.GetParent(path).FullName;
#else // 软件发布时使用生成的可执行文件路径。
            path = System.AppDomain.CurrentDomain.BaseDirectory;
#endif
            DbFile = path + @"\DB\KaiJuKu.db";
            if (!System.IO.File.Exists(DbFile))
            {
                System.Windows.MessageBox.Show($"数据库文件{DbFile}未找到，请检查文件路径，或修改SqliteHelper.cs中的文件路径。", "错误提示");
                return false;
            }
            DbSourcePath = @"data source=" + DbFile;

            return true;
        }
        /// <summary>
        /// 执行sql字符串
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <returns></returns>
        public static int ExecuteSql(string sql)
        {
            if (!DbFileExist()) return 0;
            using SQLiteConnection con = new(DbSourcePath);
            using SQLiteCommand cmd = new(sql, con);
            con.Open();
            return cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 执行sql字符串
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static int ExecuteSql(string sql, params SQLiteParameter[] param)
        {
            if (!DbFileExist()) return 0;
            using SQLiteConnection con = new(DbSourcePath);
            using SQLiteCommand cmd = new(sql, con);
            con.Open();
            if (param != null)
            {
                cmd.Parameters.AddRange(param);
            }
            return cmd.ExecuteNonQuery();
        }

        public static string ExecuteScalar(string sql, params SqliteParameter[] param)
        {
            if (!DbFileExist()) return null;
            using SQLiteConnection con = new(DbSourcePath);
            using SQLiteCommand cmd = new(sql, con);
            con.Open();
            if (param != null)
            {
                cmd.Parameters.AddRange(param);
            }
            object obj = cmd.ExecuteScalar();
            return obj == null ? "" : obj.ToString();
        }
        public static SQLiteDataReader ExecuteReader(string sql, params SQLiteParameter[] param)
        {
            if (!DbFileExist()) return null;
            using SQLiteConnection con = new(DbSourcePath);
            using SQLiteCommand cmd = new(sql, con);
            if (param != null)
            {
                cmd.Parameters.AddRange(param);
            }
            try
            {
                con.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (System.Exception)
            {
                con.Close();
                con.Dispose();
                //throw ex;
                return null;
            }
        }
        public static DataTable ExecuteTable(string sql)
        {
            if (!DbFileExist()) return null;
            DataTable dt = new();
            try
            {
                using SQLiteDataAdapter sda = new(sql, DbSourcePath);
                sda.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                _ = sda.Fill(dt);
            }
            catch (System.Exception ex)
            {
                _ = ex.Message;
            }
            return dt;
        }
        public static DataTable ExecuteTable(string sql, params SQLiteParameter[] param)
        {
            if (!DbFileExist()) return null;
            DataTable dt = new();
            try
            {
                using SQLiteDataAdapter sda = new(sql, DbSourcePath);
                if (param != null)
                {
                    sda.SelectCommand.Parameters.AddRange(param);
                }
                sda.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                _ = sda.Fill(dt);
            }
            catch (System.Exception ex)
            {
                _ = ex.Message;
            }
            return dt;
        }
        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="tbName">表名称</param>
        /// <param name="fields">字段名</param>
        /// <param name="where">条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="limit">限制数据行数</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static DataTable Select(string tbName, string fields = "*", string where = "1", string orderBy = "", string limit = "", params SQLiteParameter[] param)
        {
            //排序
            if (orderBy != "")
            {
                orderBy = "ORDER BY " + orderBy;// 示例: ORDER BY rowid desc
            }

            //行数限制
            if (limit != "")
            {
                limit = "LIMIT " + limit;// 示例: LIMIT 0,10
            }

            string sql = $"SELECT {fields} FROM `{tbName}` WHERE {where} {orderBy} {limit}";

            return ExecuteTable(sql, param);
        }
        /// <summary>
        /// 插入新数据
        /// </summary>
        /// <param name="tbName">数据表名称</param>
        /// <param name="insertData">数据字典</param>
        /// <returns>插入数据影响的行数</returns>
        public static int Insert(string tbName, Dictionary<string, object> insertData)
        {
            string point = "";  //分隔符号(,)
            string keyStr = ""; //字段名拼接字符串
            string valueStr = "";   //值的拼接字符串
            List<SQLiteParameter> param = new();
            foreach (string key in insertData.Keys)
            {
                keyStr += $"{point} `{key}`";
                valueStr += $"{point} @{key}";
                param.Add(new SQLiteParameter("@" + key, insertData[key]));
                point = ",";
            }
            string sql = $"INSERT INTO `{tbName}`({keyStr}) VALUES({valueStr})";
            return ExecuteSql(sql, param.ToArray());
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="tbName">数据表名称</param>
        /// <param name="where">条件语句，通常为"rowid=?"</param>
        /// <param name="updateData">数据字典</param>
        /// <returns>更新数据影响的行数</returns>
        public static int Update(string tbName, string where, Dictionary<string, object> updateData)
        {
            string point = "";//分隔符号(,)
            string kvStr = "";//键值对拼接字符串(Id=@Id)
            List<SQLiteParameter> param = new();
            foreach (string key in updateData.Keys)
            {
                kvStr += $"{point} {key}=@{key}";
                param.Add(new SQLiteParameter("@" + key, updateData[key]));
                point = ",";
            }
            string sql = $"UPDATE `{tbName}` SET {kvStr} WHERE {where}";
            return ExecuteSql(sql, param.ToArray());

        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="tbName">数据表名称</param>
        /// <param name="where">条件语句，通常为"rowid=?"</param>
        /// <returns>删除数据影响的行数</returns>
        public static int Delete(string tbName, string where)
        {
            if (string.IsNullOrEmpty(where)) return -1;
            string sql = $"DELETE FROM `{tbName}` WHERE {where}";
            return ExecuteSql(sql);

        }
    }
}