using clsItemLanguages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebAPI;

namespace Client.ControllersAPI
{
    [EnableCors("*", "*", "*")]
    public class TransactionController : ApiController
    {
        private ItemMasterManager obj = ItemMasterManager.GetInstance();

        [Route("api/Transaction")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public IEnumerable<Transaction> GetTransaction()
        {
            return obj.GetAllTransaction().Values;
        }


        [Route("api/Transaction/{id}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public Transaction GetTransactionById(string id)
        {
            return obj.GetTranById(id);
        }

        [Route("api/Transaction/{sDate}/{eDate}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public List<Transaction> GetItem(DateTime sDate, DateTime eDate)
        {
            return obj.GetTranByTime(sDate, eDate);
        }
             

    }
}
