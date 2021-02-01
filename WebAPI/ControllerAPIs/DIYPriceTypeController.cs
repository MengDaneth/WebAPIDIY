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
    public class DIYPriceTypeController : ApiController
    {
        private ItemMasterManager obj = ItemMasterManager.GetInstance();

        [Route("api/DIYPriceType")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public IEnumerable<DIYPriceType> GetDIYPriceType()
        {
            return obj.GetDIYPriceType().Values;
        }


        [Route("api/DIYPriceType/{id}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public DIYPriceType GetDIYPriceTypeId(string id)
        {
            return obj.GetDIYPriceTypeById(id);
        }


    }
}
