using clsItemLanguages;
using Newtonsoft.Json;
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
    public class DIYDefinitionController : ApiController
    {
        private ItemMasterManager obj = ItemMasterManager.GetInstance();

        [Route("api/DIYDefinition")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public IEnumerable<DIYDefinition> GetDIYDefinition()
        {
            return obj.GetDIYDefinition().Values;
        }

        [Route("api/DIYDefinition/{id}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public DIYDefinition GetDIYDefinitionById(string id)
        {
            return obj.GetDIYDefinitionById(id);
        }

        [Route("api/DIYDefinition/{itemId}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public IEnumerable<DIYDefinition> GetDIYDefinitionByItemId(string itemId)
        {
            return obj.GetDIYDefinitionByItemId(itemId);
        }

        [Route("api/DIYDefinition/GetDIYDefByProfile/{profId}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public IEnumerable<DIYDefinition> GetDIYDefinitionByProfileId(string profId)
        {
            List<DIYDefinition> list = obj.GetDIYDefinitionByProfileId(profId);
            string json = JsonConvert.SerializeObject(list, Formatting.Indented,
                new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            List<DIYDefinition> deserializedPeople = JsonConvert.DeserializeObject<List<DIYDefinition>>(json,
                new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

            return deserializedPeople;
        }
        

    }
}
