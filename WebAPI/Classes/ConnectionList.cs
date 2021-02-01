using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Classes
{
    public class ConnectionList
    {      

        public static List<string> GetConnectionList()
        {
            List<string> lstConnectionStr = new List<string>();
            List<ConnectionString> lstConStr = new List<ConnectionString>();

            #region Connection to DB ItemLanguage
            // 1 lstCon = 1 string of connection
            List<Connection> lstConItemLanguage = new List<Connection>();
            lstConItemLanguage.Add(new Connection(){ name = "Data Source", value = "." });
            lstConItemLanguage.Add(new Connection(){ name = "Initial Catalog", value = "ItemLanguage" });
            lstConItemLanguage.Add(new Connection(){ name = "User ID", value = "sa" });
            lstConItemLanguage.Add(new Connection(){ name = "Password", value = "@dm1n" });
            lstConItemLanguage.Add(new Connection() { name = "Persist Security Info", value = "True" });

            #endregion
             
            lstConStr.Add(new ConnectionString() { lstConnection = lstConItemLanguage});

            foreach (var tmp in lstConStr)
            {
                lstConnectionStr.Add(tmp.GetInfo());
            }

            return lstConnectionStr;
        }
    }
}