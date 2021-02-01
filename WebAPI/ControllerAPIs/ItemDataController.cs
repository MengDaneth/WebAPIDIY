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
    public class ItemDataController : ApiController
    {
        private ItemMasterManager obj = ItemMasterManager.GetInstance();

        [Route("api/ItemData")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public IEnumerable<ItemData> GetItemData()
        {
            return obj.GetAllItemData();
        }


        [Route("api/ItemData/{itemId}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public IEnumerable<ItemData> GetItemDataByItemId(string itemId)
        {
            return obj.GetItemDataByItem(itemId);
        }
        

        [Route("api/ItemData/{langId}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public IEnumerable<ItemData> GetItemDataByLangId(string langId)
        {
            return obj.GetItemDataByLang(langId);
        }

    }
}
