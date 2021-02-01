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
    public class DIYSelectionTypeController : ApiController
    {
        private ItemMasterManager obj = ItemMasterManager.GetInstance();

        [Route("api/DIYSelectionType")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public IEnumerable<DIYSelectionType> GetDIYSelType()
        {
            return obj.GetDIYSelectionType().Values;
        }


        [Route("api/DIYSelectionType/{id}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public DIYSelectionType GetDIYSelTypeById(string id)
        {
            return obj.GetDIYSelectionTypeById(id);
        }
        

    }
}
