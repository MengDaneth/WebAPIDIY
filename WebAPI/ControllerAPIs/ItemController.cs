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
    public class ItemController : ApiController
    {
        private ItemMasterManager obj = ItemMasterManager.GetInstance();

        // GET: api/Item

        /// <summary>
        /// This API returns All Items
        /// </summary>
        /// <returns>IEnumerable&lt;string&gt;</returns>
        /// 
        [EnableCors("*", "*", "*")]
        public IEnumerable<Item> Get()
        {
            return obj.GetAllItem();
        }

        
        [EnableCors("*", "*", "*")]
        [Route("api/item/{id}")]
        public IEnumerable<Item> GetItem(string id)
        {
            return obj.GetAllItem();
        }

        [EnableCors("*", "*", "*")]
        [Route("api/Item/GetItemByItemType/{itemTypeId}")]
        public IEnumerable<Item> GetItemByType(string itemTypeId)
        {
            return obj.GetItemByItemType(itemTypeId);
        }


        [EnableCors("*", "*", "*")]
        [Route("api/get_item_types/{exceiptItmTypeId}")]
        public IEnumerable<Item> GetItemExceptType(string exceiptItmTypeId)
        {
            return obj.GetItemExceptItemTypeId(exceiptItmTypeId);
        }


        [EnableCors("*", "*", "*")]
        [Route("api/Item/GetStdPriceByItem/{itemId}")]
        public double GetStdPriceByItem(string itemId)
        {
            try
            {
                return obj.GetStdPriceByItemId(itemId);
            }
            catch (HttpResponseException ex)
            {
                throw ex;
            }
            
        }


        // GET: api/Item/5
        [EnableCors("*", "*", "*")]
        public Item Get(string id)
        {
            return obj.GetItemById(id);
        }

        // POST: api/Item
        [EnableCors("*", "*", "*")]
        public void Post(Item item)
        {
            ItemMasterManager obj = ItemMasterManager.GetInstance();
            obj.InsertItem(item);
        }

        // PUT: api/Item/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Item/5
        public void Delete(int id)
        {
        }
    }
}
