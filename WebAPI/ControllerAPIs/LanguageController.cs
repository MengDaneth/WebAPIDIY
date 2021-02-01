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
    public class LanguageController : ApiController
    {
        private ItemMasterManager obj = ItemMasterManager.GetInstance();
        // GET: api/Language
        [EnableCors("*", "*", "*")]
        public IEnumerable<Language> Get()
        {
            return obj.GetAllLanguage();
        }

        // GET: api/Language/5
        [EnableCors("*", "*", "*")]
        public Language Get(string id)
        {
            return obj.GetLanguageById(id);
        }

        // POST: api/Language
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Language/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Language/5
        public void Delete(int id)
        {
        }
    }
}
