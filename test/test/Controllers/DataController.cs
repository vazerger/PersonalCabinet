using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Services;
using test.Models;
using WebApi.OutputCache.V2;


namespace test.Controllers
{
    public class DataController : ApiController
    {
        private Repository repository = new Repository();

        // оставлю пока, но здесь подключать нельзя напрямую к базе
        private Entities db = new Entities();

        // GET api/Data
        public IQueryable<Transactions> GetTransactions()
        {
            return db.Transactions;
        }

        // GET api/Data/5
        [HttpGet]
        [Route("api/Data/GetTransactions/{search=}/{purchaseamt=}/{datefrom=}/{dateto=}/{result=}")]
        public TransactionsModels GetTransactions(string search, string purchaseamt, string datefrom, string dateto, string result)
        {
            TransactionsModels model = new TransactionsModels();

            string user = User.Identity.Name;
            //string userkey = ;

            var list = db.v_Transactions.Where(c => c.UserName == user).OrderBy(c => c.ORDERID).ToList();
            //var list = db.Transactions.OrderBy(c => c.ORDERID).ToList();

            if (!String.IsNullOrWhiteSpace(search))
            {
                bool IsDigit = search.Length == search.Where(c => char.IsDigit(c)).Count();
                if (IsDigit)
                {
                    list = list.Where(c => c.ORDERID == Convert.ToInt32(search)).ToList();
                }
            }

            if (!String.IsNullOrWhiteSpace(purchaseamt))
            {
                bool IsDigit = purchaseamt.Length == purchaseamt.Where(c => char.IsDigit(c)).Count();
                if (IsDigit)
                {
                    list = list.Where(c => c.PURCHASEAMT == Convert.ToInt32(purchaseamt)).ToList();
                }
            }           

            if (!String.IsNullOrWhiteSpace(result))
            {
                list = list.Where(c => c.RESULT == Convert.ToInt32(result)).ToList();
            }

            if (list.Count > 0)
            {
                DateTime date = (DateTime)list.Min(c => c.DATETIME);
                DateTime date1 = (DateTime)list.Max(c => c.DATETIME);

                if (!String.IsNullOrWhiteSpace(datefrom))
                {
                    date = Convert.ToDateTime(datefrom);
                    date1 = Convert.ToDateTime(dateto);

                    list = list.Where(c => c.DATETIME >= date && c.DATETIME <= date1).ToList();
                }
            }

            model.transactions = list;
            model.total_count = list.Count;
            
            return model;
        }

        [HttpGet]
        [Route("api/Data/GetTransactionsDates")]
        [CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)]
        public TransactionsDatesModel GetTransactionsDates()
        {
            TransactionsDatesModel model = new TransactionsDatesModel();

            var list = db.Transactions.OrderBy(c => c.ORDERID).ToList();
            DateTime d1 = (DateTime)list.Min(d => d.DATETIME);
            DateTime d2 = (DateTime)list.Max(d => d.DATETIME);

            model.dateFrom = d1;
            model.dateTo = d2;

            return model;
        }

        // GET api/Data
        [Route("api/Data/GetResults")]
        public List<selectModel> GetResults()
        {
            return repository.GetResultsList();
        }

        // GET api/Data/5
        [ResponseType(typeof(Transactions))]
        public IHttpActionResult GetTransactions(int id)
        {
            Transactions transactions = db.Transactions.Find(id);
            if (transactions == null)
            {
                return NotFound();
            }

            return Ok(transactions);
        }

        // PUT api/Data/5
        public IHttpActionResult PutTransactions(int id, Transactions transactions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transactions.ORDERID)
            {
                return BadRequest();
            }

            db.Entry(transactions).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Data
        [ResponseType(typeof(Transactions))]
        public IHttpActionResult PostTransactions(Transactions transactions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Transactions.Add(transactions);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (TransactionsExists(transactions.ORDERID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = transactions.ORDERID }, transactions);
        }

        // DELETE api/Data/5
        [ResponseType(typeof(Transactions))]
        public IHttpActionResult DeleteTransactions(int id)
        {
            Transactions transactions = db.Transactions.Find(id);
            if (transactions == null)
            {
                return NotFound();
            }

            db.Transactions.Remove(transactions);
            db.SaveChanges();

            return Ok(transactions);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransactionsExists(int id)
        {
            return db.Transactions.Count(e => e.ORDERID == id) > 0;
        }
    }
}