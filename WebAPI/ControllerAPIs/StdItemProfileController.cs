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
    public class StdItemProfileController : ApiController
    {
        private ItemMasterManager obj = ItemMasterManager.GetInstance();

        [Route("api/StdItemProfile")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public IEnumerable<StandardItemProfile> GetStdItmProfile()
        {
            return obj.GetStdItemProfile().Values;
        }


        [Route("api/StdItemProfile/{id}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public StandardItemProfile GetStdItmProfileById(string id)
        {
            return obj.GetStdItmProfById(id);
        }

        [Route("api/StdItemProfile/{itemId}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public IEnumerable<StandardItemProfile> GetStdItmProfileByItemId(string itemId)
        {
            return obj.GetStdItmProfByItemId(itemId);
        }


        [Route("api/StdItemProfile/{periodId}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public IEnumerable<StandardItemProfile> GetStdItmProfileByPeriodId(string periodId)
        {
            return obj.GetStdItmProfByPeriodId(periodId);
        }

    }
}
