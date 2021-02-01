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
    public class ItemTypeController : ApiController
    {
        private ItemMasterManager obj = ItemMasterManager.GetInstance();

        // GET: api/ItemType
        [EnableCors("*", "*", "*")]
        public IEnumerable<ItemType> Get()
        {
            return obj.GetAllItemType();
        }

        // GET: api/ItemType/5
        [EnableCors("*", "*", "*")]
        public ItemType Get(string id)
        {
            return obj.GetItemTypeById(id);
        }

        // POST: api/ItemType
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ItemType/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ItemType/5
        public void Delete(int id)
        {
        }
    }
}
