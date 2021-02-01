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
    public class DIYSelectionController : ApiController
    {
        private ItemMasterManager obj = ItemMasterManager.GetInstance();

        [Route("api/DIYSelection")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public IEnumerable<DIYSelection> GetDIYSelection()
        {
            return obj.GetDIYSelection().Values;
        }

        [Route("api/DIYSelection/{id}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public DIYSelection GetDIYSelectionById(string id)
        {
            return obj.GetDIYSelectionById(id);
        }

        [Route("api/DIYSelection/{itemId}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public DIYSelection GetDIYSelectionByItemId(string itemId)
        {
            return obj.GetDIYSelectionByItemId(itemId);
        }

        [Route("api/DIYSelection/{defId}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public DIYSelection GetDIYSelectionByDefId(string defId)
        {
            return obj.GetDIYSelectionByDefId(defId);
        }

        [Route("api/DIYSelection/{priceId}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public DIYSelection GetDIYSelectionByPriceId(string priceId)
        {
            return obj.GetDIYSelectionByPriceId(priceId);
        }

    }
}
