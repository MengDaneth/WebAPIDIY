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
    public class SyncDataController : ApiController
    {
        // POST: api/SyncData
        [Route("api/SyncData")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public void Get()
        {
            ItemMasterManager obj = ItemMasterManager.GetInstance();
            obj.LoadData();
        }

        // GET: api/SyncData/5
        public string Get(int id)
        {
            return "value";
        }
        

        // PUT: api/SyncData/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/SyncData/5
        public void Delete(int id)
        {
        }
    }
}
