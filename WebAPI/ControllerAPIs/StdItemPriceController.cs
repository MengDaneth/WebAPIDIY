using clsItemLanguages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebAPI.ControllerAPIs
{
    [EnableCors("*", "*", "*")]
    public class StdItemPriceController : ApiController
    {
        private ItemMasterManager obj = ItemMasterManager.GetInstance();

        [Route("api/StdItemPrice")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public IEnumerable<StandardItemPrice> GetStdItemPrice()
        {
            return obj.GetStdItemPrice().Values;
        }


        [Route("api/StdItemPrice/{id}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public StandardItemPrice GetStdItemPriceById(string id)
        {
            return obj.GetStdItmPriceById(id);
        }


        [Route("api/StdItemPrice/{itemId}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public IEnumerable<StandardItemPrice> GetStdItemPriceByitemId(string itemId)
        {
            return obj.GetStdItmPriceByItemId(itemId);
        }


        [Route("api/StdItemPrice/{periodId}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public IEnumerable<StandardItemPrice> GetStdItemPriceByperiodId(string periodId)
        {
            return obj.GetStdItmPriceByPeriodId(periodId);
        }

    }
}
