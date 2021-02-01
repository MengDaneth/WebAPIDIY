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
    public class DIYPriceController : ApiController
    {
        private ItemMasterManager obj = ItemMasterManager.GetInstance();

        [Route("api/DIYPrice")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public IEnumerable<DIYPrice> GetDIYPrice()
        {
            return obj.GetDIYPrice().Values;
        }


        [Route("api/DIYPrice/{id}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public DIYPrice GetDIYPriceById(string id)
        {
            return obj.GetDIYPriceById(id);
        }


        [Route("api/DIYPrice/{tranId}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public DIYPrice GetDIYPriceByTranId(string tranId)
        {
            return obj.GetDIYPriceById(tranId);
        }


        [Route("api/DIYPrice/{profileId}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public List<DIYPrice> GetDIYPriceByProfileId(string profileId)
        {
            return obj.GetDIYPriceByProfileId(profileId);
        }


        [Route("api/DIYPrice/{selTypeId}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public DIYPrice GetDIYPriceBySelTypeId(string selTypeId)
        {
            return obj.GetDIYPriceBySaleTypeId(selTypeId);
        }

        [Route("api/DIYPrice")]
        [HttpPost]
        [EnableCors("*", "*", "*")]
        public void POST(DIYPrice diyPrice)
        {
            ItemMasterManager obj = ItemMasterManager.GetInstance();
            obj.InsertDIYTransaction(diyPrice);
        }

    }
}
