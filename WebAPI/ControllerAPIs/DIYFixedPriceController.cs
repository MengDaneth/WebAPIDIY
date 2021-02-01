using clsItemLanguages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebAPI
{
    [EnableCors("*", "*", "*")]
    public class DIYFixedPriceController : ApiController
    {
        private ItemMasterManager obj = ItemMasterManager.GetInstance();

        [Route("api/DIYFixedPrice")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public IEnumerable<DIYFixedPrice> GetDIYFixedPrice()
        {
            return obj.GetDIYFixedPrice().Values;
        }

        [Route("api/DIYFixedPrice/{id}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public DIYFixedPrice GetDIYFixedPriceById(string id)
        {
            return obj.GetDIYFixedPriceById(id);
        }


        //[Route("api/DIYFixedPrice/{DIYProfileId}")]
        //[HttpGet]
        //[EnableCors("*", "*", "*")]
        //public tblDIYFixedPriceDataTable GetDIYFixedPriceByDIYProfileId(string DIYProfileId)
        //{
        //    return daFixPrice.GetDataByDIYProfileId(DIYProfileId);
        //}


        [Route("api/DIYFixedPrice/{PeriodId}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public Dictionary<string, DIYFixedPrice> GetDIYFixedPriceByPeriodId(string periodId)
        {
            return obj.GetDIYFixedPriceByPeriodId(periodId);
        }


        [Route("api/DIYFixedPrice/GetDIYFPByProfWithActPer/{profileId}")]
        [HttpGet]
        [EnableCors("*", "*", "*")]
        public DIYFixedPrice GetDIYFPByProfIdWithActPer(string profileId) // GET DIY Fixed Price By ProfileId with active period
        {
            return obj.GetDIYFPByProfIdWithActivePer(profileId);
        }

    }
}
