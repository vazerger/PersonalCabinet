using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace test.Models
{
    public class TransactionsModel
    {
        public string searchOrder { get; set; } // поиск по номеру операции

        // фильтр по статусу
        public int resultDescKey { get; set; }
        public IEnumerable<SelectListItem> resultDesc { get; set; }        

        public int resultKey { get; set; }
        public IEnumerable<SelectListItem> results { get; set; }

        public DateTime date { get; set; }
        public IEnumerable<Transactions> list { get; set; }
    }

    public class selectModel
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class TransactionsModels
    {
        public List<v_Transactions> transactions { get; set; }
        public int total_count { get; set; }
    }

    public class TransactionsDatesModel
    {
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
    }

    public class ResultViewModel
    {
        public DateTime date { get; set; }
        public string username { get; set;  }
        public double sum1 { get; set; }
        public double sum2 { get; set; }
        public double sum3 { get; set; }
    }
}