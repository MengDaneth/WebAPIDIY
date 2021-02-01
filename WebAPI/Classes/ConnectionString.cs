using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Classes
{
    public class ConnectionString
    {
        public List<Connection> lstConnection = new List<Connection>();
        public string GetInfo()
        {
            List<string> lstStr = new List<string>();
            foreach (var tmp in lstConnection)
            {
                lstStr.Add(tmp.getInfo());
            }

            return String.Join("; ", lstStr);
        }
    }
}