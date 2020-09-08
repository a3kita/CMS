using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;

namespace MovieWeb.Controllers
{
    public class CommentController : Controller
    {

        private MySqlConnection getConnect()
        {
            String connetStr = "server=localhost;port=3306;user=root;password=1284604307; database=film;SslMode=none;";
            MySqlConnection conn = new MySqlConnection(connetStr);
            conn.Open();
            return conn;
        }
        public ActionResult TicketTable()
        {
            if (Session["aid"] != null)
            {
                MySqlConnection conn = getConnect();
                try
                {
                    MySqlCommand mycom = conn.CreateCommand();
                    mycom.CommandText = "SELECT  影片.影片ID,影片.票价,影片.名字,影片.类型,Count(`影票`.`场次ID`) as 总人数 , Count(`影票`.`场次ID`) * `影片`.`票价` as 总票房  FROM  影片 LEFT JOIN `场次` on `场次`.`影片ID` = `影片`.`影片ID` LEFT JOIN `影票` on `影票`.`场次ID` = `场次`.`场次ID` group by `影片`.`影片ID`";
                    MySqlDataAdapter adap = new MySqlDataAdapter(mycom);
                    DataSet ds = new DataSet();
                    adap.Fill(ds);
                    ViewBag.data = ds.Tables[0].Rows;
                    ViewBag.admin = Session["aid"];
                }
                catch (MySqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
                return View();
            }
            else
                throw new UnauthorizedAccessException("权限不足");
        }
        public ActionResult Comments()
        {
            if (Session["aid"] != null)
            {
                MySqlConnection conn = getConnect();
                try
                {
                    MySqlCommand mycom = conn.CreateCommand();
                    mycom.CommandText = "SELECT * From 评论记录 ";
                    MySqlDataAdapter adap = new MySqlDataAdapter(mycom);
                    DataSet ds = new DataSet();
                    adap.Fill(ds);
                    ViewBag.data = ds.Tables[0].Rows;
                    ViewBag.admin = Session["aid"];
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
                return View();
            }
            else
                throw new UnauthorizedAccessException("权限不足");
        }
        public String DelComment(String pid,String id)
        {
            if (Session["aid"] != null)
            {
                MySqlConnection conn = getConnect();
                try
                {
                    MySqlCommand mycom = conn.CreateCommand();
                    if (id == null || pid == null) return "参数不足 <a href='/film/comments'>返回</a>";
                    mycom.CommandText = $"delete from 评论记录 where 账号ID1='{id}' and 影票ID1 = '{pid}' ";
                    mycom.ExecuteNonQuery();
                    ViewBag.admin = Session["aid"];
                }
                catch (MySqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
                return "success <a href='/Comment/comments'>返回</a>";
            }
            else
                return "权限不足 <a href='/Comment/comments'>返回</a>";
        }
    }
}