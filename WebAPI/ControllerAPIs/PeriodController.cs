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
    public class PeriodController : ApiController
    {
        private ItemMasterManager obj = ItemMasterManager.GetInstance();

        [Route("api/Period")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public IEnumerable<Period> GetPeriod()
        {
            return obj.GetAllPeriod().Values;
        }

        [Route("api/Period/{id}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public Period GetPeriodById(string id)
        {
            return obj.GetPeriodById(id);
        }

        [Route("api/Period/GetStatusId/{statusId}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public Period GetPeriodByStatusId(string statusId)
        {
            return obj.GetPeriodById(statusId);
        }

    }
}
