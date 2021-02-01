using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Classes
{
    public class Connection
    {
        public string name { get; set; }
        public string value { get; set; }

        public string getInfo()
        {
            return name + "=" + value;
        }
    }
}