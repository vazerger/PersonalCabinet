using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using test.Models;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;

namespace test.Models
{
    public class Repository
    {
        Entities _db = new Entities();

        public IEnumerable<Transactions> GetTransactions()
        {
            return _db.Transactions.OrderBy(c => c.ORDERID);
        }

        public IEnumerable<SelectListItem> GetResultDesc()
        {
            List<Transactions> list = new List<Transactions>();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            con.Open();

            SqlCommand c = new SqlCommand("SELECT RESULT, RESULTDESC FROM Transactions GROUP BY RESULT, RESULTDESC", con);
            SqlDataReader read = c.ExecuteReader();
            while (read.Read())
            {
                list.Add(new Transactions { RESULT = Convert.ToInt32(read[0].ToString()), RESULTDESC = read[1].ToString()});
            }
            read.Close();

            con.Close();

            IEnumerable<SelectListItem> sel = from list1 in list select new SelectListItem { Text = list1.RESULTDESC, Value = list1.RESULT.ToString() };


            return sel;

        }

        public List<selectModel> GetResultsList()
        {

            List<selectModel> list = new List<selectModel>();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            con.Open();

            SqlCommand c = new SqlCommand("SELECT RESULT, RESULTDESC FROM Transactions GROUP BY RESULT, RESULTDESC", con);
            SqlDataReader read = c.ExecuteReader();
            while (read.Read())
            {
                list.Add(new selectModel { id = Convert.ToInt32(read[0].ToString()), name = read[1].ToString() });
            }
            read.Close();

            con.Close();

            return list;
        }

        public List<ResultViewModel> getGroupResult(string username)
        {
            List<ResultViewModel> model = new List<ResultViewModel>();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            con.Open();

            SqlCommand c = new SqlCommand("SELECT dt, sum1, sum2, sum3 FROM v_GroupResult WHERE UserName = @usr ORDER BY dt DESC", con);
            c.Parameters.AddWithValue("@usr", username);
            SqlDataReader read = c.ExecuteReader();
            while (read.Read())
            {
                model.Add(new ResultViewModel { date = Convert.ToDateTime(read[0].ToString()), sum1 = Convert.ToDouble(read[1].ToString()), sum2 = Convert.ToDouble(read[2].ToString()), sum3 = Convert.ToDouble(read[3].ToString()) });
            }
            read.Close();

            con.Close();

            return model;
        }

        public Transactions getDatailTransaction(string user, int id)
        {
            Transactions model = new Transactions();
            var transaction = _db.v_Transactions.Where(c => c.UserName == user && c.ORDERID == id).FirstOrDefault();

            if (transaction != null)
            {
                model.ORDERID = transaction.ORDERID;
                model.PURCHASEAMT = transaction.PURCHASEAMT;
                model.ACCOUNT = transaction.ACCOUNT;
                model.RESULT = transaction.RESULT;
                model.RESULTDESC = transaction.RESULTDESC;
                model.DATETIME = transaction.DATETIME;
                model.RRN = transaction.RRN;
                model.DEALID = transaction.DEALID;
            }

            return model;
        }

    }
}