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
    public class DIYProfileController : ApiController
    {
        private ItemMasterManager obj = ItemMasterManager.GetInstance();

        [Route("api/DIYProfile")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public IEnumerable<DIYProfile> GetDIYProfile()
        {
            return obj.GetDIYProfile().Values;
        }

        [Route("api/DIYProfile/{id}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public DIYProfile GetDIYProfileById(string id)
        {
            return obj.GetDIYProfileById(id);
        }

        [Route("api/DIYProfile/GetDIYProfileByItemDIY/{itemId}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public DIYProfile GetDIYProfileByItemId(string itemId)
        {
            return obj.GetDIYProfileByItemId(itemId);
        }

        [Route("api/DIYProfile/{periodId}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public DIYProfile GetDIYProfileByPeriodId(string periodId)
        {
            return obj.GetDIYProfileByPeriodId(periodId);
        }

        [Route("api/DIYProfile/{DIYPriceTypeId}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public DIYProfile GetDIYProfileByPriceTypeId(string priceTypeId)
        {
            return obj.GetDIYProfileByPriceTypeId(priceTypeId);
        }


    }
}
